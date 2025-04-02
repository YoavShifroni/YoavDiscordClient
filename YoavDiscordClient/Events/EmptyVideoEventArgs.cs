using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient.Events
{
    public class EmptyVideoEventArgs : EventArgs
    {
        /// <summary>
        /// Gets whether video is muted
        /// </summary>
        public bool IsMuted { get; }

        /// <summary>
        /// Creates a new instance of EmptyVideoEventArgs
        /// </summary>
        public EmptyVideoEventArgs(bool isMuted)
        {
            IsMuted = isMuted;
        }
    }
}
