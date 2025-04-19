using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YoavDiscordClient.Style;

namespace YoavDiscordClient.Managers
{
    /// <summary>
    /// Manages user-related functionality including profiles, roles, and UI representation.
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// Reference to the main Discord application form that owns this manager.
        /// Used to access application-wide functionality and other managers.
        /// </summary>
        private readonly DiscordApp _form;

        /// <summary>
        /// The panel on the right side of the UI displaying user information.
        /// This panel contains user lists, profile information, and status indicators.
        /// </summary>
        private readonly Panel _rightSidePanel;

        /// <summary>
        /// The current user's profile picture.
        /// </summary>
        public Image UserProfilePicture;

        /// <summary>
        /// The current user's username.
        /// </summary>
        public string Username;

        /// <summary>
        /// The unique ID of the current user.
        /// </summary>
        private int _currentUserId;

        /// <summary>
        /// Dictionary mapping user IDs to their profile images.
        /// </summary>
        public Dictionary<int, Image> UsersImages = new Dictionary<int, Image>();

        /// <summary>
        /// List of user IDs for whom profile images have already been requested.
        /// Used to prevent duplicate requests.
        /// </summary>
        private List<int> _alreadyAskedForImage = new List<int>();

        /// <summary>
        /// The role level of the current user.
        /// Lower numbers indicate higher privileges (0 = Admin, 1 = Moderator, 2 = Member).
        /// </summary>
        private int _role;

