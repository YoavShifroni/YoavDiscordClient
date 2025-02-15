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
using Org.BouncyCastle.Utilities;


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


        private readonly WaveFormat waveFormat; // Store the wave format to reuse

        private const int VIDEO_WIDTH = 640;
        private const int VIDEO_HEIGHT = 480;
        private const int SAMPLE_RATE = 44100;
        private const int CHANNELS = 1;

        public VideoStreamConnection(Panel remotePanel)
        {
            this.remotePanel = remotePanel;
            //this.localPanel = localPanel;
            this.displays = new Dictionary<string, PictureBox>();
            this.connections = new Dictionary<string, UdpClient>();
            this.frameAssemblers = new Dictionary<string, FrameAssembler>();

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

            // Calculate video size based on number of participants
            Size videoSize;
            switch (totalParticipants)
            {
                case 1:
                    videoSize = new Size(panelWidth / 2, panelHeight / 2); // Single centered video
                    break;
                case 2:
                    videoSize = new Size(panelWidth / 3, panelHeight / 2); // Two side by side
                    break;
                case 3:
                    videoSize = new Size(panelWidth / 3, panelHeight / 3); // Triangle formation
                    break;
                case 4:
                    videoSize = new Size(panelWidth / 3, panelHeight / 3); // 2x2 grid
                    break;
                default:
                    videoSize = new Size(panelWidth / 3, panelHeight / 3);
                    break;
            }

            // Calculate positions for each video
            Dictionary<string, Point> positions = CalculatePositions(totalParticipants, videoSize, panelWidth, panelHeight);

            // Update each display
            foreach (var kvp in displays)
            { 
                var display = kvp.Value;
                display.Size = videoSize;
                display.Dock = DockStyle.None; // Remove docking to allow precise positioning
                if (positions.ContainsKey(kvp.Key))
                {
                    display.Location = positions[kvp.Key];
                }
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
            await InitializeLocalVideo();
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

                    Environment.SetEnvironmentVariable("PATH",
                        @"C:\Users\royud\.nuget\packages\microsoft.mixedreality.webrtc\2.0.2\runtimes\win10-x64\native"
                        + ";" + Environment.GetEnvironmentVariable("PATH"));

                    var devices = await DeviceVideoTrackSource.GetCaptureDevicesAsync();
                    if (devices.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Attempting to create video source, attempt {currentTry + 1}");

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

                        System.Diagnostics.Debug.WriteLine("Video source created successfully");
                        return;
                    }
                    else
                    {
                        throw new Exception("No video capture devices found");
                    }
                }
                catch (Exception ex)
                {
                    currentTry++;
                    System.Diagnostics.Debug.WriteLine($"Attempt {currentTry} failed: {ex.Message}");

                    if (currentTry >= maxRetries)
                    {
                        System.Diagnostics.Debug.WriteLine("All retry attempts failed");
                        throw;
                    }

                    await Task.Delay(2000); // Increased delay between retries
                }
            }
        }



        private void CreateLocalDisplay()
        {
            var display = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
            };
            remotePanel.Controls.Add(display);
            displays["local"] = display;
            this.UpdateVideoLayout();
        }

        public async Task ConnectToParticipant(string ip, int port)
        {
            System.Diagnostics.Debug.WriteLine($"Connecting to participant: {ip}:{port}");
            var videoClient = new UdpClient();
            videoClient.Connect(ip, port);
            connections[ip] = videoClient;

            var display = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
            };
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
            {
                DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() => DiscordFormsHolder.getInstance().DiscordApp.AddNewParticipantDisplay(
               remotePanel, display)));
                displays[ip] = display;
                this.UpdateVideoLayout(); // Update layout after adding new participant 
            }));

            var waveProvider = new BufferedWaveProvider(this.waveFormat)
            {
                BufferLength = 44100, // 1 seconds buffer
                DiscardOnBufferOverflow = true // Automatically discard old data when buffer is full
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
            var bitmap = new Bitmap(VIDEO_WIDTH, VIDEO_HEIGHT, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, VIDEO_WIDTH, VIDEO_HEIGHT);
            var bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int size = VIDEO_WIDTH * VIDEO_HEIGHT * 4;  // 4 bytes per pixel (ARGB)
            byte[] frameData = new byte[size];
            Marshal.Copy(frame.data, frameData, 0, size);
            Marshal.Copy(frameData, 0, bmpData.Scan0, size);


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

            displays["local"].Invoke(new Action(() => {
                displays["local"].Image?.Dispose();
                displays["local"].Image = bitmap;
            }));


            
        }

        private void AudioInput_DataAvailable(object sender, WaveInEventArgs e)
        {
            var packet = new AudioPacket(e.Buffer);
            var packetBytes = packet.ToBytes();
            foreach (var client in connections.Values)
            {
                client.Send(packetBytes, packetBytes.Length);
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
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Receive error: {ex.Message}");
                }
            }
        }

        public void ProcessDataFromOtherUser(string ip, byte[] bytes)
        {
            try
            {
                var packetType = BasePacket.GetPacketType(bytes);

                if (packetType == PacketType.Video)
                {
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
                            var bitmap = new Bitmap(ms);
                            if (displays.ContainsKey(ip))
                            {
                                displays[ip].Invoke(new Action(() =>
                                {
                                    displays[ip].Image?.Dispose();
                                    displays[ip].Image = bitmap;
                                }));
                            }
                        }
                    }
                }
                else
                {
                    var packet = AudioPacket.FromBytes(bytes);
                    if (audioInputs.ContainsKey(ip))
                    { 

                        var waveProvider = audioInputs[ip];

                        float bufferUsagePercent = (float)waveProvider.BufferedBytes / waveProvider.BufferLength * 100;
                        System.Diagnostics.Debug.WriteLine($"Buffer usage: {bufferUsagePercent:F1}%");

                        if (bufferUsagePercent > 80)
                        {
                            System.Diagnostics.Debug.WriteLine("Buffer getting full, clearing half...");
                            byte[] remainingData = new byte[waveProvider.BufferedBytes / 2];
                            waveProvider.Read(remainingData, 0, remainingData.Length);
                        }

                        try
                        {
                            waveProvider.AddSamples(packet.AudioData, 0, packet.AudioData.Length);
                            System.Diagnostics.Debug.WriteLine($"Added {packet.AudioData.Length} bytes to buffer. Current buffer size: {waveProvider.BufferedBytes}");

                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error adding samples: {ex.Message}");
                            // Optional: clear buffer and try again
                            waveProvider.ClearBuffer();
                        }
                    }
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



        public void DisconnectFromParticipant(string ip)
        {
            if (connections.ContainsKey(ip))
            {
                // Dispose of the UDP client
                connections[ip].Dispose();
                connections.Remove(ip);

                // Remove and dispose of their display
                if (displays.ContainsKey(ip))
                {
                    displays[ip].Dispose();
                    displays.Remove(ip);
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

                // Update the video layout
                UpdateVideoLayout();
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


