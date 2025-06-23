using System.Collections.Generic;
using Serein.ConnectionProtocols.Models.Satori.V1.Channels;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;
using RegexMatch = System.Text.RegularExpressions.Match;
using V11 = Serein.ConnectionProtocols.Models.OneBot.V11.Packets;
using V12 = Serein.ConnectionProtocols.Models.OneBot.V12.Packets;

namespace Serein.Core.Models.Commands;

/// <summary>
/// 命令上下文
/// </summary>
public readonly record struct CommandContext()
{
    public RegexMatch? Match { get; init; }

    public V11.MessagePacket? OneBotV11MessagePacket { get; init; }

    public V12.MessagePacket? OneBotV12MessagePacket { get; init; }

    public EventBody? SatoriV1MessagePacket { get; init; }

    public string? ServerId { get; init; }

    public IReadOnlyDictionary<string, string?>? Variables { get; init; }

    public string? UserId =>
        OneBotV11MessagePacket?.UserId.ToString()
        ?? OneBotV12MessagePacket?.UserId
        ?? SatoriV1MessagePacket?.User?.Id;

    public string? GroupId =>
        OneBotV11MessagePacket?.GroupId.ToString()
        ?? OneBotV12MessagePacket?.GroupId
        ?? (
            SatoriV1MessagePacket?.Channel?.Type != ChannelType.Direct
                ? SatoriV1MessagePacket?.Channel?.Id
                : null
        );
}
