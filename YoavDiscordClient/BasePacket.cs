﻿using System;
using System.IO;
using YoavDiscordClient.Enums;

namespace YoavDiscordClient
{


    /// <summary>
    /// Base class for all packet types in the network protocol
    /// </summary>
    public abstract class BasePacket
    {
        /// <summary>
        /// Type of packet
        /// </summary>
        public PacketType Type { get; set; }

        /// <summary>
        /// Converts the packet to a byte array for transmission
        /// </summary>
        public abstract byte[] ToBytes();

        /// <summary>
        /// Determines the packet type from the raw byte data
        /// </summary>
        /// <param name="bytes">Raw packet data</param>
        /// <returns>Packet type</returns>
        public static PacketType GetPacketType(byte[] bytes)
        {
            if (bytes.Length < 4)
                return 0; // Invalid

            using (MemoryStream ms = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                try
                {
                    int typeValue = reader.ReadInt32();
                    if (Enum.IsDefined(typeof(PacketType), typeValue))
                    {
                        return (PacketType)typeValue;
                    }
                    return 0; // Unknown type
                }
                catch
                {
                    return 0; // Error reading
                }
            }
        }
    }
}