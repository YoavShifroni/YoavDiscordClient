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

    public class VideoStreamConnection : IDisposable
    {
        private VideoStreamConnectionHandler implementation;
        private SemaphoreSlim initLock = new SemaphoreSlim(1, 1);
        private bool disposed = false;

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

        public async Task ConnectToParticipant(string ip, int port, byte[] profilePicture, string username, int userId)
        {
            try
            {
                await implementation.ConnectToParticipant(ip, port, profilePicture, username, userId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error connecting to participant {username} ({ip}:{port}): {ex.Message}");
                throw;
            }
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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