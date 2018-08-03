// <copyright file="AccountLoginPacket.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;
using System;

namespace COMMO.Communications.Packets.Incoming
{
    /// <summary>
    /// Class that represents an account login packet.
    /// </summary>
    public class AccountLoginPacket : IPacketIncoming, IAccountLoginInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountLoginPacket"/> class.
        /// </summary>
        /// <param name="message">The message to parse the packet from.</param>
        public AccountLoginPacket(NetworkMessage message, int version)
        {
			XteaKey = new uint[]
            {
                message.GetUInt32(),
                message.GetUInt32(),
                message.GetUInt32(),
                message.GetUInt32()
            };

			if(version > 770)
				AccountNumber = (uint)Int16.Parse(message.GetString());
			else
				AccountNumber = message.GetUInt32();

			Password = message.GetString();

			SessionKey = Guid.NewGuid().ToString();
        }

        /// <inheritdoc/>
        public uint AccountNumber { get; }

        /// <inheritdoc/>
        public string Password { get; }

        /// <inheritdoc/>
        public uint[] XteaKey { get; }

		/// <inheritdoc/>
		public string SessionKey { get; }
    }
}
