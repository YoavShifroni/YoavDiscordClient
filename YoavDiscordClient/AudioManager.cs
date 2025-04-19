using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoavDiscordClient.Events;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Threading;

namespace YoavDiscordClient
{
    public class AudioManager : IDisposable
    {
        #region Constants
        /// <summary>
        /// Defines the sample rate in Hz for audio capture and playback.
        /// Standard CD-quality sample rate providing good voice clarity while limiting bandwidth usage.
        /// </summary>
        private const int SAMPLE_RATE = 44100;

        /// <summary>
        /// Defines the number of audio channels to use.
        /// Set to 1 for mono, which is sufficient for voice communication and reduces bandwidth usage.
        /// </summary>
        private const int CHANNELS = 1;

        /// <summary>
        /// Defines the size of the audio buffer in milliseconds.
        /// Lower values reduce latency but increase CPU usage and network packets.
        /// 50ms provides a good balance between responsiveness and efficiency.
        /// </summary>
        private const int BUFFER_MILLISECONDS = 50;

        /// <summary>
        /// Defines the target audio latency in milliseconds for playback.
        /// This value affects how much audio is buffered before playback begins.
        /// Higher values improve stability but increase delay in communication.
        /// </summary>
        private const int AUDIO_LATENCY = 100;
        #endregion

        #region Events
        /// <summary>
        /// Event raised when local audio data is available for transmission.
        /// Subscribers can use this event to send audio data to remote participants.
        /// Provides audio data and timestamp information via AudioPacketEventArgs.
        /// </summary>
        public event EventHandler<AudioPacketEventArgs> LocalAudioDataAvailable;
        #endregion

        #region Private properties
        /// <summary>
        /// Captures audio from the default system microphone.
        /// Provides a stream of audio samples for processing and transmission.
        /// </summary>
        private WaveIn audioInput;

        /// <summary>
        /// Handles audio playback to the default system audio output device.
        /// </summary>
        private WaveOut audioOutput;

        /// <summary>
        /// Maps remote participant identifiers to their individual audio buffers.
        /// Each remote participant has a dedicated buffer for smooth playback.
        /// </summary>
        private Dictionary<string, BufferedWaveProvider> remoteAudioInputs;

        /// <summary>
        /// Mixes audio streams from multiple remote participants into a single output stream.
        /// Allows multiple people to be heard simultaneously during voice communication.
        /// </summary>
        private MixingSampleProvider mixer;

        /// <summary>
        /// Controls the volume of the mixed audio output.
        /// Allows for global volume adjustment of all remote participants.
        /// </summary>
        private VolumeSampleProvider volumeProvider;

        /// <summary>
        /// Defines the format of the audio data used throughout the system.
        /// Ensures consistency in audio processing between capture and playback.
        /// </summary>
        private WaveFormat waveFormat;

        /// <summary>
        /// Indicates whether the local microphone is muted.
        /// When true, audio is still captured but not transmitted to other participants.
        /// </summary>
        private bool isAudioMuted = false;

        /// <summary>
        /// Indicates whether all incoming audio is muted.
        /// When true, remote participants' audio is not played locally, but the microphone remains active.
        /// </summary>
        private bool isGloballyMuted = false;

        /// <summary>
        /// Indicates whether the user is deafened (both incoming and outgoing audio disabled).
        /// When true, the user cannot hear others and cannot be heard, regardless of other mute settings.
        /// </summary>
        private bool isGloballyDeafened = false;

        /// <summary>
        /// Indicates whether the user has been muted by someone with a higher role.
        /// When true, the user cannot unmute themselves until a moderator removes this restriction.
        /// </summary>
        private bool isMutedByHigherRole = false;

        /// <summary>
        /// Tracks whether this instance has been disposed.
        /// Used to prevent multiple dispose operations and resource leaks.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Timer for periodically checking the health of audio connections.
        /// Used to detect and recover from audio device failures or disconnections.
        /// </summary>
        private System.Windows.Forms.Timer healthCheckTimer;

        /// <summary>
        /// Tracks the last time audio data was received.
        /// Used to detect audio stream interruptions and trigger recovery mechanisms.
        /// </summary>
        private DateTime lastDataAvailableCall = DateTime.MinValue;
        #endregion

        #region Public properties

        /// <summary>
        /// Gets whether audio input is currently muted
        /// </summary>
        public bool IsAudioMuted => isAudioMuted || isGloballyMuted;

        /// <summary>
        /// Gets whether audio output is currently deafened
        /// </summary>
        public bool IsAudioDeafened => isGloballyDeafened;

        #endregion

