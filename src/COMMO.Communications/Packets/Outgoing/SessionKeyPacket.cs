using COMMO.Server.Data;
using COMMO.Server.Data.Interfaces;

namespace COMMO.Communications.Packets.Outgoing
{
    public class SessionKeyPacket : PacketOutgoing
    {
        public string SessionKey { get; set; }

        public override byte PacketType => (byte)ManagementOutgoingPacketType.SessionKey;

        public override void Add(NetworkMessage message)
        {
            message.AddByte(PacketType);

			var acc = "1";
			var pass = "1";
			var token = "";
			var ticks = "51108748";

            message.AddString(acc + "\n" + pass + "\n" + token + "\n" + ticks);
        }

        public override void CleanUp()
        {
            // No references to clear.
        }
    }
}
