using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection.OneBot.Messages;
using Serein.Core.Models.Network.Connection.OneBot.Packets;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Commands;

public sealed class Matcher
{
    private record ServerLine(string Id, string Line, bool IsInput);

    private readonly ILogger<Matcher> _logger;
    private readonly MatchesProvider _matchesProvider;
    private readonly CommandRunner _commandRunner;
    private readonly SettingProvider _settingProvider;

    private readonly BlockingCollection<MessagePacket> _packets;
    private readonly BlockingCollection<ServerLine> _serverLines;

    public Matcher(
        ILogger<Matcher> logger,
        MatchesProvider matchesProvider,
        CommandRunner commandRunner,
        SettingProvider settingProvider,
        CancellationTokenProvider cancellationTokenProvider
    )
    {
        _logger = logger;
        _matchesProvider = matchesProvider;
        _commandRunner = commandRunner;
        _settingProvider = settingProvider;
        _packets = [.. new ConcurrentQueue<MessagePacket>()];
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

        lock (_matchesProvider.Value)
        {
            foreach (var match in _matchesProvider.Value)
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
                            new(matches, ServerId: serverLine.Id)
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

    private void MatchMessagePacket(MessagePacket messagePacket)
    {
        var tasks = new List<Task>();

        lock (_matchesProvider.Value)
        {
            foreach (var match in _matchesProvider.Value)
            {
                if (
                    string.IsNullOrEmpty(messagePacket.RawMessage)
                    || string.IsNullOrEmpty(match.RegExp)
                    || string.IsNullOrEmpty(match.Command)
                    || match.FieldType == MatchFieldType.Disabled
                    || match.FieldType == MatchFieldType.ServerInput
                    || match.FieldType == MatchFieldType.ServerOutput
                    || match.RegexObj is null
                    || match.CommandObj is null
                    || match.CommandObj.Type == CommandType.Invalid
                    || match.RequireAdmin && !IsFromAdmin(messagePacket)
                    || !(
                        match.FieldType == MatchFieldType.SelfMsg
                            && messagePacket.SelfId == messagePacket.UserId
                        || match.FieldType == MatchFieldType.GroupMsg
                            && messagePacket.MessageType == MessageType.Group
                        || match.FieldType == MatchFieldType.PrivateMsg
                            && messagePacket.MessageType == MessageType.Private
                    )
                    || CheckExclusions(match, messagePacket)
                )
                {
                    continue;
                }

                var matches = match.RegexObj.Match(messagePacket.RawMessage);

                if (matches.Success)
                {
                    tasks.Add(
                        _commandRunner.RunAsync(match.CommandObj, new(matches, messagePacket))
                    );
                }
            }
        }

        Task.WaitAll([.. tasks]);
    }

    /// <summary>
    /// 匹配消息
    /// </summary>
    /// <param name="messagePacket">OneBot消息数据包</param>
    public void QueueMsg(MessagePacket messagePacket)
    {
        _packets.Add(messagePacket);
    }

    private static bool CheckExclusions(Match match, string serverId)
    {
        return match.MatchExclusion.Servers.Contains(serverId);
    }

    private static bool CheckExclusions(Match match, MessagePacket messagePacket)
    {
        return match.FieldType == MatchFieldType.GroupMsg
                && match.MatchExclusion.Groups.Contains(messagePacket.GroupId)
            || match.MatchExclusion.Users.Contains(messagePacket.UserId);
    }

    private bool IsFromAdmin(MessagePacket messagePacket) =>
        _settingProvider.Value.Connection.Administrators.Contains(messagePacket.UserId)
        || _settingProvider.Value.Connection.GrantPermissionToOwnerAndAdmins
            && messagePacket.MessageType == MessageType.Group
            && messagePacket.Sender.Role != Role.Member;
}
