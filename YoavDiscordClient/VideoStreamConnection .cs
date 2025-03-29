using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.MixedReality.WebRTC;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;



namespace YoavDiscordClient
{
#pragma warning disable CA1416


    public class VideoStreamConnection : IDisposable
    {

        private readonly Panel remotePanel;
        private Dictionary<string, PictureBox> displays;
        private Dictionary<string, UdpClient> connections;
        private DeviceVideoTrackSource videoSource;
        private LocalVideoTrack localVideo;
        private WaveIn audioInput;
        private WaveOut audioOutput;
        private Dictionary<string, BufferedWaveProvider> audioInputs;
        private MixingSampleProvider mixer;
        private Dictionary<string, FrameAssembler> frameAssemblers;
        private bool isRunning;
        private bool disposed = false;
        private bool isAudioMuted = false;
        private bool isVideoMuted = false;
        private bool _isGloballyMuted = false;
        private bool _isGloballyDeafened = false;
        private DateTime timeOfLastVideoSend = DateTime.Now;
        private Timer timer;
        private Dictionary<int, string> _userIdToIp;
        private readonly WaveFormat waveFormat; // Store the wave format to reuse

        private const int VIDEO_WIDTH = 640;
        private const int VIDEO_HEIGHT = 480;
        private const int SAMPLE_RATE = 44100;
        private const int CHANNELS = 1;
        private const int VideoMuteTimeOut = 2000;

        public VideoStreamConnection(Panel remotePanel)
        {
            this.remotePanel = remotePanel;
            //this.localPanel = localPanel;
            this.displays = new Dictionary<string, PictureBox>();
            this.connections = new Dictionary<string, UdpClient>();
            this.frameAssemblers = new Dictionary<string, FrameAssembler>();
            this._userIdToIp = new Dictionary<int, string>();

            // Create wave format once
            this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(SAMPLE_RATE, CHANNELS);


            // Initialize audio mixing
            this.audioInputs = new Dictionary<string, BufferedWaveProvider>();
            this.remotePanel.Resize += (sender, e) => UpdateVideoLayout();

            InitializeAudio();

        }


        private void UpdateVideoLayout()
        {
            // Get total number of participants (including local)
            int totalParticipants = displays.Count;

            // Panel dimensions
            int panelWidth = remotePanel.Width;
            int panelHeight = remotePanel.Height;

            // Calculate the EXACT same size for all displays
            Size displaySize;
            if (totalParticipants == 1)
            {
                displaySize = new Size(panelWidth / 2, panelHeight / 2);
            }
            else if (totalParticipants == 2)
            {
                // Make both displays exactly the same size
                displaySize = new Size(400, 300); // Or whatever fixed size you prefer
            }
            else
            {
                displaySize = new Size(300, 250); // Fixed size for 3+ participants
            }

            // Calculate positions
            Dictionary<string, Point> positions = CalculatePositions(totalParticipants, displaySize, panelWidth, panelHeight);

            // Update each display with EXACTLY the same size
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
            }

            return positions;
        }


