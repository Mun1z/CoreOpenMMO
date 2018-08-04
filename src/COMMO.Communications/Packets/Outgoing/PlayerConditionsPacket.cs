// <copyright file="PlayerConditionsPacket.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;

namespace COMMO.Communications.Packets.Outgoing
{
    public class PlayerConditionsPacket : PacketOutgoing
    {
        public override byte PacketType => (byte)GameOutgoingPacketType.PlayerConditions;

        public IPlayer Player { get; set; }

        public override void Add(NetworkMessage message)
        {
            message.AddByte(PacketType);

            message.AddUInt16(0x00);
        }

        public override void CleanUp()
        {
            Player = null;
        }
    }
}
