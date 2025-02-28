using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    public class EmptyVideoPacket : BasePacket
    {
        public EmptyVideoPacket() 
        {
            Type = PacketType.Empty_Video;
        }
    }
}
