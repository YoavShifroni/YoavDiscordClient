using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    public class UserMessage
    {
        public string Id { get; set; }

        public int userId { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }

        public int ChatRoomId { get; set; }
    }
}
