using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    public class AudioPacket : BasePacket
    {
        public byte[] AudioData { get; set; }

        public AudioPacket(byte[] audioData)
        {
            Type = PacketType.Audio;
            AudioData = audioData;
        }

        public override byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((int)Type);
                writer.Write(AudioData);
                return ms.ToArray();
            }
        }

        public static AudioPacket FromBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                reader.ReadInt32(); // Skip type as we already know it
                byte[] audioData = reader.ReadBytes(bytes.Length - 4);
                return new AudioPacket(audioData);
            }
        }
    }
}