        private void InitializeAudio()
        {
            try
            {
                // Setup audio output (speakers)
                this.mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(SAMPLE_RATE, CHANNELS));
                this.mixer.ReadFully = true;  // Ensure mixer reads all available data

                var volumeProvider = new VolumeSampleProvider(mixer);
                volumeProvider.Volume = 1.0f;

                this.audioOutput = new WaveOut()
                {
                    DesiredLatency = 100  // Lower latency
                };

                System.Diagnostics.Debug.WriteLine("Initializing audio output...");

                this.audioOutput.Init(volumeProvider);
                this.audioOutput.Play();

                System.Diagnostics.Debug.WriteLine("Audio output initialized and playing");

                // Setup audio input (microphone)
                this.audioInput = new WaveIn
                {
                    WaveFormat = this.waveFormat,
                    BufferMilliseconds = 50
                };
                this.audioInput.DataAvailable += AudioInput_DataAvailable;
                this.audioInput.StartRecording();

                System.Diagnostics.Debug.WriteLine("Audio input initialized and recording");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing audio: {ex.Message}");
            }

        }

        public async Task Initialize()
        {
            CreateLocalDisplay();
            this.InitializeTimer();
            await InitializeLocalVideo();
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = VideoMuteTimeOut; // 2000 milliseconds = 2 seconds
            timer.Tick += SendEmptyVideoIfNeeded;
            timer.Start();
        }

        private void SendEmptyVideoIfNeeded(object sender, EventArgs e)
        {
            // this will run every 2 seconds
            if ((DateTime.Now - this.timeOfLastVideoSend).Seconds > VideoMuteTimeOut)
            {
                var packet = new EmptyVideoPacket();
                var packetBytes = packet.ToBytes();
                foreach (var client in connections.Values)
                {
                    client.Send(packetBytes, packetBytes.Length);
                }
            }
        }

        private async Task InitializeLocalVideo()
        {
            int maxRetries = 3;
            int currentTry = 0;

            while (currentTry < maxRetries)
            {
                try
                {
                    await CleanupVideo();
                    var devices = await DeviceVideoTrackSource.GetCaptureDevicesAsync();
                    if (devices.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Found {devices.Count} video devices. Using device: {devices[0].name}");

                        videoSource = await DeviceVideoTrackSource.CreateAsync(
                            new LocalVideoDeviceInitConfig
                            {
                                videoDevice = devices[0],
                                width = VIDEO_WIDTH,
                                height = VIDEO_HEIGHT,
                                framerate = 20
                            });

                        localVideo = LocalVideoTrack.CreateFromSource(videoSource, new LocalVideoTrackInitConfig());
                        localVideo.Argb32VideoFrameReady += LocalVideoFrame_Ready;

                        return;
                    }
                    else
                    {
                        // Create a panel with profile picture instead of black frame 
                        System.Diagnostics.Debug.WriteLine("No video devices found, showing profile picture");
                        if (displays.ContainsKey("local"))
                        {
                            displays["local"].Invoke(new Action(() =>
                            {
                                displays["local"].Image?.Dispose();

                                // Create background panel with visible border
                                Panel backgroundPanel = new Panel
                                {
                                    Name = "backgroundPanel",
                                    Size = displays["local"].Size,
                                    BackColor = Color.FromArgb(47, 49, 54),
                                    BorderStyle = BorderStyle.FixedSingle
                                };

                                // Create circle picture box for profile picture
                                CirclePictureBox profilePicture = new CirclePictureBox
                                {
                                    Size = new Size(100, 100),
                                    SizeMode = PictureBoxSizeMode.StretchImage,
                                    Image = DiscordFormsHolder.getInstance().DiscordApp.UserProfilePicture,
                                    Location = new Point(
                                        (backgroundPanel.Width - 100) / 2,
                                        (backgroundPanel.Height - 100) / 2
                                    )
                                };

                                // Add username label at the bottom
                                Label usernameLabel = new Label
                                {
                                    Text = DiscordFormsHolder.getInstance().DiscordApp.Username,
                                    ForeColor = Color.White,
                                    Font = new Font("Arial", 12, FontStyle.Regular),
                                    AutoSize = true,
                                    BackColor = Color.Transparent
                                };

                                // Position the label at bottom-right with some padding
                                usernameLabel.Location = new Point(
                                    backgroundPanel.Width - usernameLabel.PreferredWidth - 10,
                                    backgroundPanel.Height - usernameLabel.PreferredHeight - 10
                                );

                                backgroundPanel.Controls.Add(profilePicture);
                                backgroundPanel.Controls.Add(usernameLabel);
                                displays["local"].Controls.Add(backgroundPanel);
                            }));
                        }
                        return;
                    }
                }
                catch (Exception ex)
                {
                    currentTry++;
                    System.Diagnostics.Debug.WriteLine($"Attempt {currentTry} failed: {ex.Message}");

                    if (currentTry >= maxRetries)
                    {
                        // Show profile picture on failure
                        System.Diagnostics.Debug.WriteLine("All retry attempts failed, showing profile picture");
                        if (displays.ContainsKey("local"))
                        {
                            displays["local"].Invoke(new Action(() =>
                            {
                                displays["local"].Image?.Dispose();

                                Panel backgroundPanel = new Panel
                                {
                                    Name = "backgroundPanel",
                                    Size = displays["local"].Size,
                                    BackColor = Color.FromArgb(47, 49, 54),
                                    BorderStyle = BorderStyle.FixedSingle
                                };

                                CirclePictureBox profilePicture = new CirclePictureBox
                                {
                                    Size = new Size(100, 100),
                                    SizeMode = PictureBoxSizeMode.StretchImage,
                                    Image = DiscordFormsHolder.getInstance().DiscordApp.UserProfilePicture,
                                    Location = new Point(
                                        (backgroundPanel.Width - 100) / 2,
                                        (backgroundPanel.Height - 100) / 2
                                    )
                                };

                                Label usernameLabel = new Label
                                {
                                    Text = DiscordFormsHolder.getInstance().DiscordApp.Username,
                                    ForeColor = Color.White,
                                    Font = new Font("Arial", 12, FontStyle.Regular),
                                    AutoSize = true,
                                    BackColor = Color.Transparent
                                };

                                usernameLabel.Location = new Point(
                                    backgroundPanel.Width - usernameLabel.PreferredWidth - 10,
                                    backgroundPanel.Height - usernameLabel.PreferredHeight - 10
                                );

                                backgroundPanel.Controls.Add(profilePicture);
                                backgroundPanel.Controls.Add(usernameLabel);
                                displays["local"].Controls.Add(backgroundPanel);
                            }));
                        }
                        return;
                    }

                    await Task.Delay(2000);
                }
            }
        }



        private void CreateLocalDisplay()
        {
            var display = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(47, 49, 54),
                Dock = DockStyle.None,
                Size = new Size(VIDEO_WIDTH, VIDEO_HEIGHT)  // Set initial size
            };
            remotePanel.Controls.Add(display);
            displays["local"] = display;
            this.UpdateVideoLayout();
        }

        public async Task ConnectToParticipant(string ip, int port, byte[] profilePicture, string username, int userId)
        {
            System.Diagnostics.Debug.WriteLine($"Connecting to participant: {ip}:{port}");
            this._userIdToIp[userId] = ip;
            var videoClient = new UdpClient();
            videoClient.Connect(ip, port);
            connections[ip] = videoClient;

            // Get user role from UsersImages dictionary
            var user = DiscordFormsHolder.getInstance().DiscordApp.UsersImages.Keys
                .Select(id => DiscordFormsHolder.getInstance().DiscordApp
                    .usersInMediaChannels.Values
                    .SelectMany(list => list)
                    .FirstOrDefault(u => u.UserId == id))
                .FirstOrDefault(u => u != null && u.UserId == userId);

            // Default to Member role if user is not found
            int userRole = (user != null) ? user.role : 2;

            // Get the appropriate color for the user based on their role using our helper method
            Color userColor = DiscordFormsHolder.getInstance().DiscordApp.GetRoleColor(userRole);

            var display = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(47, 49, 54),
                Dock = DockStyle.None,
                Size = new Size(VIDEO_WIDTH, VIDEO_HEIGHT),  // Add initial size
                Tag = new { ProfilePicture = profilePicture, Username = username, UserId = userId, RoleColor = userColor } // Store user info and role color
            };

            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
            {
                DiscordFormsHolder.getInstance().DiscordApp.AddNewParticipantDisplay(remotePanel, display);
                displays[ip] = display;
                this.UpdateVideoLayout();
            }));

            var waveProvider = new BufferedWaveProvider(this.waveFormat)
            {
                BufferLength = this.waveFormat.AverageBytesPerSecond,
                DiscardOnBufferOverflow = false
            };

            try
            {
                System.Diagnostics.Debug.WriteLine($"Setting up audio for {ip}");
                audioInputs[ip] = waveProvider;
                var sampleProvider = new WaveToSampleProvider(waveProvider);
                mixer.AddMixerInput(sampleProvider);
                System.Diagnostics.Debug.WriteLine($"Audio setup complete for {ip}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting up audio for {ip}: {ex.Message}");
            }
            StartReceiving(ip, videoClient);
        }

        private void LocalVideoFrame_Ready(Argb32VideoFrame frame)
        {
            try
            {
                var bitmap = new Bitmap(VIDEO_WIDTH, VIDEO_HEIGHT, PixelFormat.Format32bppArgb);
                var rect = new Rectangle(0, 0, VIDEO_WIDTH, VIDEO_HEIGHT);
                var bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                int size = VIDEO_WIDTH * VIDEO_HEIGHT * 4;  // 4 bytes per pixel (ARGB)
                byte[] frameData = new byte[size];
                Marshal.Copy(frame.data, frameData, 0, size);
                Marshal.Copy(frameData, 0, bmpData.Scan0, size);

                this.timeOfLastVideoSend = DateTime.Now;

                bitmap.UnlockBits(bmpData);
                var bitmapData = BitmapToByteArray(bitmap);

                var frameId = Guid.NewGuid();
                var totalPackets = (int)Math.Ceiling(bitmapData.Length / (double)VideoPacket.MAX_PACKET_SIZE);

                for (int i = 0; i < totalPackets; i++)
                {
                    int startIndex = i * VideoPacket.MAX_PACKET_SIZE;
                    int length = Math.Min(VideoPacket.MAX_PACKET_SIZE, bitmapData.Length - startIndex);
                    byte[] packetData = new byte[length];
                    Array.Copy(bitmapData, startIndex, packetData, 0, length);

                    var packet = new VideoPacket
                    {
                        FrameId = frameId,
                        PacketIndex = i,
                        TotalPackets = totalPackets,
                        Data = packetData
                    };

                    var packetBytes = packet.ToBytes();
                    foreach (var client in connections.Values)
                    {
                        client.Send(packetBytes, packetBytes.Length);
                    }
                }

                if (displays.ContainsKey("local") && !displays["local"].IsDisposed && displays["local"].IsHandleCreated)
                {
                    displays["local"].Invoke(new Action(() => {
                        try
                        {
                            var oldImage = displays["local"].Image;
                            displays["local"].Image = bitmap;
                            oldImage?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error updating local display: {ex.Message}");
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing video frame: {ex.Message}");
            }
        }

        private void AudioInput_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                // Don't send audio if muted
                if (isAudioMuted || _isGloballyMuted)
                    return;

                var packet = new AudioPacket(e.Buffer);
                var packetBytes = packet.ToBytes();
                foreach (var client in connections.Values)
                {
                    client.Send(packetBytes, packetBytes.Length);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending audio: {ex.Message}");
            }
        }

        private async void StartReceiving(string ip, UdpClient client)
        {
            isRunning = true;
            while (isRunning)
            {
                try
                {
                    var result = await client.ReceiveAsync();
                    ProcessDataFromOtherUser(ip, result.Buffer);
                }
                catch (ObjectDisposedException)
                {
                    // Client was disposed, exit gracefully
                    System.Diagnostics.Debug.WriteLine($"UdpClient for {ip} was disposed");
                    break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Receive error from {ip}: {ex.Message}");
                    // Don't break the loop for transient errors
                    await Task.Delay(100);
                }
            }
        }

        public void ProcessDataFromOtherUser(string ip, byte[] bytes)
        {
            try
            {
                var packetType = BasePacket.GetPacketType(bytes);

                switch (packetType)
                {
                    case PacketType.Video:
                        System.Diagnostics.Debug.WriteLine("Got video packet");
                        var packet = VideoPacket.FromBytes(bytes);

                        if (!frameAssemblers.ContainsKey(ip))
                        {
                            frameAssemblers[ip] = new FrameAssembler();
                        }

                        var frameData = frameAssemblers[ip].AddPacket(packet);
                        if (frameData != null) // Frame is complete
                        {
                            using (var ms = new MemoryStream(frameData))
                            {
                                try
                                {
                                    var bitmap = new Bitmap(ms);
                                    if (displays.ContainsKey(ip) && !displays[ip].IsDisposed && displays[ip].IsHandleCreated)
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
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error creating bitmap from video data: {ex.Message}");
                                }
                            }
                        }
                        break;

                    case PacketType.Audio:
                        // If we're deafened, ignore incoming audio
                        if (_isGloballyDeafened)
                            break;

                        System.Diagnostics.Debug.WriteLine("Got audio packet");

                        var audioPacket = AudioPacket.FromBytes(bytes);
                        if (audioInputs.ContainsKey(ip))
                        {
                            var waveProvider = audioInputs[ip];

                            float bufferUsagePercent = (float)waveProvider.BufferedBytes / waveProvider.BufferLength * 100;
                            System.Diagnostics.Debug.WriteLine($"Buffer usage: {bufferUsagePercent:F1}%");

                            if (bufferUsagePercent > 95)
                            {
                                System.Diagnostics.Debug.WriteLine("Buffer getting full, clearing...");
                                // Instead of clearing half the buffer, implement a sliding window
                                byte[] remainingData = new byte[waveProvider.BufferedBytes - 8820]; // Keep all but one packet
                                waveProvider.Read(new byte[8820], 0, 8820); // Remove oldest packet
                            }

                            try
                            {
                                waveProvider.AddSamples(audioPacket.AudioData, 0, audioPacket.AudioData.Length);
                                System.Diagnostics.Debug.WriteLine($"Added {audioPacket.AudioData.Length} bytes to buffer. Current buffer size: {waveProvider.BufferedBytes}");
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error adding samples: {ex.Message}");
                                waveProvider.ClearBuffer();
                            }
                        } else
                        {
                            System.Diagnostics.Debug.WriteLine($"Got audio packet of unknown IP: {ip}");

                        }
                        break;

                    case PacketType.Empty_Video:
                        System.Diagnostics.Debug.WriteLine("Got empty video packet");

                        if (displays.ContainsKey(ip) && !displays[ip].IsDisposed && displays[ip].IsHandleCreated)
                        {
                            displays[ip].Invoke(new Action(() =>
                            {
                                try
                                {
                                    displays[ip].Controls.Clear();
                                    if (displays[ip].Image != null)
                                    {
                                        displays[ip].Image.Dispose();
                                        displays[ip].Image = null;
                                    }

                                    var userInfo = (dynamic)displays[ip].Tag;

                                    // Set fixed size for display
                                    displays[ip].Size = new Size(VIDEO_WIDTH, VIDEO_HEIGHT);

                                    Panel backgroundPanel = new Panel
                                    {
                                        Name = "backgroundPanel",
                                        Size = displays[ip].Size,
                                        BackColor = Color.FromArgb(47, 49, 54),
                                        BorderStyle = BorderStyle.FixedSingle,
                                        Location = new Point(0, 0)
                                    };

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

                                    Label usernameLabel = new Label
                                    {
                                        Text = (string)userInfo.Username,
                                        ForeColor = userColor, // Use the role-specific color
                                        Font = new Font("Arial", 12, FontStyle.Regular),
                                        AutoSize = true,
                                        BackColor = Color.Transparent
                                    };

                                    // Calculate label position after AutoSize has determined its dimensions
                                    usernameLabel.Location = new Point(
                                        displays[ip].Width - usernameLabel.PreferredWidth - 10,
                                        displays[ip].Height - usernameLabel.PreferredHeight - 10
                                    );

                                    backgroundPanel.Controls.Add(profilePicture);
                                    backgroundPanel.Controls.Add(usernameLabel);
                                    displays[ip].Controls.Add(backgroundPanel);

                                    // Force a layout update after everything is set up
                                    displays[ip].Refresh();
                                    this.UpdateVideoLayout();
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error processing empty video: {ex.Message}");
                                }
                            }));
                        }
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine($"Got unknown packet: {packetType}");
                        break;

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing packet: {ex.Message}");
            }
        }

        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (var clonedBitmap = new Bitmap(bitmap)) // Clone to avoid conflicts
            using (var ms = new MemoryStream())
            {
                var jpegEncoder = GetJpegEncoder();
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 70L); // Reduced quality
                clonedBitmap.Save(ms, jpegEncoder, encoderParameters);
                return ms.ToArray();
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

        private ImageCodecInfo GetJpegEncoder()
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; i++)
            {
                if (codecs[i].MimeType == "image/jpeg")
                    return codecs[i];
            }
            return null;
        }

        private async Task CleanupVideo()
        {
            try
            {
                if (localVideo != null)
                {
                    // Create a timeout task
                    var timeoutTask = Task.Delay(3000); // 3 second timeout

                    // Create a task to remove the event handler
                    var cleanupTask = Task.Run(() =>
                    {
                        if (localVideo != null)
                        {
                            localVideo.Argb32VideoFrameReady -= LocalVideoFrame_Ready;
                            localVideo.Dispose();
                            localVideo = null;
                        }
                    });

                    // Wait for either the cleanup to complete or timeout
                    var completedTask = await Task.WhenAny(cleanupTask, timeoutTask);
                    if (completedTask == timeoutTask)
                    {
                        System.Diagnostics.Debug.WriteLine("Video cleanup timed out");
                        // Force cleanup
                        localVideo = null;
                    }
                }

                if (videoSource != null)
                {
                    videoSource.Dispose();
                    videoSource = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during video cleanup: {ex.Message}");
            }
        }

        public void DisconnectFromParticipant(int userId)
        {
            if (this._userIdToIp.ContainsKey(userId))
            {
                this.DisconnectFromParticipant(this._userIdToIp[userId]);
                this._userIdToIp.Remove(userId);
            }

        }


        public void DisconnectFromParticipant(string ip)
        {
            if (connections.ContainsKey(ip))
            {
                try
                {
                    // Dispose of the UDP client
                    connections[ip].Dispose();
                    connections.Remove(ip);

                    // Remove and dispose of their display
                    if (displays.ContainsKey(ip))
                    {
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

                                // Remove from parent panel
                                remotePanel.Controls.Remove(displayToRemove);
                                displayToRemove.Dispose();

                                // Clear the displays dictionary for this channel
                                displays.Remove(ip);

                                // Reset display layout if we're the only one left
                                if (displays.Count == 1) // Only local display remains
                                {
                                    ResetChannelDisplay();
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
                    }

                    // Remove their audio provider
                    if (audioInputs.ContainsKey(ip))
                    {
                        audioInputs.Remove(ip);
                    }

                    // Remove their frame assembler
                    if (frameAssemblers.ContainsKey(ip))
                    {
                        frameAssemblers.Remove(ip);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error during disconnect: {ex.Message}");
                }
            }
        }

        private void ResetChannelDisplay()
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
                localDisplay.Size = new Size(VIDEO_WIDTH, VIDEO_HEIGHT);
                localDisplay.Location = new Point(
                    (remotePanel.Width - VIDEO_WIDTH) / 2,
                    (remotePanel.Height - VIDEO_HEIGHT) / 2
                );
            }

            UpdateVideoLayout();
            if (!remotePanel.IsDisposed)
                remotePanel.Refresh();
        }




        public void ToggleAudioMute()
        {
            isAudioMuted = !isAudioMuted;

            // If globally muted, don't allow unmuting
            if (this._isGloballyMuted)
            {
                // Ensure audio stays muted regardless of channel setting
                if (audioInput != null)
                {
                    try
                    {
                        audioInput.StopRecording();
                    }
                    catch (Exception ex)
                    {
                        // Ignore errors if already stopped
                        System.Diagnostics.Debug.WriteLine($"Error stopping recording: {ex.Message}");
                    }
                }
            }
            else
            {
                // Normal channel-specific mute behavior
                if (audioInput != null)
                {
                    try
                    {
                        if (isAudioMuted)
                        {
                            audioInput.StopRecording();
                        }
                        else
                        {
                            audioInput.StartRecording();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that might occur during state change
                        System.Diagnostics.Debug.WriteLine($"Error changing audio state: {ex.Message}");
                    }
                    
                }
            }

            
        }

        public void ToggleVideoMute()
        {
            isVideoMuted = !isVideoMuted;
            try
            {
                if (isVideoMuted)
                {
                    // First send the empty video packet
                    var packet = new EmptyVideoPacket();
                    var packetBytes = packet.ToBytes();
                    foreach (var client in connections.Values)
                    {
                        try
                        {
                            client.Send(packetBytes, packetBytes.Length);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error sending packet: {ex.Message}");
                        }
                    }

                    // Update UI first
                    if (displays.ContainsKey("local") && !displays["local"].IsDisposed && displays["local"].IsHandleCreated)
                    {
                        DiscordFormsHolder.getInstance().DiscordApp.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                var localDisplay = displays["local"];
                                localDisplay.Controls.Clear();
                                if (localDisplay.Image != null)
                                {
                                    localDisplay.Image.Dispose();
                                    localDisplay.Image = null;
                                }

                                Panel backgroundPanel = new Panel
                                {
                                    Name = "backgroundPanel",
                                    Size = localDisplay.Size,
                                    BackColor = Color.FromArgb(47, 49, 54),
                                    BorderStyle = BorderStyle.FixedSingle,
                                    Location = new Point(0, 0)
                                };

                                CirclePictureBox profilePicture = new CirclePictureBox
                                {
                                    Size = new Size(100, 100),
                                    SizeMode = PictureBoxSizeMode.StretchImage,
                                    Image = DiscordFormsHolder.getInstance().DiscordApp.UserProfilePicture,
                                    Location = new Point(
                                        (localDisplay.Width - 100) / 2,
                                        (localDisplay.Height - 100) / 2
                                    )
                                };

                                Label usernameLabel = new Label
                                {
                                    Text = DiscordFormsHolder.getInstance().DiscordApp.Username,
                                    ForeColor = Color.White,
                                    Font = new Font("Arial", 12, FontStyle.Regular),
                                    AutoSize = true,
                                    BackColor = Color.Transparent
                                };

                                usernameLabel.Location = new Point(
                                    localDisplay.Width - usernameLabel.PreferredWidth - 10,
                                    localDisplay.Height - usernameLabel.PreferredHeight - 10
                                );

                                backgroundPanel.Controls.Add(profilePicture);
                                backgroundPanel.Controls.Add(usernameLabel);
                                localDisplay.Controls.Add(backgroundPanel);
                                localDisplay.Refresh();
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error in UI update: {ex.Message}");
                            }
                        }));
                    }

                    // Then stop the video in a separate task
                    Task.Run(() =>
                    {
                        try
                        {
                            if (localVideo != null)
                            {
                                localVideo.Argb32VideoFrameReady -= LocalVideoFrame_Ready;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error stopping video: {ex.Message}");
                        }
                    });
                }
                else
                {
                    // Clear UI first
                    if (displays.ContainsKey("local") && !displays["local"].IsDisposed && displays["local"].IsHandleCreated)
                    {
                        DiscordFormsHolder.getInstance().DiscordApp.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                displays["local"].Controls.Clear();
                                if (displays["local"].Image != null)
                                {
                                    displays["local"].Image.Dispose();
                                    displays["local"].Image = null;
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error clearing UI: {ex.Message}");
                            }
                        }));
                    }

                    // Then restart video in a separate task
                    Task.Run(() =>
                    {
                        try
                        {
                            if (localVideo != null)
                            {
                                localVideo.Argb32VideoFrameReady += LocalVideoFrame_Ready;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error restarting video: {ex.Message}");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ToggleVideoMute: {ex.Message}");
            }
        }


        public void SetGlobalMuteState(bool muted)
        {
            this._isGloballyMuted = muted;

            // Apply global mute setting regardless of channel-specific setting
            if (this._isGloballyMuted)
            {
                // Force mute audio
                if (audioInput != null)
                {
                    try
                    {
                        // First stop recording to ensure it's muted
                        audioInput.StopRecording();
                    }
                    catch (Exception ex)
                    {
                        // Ignore errors if already stopped
                        System.Diagnostics.Debug.WriteLine($"Error stopping recording: {ex.Message}");
                    }
                }
            }
            else if (!isAudioMuted) // Only unmute if the channel-specific mute is also off
            {
                // Resume audio if not channel-muted
                if (audioInput != null)
                {
                    try
                    {
                        // Start recording if not globally muted and not channel muted
                        audioInput.StartRecording();
                    }
                    catch (Exception ex)
                    {
                        // Ignore errors if already recording
                        System.Diagnostics.Debug.WriteLine($"Error starting recording: {ex.Message}");
                    }
                }
            }
        }


        public void SetGlobalDeafenState(bool deafened)
        {
            this._isGloballyDeafened = deafened;

            // Apply global deafen setting to audio output
            if (audioOutput != null)
            {
                try
                {
                    if (this._isGloballyDeafened)
                    {
                        // Stop audio output
                        audioOutput.Stop();
                    }
                    else
                    {
                        // Resume audio output
                        audioOutput.Play();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error changing audio output state: {ex.Message}");
                }
            }
        }





        protected virtual async void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    try
                    {
                        isRunning = false;

                        // Cleanup video first
                        await CleanupVideo();

                        // Then cleanup audio
                        audioInput?.StopRecording();
                        audioInput?.Dispose();
                        audioOutput?.Stop();
                        audioOutput?.Dispose();

                        // Cleanup connections
                        foreach (var client in connections.Values)
                        {
                            client.Dispose();
                        }

                        // Cleanup displays
                        foreach (var display in displays.Values)
                        {
                            if (!display.IsDisposed)
                                display.Dispose();
                        }

                        displays.Clear();
                        connections.Clear();
                        frameAssemblers.Clear();
                        audioInputs.Clear();

                        // Additional cleanup
                        videoSource = null;
                        localVideo = null;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error during disposal: {ex.Message}");
                    }
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }


}