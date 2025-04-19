using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.MixedReality.WebRTC;
using Microsoft.VisualBasic.ApplicationServices;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using YoavDiscordClient.Enums;

namespace YoavDiscordClient
{
#pragma warning disable CA1416

    /// <summary>
    /// Manages video streaming connections between users in the Discord clone application.
    /// Acts as a wrapper around VideoStreamConnectionHandler providing exception handling,
    /// synchronization, and resource management.
    /// </summary>
    /// <remarks>
    /// This class follows the façade pattern, delegating actual implementation to 
    /// VideoStreamConnectionHandler while providing a simpler interface with
    /// consistent error handling and logging. It manages WebRTC connections and
    /// media streaming between participants.
    /// 
    /// The class is designed to be thread-safe during initialization and properly
    /// disposes all unmanaged resources when no longer needed.
    /// </remarks>
    public class VideoStreamConnection : IDisposable
    {
        /// <summary>
        /// The underlying implementation that handles the actual video streaming functionality.
        /// </summary>
        public VideoStreamConnectionHandler implementation;

        /// <summary>
        /// Semaphore used to synchronize initialization to prevent multiple simultaneous
        /// Initialize calls from different threads.
        /// </summary>
        private SemaphoreSlim initLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Flag indicating whether this instance has been disposed.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the VideoStreamConnection class.
        /// </summary>
        /// <param name="remotePanel">The panel where remote video streams will be displayed.</param>
        /// <exception cref="Exception">Thrown when the video stream connection cannot be created.</exception>
        public VideoStreamConnection(Panel remotePanel)
        {
            try
            {
                // Create the refactored implementation
                implementation = new VideoStreamConnectionHandler(remotePanel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating video stream connection: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Initializes the video streaming components asynchronously.
        /// This method is thread-safe and prevents multiple concurrent initialization attempts.
        /// </summary>
        /// <returns>A task representing the asynchronous initialization operation.</returns>
        /// <exception cref="Exception">Thrown when initialization fails.</exception>
        public async Task Initialize()
        {
            // Use a lock to prevent multiple simultaneous initializations
            await initLock.WaitAsync();

            try
            {
                System.Diagnostics.Debug.WriteLine("VideoStreamConnection: Starting initialization");
                await implementation.Initialize();
                System.Diagnostics.Debug.WriteLine("VideoStreamConnection: Initialization completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing video stream connection: {ex.Message}");
                throw;
            }
            finally
            {
                initLock.Release();
            }
        }

        /// <summary>
        /// Establishes a connection with another participant in the video stream.
        /// </summary>
        /// <param name="ip">The IP address of the participant to connect to.</param>
        /// <param name="port">The port number for the connection.</param>
        /// <param name="profilePicture">The profile picture of the participant as a byte array.</param>
        /// <param name="username">The username of the participant.</param>
        /// <param name="userId">The unique identifier of the participant.</param>
        /// <exception cref="Exception">Thrown when connection to the participant fails.</exception>
        public void ConnectToParticipant(string ip, int port, byte[] profilePicture, string username, int userId)
        {
            try
            {
                implementation.ConnectToParticipant(ip, port, profilePicture, username, userId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error connecting to participant {username} ({ip}:{port}): {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Disconnects from a participant identified by their user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the participant to disconnect from.</param>
        public void DisconnectFromParticipant(int userId)
        {
            try
            {
                implementation.DisconnectFromParticipant(userId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error disconnecting from participant with ID {userId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Disconnects from a participant identified by their IP address.
        /// </summary>
        /// <param name="ip">The IP address of the participant to disconnect from.</param>
        public void DisconnectFromParticipant(string ip)
        {
            try
            {
                implementation.DisconnectFromParticipant(ip);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error disconnecting from participant with IP {ip}: {ex.Message}");
            }
        }

        /// <summary>
        /// Toggles the mute state of the local audio stream.
        /// </summary>
        public void ToggleAudioMute()
        {
            try
            {
                implementation.ToggleAudioMute();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error toggling audio mute: {ex.Message}");
            }
        }

        /// <summary>
        /// Toggles the mute state of the local video stream.
        /// </summary>
        public void ToggleVideoMute()
        {
            try
            {
                implementation.ToggleVideoMute();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error toggling video mute: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the global mute state for all audio streams.
        /// </summary>
        /// <param name="muted">True to mute all audio, false to unmute.</param>
        public void SetGlobalMuteState(bool muted)
        {
            try
            {
                implementation.SetGlobalMuteState(muted);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting global mute state to {muted}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the mute state that is enforced by users with higher role permissions.
        /// </summary>
        /// <param name="muted">True to enforce mute by higher role, false to allow speaking.</param>
        public void SetMutedByHigherRoleState(bool muted)
        {
            try
            {
                implementation.SetMutedByHigherRoleState(muted);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting mute by higher role state to {muted}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the global deafen state, which mutes both incoming and outgoing audio.
        /// </summary>
        /// <param name="deafened">True to deafen (disable all audio), false to enable audio.</param>
        public void SetGlobalDeafenState(bool deafened)
        {
            try
            {
                implementation.SetGlobalDeafenState(deafened);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting global deafen state to {deafened}: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes incoming data packets from other users in the video stream.
        /// </summary>
        /// <param name="ip">The IP address of the user sending the data.</param>
        /// <param name="bytes">The data packet as a byte array.</param>
        public void ProcessDataFromOtherUser(string ip, byte[] bytes)
        {
            try
            {
                implementation.ProcessDataFromOtherUser(ip, bytes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing data from {ip}: {ex.Message}");
            }
        }

        /// <summary>
        /// Reinitializes the video stream components, useful when recovering from errors
        /// or when changing device configurations.
        /// </summary>
        /// <returns>A task representing the asynchronous reinitialization operation.</returns>
        /// <exception cref="Exception">Thrown when reinitialization fails.</exception>
        public async Task ReInitializeVideo()
        {
            try
            {
                // Access the internal VideoManager via the implementation
                await implementation.ReInitializeVideo();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reinitializing video: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Disposes of the resources used by the VideoStreamConnection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the VideoStreamConnection and
        /// optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources;
        /// False to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Inside the dispose method in Video Stream Connection class");
                        implementation?.Dispose();
                        initLock?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error disposing video stream connection: {ex.Message}");
                    }
                }

                disposed = true;
            }
        }
    }
}