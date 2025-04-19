using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient.Managers
{
    public class UserContextMenuSettings
    {
        // Singleton pattern
        private static UserContextMenuSettings _instance;

        // Dictionary to store user settings by user ID
        private Dictionary<int, UserState> _userSettings;

        /// <summary>
        /// Private constructor for the UserContextMenuSettings singleton.
        /// Initializes the user settings dictionary.
        /// </summary>
        /// <remarks>
        /// This constructor is private to enforce the singleton pattern.
        /// It initializes an empty dictionary to store user state information.
        /// </remarks>
        private UserContextMenuSettings()
        {
            _userSettings = new Dictionary<int, UserState>();
        }

        /// <summary>
        /// Gets the singleton instance of the UserContextMenuSettings class.
        /// </summary>
        /// <returns>The singleton instance of UserContextMenuSettings.</returns>
        /// <remarks>
        /// This method implements the lazy initialization pattern for the singleton.
        /// The instance is created on first access and reused for all subsequent calls.
        /// This ensures that user context menu settings are consistent throughout the application.
        /// </remarks>
        public static UserContextMenuSettings GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserContextMenuSettings();
            }
            return _instance;
        }

        /// <summary>
        /// Get settings for a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserState GetUserSettings(int userId)
        {
            if (!_userSettings.ContainsKey(userId))
            {
                _userSettings[userId] = new UserState();
                System.Diagnostics.Debug.WriteLine($"Created new settings for user {userId}");
            }
            return _userSettings[userId];
        }

        /// <summary>
        /// Sets the mute state for a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="isMuted">Whether the user is muted</param>
        public void SetUserMuted(int userId, bool isMuted)
        {
            try
            {
                var settings = GetUserSettings(userId);
                if (settings.IsMuted != isMuted)
                {
                    settings.IsMuted = isMuted;
                    System.Diagnostics.Debug.WriteLine($"User {userId} mute state set to {isMuted}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting mute state for user {userId} : {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the video mute state for a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="isVideoMuted">Whether the user's video is muted</param>
        public void SetUserVideoMuted(int userId, bool isVideoMuted)
        {
            try
            {
                var settings = GetUserSettings(userId);
                if (settings.IsVideoMuted != isVideoMuted)
                {
                    settings.IsVideoMuted = isVideoMuted;
                    System.Diagnostics.Debug.WriteLine($"User {userId} video mute state set to {isVideoMuted}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting video mute state for user {userId} : {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the deafen state for a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="isDeafened">Whether the user is deafened</param>
        public void SetUserDeafened(int userId, bool isDeafened)
        {
            try
            {
                var settings = GetUserSettings(userId);
                if (settings.IsDeafened != isDeafened)
                {
                    settings.IsDeafened = isDeafened;
                    System.Diagnostics.Debug.WriteLine($"User {userId} deafen state set to {isDeafened}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting deafen state for user {userId} : {ex.Message}");
            }
        }
    }

    // User settings structure
    public class UserState
    {
        public bool IsMuted { get; set; }
        public bool IsVideoMuted { get; set; }
        public bool IsDeafened { get; set; }

        public UserState()
        {
            IsMuted = false;
            IsVideoMuted = false;
            IsDeafened = false;
        }
    }

    
}