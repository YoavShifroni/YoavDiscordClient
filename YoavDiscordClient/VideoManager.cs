using Microsoft.MixedReality.WebRTC;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoavDiscordClient.Events;
using System.Threading;

namespace YoavDiscordClient
{
    public class VideoManager : IDisposable
    {
        #region Constants

        private const int VIDEO_WIDTH = 640;
        private const int VIDEO_HEIGHT = 480;
        private const int VIDEO_FRAMERATE = 20;

        #endregion

        #region Events

        public event EventHandler<VideoFrameEventArgs> LocalVideoFrameReady;
        public event EventHandler<EmptyVideoEventArgs> VideoMuteStateChanged;

        #endregion

        #region Private fields

        private DeviceVideoTrackSource videoSource;
        private LocalVideoTrack localVideo;
        private bool isVideoMuted = false;
        private bool disposed = false;
        private PictureBox localDisplay;
        private bool isInitialized = false;
        private readonly object _lockObject = new object();

        #endregion

        #region Public properties

        /// <summary>
        /// Gets whether video streaming is currently muted
        /// </summary>
        public bool IsVideoMuted => isVideoMuted;

        #endregion

        /// <summary>
        /// Creates a new VideoManager with the specified local display
        /// </summary>
        /// <param name="localDisplay">The PictureBox to display the local video feed</param>
        public VideoManager(PictureBox localDisplay)
        {
            this.localDisplay = localDisplay ?? throw new ArgumentNullException(nameof(localDisplay));
        }

