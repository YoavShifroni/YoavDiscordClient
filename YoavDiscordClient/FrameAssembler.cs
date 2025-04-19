using System;
using System.Collections.Generic;

namespace YoavDiscordClient
{
    /// <summary>
    /// Assembles video frames from received packets
    /// </summary>
    public class FrameAssembler
    {
        /// <summary>
        /// Tracks packets belonging to partial video frames that are being assembled.
        /// Maps frame identifiers to lists of received video packets for that frame.
        /// </summary>
        /// <remarks>
        /// Each key is a unique frame identifier (GUID), and the value is a collection of 
        /// VideoPacket objects representing fragments of that frame that have been received.
        /// As packets arrive, they are collected here until the frame is complete enough to process.
        /// </remarks>
        private Dictionary<Guid, List<VideoPacket>> framePackets = new Dictionary<Guid, List<VideoPacket>>();

        /// <summary>
        /// Tracks the time when the first packet of each frame was received.
        /// Used to identify and clean up stale incomplete frames.
        /// </summary>
        /// <remarks>
        /// Each key is a unique frame identifier (GUID) matching a key in framePackets, 
        /// and the value is the timestamp when the first packet for that frame was received.
        /// This allows implementing timeout logic to discard frames that remain incomplete
        /// for too long.
        /// </remarks>
        private Dictionary<Guid, DateTime> frameTimestamps = new Dictionary<Guid, DateTime>();

        /// <summary>
        /// The maximum time allowed for a frame to be assembled before it's considered stale.
        /// After this timeout period, incomplete frames are discarded to prevent memory leaks.
        /// </summary>
        /// <remarks>
        /// Set to 5 seconds, balancing between giving enough time for all packets to arrive
        /// over potentially unreliable networks, while not holding onto stale data for too long.
        /// </remarks>
        private readonly TimeSpan frameTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// The maximum number of incomplete frames to track simultaneously.
        /// Limits memory usage by preventing an unbounded number of partial frames
        /// from accumulating under heavy network loss conditions.
        /// </summary>
        /// <remarks>
        /// If this limit is reached, the oldest incomplete frames will be discarded first
        /// when new frames begin assembly. This prevents memory growth in situations
        /// where many frames are started but never completed.
        /// </remarks>
        private const int MAX_INCOMPLETE_FRAMES = 10; // Maximum number of incomplete frames to track

        /// <summary>
        /// Adds a video packet to the assembler and tries to complete a frame
        /// </summary>
        /// <param name="packet">The video packet to add</param>
        /// <returns>Completed frame data or null if frame is not yet complete</returns>
        public byte[] AddPacket(VideoPacket packet)
        {
            // Clean up old incomplete frames periodically
            CleanupOldFrames();

            // Limit number of tracked frames to prevent memory leaks
            if (framePackets.Count > MAX_INCOMPLETE_FRAMES && !framePackets.ContainsKey(packet.FrameId))
            {
                // Find and remove oldest frame if we're tracking too many
                Guid oldestFrameId = Guid.Empty;
                DateTime oldestTime = DateTime.MaxValue;

                foreach (var entry in frameTimestamps)
                {
                    if (entry.Value < oldestTime)
                    {
                        oldestTime = entry.Value;
                        oldestFrameId = entry.Key;
                    }
                }

                if (oldestFrameId != Guid.Empty)
                {
                    framePackets.Remove(oldestFrameId);
                    frameTimestamps.Remove(oldestFrameId);
                }
            }

            // Initialize tracking for this frame if it's new
            if (!framePackets.ContainsKey(packet.FrameId))
            {
                framePackets[packet.FrameId] = new List<VideoPacket>();
                frameTimestamps[packet.FrameId] = DateTime.Now;
            }

            // Add this packet to the frame collection
            framePackets[packet.FrameId].Add(packet);

            // Check if we have all packets for this frame
            var packets = framePackets[packet.FrameId];
            if (packets.Count == packet.TotalPackets)
            {
                try
                {
                    // Sort packets by index to ensure correct order
                    packets.Sort((a, b) => a.PacketIndex.CompareTo(b.PacketIndex));

                    // Calculate total data size
                    int totalSize = 0;
                    foreach (var p in packets)
                    {
                        totalSize += p.Data.Length;
                    }

                    // Combine all packet data
                    byte[] frameData = new byte[totalSize];
                    int offset = 0;
                    foreach (var p in packets)
                    {
                        Buffer.BlockCopy(p.Data, 0, frameData, offset, p.Data.Length);
                        offset += p.Data.Length;
                    }

                    // Clean up resources for completed frame
                    framePackets.Remove(packet.FrameId);
                    frameTimestamps.Remove(packet.FrameId);

                    return frameData;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error assembling frame: {ex.Message}");

                    // Clean up on error to prevent memory leaks
                    framePackets.Remove(packet.FrameId);
                    frameTimestamps.Remove(packet.FrameId);

                    return null;
                }
            }

            return null; // Frame not yet complete
        }

        /// <summary>
        /// Removes frames that have been waiting too long
        /// </summary>
        private void CleanupOldFrames()
        {
            List<Guid> framesToRemove = new List<Guid>();
            DateTime now = DateTime.Now;

            foreach (var entry in frameTimestamps)
            {
                if (now - entry.Value > frameTimeout)
                {
                    framesToRemove.Add(entry.Key);
                }
            }

            foreach (var frameId in framesToRemove)
            {
                framePackets.Remove(frameId);
                frameTimestamps.Remove(frameId);
                System.Diagnostics.Debug.WriteLine($"Removed incomplete frame {frameId} due to timeout");
            }
        }
    }
}