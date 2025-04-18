using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using YoavDiscordClient.Style;

namespace YoavDiscordClient.Managers
{
    /// <summary>
    /// Manages context menus and right-click interactions for users in media channels.
    /// </summary>
    public class ContextMenuManager
    {
        private readonly DiscordApp _form;

        /// <summary>
        /// Context menu shown when right-clicking on users in the media channels.
        /// </summary>
        private ContextMenuStrip userContextMenu;

        public ContextMenuManager(DiscordApp form)
        {
            _form = form;
        }

        /// <summary>
        /// Initializes the context menu that appears when right-clicking on users in media channels.
        /// </summary>
        public void InitializeUserContextMenu()
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

        /// <summary>
        /// Shows the context menu for a user when right-clicking on their avatar or username.
        /// </summary>
        /// <param name="control">The control that was clicked.</param>
        /// <param name="location">The location where the click occurred.</param>
        /// <param name="user">The user details for which to show the context menu.</param>
        public void ShowUserContextMenu(Control control, Point location, UserDetails user)
        {
            userContextMenu.Tag = user;

            // Update the check states before showing
            UpdateContextMenuCheckStates(user);

            userContextMenu.Show(control, location);
        }

        /// <summary>
        /// Updates the check states and enabled states of items in the user context menu
        /// based on the user's role and current settings.
        /// </summary>
        /// <param name="user">The user details for which to update the menu.</param>
        private void UpdateContextMenuCheckStates(UserDetails user)
        {
            // Get current settings for the user
            var settings = UserContextMenuSettings.GetInstance().GetUserSettings(user.UserId);
            UserManager userManager = _form.GetUserManager();

            bool isCurrentUser = (user.UserId == userManager.GetCurrentUserId());

            // Determine if the current user has permission to modify this user based on roles
            bool isMyRoleStronger = false;
            if (user.role > userManager.GetUserRole())
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
                            menuItem.Enabled = !isCurrentUser && userManager.GetUserRole() <= 1; // Only Admin (0) and Moderator (1) can update roles

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
                                            roleItem.Enabled = (userManager.GetUserRole() == 0) || (userManager.GetUserRole() == 1 && user.role == 2);
                                            // Highlight the current role
                                            roleItem.Checked = user.role == 1;
                                            break;

                                        case "Member":
                                            // Admin can demote Moderator to Member, others can't change roles
                                            roleItem.Enabled = (userManager.GetUserRole() == 0 && user.role == 1);
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

        #region Context Menu Event Handlers

        /// <summary>
        /// Event handler for the "Mute User" menu item in the user context menu.
        /// </summary>
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

        /// <summary>
        /// Event handler for the "Mute Video" menu item in the user context menu.
        /// </summary>
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

        /// <summary>
        /// Event handler for the "Deafen User" menu item in the user context menu.
        /// </summary>
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

        /// <summary>
        /// Event handler for the "Disconnect User" menu item in the user context menu.
        /// </summary>
        private void DisconnectUserItem_Click(object sender, EventArgs e)
        {
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;
            MediaChannelManager mediaManager = _form.GetMediaChannelManager();

            // Find which media channel the user is in
            int userMediaChannelId = -1;
            foreach (var entry in mediaManager.UsersInMediaChannels)
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
                    mediaManager.RemoveUserFromMediaChannel(userMediaChannelId, targetUser.UserId);

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

        /// <summary>
        /// Event handler for the "Admin" role menu item in the user context menu.
        /// </summary>
        private void SetAdminRoleItem_Click(object sender, EventArgs e)
        {
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;
            ConnectionManager.GetInstance(null).ProcessUpdateUserRole(targetUser.UserId, 0); // 0 is Admin role
        }

        /// <summary>
        /// Event handler for the "Moderator" role menu item in the user context menu.
        /// </summary>
        private void SetModeratorRoleItem_Click(object sender, EventArgs e)
        {
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;
            ConnectionManager.GetInstance(null).ProcessUpdateUserRole(targetUser.UserId, 1); // 1 is Moderator role
        }

        /// <summary>
        /// Event handler for the "Member" role menu item in the user context menu.
        /// </summary>
        private void SetMemberRoleItem_Click(object sender, EventArgs e)
        {
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;
            ConnectionManager.GetInstance(null).ProcessUpdateUserRole(targetUser.UserId, 2); // 2 is Member role
        }

        /// <summary>
        /// Event handler for the "View Profile" menu item in the user context menu.
        /// </summary>
        private void ViewProfileItem_Click(object sender, EventArgs e)
        {
            UserDetails targetUser = (UserDetails)userContextMenu.Tag;
            UserManager userManager = _form.GetUserManager();
            MediaChannelManager mediaManager = _form.GetMediaChannelManager();

            // Check if user is in any media channel - if so, they should be shown as online
            bool isInAnyMediaChannel = false;
            foreach (var channelUsers in mediaManager.UsersInMediaChannels.Values)
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
                { 0, new Tuple<string, Color>("Admin", userManager.GetRoleColor(0)) },
                { 1, new Tuple<string, Color>("Moderator", userManager.GetRoleColor(1)) },
                { 2, new Tuple<string, Color>("Member", userManager.GetRoleColor(2)) }
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
                    Image = userManager.UsersImages[targetUser.UserId],
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

        #endregion

        #region Status Change Handlers

        /// <summary>
        /// Handles a user's mute status change notification from the server.
        /// </summary>
        public void HandleUserMuteStatusChanged(int userId, bool isMuted)
        {
            MediaChannelManager mediaManager = _form.GetMediaChannelManager();
            UserManager userManager = _form.GetUserManager();

            // Update the user settings in our manager
            UserContextMenuSettings.GetInstance().SetUserMuted(userId, isMuted);

            // Update visual indicators
            UpdateUserMuteVisualIndicator(userId, isMuted);

            // If this is about the current user, apply audio muting
            if (userId == userManager.GetCurrentUserId() && MediaChannelManager.VideoStreamConnection != null)
            {
                // Only toggle if the current state doesn't match the desired state
                bool isCurrentlyMuted = _form.mediaChannelMuteButton.BackColor == ThemeManager.GetColor("MutedColor");
                _form.mediaChannelMuteButton.Enabled = !isMuted;
                _form.globalMuteButton.Enabled = !isMuted;
                mediaManager.SetMutedByHigherRole(isMuted);

                if (isCurrentlyMuted != isMuted)
                {
                    MediaChannelManager.VideoStreamConnection.SetMutedByHigherRoleState(isMuted);
                    _form.mediaChannelMuteButton.BackColor = isMuted ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");
                    _form.globalMuteButton.BackColor = isMuted ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");
                }

                if (isMuted == false)
                {
                    // Reset global mute state if we're being unmuted by a higher role
                    mediaManager.UnmuteByHigherRole();
                }
            }
        }

        /// <summary>
        /// Handles a user's video mute status change notification from the server.
        /// </summary>
        public void HandleUserVideoMuteStatusChanged(int userId, bool isVideoMuted)
        {
            MediaChannelManager mediaManager = _form.GetMediaChannelManager();
            UserManager userManager = _form.GetUserManager();

            // Update the user settings in our manager
            UserContextMenuSettings.GetInstance().SetUserVideoMuted(userId, isVideoMuted);

            // Update visual indicators
            UpdateUserVideoMuteVisualIndicator(userId, isVideoMuted);

            // If this is about the current user, apply video muting
            if (userId == userManager.GetCurrentUserId() && MediaChannelManager.VideoStreamConnection != null)
            {
                // Update button state and track that this was done by a higher role
                _form.mediaChannelVideoMuteButton.Enabled = !isVideoMuted;
                mediaManager.SetVideoMutedByHigherRole(isVideoMuted);

                // Only toggle if the current state doesn't match the desired state
                bool isCurrentlyVideoMuted = _form.mediaChannelVideoMuteButton.BackColor == ThemeManager.GetColor("MutedColor");
                if (isCurrentlyVideoMuted != isVideoMuted)
                {
                    MediaChannelManager.VideoStreamConnection.ToggleVideoMute();
                    _form.mediaChannelVideoMuteButton.BackColor = isVideoMuted ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");
                }
            }
        }

        /// <summary>
        /// Handles a user's deafen status change notification from the server.
        /// </summary>
        public void HandleUserDeafenStatusChanged(int userId, bool isDeafened)
        {
            MediaChannelManager mediaManager = _form.GetMediaChannelManager();
            UserManager userManager = _form.GetUserManager();

            // Update the user settings in our manager
            UserContextMenuSettings.GetInstance().SetUserDeafened(userId, isDeafened);

            // Update visual indicators
            UpdateUserDeafenVisualIndicator(userId, isDeafened);

            // If this is about the current user, apply audio deafening
            if (userId == userManager.GetCurrentUserId() && MediaChannelManager.VideoStreamConnection != null)
            {
                // Only update if the current state doesn't match the desired state
                _form.deafenButton.Enabled = !isDeafened;
                mediaManager.SetDeafenedByHigherRole(isDeafened);

                if (mediaManager.IsGloballyDeafened() != isDeafened)
                {
                    MediaChannelManager.VideoStreamConnection.SetGlobalDeafenState(isDeafened);
                    _form.deafenButton.BackColor = isDeafened ? ThemeManager.GetColor("MutedColor") : ThemeManager.GetColor("ButtonBackground");
                    mediaManager.SetGloballyDeafened(isDeafened);
                }

                if (isDeafened == false)
                {
                    // Reset global deafen state if we're being undeafened by a higher role
                    mediaManager.SetGloballyDeafened(false);
                }
            }
        }

        /// <summary>
        /// Handles a user role update notification from the server.
        /// </summary>
        public void HandleUserRoleUpdated(int userId, int newRole)
        {
            MediaChannelManager mediaManager = _form.GetMediaChannelManager();
            UserManager userManager = _form.GetUserManager();

            // Check if this is the current user's role being updated
            if (userId == userManager.GetCurrentUserId())
            {
                // Update the current user's role
                userManager.SetUserRole(newRole);
            }

            // Find the user's username from existing data
            string username = "Unknown";

            // Check in all media channels
            foreach (var channelUsers in mediaManager.UsersInMediaChannels.Values)
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
            foreach (var channelEntry in mediaManager.UsersInMediaChannels)
            {
                mediaManager.UpdateMediaChannelUsers(channelEntry.Key, channelEntry.Value);
            }

            // Request a refresh of the users list to update their display in the sidebar
            ConnectionManager.GetInstance(null).ProcessFetchAllUsers();

            // Show a notification to the user
            MessageBox.Show($"{username}'s role has been updated to {GetRoleNameFromId(newRole)}");
        }

        /// <summary>
        /// Converts a role ID to its human-readable name.
        /// </summary>
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
        #endregion

        #region Status Indicators

        /// <summary>
        /// Repositions status icons within a user panel to ensure proper layout.
        /// </summary>
        /// <param name="userPanel">The panel containing status icons.</param>
        /// <param name="usernameLabel">The username label to position icons relative to.</param>
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

        /// <summary>
        /// Updates the visual indicator showing a user's mute status in the media channel display.
        /// </summary>
        public void UpdateUserMuteVisualIndicator(int userId, bool isMuted)
        {
            MediaChannelManager mediaManager = _form.GetMediaChannelManager();
            Dictionary<int, Panel> mediaChannelPanels = mediaManager.GetMediaChannelPanels();

            // This method updates visual indicators for muted users
            foreach (var channelId in mediaChannelPanels.Keys)
            {
                if (mediaManager.UsersInMediaChannels.ContainsKey(channelId))
                {
                    var userInChannel = mediaManager.UsersInMediaChannels[channelId].FirstOrDefault(u => u.UserId == userId);
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

        /// <summary>
        /// Updates the visual indicator showing a user's video mute status in the media channel display.
        /// </summary>
        public void UpdateUserVideoMuteVisualIndicator(int userId, bool isVideoMuted)
        {
            MediaChannelManager mediaManager = _form.GetMediaChannelManager();
            Dictionary<int, Panel> mediaChannelPanels = mediaManager.GetMediaChannelPanels();

            foreach (var channelId in mediaChannelPanels.Keys)
            {
                if (mediaManager.UsersInMediaChannels.ContainsKey(channelId))
                {
                    var userInChannel = mediaManager.UsersInMediaChannels[channelId].FirstOrDefault(u => u.UserId == userId);
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

        /// <summary>
        /// Updates the visual indicator showing a user's deafen status in the media channel display.
        /// </summary>
        public void UpdateUserDeafenVisualIndicator(int userId, bool isDeafened)
        {
            MediaChannelManager mediaManager = _form.GetMediaChannelManager();
            Dictionary<int, Panel> mediaChannelPanels = mediaManager.GetMediaChannelPanels();

            foreach (var channelId in mediaChannelPanels.Keys)
            {
                if (mediaManager.UsersInMediaChannels.ContainsKey(channelId))
                {
                    var userInChannel = mediaManager.UsersInMediaChannels[channelId].FirstOrDefault(u => u.UserId == userId);
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

        #endregion
    }
}



        
        

