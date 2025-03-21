using System;
using System.IO;

namespace YoavDiscordClient
{
    /// <summary>
    /// Represents an audio packet for transmission over the network
    /// </summary>
    public class AudioPacket : BasePacket
    {
        /// <summary>
        /// The raw audio data contained in this packet
        /// </summary>
        public byte[] AudioData { get; set; }

        /// <summary>
        /// Timestamp when the packet was created (for synchronization)
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Sequence number to track packet ordering
        /// </summary>
        public int SequenceNumber { get; set; }

        // A static counter for generating sequence numbers
        private static int currentSequence = 0;

        /// <summary>
        /// Creates a new audio packet with the given audio data
        /// </summary>
        /// <param name="audioData">The raw audio data</param>
        public AudioPacket(byte[] audioData)
        {
            Type = PacketType.Audio;
            AudioData = audioData;
            Timestamp = DateTime.UtcNow.Ticks;
            SequenceNumber = System.Threading.Interlocked.Increment(ref currentSequence);
        }

        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private AudioPacket()
        {
            Type = PacketType.Audio;
        }

        /// <summary>
        /// Serializes the audio packet to bytes for network transmission
        /// </summary>
        /// <returns>Byte array containing the serialized packet</returns>
        public override byte[] ToBytes()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write((int)Type);
                writer.Write(Timestamp);
                writer.Write(SequenceNumber);
                writer.Write(AudioData.Length);
                writer.Write(AudioData);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes an audio packet from bytes received over the network
        /// </summary>
        /// <param name="bytes">The serialized audio packet</param>
        /// <returns>Deserialized AudioPacket</returns>
        public static AudioPacket FromBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                reader.ReadInt32(); // Skip type as we already know it

                AudioPacket packet = new AudioPacket();

                // Read packet metadata if available (newer format)
                if (ms.Length > bytes.Length - 4) // We have more than just the type and audio data
                {
                    try
                    {
                        packet.Timestamp = reader.ReadInt64();
                        packet.SequenceNumber = reader.ReadInt32();
                        int dataLength = reader.ReadInt32();
                        packet.AudioData = reader.ReadBytes(dataLength);
                    }
                    catch
                    {
                        // If we fail to read the metadata (old format packet), revert to simpler approach
                        ms.Position = 4; // Reset to just after type
                        packet.AudioData = reader.ReadBytes(bytes.Length - 4);
                    }
                }
                else // Old format (backward compatibility)
                {
                    packet.AudioData = reader.ReadBytes(bytes.Length - 4);
                    packet.Timestamp = DateTime.UtcNow.Ticks;
                    packet.SequenceNumber = 0;
                }

                return packet;
            }
        }
    }
}