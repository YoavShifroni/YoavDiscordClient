using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoavDiscordClient.Style;

namespace YoavDiscordClient.Managers
{
    /// <summary>
    /// Manages media channels including voice and video functionality.
    /// </summary>
    public class MediaChannelManager
    {
        private readonly DiscordApp _form;
        private readonly Panel _leftSidePanel;
        private readonly Panel _chatAreaPanel;
        private readonly Panel _mediaControlsPanel;

        /// <summary>
        /// Dictionary mapping media channel IDs to their corresponding UI panels.
        /// </summary>
        private Dictionary<int, Panel> _mediaChannelPanels = new Dictionary<int, Panel>();

        /// <summary>
        /// Dictionary mapping media channel IDs to lists of users in those channels.
        /// </summary>
        public Dictionary<int, List<UserDetails>> UsersInMediaChannels = new Dictionary<int, List<UserDetails>>();

        /// <summary>
        /// The ID of the media room that the current user is connected to.
        /// </summary>
        private int _mediaRoomId;

        /// <summary>
        /// Flag indicating whether the user has globally muted their audio.
        /// </summary>
        private bool _isGloballyMuted = false;

        /// <summary>
        /// Flag indicating whether the user has globally deafened themselves.
        /// </summary>
        private bool _isGloballyDeafened = false;

        /// <summary>
        /// Flag indicating if the user is muted by a user with a higher role.
        /// </summary>
        private bool _isMutedByHigherRole = false;

        /// <summary>
        /// Flag indicating if the user's video is muted by a user with a higher role.
        /// </summary>
        private bool _isVideoMutedByHigherRole = false;

        /// <summary>
        /// Flag indicating if the user is deafened by a user with a higher role.
        /// </summary>
        private bool _isDeafenedByHigherRole = false;

        /// <summary>
        /// Static reference to the current video streaming connection.
        /// </summary>
        public static VideoStreamConnection VideoStreamConnection;

        public MediaChannelManager(DiscordApp form, Panel leftSidePanel, Panel chatAreaPanel, Panel mediaControlsPanel)
        {
            _form = form;
            _leftSidePanel = leftSidePanel;
            _chatAreaPanel = chatAreaPanel;
            _mediaControlsPanel = mediaControlsPanel;
        }

        /// <summary>
        /// Initializes panels for displaying users in each media channel.
        /// Creates a panel for each of the three media channels and adds them to the left side panel.
        /// </summary>
        public void InitializeMediaChannelPanels()
        {
            // Create panels for each media channel
            for (int i = 1; i <= 3; i++)
            {
                Panel channelPanel = new Panel
                {
                    Width = _form.voiceChannel1Button.Width,
                    BackColor = Color.FromArgb(54, 57, 63), // Same as leftSidePanel
                    AutoSize = true,
                    Location = new Point(_form.voiceChannel1Button.Left, GetMediaChannelButton(i).Bottom + 5)
                };

                _leftSidePanel.Controls.Add(channelPanel);
                _mediaChannelPanels[i] = channelPanel;
                UsersInMediaChannels[i] = new List<UserDetails>();
            }
        }

        /// <summary>
        /// Gets the button object associated with a specific media channel number.
        /// </summary>
        /// <param name="channelNumber">The channel number (1-3).</param>
        /// <returns>The Button control for the specified channel.</returns>
        /// <exception cref="ArgumentException">Thrown if an invalid channel number is provided.</exception>
        private Button GetMediaChannelButton(int channelNumber)
        {
            switch (channelNumber)
            {
                case 1:
                    return _form.voiceChannel1Button;
                case 2:
                    return _form.voiceChannel2Button;
                case 3:
                    return _form.voiceChannel3Button;
                default:
                    throw new ArgumentException("Invalid channel number");
            }
        }

        /// <summary>
        /// Updates the display of users in a media channel.
        /// Clears and repopulates the channel panel with the provided list of users.
        /// </summary>
        /// <param name="channelId">The ID of the media channel to update.</param>
        /// <param name="users">The list of users to display in the channel.</param>
        public void UpdateMediaChannelUsers(int channelId, List<UserDetails> users)
        {
            if (!_mediaChannelPanels.ContainsKey(channelId))
                return;

            // Get the user manager for role colors
            UserManager userManager = _form.GetUserManager();

            // Create dictionary to map role number to color
            Dictionary<int, Color> roleColorMapping = new Dictionary<int, Color>
            {
                { 0, userManager.GetRoleColor(0) },
                { 1, userManager.GetRoleColor(1) },
                { 2, userManager.GetRoleColor(2) }
            };

            Panel channelPanel = _mediaChannelPanels[channelId];
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
                    Image = userManager.UsersImages[user.UserId],
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Tag = user // Store the user details in the picture box's Tag
                };

