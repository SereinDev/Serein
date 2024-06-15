using System;

using Serein.Core.Models.Network.Connection.OneBot.Packets;

namespace Serein.Core.Models.Network;

public class MessagePacketReceivedEventArgs(MessagePacket packet) : EventArgs
{
    public MessagePacket Packet { get; } = packet;
}