        /// <summary>
        /// Initializes video capture from the default device
        /// </summary>
        public async Task Initialize()
        {
            try
            {
                // Always do a complete cleanup first
                await CleanupVideo(true);
                isInitialized = false;

                System.Diagnostics.Debug.WriteLine("Starting video initialization...");

                var devices = await DeviceVideoTrackSource.GetCaptureDevicesAsync();

                if (devices.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Found {devices.Count} video devices. Using device: {devices[0].name}");

                    // Create the video source with specific config
                    videoSource = await DeviceVideoTrackSource.CreateAsync(
                        new LocalVideoDeviceInitConfig
                        {
                            videoDevice = devices[0],
                            width = VIDEO_WIDTH,
                            height = VIDEO_HEIGHT,
                            framerate = VIDEO_FRAMERATE
                        });

                    // Create the video track from the source
                    localVideo = LocalVideoTrack.CreateFromSource(videoSource, new LocalVideoTrackInitConfig());

                    // Add the frame ready event handler
                    localVideo.Argb32VideoFrameReady += LocalVideoFrame_Ready;

                    isInitialized = true;
                    System.Diagnostics.Debug.WriteLine("Video initialization completed successfully");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No video devices found, showing profile picture");
                    ShowProfilePicture();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing video manager: {ex.Message}");
                ShowProfilePicture();
            }
        }

        /// <summary>
        /// Toggles the video mute state
        /// </summary>
        public void ToggleVideoMute()
        {
            isVideoMuted = !isVideoMuted;

            try
            {
                if (isVideoMuted)
                {
                    // Emit empty video packet event
                    VideoMuteStateChanged?.Invoke(this, new EmptyVideoEventArgs(true));

                    // Update UI to show profile picture
                    ShowProfilePicture();

                    lock (this._lockObject)
                    {
                        // Stop video capture frames by safely removing the event handler
                        if (localVideo != null)
                        {
                            localVideo.Argb32VideoFrameReady -= LocalVideoFrame_Ready;
                        }
                    }
    
                }
                else
                {
                    // Emit empty video packet event
                    VideoMuteStateChanged?.Invoke(this, new EmptyVideoEventArgs(false));

                    // Clear profile picture
                    if (localDisplay != null && !localDisplay.IsDisposed && localDisplay.IsHandleCreated)
                    {
                        localDisplay.Invoke(new Action(() => {
                            localDisplay.Controls.Clear();
                            if (localDisplay.Image != null)
                            {
                                localDisplay.Image.Dispose();
                                localDisplay.Image = null;
                            }
                        }));
                    }

                    // If video initialization stale or failed, reinitialize
                    if (!isInitialized || localVideo == null)
                    {
                        _ = Initialize();
                        return;
                    }

                    // Restart video capture frames
                    lock (this._lockObject) 
                    {
                        if (localVideo != null)
                        {
                            // Make sure we're not double-subscribing
                            localVideo.Argb32VideoFrameReady -= LocalVideoFrame_Ready;
                            localVideo.Argb32VideoFrameReady += LocalVideoFrame_Ready;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error toggling video mute state: {ex.Message}");
            }
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

                bitmap.UnlockBits(bmpData);

                // Generate event with frame data for transmission
                var videoData = BitmapToByteArray(bitmap);
                LocalVideoFrameReady?.Invoke(this, new VideoFrameEventArgs(videoData, DateTime.Now));

                // Update local display
                if (localDisplay != null && !localDisplay.IsDisposed && localDisplay.IsHandleCreated)
                {
                    localDisplay.Invoke(new Action(() => {
                        try
                        {
                            var oldImage = localDisplay.Image;
                            localDisplay.Image = bitmap;
                            oldImage?.Dispose();
                            localDisplay.Refresh();

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

        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (var clonedBitmap = new Bitmap(bitmap)) // Clone to avoid conflicts
            using (var ms = new MemoryStream())
            {
                var jpegEncoder = GetJpegEncoder();
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 70L);
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

        private void ShowProfilePicture()
        {
            if (localDisplay == null || localDisplay.IsDisposed || !localDisplay.IsHandleCreated)
                return;

            localDisplay.Invoke(new Action(() => {
                try
                {
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
                    System.Diagnostics.Debug.WriteLine($"Error showing profile picture: {ex.Message}");
                }
            }));
        }

        /// <summary>
        /// Performs a thorough cleanup of video resources
        /// </summary>
        private async Task CleanupVideo(bool forceCleanup = false)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Starting video cleanup...");

                lock (this._lockObject)
                {
                    if (localVideo != null)
                    {
                        // First, safely remove the event handler to prevent further frames
                        localVideo.Argb32VideoFrameReady -= LocalVideoFrame_Ready;
                    }
                }

                if (localVideo != null)
                {

                    //// Create a timeout task
                    //var timeoutTask = Task.Delay(3000); // 3 second timeout

                    //// Create a task to fully dispose the track
                    //var cleanupTask = Task.Run(() =>
                    //{
                    //    try
                    //    {
                    //        localVideo.Dispose();

                    //        System.Diagnostics.Debug.WriteLine("Local video track disposed");
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        System.Diagnostics.Debug.WriteLine($"Error disposing local video track: {ex.Message}");
                    //    }
                    //    finally
                    //    {
                    //        localVideo = null;
                    //    }
                    //});

                    //// Wait for either the cleanup to complete or timeout
                    //var completedTask = await Task.WhenAny(cleanupTask, timeoutTask);
                    //if (completedTask == timeoutTask)
                    //{
                    //    System.Diagnostics.Debug.WriteLine("Video track cleanup timed out");
                    //    // Force nullify for garbage collection
                    //    localVideo = null;
                    //
                    
                    try
                    {
                        localVideo.Dispose();

                        System.Diagnostics.Debug.WriteLine("Local video track disposed");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error disposing local video track: {ex.Message}");
                    }
                    finally
                    {
                        localVideo = null;
                    }

                }

                if (videoSource != null)
                {
                    try
                    {
                        videoSource.Dispose();
                        System.Diagnostics.Debug.WriteLine("Video source disposed");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error disposing video source: {ex.Message}");
                    }
                    finally
                    {
                        videoSource = null;
                    }
                }

                // If we're doing a forced cleanup for reinitialization,
                // make sure we help the GC free memory
                if (forceCleanup)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    await Task.Delay(500); // Small delay to ensure hardware has time to reset
                }

                System.Diagnostics.Debug.WriteLine("Video cleanup completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during video cleanup: {ex.Message}");
            }
        }

        /// <summary>
        /// Releases all resources used by the VideoManager
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the VideoManager
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    System.Diagnostics.Debug.WriteLine("Inside the dispose method in Video Manager class");
                    // Clean up managed resources
                    _ = CleanupVideo();
                }

                disposed = true;
            }
        }
    }
}