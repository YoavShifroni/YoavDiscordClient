using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
#pragma warning disable CA1416

    public partial class DiscordApp : Form
    {

        public Image UserProfilePicture;

        public string Username;

        private int _currentUserId;


        public Dictionary<int, Image> UsersImages = new Dictionary<int, Image>();

        private Dictionary<int, Panel> mediaChannelPanels = new Dictionary<int, Panel>();

        private Dictionary<int, List<UserDetails>> usersInMediaChannels = new Dictionary<int, List<UserDetails>>();


        private List<int> _alreadyAskedForImage = new List<int>();

        public static VideoStreamConnection VideoStreamConnection;

        private int _videoRoomId;

        private bool _isGloballyMuted = false;

        private bool _isGloballyDeafened = false;

        private bool _isEmojiSelectionVisible = false;

        public DiscordApp()
        {
            InitializeComponent();

        }

        private void DiscordApp_Load(object sender, EventArgs e)
        {
            DiscordFormsHolder.ResizeFormBasedOnResolution(this, 2175f, 1248f);

            InitializeMediaChannelPanels();

            this.AddPicturesToTheWindow();

            this.AddEmojisToPanel();

            ConnectionManager.getInstance(null).ProcessFetchAllUsers();

            ConnectionManager.getInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);

        }

        private void DiscordApp_MouseDown(object sender, MouseEventArgs e)
        {
            if (this._isEmojiSelectionVisible)
            {
                // Convert the mouse position to client coordinates relative to the emoji selection panel
                Point mousePos = emojiSelectionPanel.PointToClient(Cursor.Position);

                // Check if the click is outside the selection panel
                if (!emojiSelectionPanel.ClientRectangle.Contains(mousePos))
                {
                    CloseEmojiPanel();
                }
            }
        }

        public void SetUsernameAndProfilePicture(byte[] profilePicture, string username, int userId)
        {
            this.UserProfilePicture = this.ByteArrayToImage(profilePicture);
            this.Username = username;
            this._currentUserId = userId;
            this.userProfilePicturePictureBox.Image = this.UserProfilePicture;
            this.usernameLabel.Text = username;
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private int WhichChatMessagesPanelIsVisible()
        {
            if (this.ChatMessagesPanel1.Visible)
            {
                return 1;
            }
            if (this.ChatMessagesPanel2.Visible)
            {
                return 2;
            }
            if (this.ChatMessagesPanel3.Visible)
            {
                return 3;
            }
            return -1;
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            if (this.messageInputTextBox.Text.Length == 0)
            {
                MessageBox.Show("you can't send an empty message!");
                return;
            }
            int chatRoomId = this.WhichChatMessagesPanelIsVisible();
            this.AddMessageToChat(this._currentUserId, this.Username, this.messageInputTextBox.Text, this.UserProfilePicture, DateTime.Now, chatRoomId);
            ConnectionManager.getInstance(null).ProcessSendMessage(this.messageInputTextBox.Text, chatRoomId);
            this.messageInputTextBox.Text = "";
        }

        public void AddMessageToChat(int userId, string username, string message, Image profileImage, DateTime time, int chatRoomId)
        {
            // Create a new ChatMessagePanel for the new message
            ChatMessagePanel newMessagePanel = new ChatMessagePanel(userId, username, message, profileImage, time);

            string nameOfActivePanel = $"ChatMessagesPanel{chatRoomId}";
            Control[] control = chatAreaPanel.Controls.Find(nameOfActivePanel, true);
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


        public void AddMessageToChatFromOtherUser(string username, int userId, string message, DateTime time, int chatRoomId)
        {
            if (!this.UsersImages.ContainsKey(userId))
            {
                if (!this._alreadyAskedForImage.Contains(userId))
                {
                    ConnectionManager.getInstance(null).ProcessFetchImageOfUser(userId);
                    this._alreadyAskedForImage.Add(userId);
                }
                this.AddMessageToChat(userId, username, message, null, time, chatRoomId);
                return;

            }
            this.AddMessageToChat(userId, username, message, this.UsersImages[userId], time, chatRoomId);
        }


        private Image ResizeImage(Image imgToResize, int width, int height)
        {
            return new Bitmap(imgToResize, new Size(width, height));
        }

        private void AddPicturesToTheWindow()
        {
            Image originalSettingsLogoImage = Properties.Resources.settingsLogo;
            Image originalDeafenLogoImage = Properties.Resources.deafenLogo;
            Image originalMuteLogoImage = Properties.Resources.muteLogo;
            Image originalVideoMuteLogoImage = Properties.Resources.videoMuteLogo;
            Image originalDisconnectMediaChannelImage = Properties.Resources.disconnectMediaChannelLogo;

            this.settingsButton.Image = this.ResizeImage(originalSettingsLogoImage, this.settingsButton.Width, this.settingsButton.Height);
            this.deafenButton.Image = this.ResizeImage(originalDeafenLogoImage, this.deafenButton.Width, this.deafenButton.Height);
            this.globalMuteButton.Image = this.ResizeImage(originalMuteLogoImage, this.globalMuteButton.Width, this.globalMuteButton.Height);
            // Add images for new media control buttons
            this.mediaChannelMuteButton.Image = this.ResizeImage(originalMuteLogoImage, this.mediaChannelMuteButton.Width, this.mediaChannelMuteButton.Height);
            this.mediaChannelVideoMuteButton.Image = this.ResizeImage(originalVideoMuteLogoImage, this.mediaChannelVideoMuteButton.Width, this.mediaChannelVideoMuteButton.Height);
            this.mediaChannelDisconnectButton.Image = this.ResizeImage(originalDisconnectMediaChannelImage, this.mediaChannelDisconnectButton.Width,
                this.mediaChannelDisconnectButton.Height);
        }

        public void AddNewUserImageAndShowItsMessage(int userId, byte[] profilePicture, string username, string messageThatTheUserSent,
            DateTime timeThatTheMessageWasSent, int chatRoomId)
        {

            this.UsersImages[userId] = this.ByteArrayToImage(profilePicture);
            this._alreadyAskedForImage.Remove(userId);
            this.AddMessageToChatFromOtherUser(username, userId, messageThatTheUserSent, timeThatTheMessageWasSent, chatRoomId);
        }

        private void textChanel1Button_Click(object sender, EventArgs e)
        {
            // If we're in a media channel
            if (VideoStreamConnection != null)
            {
                // Keep the connection but hide the video panel
                this.HideAllPanels();
            }
            else
            {
                // If we're just switching between text channels
                this.ChatMessagesPanel2.Visible = false;
                this.ChatMessagesPanel3.Visible = false;
            }
            this.ChatMessagesPanel1.Visible = true;
            messageInputTextBox.Visible = true;
            sendMessageButton.Visible = true;
            this.UpdateEmojiPanelVisibility();
            if (((string)this.ChatMessagesPanel1.Tag) == "0")
            {
                ConnectionManager.getInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);
            }

        }

        private void textChanel2Button_Click(object sender, EventArgs e)
        {
            // If we're in a media channel
            if (VideoStreamConnection != null)
            {
                // Keep the connection but hide the video panel
                this.HideAllPanels();
            }
            else
            {
                // If we're just switching between text channels
                this.ChatMessagesPanel1.Visible = false;
                this.ChatMessagesPanel3.Visible = false;
            }
            this.ChatMessagesPanel2.Visible = true;
            this.messageInputTextBox.Visible = true;
            this.sendMessageButton.Visible = true;
            this.UpdateEmojiPanelVisibility();
            if (((string)this.ChatMessagesPanel2.Tag) == "0")
            {
                ConnectionManager.getInstance(null).ProcessGetMessagesHistoryOfChatRoom(2);
            }
        }

        private void textChanel3Button_Click(object sender, EventArgs e)
        {
            // If we're in a media channel
            if (VideoStreamConnection != null)
            {
                // Keep the connection but hide the video panel
                this.HideAllPanels();
            }
            else
            {
                // If we're just switching between text channels
                this.ChatMessagesPanel1.Visible = false;
                this.ChatMessagesPanel2.Visible = false;
            }
            this.ChatMessagesPanel3.Visible = true;
            messageInputTextBox.Visible = true;
            sendMessageButton.Visible = true;
            this.UpdateEmojiPanelVisibility();
            if (((string)this.ChatMessagesPanel3.Tag) == "0")
            {
                ConnectionManager.getInstance(null).ProcessGetMessagesHistoryOfChatRoom(3);
            }
        }

        public void SetMessagesHistoryOfAChatRoom(List<UserMessage> messages)
        {
            foreach (UserMessage message in messages)
            {
                this.AddMessageToChatFromOtherUser(message.Username, message.userId, message.Message, message.Time, message.ChatRoomId);
            }
            if (messages != null && messages.Count > 0)
            {
                string nameOfActivePanel = $"ChatMessagesPanel{messages[0].ChatRoomId}";
                Control[] control = chatAreaPanel.Controls.Find(nameOfActivePanel, true);
                control[0].Tag = "1";
            }

        }

        private void HideAllPanels()
        {
            this.ChangeVisabilityOfAPanel("ChatMessagesPanel1", false);
            this.ChangeVisabilityOfAPanel("ChatMessagesPanel2", false);
            this.ChangeVisabilityOfAPanel("ChatMessagesPanel3", false);
            this.ChangeVisabilityOfAPanel("VideoPanel1", false);
            this.ChangeVisabilityOfAPanel("VideoPanel2", false);
            this.ChangeVisabilityOfAPanel("VideoPanel3", false);
            // Hide message input controls when in video panels
            this.messageInputTextBox.Visible = false;
            this.sendMessageButton.Visible = false;
            this.mediaControlsPanel.Visible = false;
            this.emojiPanel.Visible = false;
        }

        private void ChangeVisabilityOfAPanel(string panelName, bool visible)
        {
            Control[] control = chatAreaPanel.Controls.Find(panelName, true);
            Panel ChatMessagesPanel1 = ((Panel)control[0]);
            ChatMessagesPanel1.Visible = visible;
        }

        private async void voiceChannel1Button_Click(object sender, EventArgs e)
        {
            // If we're already connected to this channel, just show the video panel
            if (VideoStreamConnection != null && this._videoRoomId == 1)
            {
                this.HideAllPanels();
                this.VideoPanel1.Visible = true;

                // Make sure to show the media controls panel
                this.mediaControlsPanel.Visible = true;
                mediaControlsPanel.Location = new Point(
                    (chatAreaPanel.Width - mediaControlsPanel.Width) / 2,
                    chatAreaPanel.Height - mediaControlsPanel.Height - 30); // 30px from bottom
                this.mediaControlsPanel.BringToFront();
                return;
            }

            // If we're connected to a different channel, disconnect first
            if (VideoStreamConnection != null)
            {
                RemoveUserFromMediaChannel(this._videoRoomId, this._currentUserId);
                ConnectionManager.getInstance(null).ProcessDisconnectFromMediaRoom(this._videoRoomId);
                VideoStreamConnection.Dispose();
                VideoStreamConnection = null;
                await Task.Delay(2000);

                // Reset buttons to normal state when switching channels
                this.mediaChannelMuteButton.BackColor = Color.FromArgb(64, 68, 75); // Original color
                this.mediaChannelVideoMuteButton.BackColor = Color.FromArgb(64, 68, 75); // Original color
            }

            this._videoRoomId = 1;
            // Add the current user to the new channel
            AddUserToMediaChannel(1, new UserDetails(this._currentUserId, this.Username, this.ImageToByteArray(this.UserProfilePicture)));
            this.HideAllPanels();
            this.VideoPanel1.Visible = true;

            // Position the media controls panel with some distance from the rectangles
            mediaControlsPanel.Visible = true;

            // Center the panel horizontally at the bottom of the chat area
            mediaControlsPanel.Location = new Point(
                (chatAreaPanel.Width - mediaControlsPanel.Width) / 2,
                chatAreaPanel.Height - mediaControlsPanel.Height - 30); // 30px from bottom
            this.mediaControlsPanel.BringToFront();
            VideoStreamConnection = new VideoStreamConnection(this.VideoPanel1);
            await VideoStreamConnection.Initialize();

            // Apply global mute state to the new connection
            if (this._isGloballyMuted)
            {
                VideoStreamConnection.SetGlobalMuteState(true);
            }

            // Apply global deafen state to the new connection
            if (this._isGloballyDeafened)
            {
                VideoStreamConnection.SetGlobalDeafenState(true);
            }

            ConnectionManager.getInstance(null).ProcessConnectToMediaRoom(1);
        }

        private async void voiceChannel2Button_Click(object sender, EventArgs e)
        {
            // If we're already connected to this channel, just show the video panel
            if (VideoStreamConnection != null && this._videoRoomId == 2)
            {
                this.HideAllPanels();
                this.VideoPanel2.Visible = true;

                // Make sure to show the media controls panel
                this.mediaControlsPanel.Visible = true;
                mediaControlsPanel.Location = new Point(
                    (chatAreaPanel.Width - mediaControlsPanel.Width) / 2,
                    chatAreaPanel.Height - mediaControlsPanel.Height - 30); // 30px from bottom
                this.mediaControlsPanel.BringToFront();
                return;
            }

            // If we're connected to a different channel, disconnect first
            if (VideoStreamConnection != null)
            {
                RemoveUserFromMediaChannel(this._videoRoomId, this._currentUserId);
                ConnectionManager.getInstance(null).ProcessDisconnectFromMediaRoom(this._videoRoomId);
                VideoStreamConnection.Dispose();
                VideoStreamConnection = null;
                await Task.Delay(2000);

                // Reset buttons to normal state when switching channels
                this.mediaChannelMuteButton.BackColor = Color.FromArgb(64, 68, 75); // Original color
                this.mediaChannelVideoMuteButton.BackColor = Color.FromArgb(64, 68, 75); // Original color
            }
            this._videoRoomId = 2;
            AddUserToMediaChannel(2, new UserDetails(this._currentUserId, this.Username, this.ImageToByteArray(this.UserProfilePicture)));
            this.HideAllPanels();
            this.VideoPanel2.Visible = true;

            // Position the media controls panel with some distance from the rectangles
            mediaControlsPanel.Visible = true;

            // Center the panel horizontally at the bottom of the chat area
            mediaControlsPanel.Location = new Point(
                (chatAreaPanel.Width - mediaControlsPanel.Width) / 2,
                chatAreaPanel.Height - mediaControlsPanel.Height - 30); // 30px from bottom
            this.mediaControlsPanel.BringToFront();
            VideoStreamConnection = new VideoStreamConnection(this.VideoPanel2);
            await VideoStreamConnection.Initialize();

            // Apply global mute state to the new connection
            if (this._isGloballyMuted)
            {
                VideoStreamConnection.SetGlobalMuteState(true);
            }

            // Apply global deafen state to the new connection
            if (this._isGloballyDeafened)
            {
                VideoStreamConnection.SetGlobalDeafenState(true);
            }

            ConnectionManager.getInstance(null).ProcessConnectToMediaRoom(2);
        }

        private async void voiceChannel3Button_Click(object sender, EventArgs e)
        {
            // If we're already connected to this channel, just show the video panel
            if (VideoStreamConnection != null && this._videoRoomId == 3)
            {
                this.HideAllPanels();
                this.VideoPanel3.Visible = true;

                // Make sure to show the media controls panel
                this.mediaControlsPanel.Visible = true;
                mediaControlsPanel.Location = new Point(
                    (chatAreaPanel.Width - mediaControlsPanel.Width) / 2,
                    chatAreaPanel.Height - mediaControlsPanel.Height - 30); // 30px from bottom
                this.mediaControlsPanel.BringToFront();
                return;
            }

            // If we're connected to a different channel, disconnect first
            if (VideoStreamConnection != null)
            {
                RemoveUserFromMediaChannel(this._videoRoomId, this._currentUserId);
                ConnectionManager.getInstance(null).ProcessDisconnectFromMediaRoom(this._videoRoomId);
                VideoStreamConnection.Dispose();
                VideoStreamConnection = null;
                await Task.Delay(2000);

                // Reset buttons to normal state when switching channels
                this.mediaChannelMuteButton.BackColor = Color.FromArgb(64, 68, 75); // Original color
                this.mediaChannelVideoMuteButton.BackColor = Color.FromArgb(64, 68, 75); // Original color
            }
            this._videoRoomId = 3;
            AddUserToMediaChannel(3, new UserDetails(this._currentUserId, this.Username, this.ImageToByteArray(this.UserProfilePicture)));
            this.HideAllPanels();
            this.VideoPanel3.Visible = true;

            // Position the media controls panel with some distance from the rectangles
            mediaControlsPanel.Visible = true;

            // Center the panel horizontally at the bottom of the chat area
            mediaControlsPanel.Location = new Point(
                (chatAreaPanel.Width - mediaControlsPanel.Width) / 2,
                chatAreaPanel.Height - mediaControlsPanel.Height - 30); // 30px from bottom
            this.mediaControlsPanel.BringToFront();
            VideoStreamConnection = new VideoStreamConnection(this.VideoPanel3);
            await VideoStreamConnection.Initialize();

            // Apply global mute state to the new connection
            if (this._isGloballyMuted)
            {
                VideoStreamConnection.SetGlobalMuteState(true);
            }

            // Apply global deafen state to the new connection
            if (this._isGloballyDeafened)
            {
                VideoStreamConnection.SetGlobalDeafenState(true);
            }

            ConnectionManager.getInstance(null).ProcessConnectToMediaRoom(3);
        }

        public void AddNewParticipantDisplay(Panel panel, PictureBox pictureBox)
        {
            panel.Controls.Add(pictureBox);
        }

        public void ShowAllUsersDetails(List<UserDetails> details)
        {
            // Clear existing controls in rightSidePanel except labels
            rightSidePanel.Controls.Clear();
            rightSidePanel.Controls.Add(onlineUsersLabel);
            rightSidePanel.Controls.Add(offlineUsersLabel);

            // Separate online and offline users
            var onlineUsers = details.Where(d => d.Status).ToList();
            var offlineUsers = details.Where(d => !d.Status).ToList();

            // Update online label with count
            onlineUsersLabel.Text = $"Online - {onlineUsers.Count}";
            onlineUsersLabel.Location = new Point(6, 56); // Original position

            // Starting Y position for online users
            int currentY = onlineUsersLabel.Bottom + 20;

            // Add online users
            foreach (var user in onlineUsers)
            {
                if (!UsersImages.ContainsKey(user.UserId))
                {
                    UsersImages[user.UserId] = ByteArrayToImage(user.Picture);
                }
                if (user.MediaChannelId != -1)
                {
                    this.AddUserToMediaChannel(user.MediaChannelId, user);
                }
                AddUserEntry(user, currentY, true);
                currentY += 60;
            }

            // Position offline label closer to the last online user
            offlineUsersLabel.Text = $"Offline - {offlineUsers.Count}";
            offlineUsersLabel.Location = new Point(6, currentY + 20); // Reduced gap to 20 pixels

            // Start offline users below the label
            currentY = offlineUsersLabel.Bottom + 20;

            // Add offline users
            foreach (var user in offlineUsers)
            {
                if (!UsersImages.ContainsKey(user.UserId))
                {
                    UsersImages[user.UserId] = ByteArrayToImage(user.Picture);
                }
                AddUserEntry(user, currentY, false);
                currentY += 60;
            }
        }

        private void AddUserEntry(UserDetails user, int yPosition, bool isOnline)
        {
            // Create panel for user entry
            Panel userPanel = new Panel
            {
                Width = rightSidePanel.Width - 20,
                Height = 50,
                Location = new Point(10, yPosition)
            };

            // Create circle picture box for user avatar
            CirclePictureBox avatarPicture = new CirclePictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(5, 5),
                Image = UsersImages[user.UserId],
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Create label for username
            Label usernameLabel = new Label
            {
                Text = user.Username,
                Location = new Point(55, 15),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font(
                    "Arial",
                    12,
                    isOnline ? FontStyle.Regular : FontStyle.Italic,
                    GraphicsUnit.Point
                )
            };

            // If user is offline, make the text color slightly darker
            if (!isOnline)
            {
                usernameLabel.ForeColor = Color.FromArgb(180, 180, 180);
            }

            // Add controls to user panel
            userPanel.Controls.Add(avatarPicture);
            userPanel.Controls.Add(usernameLabel);

            // Add user panel to right side panel
            rightSidePanel.Controls.Add(userPanel);
        }

        public void InitializeMediaChannelPanels()
        {
            // Create panels for each media channel
            for (int i = 1; i <= 3; i++)
            {
                Panel channelPanel = new Panel
                {
                    Width = voiceChannel1Button.Width,
                    BackColor = Color.FromArgb(54, 57, 63), // Same as leftSidePanel
                    AutoSize = true,
                    Location = new Point(voiceChannel1Button.Left, GetMediaChannelButton(i).Bottom + 5)
                };

                leftSidePanel.Controls.Add(channelPanel);
                mediaChannelPanels[i] = channelPanel;
                usersInMediaChannels[i] = new List<UserDetails>();
            }
        }

        private Button GetMediaChannelButton(int channelNumber)
        {
            switch (channelNumber)
            {
                case 1:
                    return voiceChannel1Button;
                case 2:
                    return voiceChannel2Button;
                case 3:
                    return voiceChannel3Button;
                default:
                    throw new ArgumentException("Invalid channel number");
            }
        }

        public void UpdateMediaChannelUsers(int channelId, List<UserDetails> users)
        {
            if (!mediaChannelPanels.ContainsKey(channelId))
                return;

            Panel channelPanel = mediaChannelPanels[channelId];
            channelPanel.Controls.Clear();
            usersInMediaChannels[channelId] = users;

            int currentY = 5;

            foreach (var user in users)
            {
                // Create user entry panel
                Panel userPanel = new Panel
                {
                    Width = channelPanel.Width,
                    Height = 40,
                    Location = new Point(5, currentY)
                };

                // Create circle picture box for user avatar
                CirclePictureBox avatarPicture = new CirclePictureBox
                {
                    Size = new Size(30, 30),
                    Location = new Point(5, 5),
                    Image = UsersImages[user.UserId],
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                // Create label for username
                Label usernameLabel = new Label
                {
                    Text = user.Username,
                    Location = new Point(45, 10),
                    AutoSize = true,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Regular)
                };

                // Add controls to user panel
                userPanel.Controls.Add(avatarPicture);
                userPanel.Controls.Add(usernameLabel);

                // Add user panel to channel panel
                channelPanel.Controls.Add(userPanel);

                currentY += 45;
            }

            // Update panel height
            channelPanel.Height = users.Count > 0 ? currentY : 0;

            // Adjust positions of subsequent channel buttons and panels
            UpdateMediaChannelPositions(channelId);
        }

        private void UpdateMediaChannelPositions(int currentChannelId)
        {
            // First, update the current channel's panel position
            Panel currentPanel = mediaChannelPanels[currentChannelId];
            Button currentButton = GetMediaChannelButton(currentChannelId);
            currentPanel.Location = new Point(currentPanel.Left, currentButton.Bottom + 5);

            // Update all channel positions sequentially to maintain consistent spacing
            for (int i = 1; i <= 3; i++)
            {
                Button button = GetMediaChannelButton(i);
                Panel panel = mediaChannelPanels[i];

                if (i == 1)
                {
                    // First button position stays the same
                    panel.Location = new Point(panel.Left, button.Bottom + 5);
                }
                else
                {
                    // Position each subsequent button after the previous panel
                    Button prevButton = GetMediaChannelButton(i - 1);
                    Panel prevPanel = mediaChannelPanels[i - 1];

                    // Calculate the position based on previous panel content
                    int newButtonY = prevPanel.Controls.Count > 0
                        ? prevPanel.Bottom + 10  // If panel has users, position after them
                        : prevButton.Bottom + 45; // If panel is empty, maintain default spacing

                    button.Location = new Point(button.Left, newButtonY);
                    panel.Location = new Point(panel.Left, button.Bottom + 5);
                }
            }
        }

        // Call this when a user joins a media channel
        public void AddUserToMediaChannel(int channelId, UserDetails user)
        {
            if (!usersInMediaChannels.ContainsKey(channelId))
                return;

            var users = usersInMediaChannels[channelId];
            if (!users.Any(u => u.UserId == user.UserId))
            {
                users.Add(user);
                UpdateMediaChannelUsers(channelId, users);
            }
        }

        // Call this when a user leaves a media channel
        public void RemoveUserFromMediaChannel(int channelId, int userId)
        {
            if (!usersInMediaChannels.ContainsKey(channelId))
                return;

            var users = usersInMediaChannels[channelId];
            users.RemoveAll(u => u.UserId == userId);
            UpdateMediaChannelUsers(channelId, users);

            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.DisconnectFromParticipant(userId);
            }
        }


        private byte[] ImageToByteArray(Image image)
        {
            try
            {
                // Create a copy of the image to avoid disposal issues
                using (Bitmap bmp = new Bitmap(image))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting image: {ex.Message}");
                // Return a default/placeholder image bytes if conversion fails
                using (Bitmap defaultBmp = new Bitmap(40, 40))
                {
                    using (Graphics g = Graphics.FromImage(defaultBmp))
                    {
                        g.Clear(Color.Gray); // Create a gray placeholder
                        using (MemoryStream ms = new MemoryStream())
                        {
                            defaultBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            return ms.ToArray();
                        }
                    }
                }
            }
        }


        private void mediaChannelMuteButton_Click(object sender, EventArgs e)
        {
            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.ToggleAudioMute();
                // Update button appearance to show muted state
                mediaChannelMuteButton.BackColor = mediaChannelMuteButton.BackColor == Color.Red ?
                    Color.FromArgb(64, 68, 75) : Color.Red;
            }
        }

        private void mediaChannelVideoMuteButton_Click(object sender, EventArgs e)
        {
            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.ToggleVideoMute();
                // Update button appearance to show muted state
                mediaChannelVideoMuteButton.BackColor = mediaChannelVideoMuteButton.BackColor == Color.Red ?
                    Color.FromArgb(64, 68, 75) : Color.Red;
            }
        }

        // 3. Add this event handler method to the DiscordApp class
        private async void mediaChannelDisconnectButton_Click(object sender, EventArgs e)
        {
            if (VideoStreamConnection != null)
            {
                // Disconnect user from the current media channel
                RemoveUserFromMediaChannel(this._videoRoomId, this._currentUserId);
                ConnectionManager.getInstance(null).ProcessDisconnectFromMediaRoom(this._videoRoomId);

                // Dispose video connection
                VideoStreamConnection.Dispose();
                VideoStreamConnection = null;

                // Small delay to ensure proper cleanup
                await Task.Delay(1000);

                // Reset media control buttons to original state
                this.mediaChannelMuteButton.BackColor = Color.FromArgb(64, 68, 75);
                this.mediaChannelVideoMuteButton.BackColor = Color.FromArgb(64, 68, 75);

                // Switch to text channel 1
                this.HideAllPanels();
                this.ChatMessagesPanel1.Visible = true;
                this.messageInputTextBox.Visible = true;
                this.sendMessageButton.Visible = true;

                // Make sure to update emoji panel visibility
                this.UpdateEmojiPanelVisibility();

                // If chat history isn't loaded yet, load it
                if (((string)this.ChatMessagesPanel1.Tag) == "0")
                {
                    ConnectionManager.getInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);
                }
            }
        }


        public List<Control> FindControlsByTag(Panel panel, string searchText)
        {
            List<Control> matchingControls = new List<Control>();

            foreach (Control control in panel.Controls)
            {
                // Check if the control has a non-null tag that contains the search text
                if (control.Tag != null && control.Tag.ToString().Equals(searchText))
                {
                    matchingControls.Add(control);
                }

                // Recursively search nested containers (like GroupBox, Panel, etc.)
                if (control.HasChildren)
                {
                    matchingControls.AddRange(FindControlsByTag((Panel)control, searchText));
                }
            }

            return matchingControls;
        }

        public void UpdateUserImage(int userId, byte[] profilePicture)
        {
            this.UsersImages[userId] = this.ByteArrayToImage(profilePicture);
            List<Control> controls = this.FindControlsByTag(this.chatAreaPanel, $"UserImage_{userId}");
            Image imageOfUser = this.ByteArrayToImage(profilePicture);
            foreach (Control control in controls)
            {
                ((CirclePictureBox)control).Image = imageOfUser;
            }
        }

        private void globalMuteButton_Click(object sender, EventArgs e)
        {
            this._isGloballyMuted = !this._isGloballyMuted;
            this.globalMuteButton.BackColor = this._isGloballyMuted ? Color.Red : Color.FromArgb(64, 68, 75);

            // Apply mute setting to current connection if it exists
            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.SetGlobalMuteState(this._isGloballyMuted);
            }

        }

        private void deafenButton_Click(object sender, EventArgs e)
        {
            this._isGloballyDeafened = !this._isGloballyDeafened;
            this.deafenButton.BackColor = this._isGloballyDeafened ? Color.Red : Color.FromArgb(64, 68, 75);
            // Apply deafen setting to current connection if it exists
            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.SetGlobalDeafenState(this._isGloballyDeafened);
            }

        }

        private void messageInputTextBox_KeyDown(object sender, KeyEventArgs e)
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

                    // Call your existing send message function
                    sendMessageButton_Click(sender, e);
                }
            }
        }

        private void emojiButton_Click(object sender, EventArgs e)
        {
            // Update emoji selection panel position relative to the emoji button
            // Position it above the emoji button
            this.emojiSelectionPanel.Location = new Point(
                this.emojiButton.Parent.Left + this.emojiButton.Left - this.emojiSelectionPanel.Width + this.emojiButton.Width,
                this.emojiButton.Parent.Top + this.emojiButton.Top - this.emojiSelectionPanel.Height - 5);

            // Toggle emoji selection panel visibility
            this._isEmojiSelectionVisible = !this._isEmojiSelectionVisible;
            this.emojiSelectionPanel.Visible = this._isEmojiSelectionVisible;

            // Bring selection panel to front if visible
            if (this._isEmojiSelectionVisible)
            {
                this.emojiSelectionPanel.BringToFront();
            }
        }

        private void AddEmojisToPanel()
        {
            this.messageInputTextBox.Width -= 40;

            // Set initial position of emoji panel
            this.emojiPanel.Location = new Point(
                this.messageInputTextBox.Right,  // Place it right at the edge
                this.messageInputTextBox.Top);


            this.emojiPanel.Visible = true;

            // Initialize emoji selection panel position
            this.emojiSelectionPanel.Location = new Point(
                this.emojiButton.Left - this.emojiSelectionPanel.Width + this.emojiButton.Width,
                this.emojiButton.Top - this.emojiSelectionPanel.Height - 5);

            // Common emojis array
            string[] emojis = new string[]
            {
                "😀", "😃", "😄", "😁", "😆", "😅", "🤣", "😂", "🙂", "🙃",
                "😉", "😊", "😇", "😍", "🥰", "😘", "😗", "😚", "😙", "😋",
                "😛", "😜", "😝", "🤑", "🤗", "🤔", "🤐", "😐", "😑", "😶",
                "😏", "😒", "🙄", "😬", "🤥", "😌", "😔", "😪", "🤤", "😴",
                "😷", "🤒", "🤕", "🤢", "🤮", "🤧", "🥵", "🥶", "🥴", "😵",
                "👋", "🤚", "✋", "🖖", "👌", "✌️", "🤞", "🤟", "🤘", "🤙"
            };

            int buttonSize = 30;
            int columns = 6;
            int margin = 3;
            int xPos = margin;
            int yPos = margin;
            int column = 0;

            foreach (string emoji in emojis)
            {
                Button emojiBtn = new Button
                {
                    Text = emoji,
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(xPos, yPos),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = System.Drawing.Color.FromArgb(64, 68, 75),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI Emoji", 12)
                };

                emojiBtn.FlatAppearance.BorderSize = 0;
                emojiBtn.Click += (sender, e) => EmojiSelected(emoji);
                this.emojiSelectionPanel.Controls.Add(emojiBtn);

                column++;
                if (column >= columns)
                {
                    column = 0;
                    xPos = margin;
                    yPos += buttonSize + margin;
                }
                else
                {
                    xPos += buttonSize + margin;
                }
            }

        }

        private void EmojiSelected(string emoji)
        {
            // Insert emoji at current cursor position
            int cursorPosition = messageInputTextBox.SelectionStart;
            messageInputTextBox.Text = messageInputTextBox.Text.Insert(cursorPosition, emoji);

            // Update cursor position after insertion
            messageInputTextBox.SelectionStart = cursorPosition + emoji.Length;

            // Hide emoji selection panel after selection
            this.CloseEmojiPanel();

            // Focus back on the text input box
            messageInputTextBox.Focus();
        }

        private void UpdateEmojiPanelVisibility()
        {

            // IMPORTANT: The emoji panel should be visible only when the text input is visible
            this.emojiPanel.Visible = this.messageInputTextBox.Visible;

            // Ensure the emoji panel is in the correct position
            if (this.messageInputTextBox.Visible)
            {

                this.emojiPanel.Location = new Point(
                    this.messageInputTextBox.Right,  // Always at the right edge
                    this.messageInputTextBox.Top);
                this.emojiPanel.BringToFront();  // Ensure it's on top of other controls

            }

            // Hide emoji selection panel if we're switching channels
            if (this._isEmojiSelectionVisible)
            {
                this.CloseEmojiPanel();
            }
        }

        // Helper method to close the emoji panel
        private void CloseEmojiPanel()
        {
            this._isEmojiSelectionVisible = false;
            this.emojiSelectionPanel.Visible = false;
        }
    }
}