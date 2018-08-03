// <copyright file="CharacterListPacket.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using COMMO.Data.Models;
using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;

namespace COMMO.Communications.Packets.Outgoing
{
    public class CharacterListPacket : PacketOutgoing
    {
        public IEnumerable<ICharacterListItem> Characters { get; set; }

        public ushort PremiumDaysLeft { get; set; }

        public int Version { get; set; }

        public override byte PacketType => (byte)ManagementOutgoingPacketType.CharacterList;

        public CharacterListPacket()
        {
            Characters = Enumerable.Empty<ICharacterListItem>();
            PremiumDaysLeft = 0;
        }

        public CharacterListPacket(IEnumerable<ICharacterListItem> characters, ushort premDays)
        {
            Characters = characters;
            PremiumDaysLeft = premDays;
        }

        public override void Add(NetworkMessage message)
        {
            message.AddByte(PacketType);

			if(Version > 770)
			{
				//var worlds = new List<World>
				//{
				//	new World
				//	{
				//		Id = 0,
				//		IP = "127.0.0.1",
				//		Name = "Forgotten",
				//		Port = 7172
				//	}
				//};

				message.AddByte(1);

				//foreach (var world in worlds)
				//{
					message.AddByte(0);
					message.AddString("Forgotten");
					message.AddString("127.0.0.1");
					message.AddUInt16(7172);
					message.AddByte(0);
				//}

				message.AddByte((byte)Characters.Count());
				foreach (ICharacterListItem character in Characters)
				{
					message.AddByte(0);  //WorldID
					message.AddString(character.Name);
				}
			}
			else
			{
				message.AddByte((byte)Characters.Count());
				foreach (ICharacterListItem character in Characters)
				{
					message.AddString(character.Name);
					message.AddString(character.World);
					message.AddBytes(character.Ip);
					message.AddUInt16(character.Port);
				}
			}

			message.AddByte(0);
			message.AddByte(1);
			message.AddUInt32(0);

           // message.AddUInt16(PremiumDaysLeft);
        }

        public override void CleanUp()
        {
            Characters = null;
        }
    }
}
