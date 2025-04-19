using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    /// <summary>
    /// Represents a chat message sent by a user in the Discord clone application.
    /// Contains message content, metadata, and sender information.
    /// </summary>
    /// <remarks>
    /// This class stores all information needed to display messages in the chat UI,
    /// including who sent the message, when it was sent, and in which chat room.
    /// </remarks>
    public class UserMessage
    {
        /// <summary>
        /// The unique identifier for the message.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who sent the message.
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// The display name of the user who sent the message.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The date and time when the message was sent.
        /// Used for message ordering and timestamp display.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The identifier of the chat room where the message was sent.
        /// </summary>
        public int ChatRoomId { get; set; }
    }
}