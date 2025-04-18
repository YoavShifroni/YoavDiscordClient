using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoavDiscordClient.Enums;

namespace YoavDiscordClient
{
    /// <summary>
    /// Manages video and audio streaming connections between participants
    /// </summary>
    public class VideoStreamConnectionHandler : IDisposable
    {
        #region Private properties

        private readonly Panel remotePanel;
        private Dictionary<string, UdpClient> connections;
        private VideoManager videoManager;
        private AudioManager audioManager;
        public ParticipantManager participantManager;
        private bool isRunning;
        private bool disposed = false;
        private DateTime timeOfLastVideoSend = DateTime.Now;
        private System.Windows.Forms.Timer timer;
        private const int VideoMuteTimeOut = 2000;
        private SemaphoreSlim initializationLock = new SemaphoreSlim(1, 1);
        private bool isInitialized = false;

        #endregion

        /// <summary>
        /// Creates a new video stream connection
        /// </summary>
        /// <param name="remotePanel">The panel where videos will be displayed</param>
        public VideoStreamConnectionHandler(Panel remotePanel)
        {
            this.remotePanel = remotePanel ?? throw new ArgumentNullException(nameof(remotePanel));
            this.connections = new Dictionary<string, UdpClient>();

            // Create managers
            this.participantManager = new ParticipantManager(remotePanel, this);
            this.videoManager = new VideoManager(participantManager.CreateLocalDisplay());
            this.audioManager = new AudioManager();

            // Setup event handlers
            this.SetupEventHandlers();

            System.Diagnostics.Debug.WriteLine("Video stream connection initialized");
        }

        /// <summary>
        /// Initializes the video stream connection
        /// </summary>
        public async Task Initialize()
        {
            // Use a semaphore to prevent multiple simultaneous initializations
            await initializationLock.WaitAsync();

            try
            {
                System.Diagnostics.Debug.WriteLine("Initializing video stream connection");

                // If already initialized, clean up resources first
                if (isInitialized)
                {
                    await CleanupResources(true);
                }

                // Setup local display
                //participantManager.CreateLocalDisplay();

                // Initialize timer for heartbeat packets
                InitializeTimer();

                // Initialize video capture
                await videoManager.Initialize();

                isInitialized = true;
                System.Diagnostics.Debug.WriteLine("Video stream connection initialized successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to initialize video stream connection: {ex.Message}");
                throw;
            }
            finally
            {
                initializationLock.Release();
            }
        }

