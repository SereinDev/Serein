using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.ConnectionProtocols.Models.OneBot.V11.Messages;
using Serein.ConnectionProtocols.Models.OneBot.V12.Messages;
using Serein.ConnectionProtocols.Models.Satori.V1.Channels;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;
using V11 = Serein.ConnectionProtocols.Models.OneBot.V11.Packets;
using V12 = Serein.ConnectionProtocols.Models.OneBot.V12.Packets;

namespace Serein.Core.Services.Commands;

#pragma warning disable IDE0046
#pragma warning disable IDE0066

public sealed class Matcher
{
    private readonly record struct ServerLine(string Id, string Line, bool IsInput);

    private readonly ILogger<Matcher> _logger;
    private readonly MatchProvider _matchProvider;
    private readonly CommandRunner _commandRunner;
    private readonly SettingProvider _settingProvider;

    private readonly BlockingCollection<object> _packets;
    private readonly BlockingCollection<ServerLine> _serverLines;

    public Matcher(
        ILogger<Matcher> logger,
        MatchProvider matchProvider,
        CommandRunner commandRunner,
        SettingProvider settingProvider,
        CancellationTokenProvider cancellationTokenProvider
    )
    {
        _logger = logger;
        _matchProvider = matchProvider;
        _commandRunner = commandRunner;
        _settingProvider = settingProvider;
        _packets = [.. new ConcurrentQueue<object>()];
        _serverLines = [.. new ConcurrentQueue<ServerLine>()];

        Task.Run(
            () => StartMatchMsgLoop(cancellationTokenProvider.Token),
            cancellationTokenProvider.Token
        );
        Task.Run(
            () => StartMatchServerLineLoop(cancellationTokenProvider.Token),
            cancellationTokenProvider.Token
        );
    }

    /// <summary>
    /// 匹配服务器输出
    /// </summary>
    /// <param name="id">服务器Id</param>
    /// <param name="line">输出行</param>
    public void QueueServerOutputLine(string id, string line)
    {
        _serverLines.Add(new(id, line, false));
    }

    /// <summary>
    /// 匹配服务器输入
    /// </summary>
    /// <param name="id">服务器Id</param>
    /// <param name="line">输入行</param>
    public void QueueServerInputLine(string id, string line)
    {
        _serverLines.Add(new(id, line, true));
    }

    private void MatchServerLine(ServerLine serverLine)
    {
        var tasks = new List<Task>();

        lock (_matchProvider.Value)
        {
            foreach (var match in _matchProvider.Value)
            {
                if (
                    string.IsNullOrEmpty(match.RegExp)
                    || string.IsNullOrEmpty(match.Command)
                    || match.FieldType
                        != (
                            serverLine.IsInput
                                ? MatchFieldType.ServerInput
                                : MatchFieldType.ServerOutput
                        )
                    || match.RegexObj is null
                    || match.CommandObj is null
                    || match.CommandObj.Type == CommandType.Invalid
                    || CheckExclusions(match, serverLine.Id)
                )
                {
                    continue;
                }

                var matches = match.RegexObj.Match(serverLine.Line);

                if (matches.Success)
                {
                    tasks.Add(
                        _commandRunner.RunAsync(
                            match.CommandObj,
                            new() { Match = matches, ServerId = serverLine.Id }
                        )
                    );
                }
            }
        }

        Task.WaitAll([.. tasks]);
    }

