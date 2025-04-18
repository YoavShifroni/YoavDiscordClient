using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoavDiscordClient.Managers;

namespace YoavDiscordClient
{
    /// <summary>
    /// Partial class for DiscordApp that provides support methods for manager interactions.
    /// </summary>
    public partial class DiscordApp
    {
        /// <summary>
        /// Gets the user manager instance.
        /// </summary>
        public UserManager GetUserManager()
        {
            return _userManager;
        }

        /// <summary>
        /// Gets the chat manager instance.
        /// </summary>
        public ChatManager GetChatManager()
        {
            return _chatManager;
        }

        /// <summary>
        /// Gets the media channel manager instance.
        /// </summary>
        public MediaChannelManager GetMediaChannelManager()
        {
            return _mediaChannelManager;
        }

        /// <summary>
        /// Gets the emoji manager instance.
        /// </summary>
        public EmojiManager GetEmojiManager()
        {
            return _emojiManager;
        }

        /// <summary>
        /// Gets the context menu manager instance.
        /// </summary>
        public ContextMenuManager GetContextMenuManager()
        {
            return _contextMenuManager;
        }

        #region Server Event Handlers

        /// <summary>
        /// Handles a user mute status changed message from the server.
        /// </summary>
        public void OnUserMuteStatusChanged(int userId, bool isMuted)
        {
            _contextMenuManager.HandleUserMuteStatusChanged(userId, isMuted);
        }

        /// <summary>
        /// Handles a user video mute status changed message from the server.
        /// </summary>
        public void OnUserVideoMuteStatusChanged(int userId, bool isVideoMuted)
        {
            _contextMenuManager.HandleUserVideoMuteStatusChanged(userId, isVideoMuted);
        }

        /// <summary>
        /// Handles a user deafen status changed message from the server.
        /// </summary>
        public void OnUserDeafenStatusChanged(int userId, bool isDeafened)
        {
            _contextMenuManager.HandleUserDeafenStatusChanged(userId, isDeafened);
        }

        /// <summary>
        /// Handles a user role updated message from the server.
        /// </summary>
        public void OnUserRoleUpdated(int userId, int newRole)
        {
            _contextMenuManager.HandleUserRoleUpdated(userId, newRole);
        }

        /// <summary>
        /// Handles a user disconnected message from the server.
        /// </summary>
        public async Task OnUserDisconnected(int userId, int mediaRoomId)
        {
            await _mediaChannelManager.HandleUserDisconnect(userId, mediaRoomId);
        }

        /// <summary>
        /// Adds a new participant's display to a panel.
        /// </summary>
        public void AddNewParticipantDisplay(Panel panel, PictureBox pictureBox)
        {
            panel.Controls.Add(pictureBox);
            panel.Refresh();
        }
        #endregion
    }
}