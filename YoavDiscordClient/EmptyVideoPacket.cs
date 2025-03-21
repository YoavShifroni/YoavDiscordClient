using System;
using System.IO;

namespace YoavDiscordClient
{
    /// <summary>
    /// Represents a packet indicating video is muted or unavailable
    /// </summary>
    public class EmptyVideoPacket : BasePacket
    {
        /// <summary>
        /// Timestamp when the packet was created
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Creates a new empty video packet
        /// </summary>
        public EmptyVideoPacket()
        {
            Type = PacketType.Empty_Video;
            Timestamp = DateTime.UtcNow.Ticks;
        }

        /// <summary>
        /// Serializes the empty video packet to bytes for network transmission
        /// </summary>
        /// <returns>Byte array containing the serialized packet</returns>
        public override byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((int)Type);
                writer.Write(Timestamp);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes an empty video packet from bytes received over the network
        /// </summary>
        /// <param name="bytes">The serialized empty video packet</param>
        /// <returns>Deserialized EmptyVideoPacket</returns>
        public static EmptyVideoPacket FromBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                reader.ReadInt32(); // Skip type as we already know it

                EmptyVideoPacket packet = new EmptyVideoPacket();

                // Try to read timestamp if available (newer format)
                if (ms.Length >= 12) // We have more than just the type
                {
                    try
                    {
                        packet.Timestamp = reader.ReadInt64();
                    }
                    catch
                    {
                        // If we fail to read the timestamp, just use current time
                        packet.Timestamp = DateTime.UtcNow.Ticks;
                    }
                }

                return packet;
            }
        }
    }
}