                avatarPicture.MouseDown += (sender, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        CirclePictureBox pictureBox = (CirclePictureBox)sender;
                        UserDetails userData = (UserDetails)pictureBox.Tag;

                        // Use the context menu manager to show the menu
                        _form.GetContextMenuManager().ShowUserContextMenu(pictureBox, e.Location, userData);
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

                        // Use the context menu manager
                        _form.GetContextMenuManager().ShowUserContextMenu(label, e.Location, userData);
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

                        // Use the context menu manager
                        _form.GetContextMenuManager().ShowUserContextMenu(panel, e.Location, userData);
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

        /// <summary>
        /// Updates the positions of media channel buttons and panels to maintain proper layout.
        /// </summary>
        /// <param name="currentChannelId">The ID of the channel that was just updated.</param>
        private void UpdateMediaChannelPositions(int currentChannelId)
        {
            // First, update the current channel's panel position
            Panel currentPanel = _mediaChannelPanels[currentChannelId];
            Button currentButton = GetMediaChannelButton(currentChannelId);
            currentPanel.Location = new Point(currentPanel.Left, currentButton.Bottom + 5);

            // Update all channel positions sequentially to maintain consistent spacing
            for (int i = 1; i <= 3; i++)
            {
                Button button = GetMediaChannelButton(i);
                Panel panel = _mediaChannelPanels[i];

                if (i == 1)
                {
                    // First button position stays the same
                    panel.Location = new Point(panel.Left, button.Bottom + 5);
                }
                else
                {
                    // Position each subsequent button after the previous panel
                    Button prevButton = GetMediaChannelButton(i - 1);
                    Panel prevPanel = _mediaChannelPanels[i - 1];

                    // Calculate the position based on previous panel content
                    int newButtonY = prevPanel.Controls.Count > 0
                        ? prevPanel.Bottom + 10  // If panel has users, position after them
                        : prevButton.Bottom + 45; // If panel is empty, maintain default spacing

                    button.Location = new Point(button.Left, newButtonY);
                    panel.Location = new Point(panel.Left, button.Bottom + 5);
                }
            }
        }

        /// <summary>
        /// Adds a user to a media channel and updates the UI accordingly.
        /// </summary>
        /// <param name="channelId">The ID of the media channel to add the user to.</param>
        /// <param name="user">The user details to add to the channel.</param>
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

        /// <summary>
        /// Removes a user from a media channel and updates the UI accordingly.
        /// </summary>
        /// <param name="channelId">The ID of the media channel to remove the user from.</param>
        /// <param name="userId">The ID of the user to remove.</param>
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

        /// <summary>
        /// Connects the user to a media room.
        /// </summary>
        /// <param name="mediaRoomId">The ID of the media room to connect to.</param>
        public async Task ConnectToMediaRoom(int mediaRoomId)
        {
            UserManager userManager = _form.GetUserManager();

            // If we're already connected to this channel, just show the video panel
            if (VideoStreamConnection != null && this._mediaRoomId == mediaRoomId)
            {
                _form.GetChatManager().HideAllPanels();
                _form.VideoPanel1.Visible = true;

                // Make sure to show the media controls panel
                _mediaControlsPanel.Visible = true;
                _mediaControlsPanel.Location = new Point(
                    (_chatAreaPanel.Width - _mediaControlsPanel.Width) / 2,
                    _chatAreaPanel.Height - _mediaControlsPanel.Height - 30); // 30px from bottom
                _mediaControlsPanel.BringToFront();
                return;
            }

            // If we're connected to a different channel, disconnect first
            if (VideoStreamConnection != null)
            {
                RemoveUserFromMediaChannel(this._mediaRoomId, userManager.GetCurrentUserId());
                ConnectionManager.GetInstance(null).ProcessDisconnectFromMediaRoom(this._mediaRoomId);
                await this.CleanupVideoStreamConnection();
            }

            this._mediaRoomId = mediaRoomId;
            // Add the current user to the new channel
            AddUserToMediaChannel(this._mediaRoomId, new UserDetails(
                userManager.GetCurrentUserId(),
                userManager.Username,
                userManager.ImageToByteArray(userManager.UserProfilePicture),
                userManager.GetUserRole()));

            _form.GetChatManager().HideAllPanels();
            string panelName = "VideoPanel" + mediaRoomId.ToString();
            Panel videoPanel = ((Panel)_form.Controls.Find(panelName, true)[0]);
            videoPanel.Visible = true;

            // Position the media controls panel with some distance from the rectangles
            // Center the panel horizontally at the bottom of the chat area
            _mediaControlsPanel.Location = new Point(
                (_chatAreaPanel.Width - _mediaControlsPanel.Width) / 2,
                _chatAreaPanel.Height - _mediaControlsPanel.Height - 30); // 30px from bottom
            _mediaControlsPanel.BringToFront();

            // Reset buttons to normal state when switching channels
            if (this._isMutedByHigherRole)
            {
                _form.mediaChannelMuteButton.Enabled = false;
                _form.mediaChannelMuteButton.BackColor = ThemeManager.GetColor("MutedColor");
                _form.GetContextMenuManager().UpdateUserMuteVisualIndicator(userManager.GetCurrentUserId(), true);
            }
            else
            {
                _form.mediaChannelMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground"); // Original color
            }

            if (this._isVideoMutedByHigherRole)
            {
                _form.mediaChannelVideoMuteButton.Enabled = false;
                _form.mediaChannelVideoMuteButton.BackColor = ThemeManager.GetColor("MutedColor");
                _form.GetContextMenuManager().UpdateUserVideoMuteVisualIndicator(userManager.GetCurrentUserId(), true);

            }
            else
            {
                _form.mediaChannelVideoMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground"); // Original color
            }

            if (this._isDeafenedByHigherRole)
            {
                _form.GetContextMenuManager().UpdateUserDeafenVisualIndicator(userManager.GetCurrentUserId(), true);
            }

            VideoStreamConnection = new VideoStreamConnection(videoPanel);
            await VideoStreamConnection.Initialize();

            _mediaControlsPanel.Visible = true;

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
                _form.mediaChannelVideoMuteButton.BackColor = ThemeManager.GetColor("MutedColor");
                _form.mediaChannelVideoMuteButton.Enabled = false;
            }

            ConnectionManager.GetInstance(null).ProcessConnectToMediaRoom(this._mediaRoomId);
        }

