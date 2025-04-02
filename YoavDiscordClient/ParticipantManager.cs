using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoavDiscordClient.Enums;
using YoavDiscordClient.Events;

namespace YoavDiscordClient
{
    public class ParticipantManager : IDisposable
    {
        #region Events

        /// <summary>
        /// Occurs when a participant's status changes
        /// </summary>
        public event EventHandler<ParticipantStatusEventArgs> ParticipantStatusChanged;

        #endregion

        #region Private properties

        private readonly Panel remotePanel;

        private Dictionary<string, PictureBox> displays;

        private Dictionary<string, FrameAssembler> frameAssemblers;

        private Dictionary<int, string> userIdToIp;

        private bool disposed = false;

        private readonly object parent;

        #endregion

        /// <summary>
        /// Creates a new ParticipantManager
        /// </summary>
        /// <param name="remotePanel">The panel where participant videos will be displayed</param>
        /// <param name="parent">The parent connection</param>
        public ParticipantManager(Panel remotePanel, object parent)
        {
            this.remotePanel = remotePanel ?? throw new ArgumentNullException(nameof(remotePanel));
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));

            this.displays = new Dictionary<string, PictureBox>();
            this.frameAssemblers = new Dictionary<string, FrameAssembler>();
            this.userIdToIp = new Dictionary<int, string>();

