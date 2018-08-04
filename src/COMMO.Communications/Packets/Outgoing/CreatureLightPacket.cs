// <copyright file="CreatureLightPacket.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;

namespace COMMO.Communications.Packets.Outgoing
{
    public class CreatureLightPacket : PacketOutgoing
    {
        public override byte PacketType => (byte)GameOutgoingPacketType.CreatureLight;

        public ICreature Creature { get; set; }

        public override void Add(NetworkMessage message)
        {
            message.AddByte(PacketType);

            message.AddUInt32(Creature.CreatureId);
            message.AddByte(Creature.LightBrightness); // light level
            message.AddByte(Creature.LightColor); // color

			//END

			//sendBasicData PACKET

			message.AddByte(0x9F);
			message.AddByte(0);
			message.AddUInt32(0);

			
			message.AddByte(0);
			message.AddUInt16(0xFF); // number of known spells
			for (byte spellId = 0x00; spellId < 0xFF; spellId++) {
				message.AddByte(spellId);
			}
        }

        public override void CleanUp()
        {
            Creature = null;
        }
    }
}