        /// <summary>
        /// Cleans up resources used by the video stream connection.
        /// </summary>
        public async Task CleanupVideoStreamConnection()
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

        /// <summary>
        /// Disconnects the user from the current media room.
        /// </summary>
        public async Task DisconnectFromMediaRoom()
        {
            if (VideoStreamConnection != null)
            {
                UserManager userManager = _form.GetUserManager();

                // Disconnect user from the current media channel
                RemoveUserFromMediaChannel(this._mediaRoomId, userManager.GetCurrentUserId());
                ConnectionManager.GetInstance(null).ProcessDisconnectFromMediaRoom(this._mediaRoomId);

                await CleanupVideoStreamConnection();

                // Reset media control buttons to original state
                _form.mediaChannelMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground");
                _form.mediaChannelVideoMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground");
                _form.mediaControlsPanel.Visible = false;
            }
        }

        /// <summary>
        /// Toggles the audio mute state of the current video stream connection.
        /// </summary>
        public void ToggleAudioMute()
        {
            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.ToggleAudioMute();
                // Update button appearance to show muted state
                _form.mediaChannelMuteButton.BackColor = _form.mediaChannelMuteButton.BackColor == ThemeManager.GetColor("MutedColor") ?
                    ThemeManager.GetColor("ButtonBackground") : ThemeManager.GetColor("MutedColor");
            }
        }