            // Setup resize event handler
            this.remotePanel.Resize += (sender, e) => UpdateVideoLayout();
        }

        /// <summary>
        /// Creates the local display for the current user
        /// </summary>
        /// <returns>The PictureBox for the local display</returns>
        public PictureBox CreateLocalDisplay()
        {
            var display = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(47, 49, 54),
                Dock = DockStyle.None,
                Size = new Size(640, 480)  // Initial size
            };

            remotePanel.Controls.Add(display);
            displays["local"] = display;

            UpdateVideoLayout();

            return display;
        }

        /// <summary>
        /// Adds a new participant to the video session
        /// </summary>
        /// <param name="ip">The participant's IP address</param>
        /// <param name="userId">The participant's user ID</param>
        /// <param name="profilePicture">The participant's profile picture</param>
        /// <param name="username">The participant's username</param>
        /// <returns>True if the participant was successfully added</returns>
        public async Task<bool> AddParticipant(string ip, int userId, byte[] profilePicture, string username)
        {
            if (string.IsNullOrEmpty(ip) || userId <= 0)
                return false;

            try
            {
                System.Diagnostics.Debug.WriteLine($"Adding participant: {username} (ID: {userId}, IP: {ip})");

                // Map user ID to IP
                userIdToIp[userId] = ip;

                // Get user role from UsersImages dictionary
                var user = DiscordFormsHolder.getInstance().DiscordApp.UsersImages.Keys
                    .Select(id => DiscordFormsHolder.getInstance().DiscordApp
                        .usersInMediaChannels.Values
                        .SelectMany(list => list)
                        .FirstOrDefault(u => u.UserId == id))
                    .FirstOrDefault(u => u != null && u.UserId == userId);

                // Default to Member role if user is not found
                int userRole = (user != null) ? user.role : 2;

                // Get the appropriate color for the user based on their role
                Color userColor = DiscordFormsHolder.getInstance().DiscordApp.GetRoleColor(userRole);

                // Create the display for this participant
                var display = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.FromArgb(47, 49, 54),
                    Dock = DockStyle.None,
                    Size = new Size(640, 480),  // Initial size
                    Tag = new { ProfilePicture = profilePicture, Username = username, UserId = userId, RoleColor = userColor }
                };

                // Add the display to the UI thread-safely
                await Task.Run(() => {
                    try
                    {
                        DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        {
                            DiscordFormsHolder.getInstance().DiscordApp.AddNewParticipantDisplay(remotePanel, display);
                            displays[ip] = display;
                            UpdateVideoLayout();
                        }));
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error adding participant display: {ex.Message}");
                    }
                });

                // Create a frame assembler for this participant
                frameAssemblers[ip] = new FrameAssembler();

                // Raise participant status changed event
                ParticipantStatusChanged?.Invoke(this, new ParticipantStatusEventArgs(
                    ip, ParticipantStatusType.Connected));

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding participant: {username} (ID: {userId}): {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Removes a participant from the video session by user ID
        /// </summary>
        /// <param name="userId">The participant's user ID</param>
        /// <returns>True if the participant was successfully removed</returns>
        public bool RemoveParticipantById(int userId)
        {
            if (!userIdToIp.ContainsKey(userId))
                return false;

            string ip = userIdToIp[userId];
            bool result = RemoveParticipantByIp(ip);

            if (result)
            {
                userIdToIp.Remove(userId);
            }

            return result;
        }

        /// <summary>
        /// Removes a participant from the video session by IP address
        /// </summary>
        /// <param name="ip">The participant's IP address</param>
        /// <returns>True if the participant was successfully removed</returns>
        public bool RemoveParticipantByIp(string ip)
        {
            if (string.IsNullOrEmpty(ip) || !displays.ContainsKey(ip))
                return false;

            try
            {
                System.Diagnostics.Debug.WriteLine($"Removing participant with IP: {ip}");

                // Remove and dispose of their display
                var displayToRemove = displays[ip];
                if (!remotePanel.IsDisposed && remotePanel.IsHandleCreated)
                {
                    remotePanel.Invoke(new Action(() =>
                    {
                        try
                        {
                            // Clear all controls and images
                            displayToRemove.Controls.Clear();
                            if (displayToRemove.Image != null)
                            {
                                displayToRemove.Image.Dispose();
                                displayToRemove.Image = null;
                            }

                            // Remove from parent panel and dispose
                            remotePanel.Controls.Remove(displayToRemove);
                            displayToRemove.Dispose();

                            // Remove from displays dictionary
                            displays.Remove(ip);

                            // Update layout
                            if (displays.Count == 1) // Only local display remains
                            {
                                ResetDisplay();
                            }
                            else
                            {
                                UpdateVideoLayout();
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error removing display for {ip}: {ex.Message}");
                        }
                    }));
                }

                // Remove their frame assembler
                if (frameAssemblers.ContainsKey(ip))
                {
                    frameAssemblers.Remove(ip);
                }

                // Remove from userIdToIp mapping
                foreach (var kvp in userIdToIp.ToList())
                {
                    if (kvp.Value == ip)
                    {
                        userIdToIp.Remove(kvp.Key);
                    }
                }

                // Raise participant status changed event
                ParticipantStatusChanged?.Invoke(this, new ParticipantStatusEventArgs(
                    ip, ParticipantStatusType.Disconnected));

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing participant with IP: {ip}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Processes incoming video data from a participant
        /// </summary>
        /// <param name="ip">The participant's IP address</param>
        /// <param name="packet">The video packet</param>
        /// <returns>True if the data was successfully processed</returns>
        public bool ProcessVideoData(string ip, VideoPacket packet)
        {
            try
            {
                // Ignore if we don't have a display for this participant
                if (!displays.ContainsKey(ip) || !frameAssemblers.ContainsKey(ip))
                    return false;

                var frameData = frameAssemblers[ip].AddPacket(packet);
                if (frameData == null)
                    return true; // Frame is incomplete, wait for more packets

                // Process the complete frame
                using (var ms = new MemoryStream(frameData))
                {
                    try
                    {
                        // Create bitmap from frame data
                        var bitmap = new Bitmap(ms);

                        // Update display on UI thread
                        if (!displays[ip].IsDisposed && displays[ip].IsHandleCreated)
                        {
                            displays[ip].Invoke(new Action(() =>
                            {
                                try
                                {
                                    // First clear any existing background panel
                                    displays[ip].Controls.Clear();

                                    var oldImage = displays[ip].Image;
                                    displays[ip].Image = bitmap;
                                    oldImage?.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error updating display for {ip}: {ex.Message}");
                                }
                            }));
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error creating bitmap from video data for {ip}: {ex.Message}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing video packet for {ip}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Processes empty video notification (camera off) from a participant
        /// </summary>
        /// <param name="ip">The participant's IP address</param>
        /// <returns>True if the notification was successfully processed</returns>
        public bool ProcessEmptyVideo(string ip)
        {
            try
            {
                if (!displays.ContainsKey(ip) || displays[ip].IsDisposed || !displays[ip].IsHandleCreated)
                    return false;

                displays[ip].Invoke(new Action(() =>
                {
                    try
                    {
                        // Clear existing display
                        displays[ip].Controls.Clear();
                        if (displays[ip].Image != null)
                        {
                            displays[ip].Image.Dispose();
                            displays[ip].Image = null;
                        }

                        var userInfo = (dynamic)displays[ip].Tag;

                        // Set fixed size for display
                        displays[ip].Size = new Size(640, 480);

                        // Create background panel with profile picture
                        Panel backgroundPanel = new Panel
                        {
                            Name = "backgroundPanel",
                            Size = displays[ip].Size,
                            BackColor = Color.FromArgb(47, 49, 54),
                            BorderStyle = BorderStyle.FixedSingle,
                            Location = new Point(0, 0)
                        };

                        // Add profile picture
                        CirclePictureBox profilePicture = new CirclePictureBox
                        {
                            Size = new Size(100, 100),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Image = ByteArrayToImage((byte[])userInfo.ProfilePicture),
                            Location = new Point(
                                (displays[ip].Width - 100) / 2,
                                (displays[ip].Height - 100) / 2
                            )
                        };

                        // Get the appropriate color for the user based on their role
                        Color userColor = Color.White; // Default color
                        if (userInfo.RoleColor != null)
                        {
                            userColor = (Color)userInfo.RoleColor;
                        }

                        // Add username label
                        Label usernameLabel = new Label
                        {
                            Text = (string)userInfo.Username,
                            ForeColor = userColor,
                            Font = new Font("Arial", 12, FontStyle.Regular),
                            AutoSize = true,
                            BackColor = Color.Transparent
                        };

                        // Position the label
                        usernameLabel.Location = new Point(
                            displays[ip].Width - usernameLabel.PreferredWidth - 10,
                            displays[ip].Height - usernameLabel.PreferredHeight - 10
                        );

                        // Add controls to panel and update display
                        backgroundPanel.Controls.Add(profilePicture);
                        backgroundPanel.Controls.Add(usernameLabel);
                        displays[ip].Controls.Add(backgroundPanel);

                        // Force a layout update
                        displays[ip].Refresh();
                        UpdateVideoLayout();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error processing empty video for {ip}: {ex.Message}");
                    }
                }));

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing empty video for {ip}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets the IP address for a user ID
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>The IP address, or null if not found</returns>
        public string GetIpForUserId(int userId)
        {
            return userIdToIp.ContainsKey(userId) ? userIdToIp[userId] : null;
        }

        /// <summary>
        /// Updates the layout of all video displays
        /// </summary>
        public void UpdateVideoLayout()
        {
            // Get total number of participants (including local)
            int totalParticipants = displays.Count;
            if (totalParticipants == 0)
                return;

            // Panel dimensions
            int panelWidth = remotePanel.Width;
            int panelHeight = remotePanel.Height;

            // Calculate the optimal display size based on number of participants
            Size displaySize = CalculateOptimalDisplaySize(totalParticipants, panelWidth, panelHeight);

            // Calculate positions for each display
            Dictionary<string, Point> positions = CalculatePositions(totalParticipants, displaySize, panelWidth, panelHeight);

            // Update each display with the same size
            foreach (var kvp in displays)
            {
                var display = kvp.Value;
                if (display.IsDisposed || !display.IsHandleCreated)
                    continue;

                display.Invoke(new Action(() =>
                {
                    try
                    {
                        // Set exact same size for all displays
                        display.Size = displaySize;
                        display.Dock = DockStyle.None;

                        if (positions.ContainsKey(kvp.Key))
                        {
                            display.Location = positions[kvp.Key];
                        }

                        // If this has a background panel (no-camera display), make it exactly the same size
                        var backgroundPanel = display.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "backgroundPanel");
                        if (backgroundPanel != null)
                        {
                            backgroundPanel.Size = displaySize;
                            backgroundPanel.Dock = DockStyle.None;

                            // Center the profile picture
                            var profilePicture = backgroundPanel.Controls.OfType<CirclePictureBox>().FirstOrDefault();
                            if (profilePicture != null)
                            {
                                profilePicture.Location = new Point(
                                    (displaySize.Width - profilePicture.Width) / 2,
                                    (displaySize.Height - profilePicture.Height) / 2
                                );
                            }

                            // Keep username at bottom-right
                            var usernameLabel = backgroundPanel.Controls.OfType<Label>().FirstOrDefault();
                            if (usernameLabel != null)
                            {
                                usernameLabel.Location = new Point(
                                    displaySize.Width - usernameLabel.Width - 10,
                                    displaySize.Height - usernameLabel.Height - 10
                                );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error updating video layout: {ex.Message}");
                    }
                }));
            }
        }

        private Size CalculateOptimalDisplaySize(int totalParticipants, int panelWidth, int panelHeight)
        {
            // Calculate the optimal display size based on number of participants
            Size displaySize;

            switch (totalParticipants)
            {
                case 1:
                    // Single video takes up more space
                    displaySize = new Size(panelWidth / 2, panelHeight / 2);
                    break;

                case 2:
                    // Two videos side by side
                    displaySize = new Size(400, 300);
                    break;

                case 3:
                case 4:
                    // Grid layout for 3-4 participants
                    displaySize = new Size(300, 250);
                    break;

                default:
                    // More than 4 participants, smaller tiles
                    displaySize = new Size(240, 180);
                    break;
            }

            return displaySize;
        }

        private Dictionary<string, Point> CalculatePositions(int totalParticipants, Size videoSize, int panelWidth, int panelHeight)
        {
            var positions = new Dictionary<string, Point>();
            var participants = displays.Keys.ToList();

            switch (totalParticipants)
            {
                case 1:
                    // Single centered video
                    positions[participants[0]] = new Point(
                        (panelWidth - videoSize.Width) / 2,
                        (panelHeight - videoSize.Height) / 2
                    );
                    break;

                case 2:
                    // Two videos side by side
                    int x = (panelWidth - (videoSize.Width * 2)) / 3; // Space between and on sides
                    int y = (panelHeight - videoSize.Height) / 2;
                    positions[participants[0]] = new Point(x, y);
                    positions[participants[1]] = new Point(x * 2 + videoSize.Width, y);
                    break;

                case 3:
                    // Triangle formation
                    x = (panelWidth - (videoSize.Width * 2)) / 3;
                    int topY = panelHeight / 4;
                    int bottomY = topY + videoSize.Height + 20;
                    positions[participants[0]] = new Point(x, topY);
                    positions[participants[1]] = new Point(x * 2 + videoSize.Width, topY);
                    positions[participants[2]] = new Point(
                        (panelWidth - videoSize.Width) / 2,
                        bottomY
                    );
                    break;

                case 4:
                    // 2x2 grid
                    x = (panelWidth - (videoSize.Width * 2)) / 3;
                    int y1 = panelHeight / 4;
                    int y2 = y1 + videoSize.Height + 20;
                    positions[participants[0]] = new Point(x, y1);
                    positions[participants[1]] = new Point(x * 2 + videoSize.Width, y1);
                    positions[participants[2]] = new Point(x, y2);
                    positions[participants[3]] = new Point(x * 2 + videoSize.Width, y2);
                    break;

                default:
                    // Grid layout for more participants
                    int cols = (int)Math.Ceiling(Math.Sqrt(totalParticipants));
                    int rows = (int)Math.Ceiling((double)totalParticipants / cols);

                    int xSpacing = (panelWidth - (videoSize.Width * cols)) / (cols + 1);
                    int ySpacing = (panelHeight - (videoSize.Height * rows)) / (rows + 1);

                    for (int i = 0; i < totalParticipants; i++)
                    {
                        int col = i % cols;
                        int row = i / cols;

                        positions[participants[i]] = new Point(
                            xSpacing + col * (videoSize.Width + xSpacing),
                            ySpacing + row * (videoSize.Height + ySpacing)
                        );
                    }
                    break;
            }

            return positions;
        }

        private void ResetDisplay()
        {
            // Reset all displays
            foreach (var display in displays.Values)
            {
                if (display.IsDisposed)
                    continue;

                display.Controls.Clear();
                if (display.Image != null)
                {
                    display.Image.Dispose();
                    display.Image = null;
                }
            }

            // Clear and reinitialize the local display
            if (displays.ContainsKey("local") && !displays["local"].IsDisposed)
            {
                var localDisplay = displays["local"];
                localDisplay.Size = new Size(640, 480);
                localDisplay.Location = new Point(
                    (remotePanel.Width - 640) / 2,
                    (remotePanel.Height - 480) / 2
                );
            }

            UpdateVideoLayout();
            if (!remotePanel.IsDisposed)
                remotePanel.Refresh();
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(byteArray))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error converting byte array to image: {ex.Message}");

                // Return a default gray image in case of error
                Bitmap defaultImage = new Bitmap(100, 100);
                using (Graphics g = Graphics.FromImage(defaultImage))
                {
                    g.Clear(Color.Gray);
                }
                return defaultImage;
            }
        }

        /// <summary>
        /// Releases all resources used by the ParticipantManager
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the ParticipantManager
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    System.Diagnostics.Debug.WriteLine("Inside the dispose method in Participant Manager class");

                    // Check if we can access the remotePanel
                    if (remotePanel != null && !remotePanel.IsDisposed && remotePanel.IsHandleCreated)
                    {
                        try
                        {
                            // Use Invoke to ensure we're on the UI thread
                            remotePanel.Invoke(new Action(() =>
                            {
                                try
                                {
                                    // First, suspend layout to prevent flickering
                                    remotePanel.SuspendLayout();

                                    // Process each display
                                    foreach (var display in displays.Values)
                                    {
                                        if (display != null && !display.IsDisposed)
                                        {
                                            System.Diagnostics.Debug.WriteLine($"Cleaning up display in ParticipantManager");

                                            // Clear any child controls
                                            display.Controls.Clear();

                                            // Dispose of any image
                                            if (display.Image != null)
                                            {
                                                var oldImage = display.Image;
                                                display.Image = null;  // Set to null first
                                                oldImage.Dispose();
                                            }

                                            // Remove from parent
                                            remotePanel.Controls.Remove(display);

                                            // Dispose the control
                                            display.Dispose();
                                        }
                                    }

                                    // Completely clear the remotePanel to ensure no remnants
                                    // This is crucial as there might be other controls we're not tracking
                                    remotePanel.Controls.Clear();

                                    // Force a repaint to ensure visuals are updated
                                    remotePanel.Invalidate(true);
                                    remotePanel.Update();
                                    remotePanel.Refresh();

                                    // Resume layout
                                    remotePanel.ResumeLayout(true);

                                    // Clear our collections
                                    displays.Clear();
                                    frameAssemblers.Clear();
                                    userIdToIp.Clear();

                                    System.Diagnostics.Debug.WriteLine("ParticipantManager displays cleared and disposed");
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error in UI thread when disposing displays: {ex.Message}");
                                }
                            }));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error invoking UI thread for display cleanup: {ex.Message}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("RemotePanel is null, disposed, or handle not created - cannot clean up displays");
                    }
                }

                disposed = true;
            }
        }
    }
}
