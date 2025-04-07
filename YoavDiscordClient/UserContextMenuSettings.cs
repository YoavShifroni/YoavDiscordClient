using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace YoavDiscordClient
{
    public class UserContextMenuSettings
    {
        // Singleton pattern
        private static UserContextMenuSettings _instance;

        // Dictionary to store user settings by user ID
        private Dictionary<int, UserSettings> _userSettings;


        private UserContextMenuSettings()
        {
            _userSettings = new Dictionary<int, UserSettings>();
        }

        public static UserContextMenuSettings GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserContextMenuSettings();

            }
            return _instance;
        }

        // Get settings for a specific user
        public UserSettings GetUserSettings(int userId)
        {
            if (!_userSettings.ContainsKey(userId))
            {
                _userSettings[userId] = new UserSettings();
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
                System.Diagnostics.Debug.WriteLine($"Error setting video mute state for user {userId} : { ex.Message}");
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

    // User settings structure - No changes needed as IsVideoMuted already exists
    public class UserSettings
    {
        public bool IsMuted { get; set; }
        public bool IsVideoMuted { get; set; }
        public bool IsDeafened { get; set; }

        public UserSettings()
        {
            IsMuted = false;
            IsVideoMuted = false;
            IsDeafened = false;
        }
    }

    
}