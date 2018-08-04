// <copyright file="PlayerStatusPacket.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using System;
using COMMO.Data.Contracts;
using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;

namespace COMMO.Communications.Packets.Outgoing
{
    public class PlayerStatusPacket : PacketOutgoing
    {
        public override byte PacketType => (byte)GameOutgoingPacketType.PlayerStatus;

        public IPlayer Player { get; set; }

        public override void Add(NetworkMessage message)
        {
            message.AddByte(PacketType);

            message.AddUInt16((ushort)Math.Min(ushort.MaxValue, Player.Hitpoints));
            message.AddUInt16((ushort)Math.Min(ushort.MaxValue, Player.MaxHitpoints));

            message.AddUInt32(0); // FreeCapacity
            message.AddUInt16(0); // capacity

            message.AddUInt64(Player.Experience); // Experience: Client debugs after 2,147,483,647 exp

            message.AddUInt16(Player.Level);
            message.AddByte(Player.LevelPercent);

			message.AddUInt16(100); // base xp gain rate
			message.AddUInt16(0); // xp voucher
			message.AddUInt16(0); // low level bonus
			message.AddUInt16(0); // xp boost
			message.AddUInt16(100); // stamina multiplier (100 = x1.0)

            message.AddUInt32((ushort)Math.Min(ushort.MaxValue, Player.Manapoints));
            message.AddUInt32((ushort)Math.Min(ushort.MaxValue, Player.MaxManapoints));
			
			message.AddByte(Player.GetSkillInfo(SkillType.Magic));
			message.AddByte(Player.GetSkillInfo(SkillType.Magic));
            message.AddByte(Player.GetSkillPercent(SkillType.Magic));

            message.AddByte(Player.SoulPoints);

			message.AddUInt16(0); // staminaminutes

			message.AddUInt16(Player.Speed);

			//Condition* condition = Player.condi(CONDITION_REGENERATION);
			message.AddUInt16(0x00); // condition

			message.AddUInt16(0);

			message.AddUInt16(0); // xp boost time (seconds)
			message.AddByte(0); // enables exp boost in the store
        }

        public override void CleanUp()
        {
            Player = null;
        }
    }
}
