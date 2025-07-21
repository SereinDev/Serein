using System.Web;
using Serein.ConnectionProtocols.Models;
using Serein.ConnectionProtocols.Models.Satori.V1.Channels;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;
using Serein.Core.Utils.Extensions;
using V11 = Serein.ConnectionProtocols.Models.OneBot.V11.Packets;
using V12 = Serein.ConnectionProtocols.Models.OneBot.V12.Packets;

namespace Serein.Core.Models.Network.Connection;

public readonly record struct Packets
{
    public V11.MessagePacket? OneBotV11 { get; init; }

    public V12.MessagePacket? OneBotV12 { get; init; }

    public EventBody? SatoriV1 { get; init; }

    public Self? Self =>
        OneBotV12?.Self
        ?? (
            SatoriV1?.Login is not null
                ? new() { Platform = SatoriV1.Login.Platform, UserId = SatoriV1.Login.User.Id }
                : null
        );

    public string? UserId =>
        OneBotV11?.UserId.ToString() ?? OneBotV12?.UserId ?? SatoriV1?.User?.Id;

    public string? GroupId =>
        OneBotV11?.GroupId.ToString()
        ?? OneBotV12?.GroupId
        ?? (SatoriV1?.Channel?.Type != ChannelType.Direct ? SatoriV1?.Channel?.Id : null);

    public string? Message =>
        StringExtension.SelectValueNotNullOrEmpty(
            HttpUtility.HtmlDecode(OneBotV11?.RawMessage),
            HttpUtility.HtmlDecode(OneBotV12?.FriendlyMessage),
            SatoriV1?.Message?.Content
        );
}
