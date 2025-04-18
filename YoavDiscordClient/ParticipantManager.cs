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
            display.Resize += PrintLocalVideoSize;

            // Make sure the display is added to the panel and brought to front
            remotePanel.Controls.Add(display);
            displays["local"] = display;

            UpdateVideoLayout();
            remotePanel.Refresh(); // Force a refresh of the panel

            //Task.Delay(30000).ContinueWith(t => UpdateVideoLayout(true));

            return display;
        }

        private void PrintLocalVideoSize(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Resize event: {((PictureBox)sender).Size}");

        }

        /// <summary>
        /// Adds a new participant to the video session
        /// </summary>
        /// <param name="ip">The participant's IP address</param>
        /// <param name="userId">The participant's user ID</param>
        /// <param name="profilePicture">The participant's profile picture</param>
        /// <param name="username">The participant's username</param>
        /// <returns>True if the participant was successfully added</returns>
        public bool AddParticipant(string ip, int userId, byte[] profilePicture, string username)
        {
            if (string.IsNullOrEmpty(ip) || userId <= 0)
                return false;

            try
            {
                System.Diagnostics.Debug.WriteLine($"Adding participant: {username} (ID: {userId}, IP: {ip})");

                // Map user ID to IP
                userIdToIp[userId] = ip;

                // Get UserManager and MediaChannelManager references
                var userManager = DiscordFormsHolder.getInstance().DiscordApp.GetUserManager();
                var mediaManager = DiscordFormsHolder.getInstance().DiscordApp.GetMediaChannelManager();


                // Get user role from UsersImages dictionary
                var user = userManager.UsersImages.Keys
                    .Select(id => mediaManager.UsersInMediaChannels.Values
                        .SelectMany(list => list)
                        .FirstOrDefault(u => u.UserId == id))
                    .FirstOrDefault(u => u != null && u.UserId == userId);

                // Default to Member role if user is not found
                int userRole = (user != null) ? user.role : 2;

                // Get the appropriate color for the user based on their role
                Color userColor = userManager.GetRoleColor(userRole);

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
                DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                {
                    try
                    {
                    
                        DiscordFormsHolder.getInstance().DiscordApp.AddNewParticipantDisplay(remotePanel, display);
                        displays[ip] = display;
                        UpdateVideoLayout();
                        remotePanel.Refresh();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error adding participant display: {ex.Message}");
                    }
                }));


                // Create a frame assembler for this participant
                frameAssemblers[ip] = new FrameAssembler();

                

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
                        remotePanel.Refresh();
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

                System.Diagnostics.Debug.WriteLine($"Processing empty video for {ip}");

                // Check for recent updates differently
                bool skipUpdate = false;
                object tagObj = displays[ip].Tag;

                if (tagObj != null)
                {
                    // Try to get the LastEmptyVideoUpdate property if it exists
                    try
                    {
                        var lastUpdate = (DateTime?)GetPropertyValue(tagObj, "LastEmptyVideoUpdate");
                        if (lastUpdate.HasValue && (DateTime.Now - lastUpdate.Value).TotalMilliseconds < 500)
                        {
                            skipUpdate = true;
                        }
                    }
                    catch
                    {
                        // Property doesn't exist, continue processing
                    }
                }

                if (skipUpdate)
                    return true;


                displays[ip].Invoke(new Action(() =>
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine($"Updating display for {ip} to show profile picture");

                        // Check if profile picture is already displayed
                        if (displays[ip].Controls.Count > 0 &&
                            displays[ip].Controls[0] is Panel &&
                            displays[ip].Controls[0].Name == "backgroundPanel")
                        {
                            // Just ensure the panel is visible and correctly sized
                            displays[ip].Controls[0].Size = displays[ip].Size;
                            displays[ip].Refresh();
                            return; // Exit early, don't recreate the panel
                        }

                        // Clear existing display
                        displays[ip].Controls.Clear();
                        if (displays[ip].Image != null)
                        {
                            displays[ip].Image.Dispose();
                            displays[ip].Image = null;
                        }

                        object userInfo = (dynamic)displays[ip].Tag;
                        // Extract needed properties
                        byte[] profilePicture = null;
                        string username = "User";
                        Color roleColor = Color.White;

                        // Try to extract profile picture
                        if (userInfo is UserDetails details)
                        {
                            profilePicture = details.Picture;
                            username = details.Username;
                            roleColor = DiscordFormsHolder.getInstance().DiscordApp.GetUserManager().GetRoleColor(details.role);
                        }
                        else
                        {
                            // Try to extract using reflection
                            profilePicture = (byte[])GetPropertyValue(userInfo, "ProfilePicture");
                            username = (string)GetPropertyValue(userInfo, "Username");
                            var colorObj = GetPropertyValue(userInfo, "RoleColor");
                            if (colorObj is Color color)
                                roleColor = color;
                        }

                        // Update timestamp to prevent frequent refreshes
                        if (userInfo is IDictionary<string, object> dict)
                        {
                            dict["LastEmptyVideoUpdate"] = DateTime.Now;
                        }
                        else
                        {
                            // Try setting through reflection
                            try
                            {
                                var type = userInfo.GetType();
                                var prop = type.GetProperty("LastEmptyVideoUpdate");
                                if (prop != null && prop.CanWrite)
                                {
                                    prop.SetValue(userInfo, DateTime.Now);
                                }
                            }
                            catch
                            {
                                // Ignore reflection errors
                            }
                        }

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

                        // Add profile picture if available
                        if (profilePicture != null)
                        {
                            CirclePictureBox profilePictureBox = new CirclePictureBox
                            {
                                Size = new Size(100, 100),
                                SizeMode = PictureBoxSizeMode.StretchImage,
                                Image = ByteArrayToImage(profilePicture),
                                Location = new Point(
                                    (displays[ip].Width - 100) / 2,
                                    (displays[ip].Height - 100) / 2
                                )
                            };
                            backgroundPanel.Controls.Add(profilePictureBox);
                        }

                        // Add username label
                        Label usernameLabel = new Label
                        {
                            Text = username,
                            ForeColor = roleColor,
                            Font = new Font("Arial", 12, FontStyle.Regular),
                            AutoSize = true,
                            BackColor = Color.Transparent
                        };

                        // Position the label
                        usernameLabel.Location = new Point(
                            displays[ip].Width - usernameLabel.PreferredWidth - 10,
                            displays[ip].Height - usernameLabel.PreferredHeight - 10
                        );
                        backgroundPanel.Controls.Add(usernameLabel);

                        displays[ip].Controls.Add(backgroundPanel);
                        displays[ip].Refresh();


                        // Only update layout if necessary
                        if (displays.Count > 1)
                        {
                            UpdateVideoLayout(); 
                        }

                        System.Diagnostics.Debug.WriteLine($"Profile picture display complete for {ip}");
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

        // Helper method to safely get a property value from an object
        private object GetPropertyValue(object obj, string propertyName)
        {
            try
            {
                var type = obj.GetType();
                var propertyInfo = type.GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(obj, null);
                }

                // Try to access via dynamic reflection
                var fieldInfo = type.GetField(propertyName);
                if (fieldInfo != null)
                {
                    return fieldInfo.GetValue(obj);
                }
            }
            catch
            {
                // Ignore reflection errors
            }
            return null;
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
        public void UpdateVideoLayout(bool debug = false)
        {
            // Get total number of participants (including local)
            //bool debug = false;
            int totalParticipants = displays.Count;

            //if(totalParticipants == 1 || (totalParticipants == 2 && displays.ContainsKey("yoav")))
            //{
            //    totalParticipants = 2;
            //    debug = true;
            //}
            if (totalParticipants == 0)
                return;

            System.Diagnostics.Debug.WriteLine($"UpdateVideoLayout called with {totalParticipants} participants and debug: {debug}");


            // Panel dimensions
            int panelWidth = remotePanel.Width;
            int panelHeight = remotePanel.Height;

            // Calculate the optimal display size based on number of participants
            Size displaySize;
            if (debug)
            {
                displaySize = CalculateOptimalDisplaySize(4, panelWidth, panelHeight);
            } else
            {
                displaySize = CalculateOptimalDisplaySize(totalParticipants, panelWidth, panelHeight);

            }
            System.Diagnostics.Debug.WriteLine($"Calculated display size: {displaySize}");


            // Calculate positions for each display
            Dictionary<string, Point> positions = CalculatePositions(totalParticipants, displaySize, panelWidth, panelHeight);

            // Update each display with the same size
            foreach (var kvp in displays)
            {
                var display = kvp.Value;
                if (display.IsDisposed || !display.IsHandleCreated)
                {
                    System.Diagnostics.Debug.WriteLine($"skipped display {kvp.Key}");
                    continue;   
                }

                display.Invoke(new Action(() =>
                {
                    try
                    {
                        // Store any existing child controls that need to be preserved
                        List<Control> childControls = new List<Control>();
                        foreach (Control ctrl in display.Controls)
                        {
                            childControls.Add(ctrl);
                        }

                        // Store the Tag property to preserve user info
                        object tagValue = display.Tag;

                        // Remove the display from parent to reset its rendering state
                        remotePanel.Controls.Remove(display);

                        // Set exact same size for all displays
                        display.Size = displaySize;
                        display.Dock = DockStyle.None;

                        if (positions.ContainsKey(kvp.Key))
                        {
                            display.Location = positions[kvp.Key];
                        }


                        display.Controls.Clear();

                        foreach (var ctrl in childControls)
                        {
                            // For background panel, adjust size to match display
                            if (ctrl is Panel panel && panel.Name == "backgroundPanel")
                            {
                                panel.Size = displaySize;
                                panel.Dock = DockStyle.None;

                                // Center profile picture in the panel
                                var profilePicture = panel.Controls.OfType<CirclePictureBox>().FirstOrDefault();
                                if (profilePicture != null)
                                {
                                    profilePicture.Location = new Point(
                                        (displaySize.Width - profilePicture.Width) / 2,
                                        (displaySize.Height - profilePicture.Height) / 2
                                    );
                                }

                                // Position username label at bottom-right
                                var usernameLabel = panel.Controls.OfType<Label>().FirstOrDefault();
                                if (usernameLabel != null)
                                {
                                    usernameLabel.Location = new Point(
                                        displaySize.Width - usernameLabel.Width - 10,
                                        displaySize.Height - usernameLabel.Height - 10
                                    );
                                }
                            }

                            display.Controls.Add(ctrl);
                        }

                        // Restore the Tag property
                        display.Tag = tagValue;

                        remotePanel.Controls.Add(display);


                        System.Diagnostics.Debug.WriteLine("Updated display layout");


                        display.Refresh();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error updating video layout: {ex.Message}");
                    }
                }));
            }

            remotePanel.Invoke(new Action(() => {
                remotePanel.Refresh();
            }));
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

            //if(debug)
            //{
            //    if (!displays.ContainsKey("yoav"))
            //    {
            //        displays.Add("yoav", new PictureBox());


            //    }
            //}
            //else
            //{
            //    displays.Remove("yoav");
            //}

            

            var participants = displays.Keys.ToList();

            // Safety check
            if (participants.Count == 0)
                return positions;



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

                    System.Diagnostics.Debug.WriteLine($"Video size: {videoSize}, Panel: {panelWidth}x{panelHeight}");
                    System.Diagnostics.Debug.WriteLine($"Positions: [{participants[0]}] at ({x},{y}), [{participants[1]}] at ({x * 2 + videoSize.Width},{y})");

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
            {
                remotePanel.Refresh();
            }
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
