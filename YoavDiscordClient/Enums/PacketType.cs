using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient.Enums
{
    /// <summary>
    /// Enumeration of packet types for the protocol
    /// </summary>
    public enum PacketType
    {
        /// <summary>
        /// Video data packet
        /// </summary>
        Video = 1,

        /// <summary>
        /// Audio data packet
        /// </summary>
        Audio = 2,

        /// <summary>
        /// Empty video packet (camera off/muted)
        /// </summary>
        Empty_Video = 3,

        
    }
}
