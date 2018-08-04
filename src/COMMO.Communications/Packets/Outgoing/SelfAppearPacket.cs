// <copyright file="SelfAppearPacket.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using System;
using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;

namespace COMMO.Communications.Packets.Outgoing
{
    public class SelfAppearPacket : PacketOutgoing
    {
        public override byte PacketType => (byte)GameOutgoingPacketType.SelfAppear;

        public uint CreatureId { get; set; }

        public byte GraphicsSpeed => 0x32; // Should always be 32 apparently...

        public byte CanReportBugs => 0x00;

        public bool IsLogin { get; set; }

        public IPlayer Player { get; set; }

        // public HashSet<string> Privileges { get; set; }
        public override void Add(NetworkMessage message)
        {
			double SpeedA = 857.36;
			double SpeedB = 261.29;
			double SpeedC = -4795.01;

            message.AddByte(0x17);

            message.AddUInt32(CreatureId);
			message.AddUInt16(0x32); // beat duration (50)
			
            message.AddDouble(SpeedA, 3);
            message.AddDouble(SpeedB, 3);
            message.AddDouble(SpeedC, 3);
			
			message.AddByte(0x01); // type >= tutor
			//message.AddByte(0x00); 

			message.AddByte(0x00); // can change pvp framing option
			message.AddByte(0x00); // expert mode button enabled

			message.AddUInt16(0x00); // URL (string) to ingame store images
			message.AddUInt16(25); // premium coin package size

			//// END PACKET

			//void ProtocolGame::sendPendingStateEntered() //create packet for this
			//{
				message.AddByte(0x0A);
			//}

			//void ProtocolGame::sendEnterWorld() //create packet for this
			//{
				message.AddByte(0x0F);
			//}
			
			//message.AddByte(PacketType);

   //         message.AddUInt32(CreatureId);
   //         message.AddByte(GraphicsSpeed);
   //         message.AddByte(CanReportBugs);

   //         message.AddByte(Math.Min((byte)1, Player.AccessLevel));

   //         if (Player.AccessLevel > 0) // TODO: WTF are these, permissions?
   //         {
   //             message.AddByte(0x0B);

   //             for (var i = 0; i < 32; i++)
   //             {
   //                 message.AddByte(0xFF);
   //             }
   //         }
        }

        public override void CleanUp()
        {
            Player = null;
        }
    }
}
