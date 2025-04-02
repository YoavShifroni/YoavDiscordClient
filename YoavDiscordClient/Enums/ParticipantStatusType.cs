using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient.Enums
{
    public enum ParticipantStatusType
    {
        /// <summary>
        /// Participant connected
        /// </summary>
        Connected,

        /// <summary>
        /// Participant disconnected
        /// </summary>
        Disconnected,

        /// <summary>
        /// Participant muted audio
        /// </summary>
        AudioMuted,

        /// <summary>
        /// Participant unmuted audio
        /// </summary>
        AudioUnmuted,

        /// <summary>
        /// Participant muted video
        /// </summary>
        VideoMuted,

        /// <summary>
        /// Participant unmuted video
        /// </summary>
        VideoUnmuted
    }
}