        /// <summary>
        /// Initializes a new instance of the UserManager class.
        /// </summary>
        /// <param name="form">The main Discord application form that owns this manager.</param>
        /// <param name="rightSidePanel">The panel on the right side of the UI for displaying user information.</param>
        /// <remarks>
        /// The UserManager is responsible for:
        /// - Managing user profiles and status information
        /// - Displaying the user list in the right side panel
        /// - Handling user selection and context menu interactions
        /// - Updating user status indicators (online, offline, muted, etc.)
        /// - Loading and caching user profile pictures
        /// - Coordinating with the server for user data updates
        /// 
        /// It maintains a reference to the main form and the right side panel to
        /// coordinate user-related UI updates throughout the application.
        /// </remarks>
        public UserManager(DiscordApp form, Panel rightSidePanel)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));
            _rightSidePanel = rightSidePanel ?? throw new ArgumentNullException(nameof(rightSidePanel));
        }

        /// <summary>
        /// Sets the current user's profile information in the UI.
        /// </summary>
        /// <param name="profilePicture">Byte array containing the profile picture data.</param>
        /// <param name="username">The user's username.</param>
        /// <param name="userId">The user's unique ID.</param>
        /// <param name="role">The user's role level (0 = Admin, 1 = Moderator, 2 = Member).</param>
        public void SetUsernameAndProfilePicture(byte[] profilePicture, string username, int userId, int role)
        {
            this.UserProfilePicture = this.ByteArrayToImage(profilePicture);
            this.Username = username;
            this._currentUserId = userId;
            this._form.userProfilePicturePictureBox.Image = this.UserProfilePicture;
            this._form.usernameLabel.Text = username;
            this._role = role;
        }

        /// <summary>
        /// Converts a byte array to an Image object.
        /// </summary>
        /// <param name="byteArray">Byte array containing image data.</param>
        /// <returns>The converted Image object.</returns>
        public Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Updates a user's profile image in the UI.
        /// Finds all controls displaying the user's image and updates them.
        /// </summary>
        /// <param name="userId">The ID of the user whose image should be updated.</param>
        /// <param name="profilePicture">Byte array containing the new profile picture data.</param>
        public void UpdateUserImage(int userId, byte[] profilePicture)
        {
            this.UsersImages[userId] = this.ByteArrayToImage(profilePicture);
            List<Control> controls = this.FindControlsByTag(_form.chatAreaPanel, $"UserImage_{userId}");
            Image imageOfUser = this.ByteArrayToImage(profilePicture);
            foreach (Control control in controls)
            {
                ((CirclePictureBox)control).Image = imageOfUser;
            }
        }

        /// <summary>
        /// Adds a new user's profile image and displays their message.
        /// Called when a user's profile image is received from the server.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="profilePicture">Byte array containing the profile picture data.</param>
        /// <param name="username">The user's username.</param>
        /// <param name="messageThatTheUserSent">The message content.</param>
        /// <param name="timeThatTheMessageWasSent">The timestamp of the message.</param>
        /// <param name="chatRoomId">The ID of the chat room.</param>
        public void AddNewUserImageAndShowItsMessage(int userId, byte[] profilePicture, string username, string messageThatTheUserSent,
            DateTime timeThatTheMessageWasSent, int chatRoomId)
        {
            this.UsersImages[userId] = this.ByteArrayToImage(profilePicture);
            this._alreadyAskedForImage.Remove(userId);

            // Call the ChatManager through the main form to display the message
            _form.GetChatManager().AddMessageToChatFromOtherUser(username, userId, messageThatTheUserSent, timeThatTheMessageWasSent, chatRoomId);
        }

        /// <summary>
        /// Updates the user list in the right side panel, showing online and offline users grouped by role.
        /// </summary>
        /// <param name="details">List of user details to display.</param>
        public void ShowAllUsersDetails(List<UserDetails> details)
        {
            // Clear existing controls in rightSidePanel except essential controls
            _rightSidePanel.Controls.Clear();

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
            _rightSidePanel.Controls.Add(onlineLabel);

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
                _rightSidePanel.Controls.Add(roleHeader);
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
                        _form.GetMediaChannelManager().AddUserToMediaChannel(user.MediaChannelId, user);
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
            _rightSidePanel.Controls.Add(offlineLabel);
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

        /// <summary>
        /// Creates and adds a user entry to the right side panel.
        /// </summary>
        /// <param name="user">User details to display.</param>
        /// <param name="yPosition">Vertical position for the entry.</param>
        /// <param name="isOnline">Whether the user is online or offline.</param>
        /// <param name="roleColor">The color to use for the username based on user's role.</param>
        private void AddUserEntry(UserDetails user, int yPosition, bool isOnline, Color roleColor)
        {
            // Create panel for user entry
            Panel userPanel = new Panel
            {
                Width = _rightSidePanel.Width - 20,
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
            _rightSidePanel.Controls.Add(userPanel);
        }

        /// <summary>
        /// Finds all controls within a panel that have a specific tag.
        /// </summary>
        /// <param name="panel">The panel to search within.</param>
        /// <param name="searchText">The tag text to search for.</param>
        /// <returns>A list of controls that match the search criteria.</returns>
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

        /// <summary>
        /// Adds various icon images to UI elements in the window.
        /// </summary>
        public void AddPicturesToTheWindow()
        {
            Image originalSettingsLogoImage = Properties.Resources.settingsLogo;
            Image originalDeafenLogoImage = Properties.Resources.deafenLogo;
            Image originalMuteLogoImage = Properties.Resources.muteLogo;
            Image originalVideoMuteLogoImage = Properties.Resources.videoMuteLogo;
            Image originalDisconnectMediaChannelImage = Properties.Resources.disconnectMediaChannelLogo;

            _form.deafenButton.Image = ResizeImage(originalDeafenLogoImage, _form.deafenButton.Width, _form.deafenButton.Height);
            _form.globalMuteButton.Image = ResizeImage(originalMuteLogoImage, _form.globalMuteButton.Width, _form.globalMuteButton.Height);
            // Add images for new media control buttons
            _form.mediaChannelMuteButton.Image = ResizeImage(originalMuteLogoImage, _form.mediaChannelMuteButton.Width, _form.mediaChannelMuteButton.Height);
            _form.mediaChannelVideoMuteButton.Image = ResizeImage(originalVideoMuteLogoImage, _form.mediaChannelVideoMuteButton.Width, _form.mediaChannelVideoMuteButton.Height);
            _form.mediaChannelDisconnectButton.Image = ResizeImage(originalDisconnectMediaChannelImage, _form.mediaChannelDisconnectButton.Width,
                _form.mediaChannelDisconnectButton.Height);
        }

        /// <summary>
        /// Resizes an image to the specified dimensions.
        /// </summary>
        /// <param name="imgToResize">The image to resize.</param>
        /// <param name="width">The target width.</param>
        /// <param name="height">The target height.</param>
        /// <returns>The resized image.</returns>
        private Image ResizeImage(Image imgToResize, int width, int height)
        {
            return new Bitmap(imgToResize, new Size(width, height));
        }

        /// <summary>
        /// Converts an Image object to a byte array.
        /// If conversion fails, creates and returns a gray placeholder image.
        /// </summary>
        /// <param name="image">The image to convert.</param>
        /// <returns>A byte array representing the image data in PNG format.</returns>
        public byte[] ImageToByteArray(Image image)
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

        /// <summary>
        /// Gets the color to use for a user based on their role and online status.
        /// </summary>
        /// <param name="roleNumber">The role number (0 = Admin, 1 = Moderator, 2 = Member).</param>
        /// <param name="isOnline">Whether the user is online or offline.</param>
        /// <returns>The color to use for the user's name in the UI.</returns>
        public Color GetRoleColor(int roleNumber, bool isOnline = true)
        {
            return ThemeManager.GetRoleColor(roleNumber, isOnline);
        }

        /// <summary>
        /// Gets the current user's ID.
        /// </summary>
        public int GetCurrentUserId()
        {
            return _currentUserId;
        }

        /// <summary>
        /// Gets the current user's role.
        /// </summary>
        public int GetUserRole()
        {
            return _role;
        }

        /// <summary>
        /// Updates the current user's role.
        /// </summary>
        public void SetUserRole(int role)
        {
            _role = role;
        }
    }
}