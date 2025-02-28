using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    public abstract class BasePacket
    {
        public PacketType Type { get; set; }

        public virtual byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((int)Type);
                return ms.ToArray();
            }
        }

        public static PacketType GetPacketType(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                return (PacketType)reader.ReadInt32();
            }
        }
    }


    public enum PacketType
    {
        Video,
        Audio,
        Empty_Video,
    }
}
