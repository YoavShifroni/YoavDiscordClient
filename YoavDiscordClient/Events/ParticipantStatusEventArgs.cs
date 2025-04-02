using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoavDiscordClient.Enums;

namespace YoavDiscordClient.Events
{
    public class ParticipantStatusEventArgs : EventArgs
    {
        /// <summary>
        /// The participant's IP address
        /// </summary>
        public string IpAddress { get; }

        /// <summary>
        /// The type of status change
        /// </summary>
        public ParticipantStatusType StatusType { get; }

        /// <summary>
        /// Creates a new instance of ParticipantStatusEventArgs
        /// </summary>
        public ParticipantStatusEventArgs(string ipAddress, ParticipantStatusType statusType)
        {
            IpAddress = ipAddress;
            StatusType = statusType;
        }
    }
}
