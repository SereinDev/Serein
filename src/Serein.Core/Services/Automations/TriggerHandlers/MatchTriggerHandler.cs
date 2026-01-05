using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.ConnectionProtocols.Models.OneBot.V11.Messages;
using Serein.ConnectionProtocols.Models.OneBot.V12.Messages;
using Serein.ConnectionProtocols.Models.Satori.V1.Channels;
using Serein.Core.Models.Automations;
using Serein.Core.Models.Automations.Triggers;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection;

namespace Serein.Core.Services.Automations.TriggerHandlers;

public sealed class MatchTriggerHandler
{
    private readonly record struct ServerLine(string Id, string Text, bool IsInput);

    private readonly ILogger<MatchTriggerHandler> _logger;
    private readonly TaskHost _taskHost;
    private readonly CancellationTokenProvider _cancellationTokenProvider;

    private readonly BlockingCollection<Packets> _packets;
    private readonly BlockingCollection<ServerLine> _serverLines;

    public MatchTriggerHandler(
        ILogger<MatchTriggerHandler> logger,
        TaskHost taskHost,
        CancellationTokenProvider cancellationTokenProvider
    )
    {
        _logger = logger;
        _taskHost = taskHost;
        _cancellationTokenProvider = cancellationTokenProvider;
        _packets = [.. new ConcurrentQueue<Packets>()];
        _serverLines = [.. new ConcurrentQueue<ServerLine>()];

        _cancellationTokenProvider.Token.Register(_packets.Dispose);
        Task.Run(
            () => StartMatchMsgLoop(_cancellationTokenProvider.Token),
            _cancellationTokenProvider.Token
        );

        _cancellationTokenProvider.Token.Register(_serverLines.Dispose);
        Task.Run(
            () => StartMatchServerLineLoop(_cancellationTokenProvider.Token),
            _cancellationTokenProvider.Token
        );
    }

    public void QueueServerOutputLine(string id, string text)
    {
        _serverLines.Add(new(id, text, false));
    }

    public void QueueServerInputLine(string id, string text)
    {
        _serverLines.Add(new(id, text, true));
    }

    public void QueueMsg(Packets packets)
    {
        _packets.Add(packets);
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

    private void MatchServerLine(ServerLine serverLine)
    {
        var tasks = new List<Task>();

        _taskHost.EnumerateAllTriggers(
            (task, trigger) =>
            {
                if (trigger is not MatchTrigger matchTrigger)
                {
                    return;
                }

                if (
                    matchTrigger.FieldType
                    != (
                        serverLine.IsInput
                            ? MatchFieldType.ServerInput
                            : MatchFieldType.ServerOutput
                    )
                )
                {
                    return;
                }

                if (matchTrigger.IsRegex && matchTrigger.Regex is not null)
                {
                    var matches = matchTrigger.Regex.Match(serverLine.Text);

                    if (matches.Success)
                    {
                        tasks.Add(
                            _taskHost.RunTaskAsync(
                                task,
                                new CommandContext { Match = matches, ServerId = serverLine.Id }
                            )
                        );
                    }
                }
                else if (serverLine.Text.Contains(matchTrigger.Pattern))
                {
                    tasks.Add(
                        _taskHost.RunTaskAsync(
                            task,
                            new CommandContext { ServerId = serverLine.Id }
                        )
                    );
                }
            }
        );

        Task.WaitAll([.. tasks]);
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

        _taskHost.EnumerateAllTriggers(
            (task, trigger) =>
            {
                if (trigger is not MatchTrigger matchTrigger)
                {
                    return;
                }

                if (!CheckFieldType(matchTrigger, packets))
                {
                    return;
                }

                if (string.IsNullOrEmpty(packets.Message))
                {
                    return;
                }

                if (matchTrigger.IsRegex && matchTrigger.Regex is not null)
                {
                    var matches = matchTrigger.Regex.Match(packets.Message);

                    if (matches.Success)
                    {
                        tasks.Add(
                            _taskHost.RunTaskAsync(
                                task,
                                new CommandContext { Match = matches, Packets = packets }
                            )
                        );
                    }
                }
                else if (packets.Message.Contains(matchTrigger.Pattern))
                {
                    tasks.Add(
                        _taskHost.RunTaskAsync(task, new CommandContext { Packets = packets })
                    );
                }
            }
        );

        Task.WaitAll([.. tasks]);
    }

    private static bool CheckFieldType(MatchTrigger match, Packets packets)
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
