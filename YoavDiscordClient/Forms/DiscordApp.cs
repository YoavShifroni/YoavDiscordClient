using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoavDiscordClient.Style;

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

        public Dictionary<int, List<UserDetails>> UsersInMediaChannels = new Dictionary<int, List<UserDetails>>();


        private List<int> _alreadyAskedForImage = new List<int>();

        public static VideoStreamConnection VideoStreamConnection;

        private int _videoRoomId;
        private bool _isEmojiSelectionVisible = false;

        private bool _isGloballyMuted = false;

        private bool _isGloballyDeafened = false;


        private bool _isMutedByHigherRole = false;

        private bool _isVideoMutedByHigherRole = false;
        private bool _isDeafenedByHigherRole = false;



        private int role;

        private ContextMenuStrip userContextMenu;

        public DiscordApp()
        {
            InitializeComponent();

        }

        private void DiscordApp_Load(object sender, EventArgs e)
        {

            DiscordFormsHolder.ResizeFormBasedOnResolution(this, 2175f, 1248f);

            InitializeMediaChannelPanels();

            this.InitializeUserContextMenu();

            this.AddPicturesToTheWindow();

            this.AddEmojisToPanel();

            ConnectionManager.GetInstance(null).ProcessFetchAllUsers();

            ConnectionManager.GetInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);

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


        public void SetUsernameAndProfilePicture(byte[] profilePicture, string username, int userId, int role)
        {
            this.UserProfilePicture = this.ByteArrayToImage(profilePicture);
            this.Username = username;
            this._currentUserId = userId;
            this.userProfilePicturePictureBox.Image = this.UserProfilePicture;
            this.usernameLabel.Text = username;
            this.role = role;
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
            ConnectionManager.GetInstance(null).ProcessSendMessage(this.messageInputTextBox.Text, chatRoomId);
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
                    ConnectionManager.GetInstance(null).ProcessFetchImageOfUser(userId);
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
                ConnectionManager.GetInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);
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
                ConnectionManager.GetInstance(null).ProcessGetMessagesHistoryOfChatRoom(2);
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
                ConnectionManager.GetInstance(null).ProcessGetMessagesHistoryOfChatRoom(3);
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

        private async Task ClickHandlerForMediaRoom(int mediaRoomId)
        {
            // If we're already connected to this channel, just show the video panel
            if (VideoStreamConnection != null && this._videoRoomId == mediaRoomId)
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
                ConnectionManager.GetInstance(null).ProcessDisconnectFromMediaRoom(this._videoRoomId);
                await this.CleanupVideoStreamConnection();

                
            }

            this._videoRoomId = mediaRoomId;
            // Add the current user to the new channel
            AddUserToMediaChannel(this._videoRoomId, new UserDetails(this._currentUserId, this.Username, this.ImageToByteArray(this.UserProfilePicture), this.role));
            this.HideAllPanels();
            string panelName = "VideoPanel" + mediaRoomId.ToString();
            Panel videoPanel = ((Panel)this.Controls.Find(panelName, true)[0]);
            videoPanel.Visible = true;

            // Position the media controls panel with some distance from the rectangles

            // Center the panel horizontally at the bottom of the chat area
            mediaControlsPanel.Location = new Point(
                (chatAreaPanel.Width - mediaControlsPanel.Width) / 2,
                chatAreaPanel.Height - mediaControlsPanel.Height - 30); // 30px from bottom
            this.mediaControlsPanel.BringToFront();

            // Reset buttons to normal state when switching channels
            if (this._isMutedByHigherRole)
            {
                this.mediaChannelMuteButton.Enabled = false;
                this.mediaChannelMuteButton.BackColor = ThemeManager.GetColor("MutedColor");
                this.UpdateUserMuteVisualIndicator(this._currentUserId, true);
            }
            else
            {
                this.mediaChannelMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground"); // Original color
            }
            if (this._isVideoMutedByHigherRole)
            {
                this.mediaChannelVideoMuteButton.Enabled = false;
                this.mediaChannelVideoMuteButton.BackColor = ThemeManager.GetColor("MutedColor");
                this.UpdateUserVideoMuteVisualIndicator(this._currentUserId, true);

            }
            else
            {
                this.mediaChannelVideoMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground"); // Original color
            }
            if (this._isDeafenedByHigherRole)
            {
                this.UpdateUserDeafenVisualIndicator(this._currentUserId, true);
            }

            VideoStreamConnection = new VideoStreamConnection(videoPanel);
            await VideoStreamConnection.Initialize();


            mediaControlsPanel.Visible = true;

            if (this._isMutedByHigherRole)
            {
                VideoStreamConnection.SetMutedByHigherRoleState(true);
            }

            // Apply global mute state to the new connection
            if (this._isGloballyMuted)
            {
                VideoStreamConnection.SetGlobalMuteState(true);
            }


            // Apply global deafen state to the new connection
            if (this._isGloballyDeafened || this._isDeafenedByHigherRole)
            {
                VideoStreamConnection.SetGlobalDeafenState(true);
            }

            // Apply video mute state to the new connection
            if (this._isVideoMutedByHigherRole)
            {
                // Ensure the video is muted in the new connection
                await Task.Delay(500); // Small delay to ensure udp connections are ready
                VideoStreamConnection.ToggleVideoMute(); // Mute the video if it's not already muted
                mediaChannelVideoMuteButton.BackColor = ThemeManager.GetColor("MutedColor");
                mediaChannelVideoMuteButton.Enabled = false;
            }
            
            ConnectionManager.GetInstance(null).ProcessConnectToMediaRoom(this._videoRoomId);
        }

        private async void voiceChannel1Button_Click(object sender, EventArgs e)
        {
            await this.ClickHandlerForMediaRoom(1);
        }

        private async void voiceChannel2Button_Click(object sender, EventArgs e)
        {
            await this.ClickHandlerForMediaRoom(2);

        }

        private async void voiceChannel3Button_Click(object sender, EventArgs e)
        {
            await this.ClickHandlerForMediaRoom(3);
        }

        public void AddNewParticipantDisplay(Panel panel, PictureBox pictureBox)
        {
            panel.Controls.Add(pictureBox);
            panel.Refresh();
        }

        public void ShowAllUsersDetails(List<UserDetails> details)
        {
            // Clear existing controls in rightSidePanel except essential controls
            rightSidePanel.Controls.Clear();

            // Create dictionary to map role number to descriptive name and color
            Dictionary<int, Tuple<string, Color>> roleMapping = new Dictionary<int, Tuple<string, Color>>
            {
                { 0, new Tuple<string, Color>("Admin", GetRoleColor(0)) },
                { 1, new Tuple<string, Color>("Moderator", GetRoleColor(1)) },
                { 2, new Tuple<string, Color>("Member", GetRoleColor(2)) }
            };

            // Separate online and offline users
            var onlineUsers = details.Where(d => d.Status).ToList();
            var offlineUsers = details.Where(d => !d.Status).ToList();

            // Group online users by role
            var onlineUsersByRole = onlineUsers.GroupBy(u => u.role)
                                             .OrderBy(g => g.Key) // Sort by role number (lower = higher rank)
                                             .ToDictionary(g => g.Key, g => g.ToList());

            // Update and add the online label at the top
            Label onlineLabel = new Label
            {
                Text = $"Online - {onlineUsers.Count}",
                Location = new Point(6, 56),
                Font = new Font("Arial Narrow", 14.25f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };
            rightSidePanel.Controls.Add(onlineLabel);

            int currentY = onlineLabel.Bottom + 10;

            // Add role headers and users for each role that has online users
            foreach (var roleKey in onlineUsersByRole.Keys.OrderBy(k => k)) // Ensure Admin appears first, then Moderator, then Member
            {
                var roleName = roleMapping[roleKey].Item1;
                var roleColor = roleMapping[roleKey].Item2;
                var usersInRole = onlineUsersByRole[roleKey];

                // Add role header
                Label roleHeader = new Label
                {
                    Text = roleName + "s",
                    Location = new Point(20, currentY),
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = roleColor,
                    AutoSize = true
                };
                rightSidePanel.Controls.Add(roleHeader);
                currentY = roleHeader.Bottom + 5;

                // Add users for this role
                foreach (var user in usersInRole)
                {
                    if (!UsersImages.ContainsKey(user.UserId))
                    {
                        UsersImages[user.UserId] = ByteArrayToImage(user.Picture);
                    }

                    if (user.MediaChannelId != -1)
                    {
                        this.AddUserToMediaChannel(user.MediaChannelId, user);
                    }

                    AddUserEntry(user, currentY, true, roleMapping[user.role].Item2);
                    currentY += 60;
                }

                currentY += 10; // Add spacing between role groups
            }

            // Position offline label
            Label offlineLabel = new Label
            {
                Text = $"Offline - {offlineUsers.Count}",
                Location = new Point(6, currentY + 10),
                Font = new Font("Arial Narrow", 14.25f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };
            rightSidePanel.Controls.Add(offlineLabel);
            currentY = offlineLabel.Bottom + 20;

            // Add offline users
            foreach (var user in offlineUsers)
            {
                if (!UsersImages.ContainsKey(user.UserId))
                {
                    UsersImages[user.UserId] = ByteArrayToImage(user.Picture);
                }

                // Use the appropriate color for the offline user based on their role
                Color userColor = GetRoleColor(user.role, false); // false means offline

                AddUserEntry(user, currentY, false, userColor);
                currentY += 60;
            }
        }
        private void AddUserEntry(UserDetails user, int yPosition, bool isOnline, Color roleColor)
        {
            // Create panel for user entry
            Panel userPanel = new Panel
            {
                Width = rightSidePanel.Width - 20,
                Height = 50,
                Location = new Point(10, yPosition),
                Tag = user.role
            };

            // Create circle picture box for user avatar
            CirclePictureBox avatarPicture = new CirclePictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(5, 5),
                Image = UsersImages[user.UserId],
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Create label for username with role-specific color
            Label usernameLabel = new Label
            {
                Text = user.Username,
                Location = new Point(55, 15),
                AutoSize = true,
                ForeColor = roleColor, // Use the role-specific color
                Font = new Font(
                    "Arial",
                    12,
                    isOnline ? FontStyle.Regular : FontStyle.Italic,
                    GraphicsUnit.Point
                )
            };

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
                UsersInMediaChannels[i] = new List<UserDetails>();
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

            // Create dictionary to map role number to color using our helper method
            Dictionary<int, Color> roleColorMapping = new Dictionary<int, Color>
    {
        { 0, GetRoleColor(0) },
        { 1, GetRoleColor(1) },
        { 2, GetRoleColor(2) }
    };

            Panel channelPanel = mediaChannelPanels[channelId];
            channelPanel.Controls.Clear();
            UsersInMediaChannels[channelId] = users;

            int currentY = 5;

            foreach (var user in users)
            {
                // Get the appropriate color for the user based on their role
                Color userColor = roleColorMapping[user.role];

                // Create user entry panel
                Panel userPanel = new Panel
                {
                    Width = channelPanel.Width,
                    Height = 40,
                    Location = new Point(5, currentY),
                    Tag = user // Store the user details in the panel's Tag
                };

                // Create circle picture box for user avatar
                CirclePictureBox avatarPicture = new CirclePictureBox
                {
                    Size = new Size(30, 30),
                    Location = new Point(5, 5),
                    Image = UsersImages[user.UserId],
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Tag = user // Store the user details in the picture box's Tag
                };

                avatarPicture.MouseDown += (sender, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        CirclePictureBox pictureBox = (CirclePictureBox)sender;
                        UserDetails userData = (UserDetails)pictureBox.Tag;

                        // Use the centralized method instead of directly showing the menu
                        ShowUserContextMenu(pictureBox, e.Location, userData);
                    }
                };

                // Create label for username with role-specific color
                Label usernameLabel = new Label
                {
                    Text = user.Username,
                    Location = new Point(45, 10),
                    AutoSize = true,
                    ForeColor = userColor, // Use the role-specific color
                    Font = new Font("Arial", 10, FontStyle.Regular),
                    Tag = user // Store the user details in the label's Tag
                };

                usernameLabel.MouseDown += (sender, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        Label label = (Label)sender;
                        UserDetails userData = (UserDetails)label.Tag;

                        // Use the centralized method
                        ShowUserContextMenu(label, e.Location, userData);
                    }
                };

                // Add controls to user panel
                userPanel.Controls.Add(avatarPicture);
                userPanel.Controls.Add(usernameLabel);

                userPanel.MouseDown += (sender, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        Panel panel = (Panel)sender;
                        UserDetails userData = (UserDetails)panel.Tag;

                        // Use the centralized method
                        ShowUserContextMenu(panel, e.Location, userData);
                    }
                };

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
            if (!UsersInMediaChannels.ContainsKey(channelId))
                return;

            var users = UsersInMediaChannels[channelId];
            if (!users.Any(u => u.UserId == user.UserId))
            {
                users.Add(user);
                UpdateMediaChannelUsers(channelId, users);
            }
        }

        // Call this when a user leaves a media channel
        public void RemoveUserFromMediaChannel(int channelId, int userId)
        {
            if (!UsersInMediaChannels.ContainsKey(channelId))
                return;

            var users = UsersInMediaChannels[channelId];
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
                mediaChannelMuteButton.BackColor = mediaChannelMuteButton.BackColor == ThemeManager.GetColor("MutedColor") ?
                    ThemeManager.GetColor("ButtonBackground") : ThemeManager.GetColor("MutedColor");
            }
        }

        private void mediaChannelVideoMuteButton_Click(object sender, EventArgs e)
        {
            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.ToggleVideoMute();
                // Update button appearance to show muted state
                mediaChannelVideoMuteButton.BackColor = mediaChannelVideoMuteButton.BackColor == ThemeManager.GetColor("MutedColor") ?
                    ThemeManager.GetColor("ButtonBackground") : ThemeManager.GetColor("MutedColor");
            }
        }

        // 3. Add this event handler method to the DiscordApp class
        private async void mediaChannelDisconnectButton_Click(object sender, EventArgs e)
        {
            if (VideoStreamConnection != null)
            {
                // Disconnect user from the current media channel
                RemoveUserFromMediaChannel(this._videoRoomId, this._currentUserId);
                ConnectionManager.GetInstance(null).ProcessDisconnectFromMediaRoom(this._videoRoomId);

                await CleanupVideoStreamConnection();

                // Reset media control buttons to original state
                this.mediaChannelMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground");
                this.mediaChannelVideoMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground");

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
                    ConnectionManager.GetInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);
                }
            }
        }

        private async Task CleanupVideoStreamConnection()
        {
            var oldConnection = VideoStreamConnection;
            VideoStreamConnection = null;

            await Task.Run(async () =>
            {
                oldConnection.Dispose();
                // Force garbage collection to release camera resources
                GC.Collect();
                GC.WaitForPendingFinalizers();
                await Task.Delay(2000); // Longer delay to ensure device is released
            });
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
            this.globalMuteButton.BackColor = this._isGloballyMuted ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");

            // Apply mute setting to current connection if it exists
            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.SetGlobalMuteState(this._isGloballyMuted);
            }

        }

        private void deafenButton_Click(object sender, EventArgs e)
        {
            this._isGloballyDeafened = !this._isGloballyDeafened;
            this.deafenButton.BackColor = this._isGloballyDeafened ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");
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
                    BackColor = ThemeManager.GetColor("ButtonBackground"),
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

        private void InitializeUserContextMenu()
        {
            userContextMenu = new ContextMenuStrip();

            // Add menu items with checkboxes
            ToolStripMenuItem muteUserItem = new ToolStripMenuItem("Mute User");
            muteUserItem.CheckOnClick = true;
            muteUserItem.Click += MuteUserItem_Click;

            ToolStripMenuItem muteVideoItem = new ToolStripMenuItem("Mute Video");
            muteVideoItem.CheckOnClick = true;
            muteVideoItem.Click += MuteVideoItem_Click;

            ToolStripMenuItem deafenUserItem = new ToolStripMenuItem("Deafen User");
            deafenUserItem.CheckOnClick = true;
            deafenUserItem.Click += DeafenUserItem_Click;

            // Add disconnect option (not a checkbox)
            ToolStripMenuItem disconnectUserItem = new ToolStripMenuItem("Disconnect User");
            disconnectUserItem.Click += DisconnectUserItem_Click;

            // Add the Update Role submenu
            ToolStripMenuItem updateRoleItem = new ToolStripMenuItem("Update Role");

            // Create submenu items for roles
            ToolStripMenuItem setAdminRoleItem = new ToolStripMenuItem("Admin");
            setAdminRoleItem.Click += SetAdminRoleItem_Click;

            ToolStripMenuItem setModeratorRoleItem = new ToolStripMenuItem("Moderator");
            setModeratorRoleItem.Click += SetModeratorRoleItem_Click;

            ToolStripMenuItem setMemberRoleItem = new ToolStripMenuItem("Member");
            setMemberRoleItem.Click += SetMemberRoleItem_Click;

            // Add submenu items to update role item
            updateRoleItem.DropDownItems.Add(setAdminRoleItem);
            updateRoleItem.DropDownItems.Add(setModeratorRoleItem);
            updateRoleItem.DropDownItems.Add(setMemberRoleItem);

            // Add a separator
            ToolStripSeparator separator = new ToolStripSeparator();

            // View Profile option
            ToolStripMenuItem viewProfileItem = new ToolStripMenuItem("View Profile");
            viewProfileItem.Click += ViewProfileItem_Click;

            // Add items to the context menu
            userContextMenu.Items.Add(muteUserItem);
            userContextMenu.Items.Add(muteVideoItem);
            userContextMenu.Items.Add(deafenUserItem);
            userContextMenu.Items.Add(disconnectUserItem);
            userContextMenu.Items.Add(updateRoleItem); // Add the Update Role option
            userContextMenu.Items.Add(separator);
            userContextMenu.Items.Add(viewProfileItem);

            // Apply styling to match your application
            userContextMenu.BackColor = Color.FromArgb(47, 49, 54);
            userContextMenu.ForeColor = Color.White;
            userContextMenu.RenderMode = ToolStripRenderMode.System;
            userContextMenu.Renderer = new DarkContextMenuRenderer();
        }

        private void MuteUserItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;

            // Update the user settings in our manager
            UserContextMenuSettings.GetInstance().SetUserMuted(targetUser.UserId, item.Checked);

            // You can also implement visual indicators for muted users
            // For example, adding a mute icon overlay on their avatar
            UpdateUserMuteVisualIndicator(targetUser.UserId, item.Checked);

            // Send the mute command to the server to propagate to all clients
            ConnectionManager.GetInstance(null).ProcessSetUserMuted(targetUser.UserId, item.Checked);

            
        }

        private void MuteVideoItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;

            // Update the user settings in our manager
            UserContextMenuSettings.GetInstance().SetUserVideoMuted(targetUser.UserId, item.Checked);

            // You can also implement visual indicators for video muted users
            // For example, adding a video mute icon overlay on their avatar
            UpdateUserVideoMuteVisualIndicator(targetUser.UserId, item.Checked);

            // Send the video mute command to the server to propagate to all clients
            ConnectionManager.GetInstance(null).ProcessSetUserVideoMuted(targetUser.UserId, item.Checked);

            
        }

        private void DeafenUserItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;

            // Update the user settings
            UserContextMenuSettings.GetInstance().SetUserDeafened(targetUser.UserId, item.Checked);

            // Update visual indicators
            UpdateUserDeafenVisualIndicator(targetUser.UserId, item.Checked);

            // Send the deafen command to the server to propagate to all clients
            ConnectionManager.GetInstance(null).ProcessSetUserDeafened(targetUser.UserId, item.Checked);

            
        }

        private void DisconnectUserItem_Click(object sender, EventArgs e)
        {
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;

            // Find which media channel the user is in
            int userMediaChannelId = -1;
            foreach (var entry in UsersInMediaChannels)
            {
                int channelId = entry.Key;
                List<UserDetails> users = entry.Value;

                if (users.Any(u => u.UserId == targetUser.UserId))
                {
                    userMediaChannelId = channelId;
                    break;
                }
            }

            // If user is found in a media channel, remove them
            if (userMediaChannelId != -1)
            {
                // Ask for confirmation before disconnecting
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to disconnect {targetUser.Username} from the voice channel?",
                    "Disconnect User",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Remove user from the media channel locally
                    RemoveUserFromMediaChannel(userMediaChannelId, targetUser.UserId);

                    // Send disconnect command to the server to propagate to all clients including the target
                    ConnectionManager.GetInstance(null).ProcessDisconnectUserFromMediaRoom(targetUser.UserId, userMediaChannelId);
                }
            }
            else
            {
                // This shouldn't normally happen since the context menu is only available for users in media channels
                // But just in case, handle the situation
                MessageBox.Show(
                    $"{targetUser.Username} is not currently in a voice channel.",
                    "Cannot Disconnect",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void SetAdminRoleItem_Click(object sender, EventArgs e)
        {
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;
            ConnectionManager.GetInstance(null).ProcessUpdateUserRole(targetUser.UserId, 0); // 0 is Admin role
        }

        private void SetModeratorRoleItem_Click(object sender, EventArgs e)
        {
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;
            ConnectionManager.GetInstance(null).ProcessUpdateUserRole(targetUser.UserId, 1); // 1 is Moderator role
        }

        private void SetMemberRoleItem_Click(object sender, EventArgs e)
        {
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;
            ConnectionManager.GetInstance(null).ProcessUpdateUserRole(targetUser.UserId, 2); // 2 is Member role
        }

        private void UpdateContextMenuCheckStates(UserDetails user)
        {
            // Get current settings for the user
            var settings = UserContextMenuSettings.GetInstance().GetUserSettings(user.UserId);

            bool isCurrentUser = (user.UserId == this._currentUserId);

            // Determine if the current user has permission to modify this user based on roles
            bool isMyRoleStronger = false;
            if (user.role > this.role)
            {
                isMyRoleStronger = true;
            }

            foreach (ToolStripItem item in userContextMenu.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    switch (menuItem.Text)
                    {
                        case "Mute User":
                            menuItem.Checked = settings.IsMuted;
                            menuItem.Enabled = isMyRoleStronger;
                            break;

                        case "Mute Video":
                            menuItem.Checked = settings.IsVideoMuted;
                            menuItem.Enabled = isMyRoleStronger;
                            break;

                        case "Deafen User":
                            menuItem.Checked = settings.IsDeafened;
                            menuItem.Enabled = isMyRoleStronger;
                            break;

                        case "Disconnect User":
                            menuItem.Enabled = isMyRoleStronger;
                            break;

                        case "Update Role":
                            // Apply role update permissions according to requirements
                            menuItem.Enabled = !isCurrentUser && this.role <= 1; // Only Admin (0) and Moderator (1) can update roles

                            // Check each submenu item and set its enabled state
                            foreach (ToolStripItem subItem in menuItem.DropDownItems)
                            {
                                if (subItem is ToolStripMenuItem roleItem)
                                {
                                    switch (roleItem.Text)
                                    {
                                        case "Admin":
                                            // Nobody can set Admin role
                                            roleItem.Enabled = false;
                                            break;

                                        case "Moderator":
                                            // Admin can set anyone to Moderator, Moderator can set Member to Moderator
                                            roleItem.Enabled = (this.role == 0) || (this.role == 1 && user.role == 2);
                                            // Highlight the current role
                                            roleItem.Checked = user.role == 1;
                                            break;

                                        case "Member":
                                            // Admin can demote Moderator to Member, others can't change roles
                                            roleItem.Enabled = (this.role == 0 && user.role == 1);
                                            // Highlight the current role
                                            roleItem.Checked = user.role == 2;
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void ViewProfileItem_Click(object sender, EventArgs e)
        {
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;

            // Check if user is in any media channel - if so, they should be shown as online
            bool isInAnyMediaChannel = false;
            foreach (var channelUsers in UsersInMediaChannels.Values)
            {
                if (channelUsers.Any(u => u.UserId == targetUser.UserId))
                {
                    isInAnyMediaChannel = true;
                    break;
                }
            }

            // User should be online if either their Status is true or they are in a media channel
            bool isUserOnline = targetUser.Status || isInAnyMediaChannel;

            // Create dictionary to map role number to descriptive name and color using our helper method
            Dictionary<int, Tuple<string, Color>> roleMapping = new Dictionary<int, Tuple<string, Color>>
            {
                { 0, new Tuple<string, Color>("Admin", GetRoleColor(0)) },
                { 1, new Tuple<string, Color>("Moderator", GetRoleColor(1)) },
                { 2, new Tuple<string, Color>("Member", GetRoleColor(2)) }
            };

            // Get role information for the target user
            string roleName = roleMapping[targetUser.role].Item1;
            Color roleColor = roleMapping[targetUser.role].Item2;

            // Create a profile dialog
            using (var profileForm = new Form())
            {
                profileForm.Text = $"{targetUser.Username}'s Profile";
                profileForm.Size = new Size(400, 350); // Make it a bit taller for the role info
                profileForm.StartPosition = FormStartPosition.CenterParent;
                profileForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                profileForm.MaximizeBox = false;
                profileForm.MinimizeBox = false;
                profileForm.BackColor = Color.FromArgb(47, 49, 54); // Discord dark theme

                // Add profile picture
                CirclePictureBox profilePic = new CirclePictureBox
                {
                    Size = new Size(100, 100),
                    Location = new Point(150, 20),
                    Image = UsersImages[targetUser.UserId],
                    SizeMode = PictureBoxSizeMode.StretchImage
                };

                // Add username label
                Label usernameLabel = new Label
                {
                    Text = targetUser.Username,
                    Location = new Point(150, 130),
                    AutoSize = true,
                    ForeColor = roleColor, // Use role color for username
                    Font = new Font("Arial", 16, FontStyle.Bold)
                };

                // Center the username label
                usernameLabel.Location = new Point(
                    profileForm.ClientSize.Width / 2 - usernameLabel.PreferredWidth / 2,
                    130);

                // Add role label
                Label roleLabel = new Label
                {
                    Text = roleName,
                    Location = new Point(150, 160),
                    AutoSize = true,
                    ForeColor = roleColor,
                    Font = new Font("Arial", 14, FontStyle.Regular)
                };

                // Center the role label
                roleLabel.Location = new Point(
                    profileForm.ClientSize.Width / 2 - roleLabel.PreferredWidth / 2,
                    160);

                // Add status (online/offline)
                Label statusLabel = new Label
                {
                    Text = isUserOnline ? "Online" : "Offline",
                    Location = new Point(150, 190),
                    AutoSize = true,
                    ForeColor = isUserOnline ? Color.LightGreen : Color.Gray,
                    Font = new Font("Arial", 12)
                };

                // Center the status label
                statusLabel.Location = new Point(
                    profileForm.ClientSize.Width / 2 - statusLabel.PreferredWidth / 2,
                    190);

                // Add user settings indicators
                var settings = UserContextMenuSettings.GetInstance().GetUserSettings(targetUser.UserId);

                FlowLayoutPanel settingsPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    AutoSize = true,
                    Location = new Point(20, 220),
                    Width = profileForm.ClientSize.Width - 40
                };

                if (settings.IsMuted)
                {
                    Label mutedLabel = new Label
                    {
                        Text = "🔇 Muted",
                        ForeColor = ThemeManager.GetColor("MutedColor"),
                        Font = new Font("Arial", 10),
                        AutoSize = true
                    };
                    settingsPanel.Controls.Add(mutedLabel);
                }

                if (settings.IsVideoMuted)
                {
                    Label videoMutedLabel = new Label
                    {
                        Text = "🎥 Video Muted",
                        ForeColor = ThemeManager.GetColor("MutedColor"),
                        Font = new Font("Arial", 10),
                        AutoSize = true
                    };
                    settingsPanel.Controls.Add(videoMutedLabel);
                }

                if (settings.IsDeafened)
                {
                    Label deafenedLabel = new Label
                    {
                        Text = "🔈 Deafened",
                        ForeColor = ThemeManager.GetColor("MutedColor"),
                        Font = new Font("Arial", 10),
                        AutoSize = true
                    };
                    settingsPanel.Controls.Add(deafenedLabel);
                }

                // Add close button
                Button closeButton = new Button
                {
                    Text = "Close",
                    Size = new Size(100, 30),
                    Location = new Point(150, 280),
                    BackColor = ThemeManager.GetColor("ButtonBackground"),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                // Center the close button
                closeButton.Location = new Point(
                    profileForm.ClientSize.Width / 2 - closeButton.Width / 2,
                    280);

                closeButton.Click += (s, args) => profileForm.Close();

                // Adjust settings panel position to center it
                settingsPanel.Location = new Point(
                    profileForm.ClientSize.Width / 2 - settingsPanel.Width / 2,
                    220);

                // Add controls to form
                profileForm.Controls.Add(profilePic);
                profileForm.Controls.Add(usernameLabel);
                profileForm.Controls.Add(roleLabel);
                profileForm.Controls.Add(statusLabel);
                profileForm.Controls.Add(settingsPanel);
                profileForm.Controls.Add(closeButton);

                // Show the profile dialog
                profileForm.ShowDialog();
            }
        }

        private void RepositionStatusIcons(Panel userPanel, Label usernameLabel)
        {
            // Find all status icons in the panel
            List<Control> statusIcons = new List<Control>();
            foreach (Control control in userPanel.Controls)
            {
                if (control.Tag != null && control.Tag.ToString() == "StatusIcon")
                {
                    statusIcons.Add(control);
                }
            }

            if (statusIcons.Count == 0)
                return;

            // Sort icons by their names to ensure consistent order
            statusIcons.Sort((a, b) => string.Compare(a.Name, b.Name));

            // Position the first icon right after the username
            int xPos = usernameLabel.Right + 8;
            int yPos = usernameLabel.Top;

            foreach (Control icon in statusIcons)
            {
                icon.Location = new Point(xPos, yPos);
                icon.BringToFront();
                xPos += icon.Width + 4; // Add spacing between icons
            }
        }

        private void UpdateUserMuteVisualIndicator(int userId, bool isMuted)
        {
            // This method updates visual indicators for muted users
            foreach (var channelId in mediaChannelPanels.Keys)
            {
                if (UsersInMediaChannels.ContainsKey(channelId))
                {
                    var userInChannel = UsersInMediaChannels[channelId].FirstOrDefault(u => u.UserId == userId);
                    if (userInChannel != null)
                    {
                        Panel channelPanel = mediaChannelPanels[channelId];

                        // Find the user's panel
                        foreach (Control control in channelPanel.Controls)
                        {
                            if (control is Panel userPanel && userPanel.Tag is UserDetails userData && userData.UserId == userId)
                            {
                                // Find the username label
                                Label usernameLabel = null;
                                foreach (Control childControl in userPanel.Controls)
                                {
                                    if (childControl is Label label)
                                    {
                                        usernameLabel = label;
                                        break;
                                    }
                                }

                                if (usernameLabel != null)
                                {
                                    // Check if mute indicator already exists
                                    Control existingIndicator = userPanel.Controls.Find("muteIndicator_" + userId, true).FirstOrDefault();

                                    if (isMuted)
                                    {
                                        if (existingIndicator == null)
                                        {
                                            // Create mute indicator with light red color
                                            PictureBox muteIndicator = new PictureBox
                                            {
                                                Name = "muteIndicator_" + userId,
                                                Size = new Size(16, 16),
                                                Image = Properties.Resources.muteLogo, // You should have this resource
                                                SizeMode = PictureBoxSizeMode.StretchImage,
                                                BackColor = Color.Transparent,
                                                Tag = "StatusIcon" // Tag to identify status icons
                                            };

                                            // Apply a light red tint to the icon
                                            Bitmap originalImage = new Bitmap(Properties.Resources.muteLogo);
                                            Bitmap redTintedImage = new Bitmap(originalImage.Width, originalImage.Height);

                                            using (Graphics g = Graphics.FromImage(redTintedImage))
                                            {
                                                // Create a lighter red colorization matrix
                                                ColorMatrix colorMatrix = new ColorMatrix(
                                                    new float[][]
                                                    {
                                                        new float[] {1, 0, 0, 0, 0},
                                                        new float[] {0, 0, 0, 0, 0},
                                                        new float[] {0, 0, 0, 0, 0},
                                                        new float[] {0, 0, 0, 1, 0},
                                                        new float[] {0.7f, 0.3f, 0.3f, 0, 1} // Lighter red with some pink tone
                                                    });

                                                using (ImageAttributes attributes = new ImageAttributes())
                                                {
                                                    attributes.SetColorMatrix(colorMatrix);
                                                    g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                                        0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, attributes);
                                                }
                                            }

                                            muteIndicator.Image = redTintedImage;
                                            userPanel.Controls.Add(muteIndicator);
                                        }
                                    }
                                    else if (existingIndicator != null)
                                    {
                                        userPanel.Controls.Remove(existingIndicator);
                                        existingIndicator.Dispose();

                                    }

                                    // Reposition all status icons
                                    RepositionStatusIcons(userPanel, usernameLabel);
                                    break;
                                    
                                }

                            }
                        }
                    }
                }
            }
        }

        private void UpdateUserVideoMuteVisualIndicator(int userId, bool isVideoMuted)
        {

            foreach (var channelId in mediaChannelPanels.Keys)
            {
                if (UsersInMediaChannels.ContainsKey(channelId))
                {
                    var userInChannel = UsersInMediaChannels[channelId].FirstOrDefault(u => u.UserId == userId);
                    if (userInChannel != null)
                    {
                        Panel channelPanel = mediaChannelPanels[channelId];

                        foreach (Control control in channelPanel.Controls)
                        {
                            if (control is Panel userPanel && userPanel.Tag is UserDetails userData && userData.UserId == userId)
                            {
                                // Find the username label
                                Label usernameLabel = null;
                                foreach (Control childControl in userPanel.Controls)
                                {
                                    if (childControl is Label label)
                                    {
                                        usernameLabel = label;
                                        break;
                                    }
                                }

                                if (usernameLabel != null)
                                {
                                    // Check if video mute indicator already exists
                                    Control existingVideoIndicator = userPanel.Controls.Find("videoMuteIndicator_" + userId, true).FirstOrDefault();

                                    if (isVideoMuted)
                                    {
                                        if (existingVideoIndicator == null)
                                        {

                                            PictureBox videoMuteIndicator = new PictureBox
                                            {
                                                Name = "videoMuteIndicator_" + userId,
                                                Size = new Size(16, 16),
                                                Image = Properties.Resources.videoMuteLogo, // You should have this resource
                                                SizeMode = PictureBoxSizeMode.StretchImage,
                                                BackColor = Color.Transparent,
                                                Tag = "StatusIcon" // Tag to identify status icons
                                            };

                                            // Apply a light red tint to the icon
                                            Bitmap originalImage = new Bitmap(Properties.Resources.videoMuteLogo);
                                            Bitmap redTintedImage = new Bitmap(originalImage.Width, originalImage.Height);

                                            using (Graphics g = Graphics.FromImage(redTintedImage))
                                            {
                                                // Create a lighter red colorization matrix
                                                ColorMatrix colorMatrix = new ColorMatrix(
                                                    new float[][]
                                                    {
                                                        new float[] {1, 0, 0, 0, 0},
                                                        new float[] {0, 0, 0, 0, 0},
                                                        new float[] {0, 0, 0, 0, 0},
                                                        new float[] {0, 0, 0, 1, 0},
                                                        new float[] {0.7f, 0.3f, 0.3f, 0, 1} // Lighter red with some pink tone
                                                    });

                                                using (ImageAttributes attributes = new ImageAttributes())
                                                {
                                                    attributes.SetColorMatrix(colorMatrix);
                                                    g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                                        0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, attributes);
                                                }
                                            }

                                            videoMuteIndicator.Image = redTintedImage;
                                            userPanel.Controls.Add(videoMuteIndicator);
                                        }
                                    }
                                    else if (existingVideoIndicator != null)
                                    {
                                        userPanel.Controls.Remove(existingVideoIndicator);
                                        existingVideoIndicator.Dispose();

                                    }

                                    // Reposition all status icons
                                    RepositionStatusIcons(userPanel, usernameLabel);
                                    break;
                                }

                            }
                        }
                    }
                }
            }
        }

        private void UpdateUserDeafenVisualIndicator(int userId, bool isDeafened)
        {

            foreach (var channelId in mediaChannelPanels.Keys)
            {
                if (UsersInMediaChannels.ContainsKey(channelId))
                {
                    var userInChannel = UsersInMediaChannels[channelId].FirstOrDefault(u => u.UserId == userId);
                    if (userInChannel != null)
                    {
                        Panel channelPanel = mediaChannelPanels[channelId];

                        foreach (Control control in channelPanel.Controls)
                        {
                            if (control is Panel userPanel && userPanel.Tag is UserDetails userData && userData.UserId == userId)
                            {
                                // Find the username label
                                Label usernameLabel = null;
                                foreach (Control childControl in userPanel.Controls)
                                {
                                    if (childControl is Label label)
                                    {
                                        usernameLabel = label;
                                        break;
                                    }
                                }

                                if (usernameLabel != null)
                                {
                                    // Check if deafen indicator already exists
                                    Control existingIndicator = userPanel.Controls.Find("deafenIndicator_" + userId, true).FirstOrDefault();

                                    if (isDeafened)
                                    {
                                        if (existingIndicator == null)
                                        { 

                                            PictureBox deafenIndicator = new PictureBox
                                            {
                                                Name = "deafenIndicator_" + userId,
                                                Size = new Size(16, 16),
                                                Image = Properties.Resources.deafenLogo, // You should have this resource
                                                SizeMode = PictureBoxSizeMode.StretchImage,
                                                BackColor = Color.Transparent,
                                                Tag = "StatusIcon" // Tag to identify status icons
                                            };

                                            // Apply a light red tint to the icon
                                            Bitmap originalImage = new Bitmap(Properties.Resources.deafenLogo);
                                            Bitmap redTintedImage = new Bitmap(originalImage.Width, originalImage.Height);

                                            using (Graphics g = Graphics.FromImage(redTintedImage))
                                            {
                                                // Create a lighter red colorization matrix
                                                ColorMatrix colorMatrix = new ColorMatrix(
                                                    new float[][]
                                                    {
                                                        new float[] {1, 0, 0, 0, 0},
                                                        new float[] {0, 0, 0, 0, 0},
                                                        new float[] {0, 0, 0, 0, 0},
                                                        new float[] {0, 0, 0, 1, 0},
                                                        new float[] {0.7f, 0.3f, 0.3f, 0, 1} // Lighter red with some pink tone
                                                    });

                                                using (ImageAttributes attributes = new ImageAttributes())
                                                {
                                                    attributes.SetColorMatrix(colorMatrix);
                                                    g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                                                        0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, attributes);
                                                }
                                            }

                                            deafenIndicator.Image = redTintedImage;
                                            userPanel.Controls.Add(deafenIndicator);
                                        }
                                    }
                                    else if (existingIndicator != null)
                                    {
                                        userPanel.Controls.Remove(existingIndicator);
                                        existingIndicator.Dispose();
                                    }

                                    // Reposition all status icons
                                    RepositionStatusIcons(userPanel, usernameLabel);
                                    break;
                                }

                            }
                        }
                    }
                }
            }
        }

        // Add this method to show the context menu for a user
        public void ShowUserContextMenu(Control control, Point location, UserDetails user)
        {
            userContextMenu.Tag = user;

            // Update the check states before showing
            UpdateContextMenuCheckStates(user);

            userContextMenu.Show(control, location);
        }

        public void HandleUserMuteStatusChanged(int userId, bool isMuted)
        {
            // Update the user settings in our manager
            UserContextMenuSettings.GetInstance().SetUserMuted(userId, isMuted);

            // Update visual indicators
            UpdateUserMuteVisualIndicator(userId, isMuted);

            // If this is about the current user, apply audio muting
            if (userId == _currentUserId && VideoStreamConnection != null)
            {
                // Only toggle if the current state doesn't match the desired state
                bool isCurrentlyMuted = mediaChannelMuteButton.BackColor == ThemeManager.GetColor("MutedColor");
                mediaChannelMuteButton.Enabled = !isMuted;
                this.globalMuteButton.Enabled = !isMuted;
                this._isMutedByHigherRole = isMuted;
                if (isCurrentlyMuted != isMuted)
                {
                    VideoStreamConnection.SetMutedByHigherRoleState(isMuted);
                    mediaChannelMuteButton.BackColor = isMuted ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");
                    globalMuteButton.BackColor = isMuted ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");
                }
                if(isMuted == false)
                {
                    this._isGloballyMuted = false;
                }

            }
        }

        public void HandleUserVideoMuteStatusChanged(int userId, bool isVideoMuted)
        {
            // Update the user settings in our manager
            UserContextMenuSettings.GetInstance().SetUserVideoMuted(userId, isVideoMuted);

            // Update visual indicators
            UpdateUserVideoMuteVisualIndicator(userId, isVideoMuted);

            // If this is about the current user, apply video muting
            if (userId == _currentUserId && VideoStreamConnection != null)
            {
                // Update button state and track that this was done by a higher role
                mediaChannelVideoMuteButton.Enabled = !isVideoMuted;
                this._isVideoMutedByHigherRole = isVideoMuted;

                // Only toggle if the current state doesn't match the desired state
                bool isCurrentlyVideoMuted = mediaChannelVideoMuteButton.BackColor == ThemeManager.GetColor("MutedColor");
                if (isCurrentlyVideoMuted != isVideoMuted)
                {
                    VideoStreamConnection.ToggleVideoMute();
                    mediaChannelVideoMuteButton.BackColor = isVideoMuted ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");
                }
            }
        }

        public void HandleUserDeafenStatusChanged(int userId, bool isDeafened)
        {
            // Update the user settings in our manager
            UserContextMenuSettings.GetInstance().SetUserDeafened(userId, isDeafened);

            // Update visual indicators
            UpdateUserDeafenVisualIndicator(userId, isDeafened);

            // If this is about the current user, apply audio deafening
            if (userId == _currentUserId && VideoStreamConnection != null)
            {
                // Only update if the current state doesn't match the desired state
                this.deafenButton.Enabled = !isDeafened;
                this._isDeafenedByHigherRole = isDeafened;
                if (_isGloballyDeafened != isDeafened)
                {
                    VideoStreamConnection.SetGlobalDeafenState(isDeafened);
                    deafenButton.BackColor = isDeafened ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");
                    _isGloballyDeafened = isDeafened;
                }

                if (isDeafened == false)
                {
                    this._isGloballyDeafened = false;
                }
            }
        }

        public async void HandleUserDisconnect(int userId, int mediaRoomId)
        {
            // If this is about the current user, perform full disconnect
            if (userId == _currentUserId && VideoStreamConnection != null)
            {
                // Disconnect user from the current media channel
                RemoveUserFromMediaChannel(mediaRoomId, _currentUserId);

                // Dispose video connection
                VideoStreamConnection.Dispose();
                VideoStreamConnection = null;

                // Small delay to ensure proper cleanup
                await Task.Delay(1000);

                // Reset media control buttons to original state
                this.mediaChannelMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground");
                this.mediaChannelVideoMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground");

                // Switch to text channel 1 (default)
                this.HideAllPanels();
                this.ChatMessagesPanel1.Visible = true;
                this.messageInputTextBox.Visible = true;
                this.sendMessageButton.Visible = true;

                // Make sure to update emoji panel visibility
                this.UpdateEmojiPanelVisibility();

                // If chat history isn't loaded yet, load it
                if (((string)this.ChatMessagesPanel1.Tag) == "0")
                {
                    ConnectionManager.GetInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);
                }
            }
            else
            {
                // If it's another user, just update the UI
                RemoveUserFromMediaChannel(mediaRoomId, userId);
            }
        }

        public Color GetRoleColor(int roleNumber, bool isOnline = true)
        {
            return ThemeManager.GetRoleColor(roleNumber, isOnline);
        }

        public void HandleUserRoleUpdated(int userId, int newRole)
        {

            // Check if this is the current user's role being updated
            if (userId == this._currentUserId)
            {
                // Update the current user's role
                this.role = newRole;

                // Update any UI elements that are role-dependent
                // For example, refresh the context menu permissions
            }

            // Find the user's username from existing data
            string username = "Unknown";

            // Check in all media channels
            foreach (var channelUsers in UsersInMediaChannels.Values)
            {
                var user = channelUsers.FirstOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    username = user.Username;
                    // Update the user's role
                    user.role = newRole;
                }
            }

            // Update all media channel displays
            foreach (var channelEntry in UsersInMediaChannels)
            {
                UpdateMediaChannelUsers(channelEntry.Key, channelEntry.Value);
            }

            // Request a refresh of the users list to update their display in the sidebar
            ConnectionManager.GetInstance(null).ProcessFetchAllUsers();

            // Show a notification to the user
            MessageBox.Show($"{username}'s role has been updated to {GetRoleNameFromId(newRole)}");
        }

        private string GetRoleNameFromId(int roleId)
        {
            switch (roleId)
            {
                case 0:
                    return "Admin";
                case 1:
                    return "Moderator";
                case 2:
                    return "Member";
                default:
                    return "Unknown";
            }
        }


    }
}