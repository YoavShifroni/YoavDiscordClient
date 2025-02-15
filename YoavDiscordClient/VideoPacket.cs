using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace YoavDiscordClient
{
    public class VideoPacket : BasePacket
    {
        public const int MAX_PACKET_SIZE = 60000;
        public Guid FrameId { get; set; }
        public int PacketIndex { get; set; }
        public int TotalPackets { get; set; }
        public byte[] Data { get; set; }

        public VideoPacket()
        {
            Type = PacketType.Video;
        }

        public override byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((int)Type);
                writer.Write(FrameId.ToByteArray());
                writer.Write(PacketIndex);
                writer.Write(TotalPackets);
                writer.Write(Data);
                return ms.ToArray();
            }
        }

        public static VideoPacket FromBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                reader.ReadInt32(); // Skip type as we already know it
                return new VideoPacket
                {
                    Type = PacketType.Video,
                    FrameId = new Guid(reader.ReadBytes(16)),
                    PacketIndex = reader.ReadInt32(),
                    TotalPackets = reader.ReadInt32(),
                    Data = reader.ReadBytes(bytes.Length - 28) // 4 + 16 + 4 + 4 = 28 bytes of header
                };
            }
        }
    }

    public class FrameAssembler
    {
        private Dictionary<Guid, Dictionary<int, byte[]>> framePackets = new Dictionary<Guid, Dictionary<int, byte[]>>();
        private Dictionary<Guid, int> expectedPackets = new Dictionary<Guid, int>();
        private Dictionary<Guid, DateTime> frameTimestamps = new Dictionary<Guid, DateTime>();
        private const int MAX_FRAMES_IN_MEMORY = 10;



        public byte[] AddPacket(VideoPacket packet)
        {
            this.CleanupOldFrames();

            if (!framePackets.ContainsKey(packet.FrameId))
            {
                framePackets[packet.FrameId] = new Dictionary<int, byte[]>();
                expectedPackets[packet.FrameId] = packet.TotalPackets;
                frameTimestamps[packet.FrameId] = DateTime.UtcNow;
            }

            framePackets[packet.FrameId][packet.PacketIndex] = packet.Data;

            // Check if we have all packets for this frame
            if (framePackets[packet.FrameId].Count == expectedPackets[packet.FrameId])
            {
                // Reassemble the frame
                using (MemoryStream ms = new MemoryStream())
                {
                    for (int i = 0; i < expectedPackets[packet.FrameId]; i++)
                    {
                        if (framePackets[packet.FrameId].TryGetValue(i, out byte[] data))
                        {
                            ms.Write(data, 0, data.Length);
                        }
                    }

                    // Cleanup
                    this.CleanupFrame(packet.FrameId);
                    return ms.ToArray();
                }
            }

            return null; // Frame not complete yet
        }

        private void CleanupFrame(Guid frameId)
        {
            framePackets.Remove(frameId);
            expectedPackets.Remove(frameId);
            frameTimestamps.Remove(frameId);
        }


        // Cleanup old incomplete frames
        private void CleanupOldFrames()
        {
            var now = DateTime.UtcNow;
            var oldFrames = frameTimestamps
                .Where(kvp => (now - kvp.Value).TotalSeconds > 2 ||
                              framePackets.Count > MAX_FRAMES_IN_MEMORY)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var frameId in oldFrames)
            {
                CleanupFrame(frameId);
            }
        }
    }
}