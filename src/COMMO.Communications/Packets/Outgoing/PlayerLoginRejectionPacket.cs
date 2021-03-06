// <copyright file="PlayerLoginRejectionPacket.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;

namespace COMMO.Communications.Packets.Outgoing
{
    public class PlayerLoginRejectionPacket : PacketOutgoing
    {
        public override byte PacketType => (byte)ManagementOutgoingPacketType.NoType;

        public byte Reason { get; set; }

        public override void Add(NetworkMessage message)
        {
            message.AddByte(0x01); // Should always be 1 for this packet; means there was an error.
            message.AddByte(Reason);
            message.AddByte(0xFF); // EOM ?
        }

        public override void CleanUp()
        {
            // No references to clear.
        }
    }
}