        /// <summary>
        /// Creates a new AudioManager
        /// </summary>
        public AudioManager()
        {
            // Create wave format once
            this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(SAMPLE_RATE, CHANNELS);
            this.remoteAudioInputs = new Dictionary<string, BufferedWaveProvider>();

            InitializeAudio();

            // Setup health check timer to detect and repair audio input issues
            InitializeHealthCheckTimer();

            System.Diagnostics.Debug.WriteLine("Completed AudioManager constructor");
        }

        /// <summary>
        /// Toggles the audio mute state
        /// </summary>
        public void ToggleAudioMute()
        {
            if (isGloballyMuted)
                return; // Can't toggle if globally muted

            if (isMutedByHigherRole)
            {
                return;
            }

            isAudioMuted = !isAudioMuted;
            System.Diagnostics.Debug.WriteLine($"In ToggleAudioMute: {isAudioMuted}");

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
                    // Attempt to repair audio input
                    ReinitializeAudioInput();
                }
            }
        }

        /// <summary>
        /// Sets the global mute state
        /// </summary>
        /// <param name="muted">Whether audio should be globally muted</param>
        public void SetGlobalMuteState(bool muted)
        {
            if (this.isMutedByHigherRole)
            {
                return;
            }
            isGloballyMuted = muted;

            // Apply global mute setting regardless of channel-specific setting
            if (audioInput != null)
            {
                try
                {
                    if (isGloballyMuted)
                    {
                        // Force mute audio
                        audioInput.StopRecording();
                    }
                    else if (!isAudioMuted) // Only unmute if the channel-specific mute is also off
                    {
                        // Resume audio if not channel-muted
                        audioInput.StartRecording();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error applying global mute state: {ex.Message}");
                    // Attempt to repair audio input
                    ReinitializeAudioInput();
                }
            }
        }

        /// <summary>
        /// Set muted by higher role
        /// </summary>
        /// <param name="muted"></param>
        public void SetMutedByHigherRoleState(bool muted)
        {
            this.isMutedByHigherRole = muted;
            if (audioInput != null)
            {
                try
                {
                    if (isMutedByHigherRole)
                    {
                        // Force mute audio
                        audioInput.StopRecording();
                    }
                    else
                    {
                        // Resume audio if not channel-muted
                        audioInput.StartRecording();
                        isAudioMuted = false;
                        isGloballyMuted = false;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error applying global mute state: {ex.Message}");
                    // Attempt to repair audio input
                    ReinitializeAudioInput();
                }
            }
        }

        /// <summary>
        /// Sets the global deafen state
        /// </summary>
        /// <param name="deafened">Whether audio should be globally deafened</param>
        public void SetGlobalDeafenState(bool deafened)
        {
            isGloballyDeafened = deafened;

            // Apply global deafen setting to audio output
            if (audioOutput != null)
            {
                try
                {
                    if (isGloballyDeafened)
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

        /// <summary>
        /// Adds a remote participant for audio processing
        /// </summary>
        /// <param name="participantId">The unique identifier for the participant</param>
        /// <returns>True if the participant was successfully added</returns>
        public bool AddParticipant(string participantId)
        {
            if (string.IsNullOrEmpty(participantId) || remoteAudioInputs.ContainsKey(participantId))
                return false;

            try
            {
                var waveProvider = new BufferedWaveProvider(this.waveFormat)
                {
                    BufferLength = this.waveFormat.AverageBytesPerSecond,
                    DiscardOnBufferOverflow = false
                };

                remoteAudioInputs[participantId] = waveProvider;
                var sampleProvider = new WaveToSampleProvider(waveProvider);
                mixer.AddMixerInput(sampleProvider);

                System.Diagnostics.Debug.WriteLine($"Audio setup complete for {participantId}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting up audio for {participantId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Removes a remote participant from audio processing
        /// </summary>
        /// <param name="participantId">The unique identifier for the participant</param>
        /// <returns>True if the participant was successfully removed</returns>
        public bool RemoveParticipant(string participantId)
        {
            if (string.IsNullOrEmpty(participantId) || !remoteAudioInputs.ContainsKey(participantId))
                return false;

            try
            {
                remoteAudioInputs.Remove(participantId);

                // Recreate the mixer with remaining inputs
                RecreateAudioMixer();
                System.Diagnostics.Debug.WriteLine($"Removed Audio setup for {participantId}");

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing audio for {participantId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Processes audio data from a remote participant
        /// </summary>
        /// <param name="participantId">The unique identifier for the participant</param>
        /// <param name="audioData">The audio data to process</param>
        /// <returns>True if the audio data was successfully processed</returns>
        public bool ProcessRemoteAudioData(string participantId, byte[] audioData)
        {
            // If we're deafened, ignore incoming audio
            if (isGloballyDeafened)
                return false;

            if (string.IsNullOrEmpty(participantId) || !remoteAudioInputs.ContainsKey(participantId))
                return false;

            try
            {
                var waveProvider = remoteAudioInputs[participantId];

                float bufferUsagePercent = (float)waveProvider.BufferedBytes / waveProvider.BufferLength * 100;

                if (bufferUsagePercent > 95)
                {
                    System.Diagnostics.Debug.WriteLine($"Buffer getting full ({bufferUsagePercent:F1}%), clearing...");
                    // Instead of clearing half the buffer, implement a sliding window
                    byte[] remainingData = new byte[waveProvider.BufferedBytes - 8820]; // Keep all but one packet
                    waveProvider.Read(new byte[8820], 0, 8820); // Remove oldest packet
                }

                waveProvider.AddSamples(audioData, 0, audioData.Length);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing audio data for {participantId}: {ex.Message}");

                // If there's an error, clear the buffer to prevent further issues
                if (remoteAudioInputs.ContainsKey(participantId))
                {
                    remoteAudioInputs[participantId].ClearBuffer();
                }

                return false;
            }
        }

        /// <summary>
        /// Sets the master volume for audio output
        /// </summary>
        /// <param name="volume">Volume level between 0.0 and 1.0</param>
        public void SetVolume(float volume)
        {
            if (volumeProvider != null)
            {
                volume = Math.Max(0f, Math.Min(1f, volume)); // Clamp between 0 and 1
                volumeProvider.Volume = volume;
            }
        }

        /// <summary>
        /// Initialize the user audio
        /// </summary>
        private void InitializeAudio()
        {
            try
            {
                // Setup audio output (speakers)
                this.mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(SAMPLE_RATE, CHANNELS));
                this.mixer.ReadFully = true;  // Ensure mixer reads all available data

                this.volumeProvider = new VolumeSampleProvider(mixer);
                this.volumeProvider.Volume = 1.0f;

                this.audioOutput = new WaveOut()
                {
                    DesiredLatency = AUDIO_LATENCY
                };

                System.Diagnostics.Debug.WriteLine("Initializing audio output...");

                this.audioOutput.Init(volumeProvider);
                this.audioOutput.Play();

                System.Diagnostics.Debug.WriteLine("Audio output initialized and playing");

                // Setup audio input (microphone)
                InitializeAudioInput();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing audio: {ex.Message}");
            }
        }

        /// <summary>
        /// Initialize audio input
        /// </summary>
        private void InitializeAudioInput()
        {
            try
            {
                // Clean up existing audio input if it exists
                if (audioInput != null)
                {
                    try
                    {
                        audioInput.StopRecording();
                        audioInput.DataAvailable -= AudioInput_DataAvailable;
                        audioInput.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error cleaning up existing audio input: {ex.Message}");
                    }
                }

                // Create new audio input
                this.audioInput = new WaveIn
                {
                    WaveFormat = this.waveFormat,
                    BufferMilliseconds = BUFFER_MILLISECONDS
                };

                // Make sure to attach event handler before starting recording
                this.audioInput.DataAvailable += AudioInput_DataAvailable;

                // Reset timestamp
                this.lastDataAvailableCall = DateTime.Now;

                // Start recording if not muted
                if (!(isAudioMuted || isGloballyMuted || isMutedByHigherRole))
                {
                    this.audioInput.StartRecording();
                    System.Diagnostics.Debug.WriteLine("Audio input initialized and recording");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Audio input initialized but not recording (muted)");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing audio input: {ex.Message}");
            }
        }

        /// <summary>
        /// Reinitialize audio input
        /// </summary>
        private void ReinitializeAudioInput()
        {
            System.Diagnostics.Debug.WriteLine("Reinitializing audio input due to potential issues");

            // Execute on UI thread to avoid cross-thread issues
            if (DiscordFormsHolder.getInstance().GetActiveForm() != null)
            {
                if (DiscordFormsHolder.getInstance().GetActiveForm().InvokeRequired)
                {
                    DiscordFormsHolder.getInstance().GetActiveForm().Invoke(new Action(() =>
                    {
                        InitializeAudioInput();
                    }));
                }
                else
                {
                    InitializeAudioInput();
                }
            }
            else
            {
                // Fallback if no form is available
                InitializeAudioInput();
            }
        }

        /// <summary>
        /// initialize the health check timer
        /// </summary>
        private void InitializeHealthCheckTimer()
        {
            // Create timer to periodically check audio input health
            this.healthCheckTimer = new System.Windows.Forms.Timer
            {
                Interval = 2000, // Check every 2 seconds
                Enabled = true
            };
            this.healthCheckTimer.Tick += HealthCheckTimer_Tick;
            this.healthCheckTimer.Start();
        }

        /// <summary>
        /// Event hendler for the health check timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HealthCheckTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Don't check while muted
                if (isAudioMuted || isGloballyMuted || isMutedByHigherRole)
                    return;

                // If we haven't received any audio data for 2 seconds, reinitialize
                TimeSpan timeSinceLastData = DateTime.Now - lastDataAvailableCall;

                if (timeSinceLastData.TotalSeconds > 2)
                {
                    System.Diagnostics.Debug.WriteLine("Audio input health check: No recent data received, reinitializing");
                    ReinitializeAudioInput();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in health check timer: {ex.Message}");
            }
        }

        /// <summary>
        /// Callback when there is audio data available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AudioInput_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                // Update last activity timestamp
                lastDataAvailableCall = DateTime.Now;

                // Don't send audio if muted
                if (isAudioMuted || isGloballyMuted || isMutedByHigherRole)
                    return;

                // Raise event with audio data
                LocalAudioDataAvailable?.Invoke(this, new AudioPacketEventArgs(e.Buffer));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error processing audio input: {ex.Message}");
            }
        }

        /// <summary>
        /// Recreate the audio mixer
        /// </summary>
        private void RecreateAudioMixer()
        {
            try
            {
                // Create a new mixer
                var newMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(SAMPLE_RATE, CHANNELS));
                newMixer.ReadFully = true;

                // Add all remaining inputs
                foreach (var provider in remoteAudioInputs.Values)
                {
                    newMixer.AddMixerInput(new WaveToSampleProvider(provider));
                }

                // Create a new volume provider
                var newVolumeProvider = new VolumeSampleProvider(newMixer);
                newVolumeProvider.Volume = volumeProvider?.Volume ?? 1.0f;

                // Temporarily stop the output
                bool wasPlaying = audioOutput != null && audioOutput.PlaybackState == PlaybackState.Playing;
                audioOutput?.Stop();

                // Re-initialize with the new mixer
                audioOutput?.Init(newVolumeProvider);

                // Store the new providers
                mixer = newMixer;
                volumeProvider = newVolumeProvider;

                // Restart if it was playing
                if (wasPlaying && !isGloballyDeafened)
                {
                    audioOutput?.Play();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error recreating audio mixer: {ex.Message}");
            }
        }

        /// <summary>
        /// Releases all resources used by the AudioManager
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the AudioManager
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine($"Calling dispose method in Audio Manager class: {disposing}");

            if (!disposed)
            {
                if (disposing)
                {
                    System.Diagnostics.Debug.WriteLine("Inside the dispose method in Audio Manager class");

                    try
                    {
                        // Stop health check timer first
                        if (healthCheckTimer != null)
                        {
                            healthCheckTimer.Stop();
                            healthCheckTimer.Tick -= HealthCheckTimer_Tick;
                            healthCheckTimer.Dispose();
                            healthCheckTimer = null;
                        }

                        if (DiscordFormsHolder.getInstance().GetActiveForm() != null && DiscordFormsHolder.getInstance().GetActiveForm().InvokeRequired)
                        {
                            DiscordFormsHolder.getInstance().GetActiveForm().Invoke(new Action(() =>
                            {
                                System.Diagnostics.Debug.WriteLine("Calling dispose method in Audio Manager class Invoke ");

                                try
                                {
                                    // Cleanup managed resources
                                    if (audioInput != null)
                                    {
                                        audioInput.StopRecording();
                                        // Safely unsubscribe from event before disposal
                                        audioInput.DataAvailable -= AudioInput_DataAvailable;
                                        audioInput.Dispose();
                                        audioInput = null;
                                    }

                                    if (audioOutput != null)
                                    {
                                        // Make sure we're on the UI thread when stopping/disposing audio output
                                        audioOutput.Stop();
                                        audioOutput.Dispose();
                                    }
                                    audioOutput = null;
                                    System.Diagnostics.Debug.WriteLine("Completed dispose method in Audio Manager class Invoke ");
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error during AudioManager disposal 2: {ex}");
                                }
                            }));

                            remoteAudioInputs?.Clear();
                            remoteAudioInputs = null;
                            mixer = null;
                            volumeProvider = null;
                            System.Diagnostics.Debug.WriteLine($"Completed dispose method in Audio Manager class: {disposing}");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error during AudioManager disposal: {ex}");
                        // Continue with disposal even if there's an error
                    }
                }

                // Cleanup unmanaged resources
                disposed = true;
            }
        }
    }
}