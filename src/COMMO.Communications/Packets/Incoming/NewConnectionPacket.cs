// <copyright file="NewConnectionPacket.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;

namespace COMMO.Communications.Packets.Incoming
{
    public class NewConnectionPacket : IPacketIncoming, INewConnectionInfo
    {
        public NewConnectionPacket(NetworkMessage message)
        {
			Os = message.GetUInt16();
			Version = message.GetUInt16();

			if (Version >= 971)
				message.SkipBytes(17);
			else
				message.SkipBytes(12);
        }

        public ushort Os { get; set; }

        public ushort Version { get; set; }
    }
}
