using System;
using System.IO;
using YoavDiscordClient.Enums;

namespace YoavDiscordClient
{
    /// <summary>
    /// Represents a video packet for transmission over the network
    /// </summary>
    public class VideoPacket : BasePacket
    {
        /// <summary>
        /// Maximum packet data size to avoid UDP fragmentation
        /// </summary>
        public const int MAX_PACKET_SIZE = 60000;

        /// <summary>
        /// Unique identifier for the frame this packet belongs to
        /// </summary>
        public Guid FrameId { get; set; }

        /// <summary>
        /// Index of this packet within the frame
        /// </summary>
        public int PacketIndex { get; set; }

        /// <summary>
        /// Total number of packets in the frame
        /// </summary>
        public int TotalPackets { get; set; }

        /// <summary>
        /// The actual video data for this packet
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Creates a new video packet
        /// </summary>
        public VideoPacket()
        {
            Type = PacketType.Video;
        }

        /// <summary>
        /// Serializes the video packet to bytes for network transmission
        /// </summary>
        public override byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((int)Type);
                writer.Write(FrameId.ToByteArray());
                writer.Write(PacketIndex);
                writer.Write(TotalPackets);
                writer.Write(Data.Length);
                writer.Write(Data);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes a video packet from bytes received over the network
        /// </summary>
        /// <param name="bytes">The serialized video packet</param>
        /// <returns>The deserialized VideoPacket</returns>
        public static VideoPacket FromBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                try
                {
                    reader.ReadInt32(); // Skip type, we already know it

                    VideoPacket packet = new VideoPacket();
                    packet.FrameId = new Guid(reader.ReadBytes(16));
                    packet.PacketIndex = reader.ReadInt32();
                    packet.TotalPackets = reader.ReadInt32();
                    int dataLength = reader.ReadInt32();

                    // Safety check to prevent buffer overflow
                    dataLength = Math.Min(dataLength, bytes.Length - (int)ms.Position);

                    packet.Data = reader.ReadBytes(dataLength);
                    return packet;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error deserializing video packet: {ex.Message}");
                    return new VideoPacket
                    {
                        FrameId = Guid.NewGuid(),
                        PacketIndex = 0,
                        TotalPackets = 1,
                        Data = new byte[0]
                    };
                }
            }
        }
    }
}