    private void StartMatchServerLineLoop(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var line = _serverLines.Take(cancellationToken);
                MatchServerLine(line);
            }
            catch (Exception e) when (e is OperationCanceledException or TaskCanceledException)
            {
                break;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "匹配服务器行失败");
            }
        }
    }

    private void StartMatchMsgLoop(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var packet = _packets.Take(cancellationToken);
                MatchMessagePacket(packet);
            }
            catch (Exception e) when (e is OperationCanceledException or TaskCanceledException)
            {
                break;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "匹配消息失败");
            }
        }
    }

    private void MatchMessagePacket(object packet)
    {
        var v11Packet = packet as V11.MessagePacket;
        var v12Packet = packet as V12.MessagePacket;
        var satoriPacket = packet as EventBody;

        var msg =
            v11Packet?.RawMessage ?? v12Packet?.FriendlyMessage ?? satoriPacket?.Message?.Content;
        var userId = v11Packet?.UserId.ToString() ?? v12Packet?.UserId ?? satoriPacket?.User?.Id;

        var tasks = new List<Task>();

        lock (_matchProvider.Value)
        {
            foreach (var match in _matchProvider.Value)
            {
                if (
                    string.IsNullOrEmpty(match.RegExp)
                    || string.IsNullOrEmpty(match.Command)
                    || string.IsNullOrEmpty(msg)
                )
                {
                    continue;
                }

                if (!CheckFieldType(match))
                {
                    continue;
                }

                if (
                    match.RequireAdmin
                    && (
                        !_settingProvider.Value.Connection.AdministratorUserIds.Contains(userId)
                        || !IsAdmin(v11Packet)
                    )
                )
                {
                    continue;
                }

                if (CheckExclusions(match, v11Packet, v12Packet, satoriPacket))
                {
                    continue;
                }

                RunMatch(match);
            }
        }

        Task.WaitAll([.. tasks]);

        void RunMatch(Match match)
        {
            if (
                match.RegexObj is null
                || match.CommandObj is null
                || match.CommandObj.Type == CommandType.Invalid
            )
            {
                return;
            }

            var matches = match.RegexObj.Match(msg);

            if (matches.Success)
            {
                tasks.Add(
                    _commandRunner.RunAsync(
                        match.CommandObj,
                        new()
                        {
                            Match = matches,
                            OneBotV11MessagePacket = v11Packet,
                            OneBotV12MessagePacket = v12Packet,
                            SatoriV1MessagePacket = satoriPacket,
                        }
                    )
                );
            }
        }

        bool CheckFieldType(Match match)
        {
            switch (match.FieldType)
            {
                case MatchFieldType.GroupMsg:
                    return v11Packet?.MessageType == MessageType.Group
                        || v12Packet?.DetailType == MessageDetailType.Group
                        || satoriPacket is not null
                            && satoriPacket.Channel?.Type != ChannelType.Direct;

                case MatchFieldType.PrivateMsg:
                    return v11Packet?.MessageType == MessageType.Private
                        || v12Packet?.DetailType == MessageDetailType.Private
                        || satoriPacket is { Channel.Type: ChannelType.Direct };

                case MatchFieldType.SelfMsg:
                    return v11Packet is not null && v11Packet.UserId == v11Packet.SelfId;

                case MatchFieldType.ChannelMsg:
                    return v12Packet?.DetailType == MessageDetailType.Channel;

                case MatchFieldType.GuildMsg:
                    return v12Packet?.DetailType == MessageDetailType.Guild;

                default:
                    return false;
            }
        }
    }

    public void QueueMsg(V11.MessagePacket messagePacket)
    {
        _packets.Add(messagePacket);
    }

    public void QueueMsg(V12.MessagePacket messagePacket)
    {
        _packets.Add(messagePacket);
    }

    public void QueueMsg(EventBody eventBody)
    {
        _packets.Add(eventBody);
    }

    private static bool CheckExclusions(Match match, string serverId)
    {
        return match.MatchExclusion.Servers.Contains(serverId);
    }

    private static bool CheckExclusions(
        Match match,
        V11.MessagePacket? v11Packet,
        V12.MessagePacket? v12Packet,
        EventBody? eventBody
    )
    {
        if (v11Packet is not null)
        {
            return match.FieldType == MatchFieldType.GroupMsg
                    && match.MatchExclusion.Groups.Contains(v11Packet.GroupId.ToString())
                || match.MatchExclusion.Users.Contains(v11Packet.UserId.ToString());
        }

        if (v12Packet is not null)
        {
            return match.FieldType == MatchFieldType.GroupMsg
                    && !string.IsNullOrEmpty(v12Packet.GroupId)
                    && match.MatchExclusion.Groups.Contains(v12Packet.GroupId)
                || match.FieldType == MatchFieldType.ChannelMsg
                    && !string.IsNullOrEmpty(v12Packet.ChannelId)
                    && match.MatchExclusion.Channels.Contains(v12Packet.ChannelId)
                || match.FieldType is MatchFieldType.GuildMsg or MatchFieldType.ChannelMsg
                    && !string.IsNullOrEmpty(v12Packet.GuildId)
                    && match.MatchExclusion.Channels.Contains(v12Packet.GuildId)
                || match.MatchExclusion.Users.Contains(v12Packet.UserId);
        }

        if (eventBody is not null)
        {
            return match.FieldType == MatchFieldType.GroupMsg
                    && !string.IsNullOrEmpty(eventBody.Channel?.Id)
                    && match.MatchExclusion.Groups.Contains(eventBody.Channel.Id)
                || match.MatchExclusion.Users.Contains(eventBody.User?.Id ?? string.Empty);
        }

        return false;
    }

    private bool IsAdmin(V11.MessagePacket? messagePacket)
    {
        return messagePacket?.MessageType == MessageType.Group
            && messagePacket.Sender.Role != Role.Member
            && _settingProvider.Value.Connection.OneBot.GrantPermissionToGroupOwnerAndAdmins;
    }
}
