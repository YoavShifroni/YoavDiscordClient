using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient.Events
{
    public class VideoFrameEventArgs : EventArgs
    {
        /// <summary>
        /// The video frame data
        /// </summary>
        public byte[] FrameData { get; }

        /// <summary>
        /// The timestamp when the frame was captured
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Creates a new instance of VideoFrameEventArgs
        /// </summary>
        public VideoFrameEventArgs(byte[] frameData, DateTime timestamp)
        {
            FrameData = frameData;
            Timestamp = timestamp;
        }
    }
}
