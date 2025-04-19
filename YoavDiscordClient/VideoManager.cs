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
    /// <summary>
    /// Manages video capture, processing, and display for Discord clone client.
    /// Responsible for handling local video sources, toggling video mute state,
    /// and generating frame data for transmission to remote clients.
    /// </summary>
    /// <remarks>
    /// The VideoManager uses Microsoft's Mixed Reality WebRTC library to access
    /// the local camera and generate video frames. It provides video frames in ARGB32 format
    /// and handles conversion to appropriate formats for display and transmission.
    /// 
    /// Thread safety is maintained through internal locking mechanisms when accessing
    /// shared resources like video tracks and event handlers.
    /// </remarks>
    public class VideoManager : IDisposable
    {
        #region Constants

        /// <summary>
        /// Width of the video frames in pixels.
        /// </summary>
        private const int VIDEO_WIDTH = 640;

        /// <summary>
        /// Height of the video frames in pixels.
        /// </summary>
        private const int VIDEO_HEIGHT = 480;

        /// <summary>
        /// Target framerate for video capture in frames per second.
        /// </summary>
        private const int VIDEO_FRAMERATE = 20;

        #endregion

        #region Events

        /// <summary>
        /// Event fired when a new local video frame is ready for processing or transmission.
        /// Provides the frame data as a byte array and timestamp.
        /// </summary>
        public event EventHandler<VideoFrameEventArgs> LocalVideoFrameReady;

        /// <summary>
        /// Event fired when the video mute state changes.
        /// Used to notify other components about video mute state changes.
        /// </summary>
        public event EventHandler<EmptyVideoEventArgs> VideoMuteStateChanged;

        #endregion

        #region Private fields

        /// <summary>
        /// The device video track source providing access to the hardware camera.
        /// </summary>
        private DeviceVideoTrackSource videoSource;

        /// <summary>
        /// The local video track created from the video source.
        /// </summary>
        private LocalVideoTrack localVideo;

        /// <summary>
        /// Flag indicating whether video is currently muted.
        /// </summary>
        private bool isVideoMuted = false;

        /// <summary>
        /// Flag indicating whether this instance has been disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// PictureBox control for displaying the local video feed.
        /// </summary>
        private PictureBox localDisplay;

        /// <summary>
        /// Flag indicating whether video has been successfully initialized.
        /// </summary>
        private bool isInitialized = false;

        /// <summary>
        /// Lock object for thread synchronization when accessing shared resources.
        /// </summary>
        private readonly object _lockObject = new object();

        #endregion

        #region Public properties

        /// <summary>
        /// Gets whether video streaming is currently muted.
        /// When muted, the profile picture is displayed instead of the video feed.
        /// </summary>
        public bool IsVideoMuted => isVideoMuted;

        #endregion

        /// <summary>
        /// Creates a new VideoManager with the specified local display.
        /// </summary>
        /// <param name="localDisplay">The PictureBox control to display the local video feed.</param>
        /// <exception cref="ArgumentNullException">Thrown when localDisplay is null.</exception>
        public VideoManager(PictureBox localDisplay)
        {
            this.localDisplay = localDisplay ?? throw new ArgumentNullException(nameof(localDisplay));
        }

        /// <summary>
        /// Initializes video capture from the default device.
        /// Creates the video source and track, and sets up event handlers.
        /// </summary>
        /// <returns>A task representing the asynchronous initialization operation.</returns>
        /// <remarks>
        /// If initialization fails or no video devices are found, the profile picture
        /// will be displayed instead of a video feed.
        /// </remarks>
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
        /// Toggles the video mute state between muted and unmuted.
        /// When muted, the profile picture is displayed instead of the video feed.
        /// </summary>
        /// <remarks>
        /// This method handles:
        /// - Updating the mute state flag
        /// - Firing the VideoMuteStateChanged event
        /// - Showing/hiding the profile picture
        /// - Managing event subscriptions for frame processing
        /// - Reinitializing video if needed
        /// </remarks>
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

        /// <summary>
        /// Processes video frames from the local video track.
        /// Converts the frame to a bitmap, raises the LocalVideoFrameReady event,
        /// and updates the local display.
        /// </summary>
        /// <param name="frame">The ARGB32 video frame received from the camera.</param>
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

        /// <summary>
        /// Converts a Bitmap to a compressed JPEG byte array for transmission.
        /// </summary>
        /// <param name="bitmap">The bitmap to convert.</param>
        /// <returns>A byte array containing the JPEG-compressed bitmap data.</returns>
        /// <remarks>
        /// The compression quality is set to 70%, balancing image quality and data size
        /// for efficient transmission over the network.
        /// </remarks>
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

        /// <summary>
        /// Gets the JPEG codec information for image compression.
        /// </summary>
        /// <returns>The ImageCodecInfo for JPEG encoding.</returns>
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

        /// <summary>
        /// Displays the user's profile picture in the local video display.
        /// Creates a panel with the profile picture and username display.
        /// </summary>
        /// <remarks>
        /// This method is called when video is muted or when video initialization fails.
        /// It creates a circular profile picture with the username displayed below it.
        /// </remarks>
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
                        Image = DiscordFormsHolder.getInstance().DiscordApp.GetUserManager().UserProfilePicture,
                        Location = new Point(
                            (localDisplay.Width - 100) / 2,
                            (localDisplay.Height - 100) / 2
                        )
                    };

                    Label usernameLabel = new Label
                    {
                        Text = DiscordFormsHolder.getInstance().DiscordApp.GetUserManager().Username,
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
        /// Performs a thorough cleanup of video resources.
        /// Safely removes event handlers and disposes of video track and source objects.
        /// </summary>
        /// <param name="forceCleanup">If true, performs additional cleanup operations including
        /// garbage collection to ensure hardware resources are released.</param>
        /// <returns>A task representing the asynchronous cleanup operation.</returns>
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
        /// Releases all resources used by the VideoManager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the VideoManager.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources;
        /// False to release only unmanaged resources.</param>
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