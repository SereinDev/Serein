using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.OneBot.Messages;
using Serein.Core.Models.Network.OneBot.Packets;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

namespace Serein.Core.Services;

public class Matcher
{
    private readonly IHost _host;
    private readonly MatchesProvider _matchesProvider;
    private readonly CommandRunner _commandRunner;

    private IServiceProvider Services => _host.Services;
    private ConnectionSetting BotSetting => Services.GetRequiredService<SettingProvider>().Value.Connection;

    public Matcher(IHost host, MatchesProvider matchesProvider, CommandRunner commandRunner)
    {
        _host = host;
        _matchesProvider = matchesProvider;
        _commandRunner = commandRunner;
    }

    public async Task MatchServerOutputAsync(string line)
    {
        var tasks = new List<Task>();

        lock (_matchesProvider.Value)
        {
            foreach (var match in _matchesProvider.Value)
            {
                if (
                    string.IsNullOrEmpty(match.RegExp)
                    || string.IsNullOrEmpty(match.Command)
                    || match.FieldType != MatchFieldType.ServerOutput
                    || match.RegexObj is null
                    || match.CommandObj is null
                    || match.CommandObj.Type == CommandType.Invalid
                )
                    continue;

                var matches = match.RegexObj.Match(line);

                if (matches.Success)
                    tasks.Add(_commandRunner.RunAsync(match.CommandObj, new(matches)));
            }
        }

        await Task.WhenAll(tasks);
    }

    public async Task MatchServerInputAsync(string line)
    {
        var tasks = new List<Task>();

        lock (_matchesProvider.Value)
        {
            foreach (var match in _matchesProvider.Value)
            {
                if (
                    string.IsNullOrEmpty(match.RegExp)
                    || string.IsNullOrEmpty(match.Command)
                    || match.FieldType != MatchFieldType.ServerInput
                    || match.RegexObj is null
                    || match.CommandObj is null
                    || match.CommandObj.Type == CommandType.Invalid
                )
                    continue;

                var matches = match.RegexObj.Match(line);

                if (matches.Success)
                    tasks.Add(_commandRunner.RunAsync(match.CommandObj, new(matches)));
            }
        }

        await Task.WhenAll(tasks);
    }

    public async Task MatchMsgAsync(MessagePacket messagePacket)
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
                )
                    continue;

                var matches = match.RegexObj.Match(messagePacket.RawMessage);

                if (matches.Success)
                    if (
                        match.FieldType == MatchFieldType.SelfMsg
                            && messagePacket.SelfId == messagePacket.UserId
                        || match.FieldType == MatchFieldType.GroupMsg
                            && messagePacket.MessageType == MessageType.Group
                        || match.FieldType == MatchFieldType.PrivateMsg
                            && messagePacket.MessageType == MessageType.Private
                    )
                        tasks.Add(
                            _commandRunner.RunAsync(match.CommandObj, new(matches, messagePacket))
                        );
            }
        }

        await Task.WhenAll(tasks);
    }

    private bool IsFromAdmin(MessagePacket messagePacket) =>
        BotSetting.PermissionList.Contains(messagePacket.UserId.ToString())
        || BotSetting.GivePermissionToAllAdmins
            && messagePacket.MessageType == MessageType.Group
            && messagePacket.Sender.Role != Role.Member;
}
