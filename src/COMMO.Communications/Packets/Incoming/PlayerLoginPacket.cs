// <copyright file="PlayerLoginPacket.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;
using System;

namespace COMMO.Communications.Packets.Incoming
{
    public class PlayerLoginPacket : IPacketIncoming, IPlayerLoginInfo
    {
        public PlayerLoginPacket(NetworkMessage message)
        {
			XteaKey = new uint[4];
			XteaKey[0] = message.GetUInt32();
			XteaKey[1] = message.GetUInt32();
			XteaKey[2] = message.GetUInt32();
			XteaKey[3] = message.GetUInt32();
			
			IsGm = message.GetByte() > 0;

			SessionKey = message.GetString();

			var sessionKey = SessionKey.Split('\n');
			
			AccountNumber =  (uint)Int16.Parse(sessionKey[0]);
			Password =  sessionKey[1];
			var token =  sessionKey[2];

			CharacterName = message.GetString();

			uint challengeTimestamp = message.GetUInt32();
            byte challengeRandom = message.GetByte();

			//XteaKey = new uint[4];
			//XteaKey[0] = message.GetUInt32();
			//XteaKey[1] = message.GetUInt32();
			//XteaKey[2] = message.GetUInt32();
			//XteaKey[3] = message.GetUInt32();

			//Os = message.GetUInt16();
			//Version = message.GetUInt16();

			//IsGm = message.GetByte() > 0;

			//AccountNumber = message.GetUInt32();
			//CharacterName = message.GetString();
			//Password = message.GetString();
        }

        public ushort Os { get; set; }

        public ushort Version { get; set; }

        public uint[] XteaKey { get; set; }

        public bool IsGm { get; set; }

        public uint AccountNumber { get; set; }

        public string CharacterName { get; set; }

        public string Password { get; set; }

		public string SessionKey { get; }
    }
}
