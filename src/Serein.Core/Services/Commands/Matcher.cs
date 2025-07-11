using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.ConnectionProtocols.Models.OneBot.V11.Messages;
using Serein.ConnectionProtocols.Models.OneBot.V11.Packets;
using Serein.ConnectionProtocols.Models.OneBot.V12.Messages;
using Serein.ConnectionProtocols.Models.Satori.V1.Channels;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Data;

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

    private readonly BlockingCollection<Packets> _packets;
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
        _packets = [.. new ConcurrentQueue<Packets>()];
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
                    || match.RegexInstance is null
                    || match.CommandInstance is null
                    || match.CommandInstance.Type == CommandType.Invalid
                    || CheckExclusions(match, serverLine.Id)
                )
                {
                    continue;
                }

                var matches = match.RegexInstance.Match(serverLine.Line);

                if (matches.Success)
                {
                    tasks.Add(
                        _commandRunner.RunAsync(
                            match.CommandInstance,
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
                var packets = _packets.Take(cancellationToken);
                MatchMessagePacket(packets);
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

    private void MatchMessagePacket(Packets packets)
    {
        var tasks = new List<Task>();

        lock (_matchProvider.Value)
        {
            foreach (var match in _matchProvider.Value)
            {
                if (
                    string.IsNullOrEmpty(match.RegExp)
                    || string.IsNullOrEmpty(match.Command)
                    || string.IsNullOrEmpty(packets.Message)
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
                    && !_settingProvider.Value.Connection.AdministratorUserIds.Contains(
                        packets.UserId
                    )
                    && !IsAdmin(packets.OneBotV11)
                )
                {
                    continue;
                }

                if (CheckExclusions(match, packets))
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
                match.RegexInstance is null
                || match.CommandInstance is null
                || match.CommandInstance.Type == CommandType.Invalid
            )
            {
                return;
            }

            var matches = match.RegexInstance.Match(packets.Message);

            if (matches.Success)
            {
                tasks.Add(
                    _commandRunner.RunAsync(
                        match.CommandInstance,
                        new() { Match = matches, Packets = packets }
                    )
                );
            }
        }

        bool CheckFieldType(Match match)
        {
            switch (match.FieldType)
            {
                case MatchFieldType.GroupMsg:
                    return packets.OneBotV11?.MessageType == MessageType.Group
                        || packets.OneBotV12?.DetailType == MessageDetailType.Group
                        || packets.SatoriV1 is not null
                            && packets.SatoriV1.Channel?.Type != ChannelType.Direct;

                case MatchFieldType.PrivateMsg:
                    return packets.OneBotV11?.MessageType == MessageType.Private
                        || packets.OneBotV12?.DetailType == MessageDetailType.Private
                        || packets.SatoriV1 is { Channel.Type: ChannelType.Direct };

                case MatchFieldType.SelfMsg:
                    return packets.OneBotV11 is not null
                        && packets.OneBotV11.UserId == packets.OneBotV11.SelfId;

                case MatchFieldType.ChannelMsg:
                    return packets.OneBotV12?.DetailType == MessageDetailType.Channel
                        || packets.SatoriV1 is not null;

                case MatchFieldType.GuildMsg:
                    return packets.OneBotV12?.DetailType == MessageDetailType.Guild;

                default:
                    return false;
            }
        }
    }

    public void QueueMsg(Packets packets)
    {
        _packets.Add(packets);
    }

    private static bool CheckExclusions(Match match, string serverId)
    {
        return match.ExclusionInstance.Servers.Contains(serverId);
    }

    private static bool CheckExclusions(Match match, Packets packets)
    {
        if (packets.OneBotV11 is not null)
        {
            return match.FieldType == MatchFieldType.GroupMsg
                    && match.ExclusionInstance.Groups.Contains(packets.OneBotV11.GroupId.ToString())
                || match.ExclusionInstance.Users.Contains(packets.OneBotV11.UserId.ToString());
        }

        if (packets.OneBotV12 is not null)
        {
            return match.FieldType == MatchFieldType.GroupMsg
                    && !string.IsNullOrEmpty(packets.OneBotV12.GroupId)
                    && match.ExclusionInstance.Groups.Contains(packets.OneBotV12.GroupId)
                || match.FieldType == MatchFieldType.ChannelMsg
                    && !string.IsNullOrEmpty(packets.OneBotV12.ChannelId)
                    && match.ExclusionInstance.Channels.Contains(packets.OneBotV12.ChannelId)
                || match.FieldType is MatchFieldType.GuildMsg or MatchFieldType.ChannelMsg
                    && !string.IsNullOrEmpty(packets.OneBotV12.GuildId)
                    && match.ExclusionInstance.Channels.Contains(packets.OneBotV12.GuildId)
                || match.ExclusionInstance.Users.Contains(packets.OneBotV12.UserId);
        }

        if (packets.SatoriV1 is not null)
        {
            return match.FieldType == MatchFieldType.GroupMsg
                    && !string.IsNullOrEmpty(packets.SatoriV1.Channel?.Id)
                    && packets.SatoriV1.Channel.Type != ChannelType.Direct
                    && match.ExclusionInstance.Groups.Contains(packets.SatoriV1.Channel.Id)
                || match.ExclusionInstance.Users.Contains(
                    packets.SatoriV1.User?.Id ?? string.Empty
                );
        }

        return false;
    }

    private bool IsAdmin(MessagePacket? messagePacket)
    {
        return messagePacket?.MessageType == MessageType.Group
            && messagePacket.Sender.Role != Role.Member
            && _settingProvider.Value.Connection.OneBot.GrantPermissionToGroupOwnerAndAdmins;
    }
}
