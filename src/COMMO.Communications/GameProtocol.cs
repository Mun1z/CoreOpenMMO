// <copyright file="GameProtocol.cs" company="2Dudes">
// Copyright (c) 2018 2Dudes. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Linq;
using COMMO.Communications.Interfaces;
using COMMO.Configuration;
using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;

namespace COMMO.Communications
{
    internal class GameProtocol : OpenTibiaProtocol
    {
        public GameProtocol(IHandlerFactory handlerFactory)
            : base(handlerFactory)
        {
        }

        public override bool KeepConnectionOpen => true;

        public override void ProcessMessage(Connection connection, NetworkMessage inboundMessage)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (inboundMessage == null)
            {
                throw new ArgumentNullException(nameof(inboundMessage));
            }

            byte packetType;

			uint recvChecksum = inboundMessage.GetUInt32(); //Adler Checksum
            uint checksum = Tools.AdlerChecksum(inboundMessage.Buffer, inboundMessage.Position, inboundMessage.Length - 6);
            if (checksum != recvChecksum)
                inboundMessage.SkipBytes(-4);

            if (!connection.IsAuthenticated || connection.XTeaKey.Sum(b => b) == 0)
            {
                // this is a new connection...
                packetType = inboundMessage.GetByte();

                if (packetType != (byte)GameIncomingPacketType.PlayerLoginRequest)
                {
                    // but this is not the packet we were expecting for a new connection.
                    connection.Close();
                    return;
                }

                var gameConfig = ServiceConfiguration.GetConfiguration();

				inboundMessage.SetPosition(7);
				var Os = inboundMessage.GetUInt16();
				var Version = inboundMessage.GetUInt16();

				inboundMessage.SkipBytes(7); // u32 client version, u8 client type, u16 dat revision
				
                // Make a copy of the message in case we fail to decrypt using the first set of keys.
                var messageCopy = NetworkMessage.Copy(inboundMessage);
				
				inboundMessage.RsaDecrypt(useRsa2: true);

                if (inboundMessage.GetByte() != 0)
                {
                    // means the RSA decrypt was unsuccessful, lets try with the other set of RSA keys...
                    inboundMessage = messageCopy;

                    inboundMessage.RsaDecrypt(useCipKeys: gameConfig.UsingCipsoftRsaKeys);

                    if (inboundMessage.GetByte() != 0)
                    {
						  // means the RSA decrypt was unsuccessful, lets try with the other set of RSA keys...
						inboundMessage = messageCopy;
						inboundMessage.RsaDecrypt(useCipKeys: !gameConfig.UsingCipsoftRsaKeys);

						if (inboundMessage.GetByte() != 0)
						{
							// These RSA keys are also usuccessful... so give up.
							connection.Close();
							return;
						}
                    }
                }
            }
            else
            {
                // Decrypt message using XTea
                inboundMessage.XteaDecrypt(connection.XTeaKey);
                inboundMessage.GetUInt16();
                packetType = inboundMessage.GetByte();
            }

            var handler = HandlerFactory.CreateIncommingForType(packetType);

            handler?.HandleMessageContents(inboundMessage, connection);

            if (handler?.ResponsePackets != null && handler.ResponsePackets.Any())
            {
                // Send any responses prepared for 
                var message = new NetworkMessage(6);
                message.AddByte(71);
                message.AddByte(17);
                message.AddByte(50);
				message.SkipBytes(3);

                foreach (var outPacket in handler.ResponsePackets)
                {
                    message.AddPacket(outPacket);
                }

                connection.Send(message);
            }
        }
    }
}