        /// <summary>
        /// Toggles the video mute state of the current video stream connection.
        /// </summary>
        public void ToggleVideoMute()
        {
            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.ToggleVideoMute();
                // Update button appearance to show muted state
                _form.mediaChannelVideoMuteButton.BackColor = _form.mediaChannelVideoMuteButton.BackColor == ThemeManager.GetColor("MutedColor") ?
                    ThemeManager.GetColor("ButtonBackground") : ThemeManager.GetColor("MutedColor");
            }
        }

        /// <summary>
        /// Toggles the global mute state.
        /// </summary>
        public void ToggleGlobalMute(Button globalMuteButton)
        {
            this._isGloballyMuted = !this._isGloballyMuted;
            globalMuteButton.BackColor = this._isGloballyMuted ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");

            // Apply mute setting to current connection if it exists
            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.SetGlobalMuteState(this._isGloballyMuted);
            }
        }

        public void UnmuteByHigherRole()
        {
            // Apply mute setting to current connection if it exists
            if (VideoStreamConnection != null)
            {
                this._isGloballyMuted = false;
                VideoStreamConnection.SetGlobalMuteState(this._isGloballyMuted);
            }
        }


        /// <summary>
        /// Toggles the global deafen state.
        /// </summary>
        public void ToggleGlobalDeafen(Button deafenButton)
        {
            this._isGloballyDeafened = !this._isGloballyDeafened;
            deafenButton.BackColor = this._isGloballyDeafened ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");

            // Apply deafen setting to current connection if it exists
            if (VideoStreamConnection != null)
            {
                VideoStreamConnection.SetGlobalDeafenState(this._isGloballyDeafened);
            }
        }

        /// <summary>
        /// Checks if the user is currently in a media channel.
        /// </summary>
        public bool IsInMediaChannel()
        {
            return VideoStreamConnection != null;
        }

        /// <summary>
        /// Sets the muted by higher role state.
        /// </summary>
        public void SetMutedByHigherRole(bool isMuted)
        {
            _isMutedByHigherRole = isMuted;
        }

        /// <summary>
        /// Sets the video muted by higher role state.
        /// </summary>
        public void SetVideoMutedByHigherRole(bool isMuted)
        {
            _isVideoMutedByHigherRole = isMuted;
        }

        /// <summary>
        /// Sets the deafened by higher role state.
        /// </summary>
        public void SetDeafenedByHigherRole(bool isDeafened)
        {
            _isDeafenedByHigherRole = isDeafened;
        }

        /// <summary>
        /// Sets the globally deafened state.
        /// </summary>
        public void SetGloballyDeafened(bool isDeafened)
        {
            _isGloballyDeafened = isDeafened;
        }

        /// <summary>
        /// Gets the media channel panels.
        /// </summary>
        public Dictionary<int, Panel> GetMediaChannelPanels()
        {
            return _mediaChannelPanels;
        }

        /// <summary>
        /// Gets the global mute state.
        /// </summary>
        public bool IsGloballyMuted()
        {
            return _isGloballyMuted;
        }

        /// <summary>
        /// Gets the global deafen state.
        /// </summary>
        public bool IsGloballyDeafened()
        {
            return _isGloballyDeafened;
        }

        /// <summary>
        /// Handles a user disconnect notification from the server.
        /// </summary>
        public async Task HandleUserDisconnect(int userId, int mediaRoomId)
        {
            UserManager userManager = _form.GetUserManager();

            // If this is about the current user, perform full disconnect
            if (userId == userManager.GetCurrentUserId() && VideoStreamConnection != null)
            {
                // Disconnect user from the current media channel
                RemoveUserFromMediaChannel(mediaRoomId, userManager.GetCurrentUserId());

                // Dispose video connection
                VideoStreamConnection.Dispose();
                VideoStreamConnection = null;

                // Small delay to ensure proper cleanup
                await Task.Delay(1000);

                // Reset media control buttons to original state
                _form.mediaChannelMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground");
                _form.mediaChannelVideoMuteButton.BackColor = ThemeManager.GetColor("ButtonBackground");

                // Switch to text channel 1 (default)
                _form.GetChatManager().HideAllPanels();
                _form.ChatMessagesPanel1.Visible = true;
                _form.messageInputTextBox.Visible = true;
                _form.sendMessageButton.Visible = true;

                // Make sure to update emoji panel visibility
                _form.GetEmojiManager().UpdateEmojiPanelVisibility();

                // If chat history isn't loaded yet, load it
                if (((string)_form.ChatMessagesPanel1.Tag) == "0")
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
    }
}