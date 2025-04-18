using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace YoavDiscordClient.Managers
{
    /// <summary>
    /// Manages chat functionality including messages, chat rooms, and message input.
    /// </summary>
    public class ChatManager
    {
        private readonly DiscordApp _form;
        private readonly Panel _chatAreaPanel;

        public ChatManager(DiscordApp form, Panel chatAreaPanel)
        {
            _form = form;
            _chatAreaPanel = chatAreaPanel;
        }

        /// <summary>
        /// Adds a message to the appropriate chat panel.
        /// </summary>
        /// <param name="userId">The ID of the user sending the message.</param>
        /// <param name="username">The username of the sender.</param>
        /// <param name="message">The message content.</param>
        /// <param name="profileImage">The profile image of the sender.</param>
        /// <param name="time">The timestamp of the message.</param>
        /// <param name="chatRoomId">The ID of the chat room to add the message to.</param>
        public void AddMessageToChat(int userId, string username, string message, Image profileImage, DateTime time, int chatRoomId)
        {
            // Create a new ChatMessagePanel for the new message
            ChatMessagePanel newMessagePanel = new ChatMessagePanel(userId, username, message, profileImage, time);

            string nameOfActivePanel = $"ChatMessagesPanel{chatRoomId}";
            Control[] control = _chatAreaPanel.Controls.Find(nameOfActivePanel, true);
            // Calculate the Y-position where the new message will go (at the bottom)
            Panel messagesPanel = ((Panel)control[0]);
            int newYPosition = messagesPanel.Controls.Count > 1
                ? messagesPanel.Controls[messagesPanel.Controls.Count - 1].Bottom + 10  // Add some space between messages
                : 50;  // If no controls are in the panel yet, start from the top

            // Set the location of the new message panel
            newMessagePanel.Location = new Point(10, newYPosition);

            // Add the new message panel to the chatAreaPanel
            messagesPanel.Controls.Add(newMessagePanel);

            //// Optionally, scroll the panel to the latest message (if you are using a scrollable panel)
            messagesPanel.ScrollControlIntoView(newMessagePanel);
        }

        /// <summary>
        /// Adds a message from another user to the chat.
        /// If the user's profile image hasn't been loaded yet, requests it from the server.
        /// </summary>
        /// <param name="username">The username of the sender.</param>
        /// <param name="userId">The ID of the user sending the message.</param>
        /// <param name="message">The message content.</param>
        /// <param name="time">The timestamp of the message.</param>
        /// <param name="chatRoomId">The ID of the chat room to add the message to.</param>
        public void AddMessageToChatFromOtherUser(string username, int userId, string message, DateTime time, int chatRoomId)
        {
            UserManager userManager = _form.GetUserManager();

            if (!userManager.UsersImages.ContainsKey(userId))
            {
                ConnectionManager.GetInstance(null).ProcessFetchImageOfUser(userId);
                this.AddMessageToChat(userId, username, message, null, time, chatRoomId);
                return;
            }
            this.AddMessageToChat(userId, username, message, userManager.UsersImages[userId], time, chatRoomId);
        }

        /// <summary>
        /// Populates the message history for a chat room with messages from the server.
        /// </summary>
        /// <param name="messages">List of messages to display in the chat room.</param>
        public void SetMessagesHistoryOfAChatRoom(List<UserMessage> messages)
        {
            foreach (UserMessage message in messages)
            {
                this.AddMessageToChatFromOtherUser(message.Username, message.userId, message.Message, message.Time, message.ChatRoomId);
            }
            if (messages != null && messages.Count > 0)
            {
                string nameOfActivePanel = $"ChatMessagesPanel{messages[0].ChatRoomId}";
                Control[] control = _chatAreaPanel.Controls.Find(nameOfActivePanel, true);
                control[0].Tag = "1";
            }
        }

        /// <summary>
        /// Determines which chat messages panel is currently visible.
        /// </summary>
        /// <returns>The ID of the visible chat room panel, or -1 if none are visible.</returns>
        public int WhichChatMessagesPanelIsVisible()
        {
            if (_form.ChatMessagesPanel1.Visible)
            {
                return 1;
            }
            if (_form.ChatMessagesPanel2.Visible)
            {
                return 2;
            }
            if (_form.ChatMessagesPanel3.Visible)
            {
                return 3;
            }
            return -1;
        }

        /// <summary>
        /// Sends a message to the active chat room.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("you can't send an empty message!");
                return;
            }

            UserManager userManager = _form.GetUserManager();
            int chatRoomId = this.WhichChatMessagesPanelIsVisible();

            this.AddMessageToChat(
                userManager.GetCurrentUserId(),
                userManager.Username,
                message,
                userManager.UserProfilePicture,
                DateTime.Now,
                chatRoomId);

            ConnectionManager.GetInstance(null).ProcessSendMessage(message, chatRoomId);
            _form.messageInputTextBox.Text = "";
        }

        /// <summary>
        /// Switches to the specified text channel.
        /// </summary>
        /// <param name="channelId">The ID of the text channel to switch to (1-3).</param>
        public void SwitchToTextChannel(int channelId)
        {
            MediaChannelManager mediaManager = _form.GetMediaChannelManager();
            EmojiManager emojiManager = _form.GetEmojiManager();

            // If we're in a media channel
            if (mediaManager.IsInMediaChannel())
            {
                // Keep the connection but hide the video panel
                this.HideAllPanels();
            }
            else
            {
                // If we're just switching between text channels
                _form.ChatMessagesPanel1.Visible = channelId == 1;
                _form.ChatMessagesPanel2.Visible = channelId == 2;
                _form.ChatMessagesPanel3.Visible = channelId == 3;
            }

            // Show the appropriate text channel
            switch (channelId)
            {
                case 1:
                    _form.ChatMessagesPanel1.Visible = true;
                    break;
                case 2:
                    _form.ChatMessagesPanel2.Visible = true;
                    break;
                case 3:
                    _form.ChatMessagesPanel3.Visible = true;
                    break;
            }

            _form.messageInputTextBox.Visible = true;
            _form.sendMessageButton.Visible = true;

            // Update emoji panel visibility
            emojiManager.UpdateEmojiPanelVisibility();

            // Check if we need to load messages
            Panel targetPanel = null;
            switch (channelId)
            {
                case 1:
                    targetPanel = _form.ChatMessagesPanel1;
                    break;
                case 2:
                    targetPanel = _form.ChatMessagesPanel2;
                    break;
                case 3:
                    targetPanel = _form.ChatMessagesPanel3;
                    break;
            }

            if (targetPanel != null && ((string)targetPanel.Tag) == "0")
            {
                ConnectionManager.GetInstance(null).ProcessGetMessagesHistoryOfChatRoom(channelId);
            }
        }

        /// <summary>
        /// Hides all chat and video panels in the UI.
        /// </summary>
        public void HideAllPanels()
        {
            this.ChangeVisabilityOfAPanel("ChatMessagesPanel1", false);
            this.ChangeVisabilityOfAPanel("ChatMessagesPanel2", false);
            this.ChangeVisabilityOfAPanel("ChatMessagesPanel3", false);
            this.ChangeVisabilityOfAPanel("VideoPanel1", false);
            this.ChangeVisabilityOfAPanel("VideoPanel2", false);
            this.ChangeVisabilityOfAPanel("VideoPanel3", false);

            // Hide message input controls when in video panels
            _form.messageInputTextBox.Visible = false;
            _form.sendMessageButton.Visible = false;
            _form.mediaControlsPanel.Visible = false;
            _form.GetEmojiManager().HideEmojiPanel();
        }

        /// <summary>
        /// Changes the visibility of a panel by name.
        /// </summary>
        /// <param name="panelName">The name of the panel to change.</param>
        /// <param name="visible">True to make the panel visible, false to hide it.</param>
        private void ChangeVisabilityOfAPanel(string panelName, bool visible)
        {
            Control[] control = _chatAreaPanel.Controls.Find(panelName, true);
            if (control.Length > 0)
            {
                Panel panel = ((Panel)control[0]);
                panel.Visible = visible;
            }
        }

        /// <summary>
        /// Handles key down events in the message input text box.
        /// </summary>
        /// <param name="e">Key event arguments.</param>
        public void HandleMessageInputKeyDown(KeyEventArgs e)
        {
            // Check if Enter key is pressed
            if (e.KeyCode == Keys.Enter)
            {
                // If Shift is held down, insert a new line
                if (e.Shift)
                {
                    // Don't suppress the key so the newline is added
                    return;
                }
                else
                {
                    // Prevent the Enter character from being added to the text box
                    e.SuppressKeyPress = true;

                    // Call the send message function
                    SendMessage(_form.messageInputTextBox.Text);
                }
            }
        }
    }
}