        /// <summary>
        /// Connects to a remote participant
        /// </summary>
        /// <param name="ip">The participant's IP address</param>
        /// <param name="port">The participant's port</param>
        /// <param name="profilePicture">The participant's profile picture</param>
        /// <param name="username">The participant's username</param>
        /// <param name="userId">The participant's user ID</param>
        public void ConnectToParticipant(string ip, int port, byte[] profilePicture, string username, int userId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Connecting to participant: {username} ({ip}:{port})");

                // Reinitialize if needed before connecting to ensure fresh state
                //if (!isInitialized)
                //{
                //    await Initialize();
                //}

                // Create UDP client for this participant
                var videoClient = new UdpClient();
                videoClient.Connect(ip, port);
                connections[ip] = videoClient;

                // Setup participant display
                participantManager.AddParticipant(ip, userId, profilePicture, username);

                // Setup audio for this participant
                audioManager.AddParticipant(ip);

                // Start receiving data from this participant
                StartReceiving(ip, videoClient);

                System.Diagnostics.Debug.WriteLine($"Connected to participant: {username} ({ip}:{port})");

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to connect to participant: {username} ({ip}:{port}): {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Disconnects from a participant by user ID
        /// </summary>
        /// <param name="userId">The participant's user ID</param>
        public void DisconnectFromParticipant(int userId)
        {
            string ip = participantManager.GetIpForUserId(userId);
            if (!string.IsNullOrEmpty(ip))
            {
                DisconnectFromParticipant(ip);
            }
        }

        /// <summary>
        /// Disconnects from a participant by IP address
        /// </summary>
        /// <param name="ip">The participant's IP address</param>
        public void DisconnectFromParticipant(string ip)
        {
            try
            {
                if (connections.ContainsKey(ip))
                {
                    System.Diagnostics.Debug.WriteLine($"Disconnecting from participant: {ip}");

                    // Dispose of UDP client
                    connections[ip].Dispose();
                    connections.Remove(ip);

                    // Remove participant display
                    participantManager.RemoveParticipantByIp(ip);

                    // Remove audio provider
                    audioManager.RemoveParticipant(ip);

                    System.Diagnostics.Debug.WriteLine($"Disconnected from participant: {ip}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error disconnecting from participant: {ip}: {ex.Message}");
            }
        }

        /// <summary>
        /// Toggles audio mute state
        /// </summary>
        public void ToggleAudioMute()
        {
            try
            {
                audioManager.ToggleAudioMute();
                System.Diagnostics.Debug.WriteLine($"Audio mute toggled: {audioManager.IsAudioMuted}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error toggling audio mute: {ex.Message}");
            }
        }

        /// <summary>
        /// Toggles video mute state
        /// </summary>
        public void ToggleVideoMute()
        {
            try
            {
                videoManager.ToggleVideoMute();

                // Make sure an empty video packet is sent immediately when video is muted
                if (videoManager.IsVideoMuted)
                {
                    SendEmptyVideoPacket(true);
                    // Send it a second time to ensure delivery
                    Task.Delay(100).ContinueWith(t => SendEmptyVideoPacket(true));
                }

                System.Diagnostics.Debug.WriteLine($"Video mute toggled: {videoManager.IsVideoMuted}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error toggling video mute: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the global mute state
        /// </summary>
        /// <param name="muted">Whether audio should be globally muted</param>
        public void SetGlobalMuteState(bool muted)
        {
            try
            {
                audioManager.SetGlobalMuteState(muted);
                System.Diagnostics.Debug.WriteLine($"Global mute state set to: {muted}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting global mute state: {ex.Message}");
            }
        }

        public void SetMutedByHigherRoleState(bool muted)
        {
            try
            {
                audioManager.SetMutedByHigherRoleState(muted);
                System.Diagnostics.Debug.WriteLine($"Mute by higher role state set to: {muted}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting mute by higher role state: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the global deafen state
        /// </summary>
        /// <param name="deafened">Whether audio should be globally deafened</param>
        public void SetGlobalDeafenState(bool deafened)
        {
            try
            {
                audioManager.SetGlobalDeafenState(deafened);
                System.Diagnostics.Debug.WriteLine($"Global deafen state set to: {deafened}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting global deafen state: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes data received from another participant
        /// </summary>
        /// <param name="ip">The participant's IP address</param>
        /// <param name="bytes">The received data</param>
        public void ProcessDataFromOtherUser(string ip, byte[] bytes)
        {
            try
            {
                var packetType = BasePacket.GetPacketType(bytes);

                switch (packetType)
                {
                    case PacketType.Video:
                        var videoPacket = VideoPacket.FromBytes(bytes);
                        participantManager?.ProcessVideoData(ip, videoPacket);
                        break;

                    case PacketType.Audio:
                        // Process audio data
                        var audioPacket = AudioPacket.FromBytes(bytes);
                        audioManager?.ProcessRemoteAudioData(ip, audioPacket.AudioData);
                        break;

                    case PacketType.Empty_Video:
                        // Process empty video notification (camera off)
                        participantManager?.ProcessEmptyVideo(ip);
                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine($"Received unknown packet type from {ip}: {packetType}");
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing data from {ip}: {ex.Message}");
            }
        }

        /// <summary>
        /// Cleans up resources and prepares for reinitialization
        /// </summary>
        private async Task CleanupResources(bool forReinitialization = false)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Cleaning up resources for reinitialization");

                // Stop timer
                timer?.Stop();

                // Set flag to stop receive loops
                isRunning = false;

                // Wait a moment for receive loops to exit
                await Task.Delay(100);

                // Dispose all UDP clients
                foreach (var client in connections.Values)
                {
                    try
                    {
                        client.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error disposing client: {ex.Message}");
                    }
                }

                connections.Clear();

                // Reset initialization state
                isInitialized = false;

                // Only force a GC collect during reinitialization
                if (forReinitialization)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    await Task.Delay(500);
                }

                System.Diagnostics.Debug.WriteLine("Resource cleanup completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during resource cleanup: {ex.Message}");
            }
        }

        private void SetupEventHandlers()
        {
            // Set up video frame event handler
            videoManager.LocalVideoFrameReady += (sender, e) => SendVideoFrame(e.FrameData, e.Timestamp);

            // Set up video mute state change event handler
            videoManager.VideoMuteStateChanged += (sender, e) => SendEmptyVideoPacket(e.IsMuted);

            // Set up audio data event handler
            audioManager.LocalAudioDataAvailable += (sender, e) => SendAudioData(e.AudioData);
        }

        private void InitializeTimer()
        {
            // Dispose existing timer if necessary
            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= SendEmptyVideoIfNeeded;
                timer.Dispose();
            }

            timer = new System.Windows.Forms.Timer();
            timer.Interval = VideoMuteTimeOut; // 2000 milliseconds = 2 seconds
            timer.Tick += SendEmptyVideoIfNeeded;
            timer.Start();
        }

        private void SendEmptyVideoIfNeeded(object sender, EventArgs e)
        {
            // This will run every 2 seconds
            if ((DateTime.Now - this.timeOfLastVideoSend).TotalMilliseconds > VideoMuteTimeOut)
            {
                SendEmptyVideoPacket(true);
            }
        }

        private void SendEmptyVideoPacket(bool isMuted)
        {
            try
            {
                if (isMuted)
                {
                    // Create the packet only once
                    var packet = new EmptyVideoPacket();
                    var packetBytes = packet.ToBytes();

                    System.Diagnostics.Debug.WriteLine($"Sending empty video packet to {connections.Count} connections");

                    foreach (var client in connections.Values)
                    {
                        try
                        {
                            client.Send(packetBytes, packetBytes.Length);
                        }
                        catch (ObjectDisposedException)
                        {
                            // Skip disposed clients
                            continue;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error sending empty video packet: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending empty video packet: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a video frame to all connected clients by splitting it into multiple packets.
        /// </summary>
        /// <param name="frameData">The raw video frame data as a byte array.</param>
        /// <param name="timestamp">The timestamp indicating when the frame was captured or sent.</param>
        /// <remarks>
        /// This method generates a unique frame ID for each frame and splits the frame into smaller packets
        /// according to <see cref="VideoPacket.MAX_PACKET_SIZE"/>. Each packet is sent individually to all connected clients.
        /// Errors in sending to individual clients are logged and do not interrupt the sending process for others.
        /// </remarks>
        private void SendVideoFrame(byte[] frameData, DateTime timestamp)
        {
            try
            {
                this.timeOfLastVideoSend = timestamp;

                var frameId = Guid.NewGuid();
                var totalPackets = (int)Math.Ceiling(frameData.Length / (double)VideoPacket.MAX_PACKET_SIZE);

                for (int i = 0; i < totalPackets; i++)
                {
                    int startIndex = i * VideoPacket.MAX_PACKET_SIZE;
                    int length = Math.Min(VideoPacket.MAX_PACKET_SIZE, frameData.Length - startIndex);
                    byte[] packetData = new byte[length];
                    Array.Copy(frameData, startIndex, packetData, 0, length);

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
                        try
                        {
                            client.Send(packetBytes, packetBytes.Length);
                        }
                        catch (ObjectDisposedException)
                        {
                            // Skip disposed clients
                            continue;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error sending video packet: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending video frame: {ex.Message}");
            }
        }


        private void SendAudioData(byte[] audioData)
        {
            try
            {
                var packet = new AudioPacket(audioData);
                var packetBytes = packet.ToBytes();

                foreach (var client in connections.Values)
                {
                    try
                    {
                        client.Send(packetBytes, packetBytes.Length);
                    }
                    catch (ObjectDisposedException)
                    {
                        // Skip disposed clients
                        continue;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error sending audio data: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending audio data: {ex.Message}");
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

        public async Task ReInitializeVideo()
        {
            try
            {
                // Force a complete video reinitialization
                await videoManager.Initialize();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reinitializing video: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Releases all resources used by the VideoStreamConnection
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the VideoStreamConnection
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Inside the dispose method in Video Stream Connection Handler class");

                        // Stop receiving data
                        isRunning = false;

                        // Stop timer
                        timer?.Stop();
                        timer?.Dispose();
                        timer = null;

                        // Dispose of managers
                        videoManager?.Dispose();
                        videoManager = null;

                        audioManager?.Dispose();
                        audioManager = null;

                        participantManager?.Dispose();
                        participantManager = null;

                        // Dispose of connections
                        foreach (var client in connections.Values)
                        {
                            try
                            {
                                client.Dispose();
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error disposing client: {ex.Message}");
                            }
                        }

                        connections.Clear();

                        // Dispose of the initialization lock
                        initializationLock?.Dispose();
                        initializationLock = null;

                        // Force GC to clean up resources
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        System.Diagnostics.Debug.WriteLine("Video stream connection disposed");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error during disposal: {ex.Message}");
                    }
                }

                disposed = true;
            }
        }
    }
}