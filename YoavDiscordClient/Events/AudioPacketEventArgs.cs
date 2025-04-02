using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient.Events
{
    public class AudioPacketEventArgs : EventArgs
    {
        /// <summary>
        /// The audio data
        /// </summary>
        public byte[] AudioData { get; }

        /// <summary>
        /// Creates a new instance of AudioPacketEventArgs
        /// </summary>
        public AudioPacketEventArgs(byte[] audioData)
        {
            AudioData = audioData;
        }
    }
}
