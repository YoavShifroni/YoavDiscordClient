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
        // Event for user settings changes
        public event EventHandler<UserSettingsChangedEventArgs> UserSettingsChanged;

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
            }
            return _userSettings[userId];
        }

        // Update user mute status
        public void SetUserMuted(int userId, bool isMuted)
        {
            GetUserSettings(userId).IsMuted = isMuted;
            // Raise an event or notify other parts of the application
            OnUserSettingsChanged(userId);
        }

        // Update user video mute status
        public void SetUserVideoMuted(int userId, bool isVideoMuted)
        {
            GetUserSettings(userId).IsVideoMuted = isVideoMuted;
            // Raise an event or notify other parts of the application
            OnUserSettingsChanged(userId);
        }

        // Update user deafen status
        public void SetUserDeafened(int userId, bool isDeafened)
        {
            GetUserSettings(userId).IsDeafened = isDeafened;
            // Raise an event or notify other parts of the application
            OnUserSettingsChanged(userId);
        }

        protected virtual void OnUserSettingsChanged(int userId)
        {
            UserSettingsChanged?.Invoke(this, new UserSettingsChangedEventArgs(userId));
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

    public class UserSettingsChangedEventArgs : EventArgs
    {
        public int UserId { get; private set; }

        public UserSettingsChangedEventArgs(int userId)
        {
            UserId = userId;
        }
    }
}