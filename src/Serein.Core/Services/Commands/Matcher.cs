using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection.OneBot.Messages;
using Serein.Core.Models.Network.Connection.OneBot.Packets;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Commands;

public class Matcher(
    MatchesProvider matchesProvider,
    CommandRunner commandRunner,
    SettingProvider settingProvider
    )
{
    private readonly MatchesProvider _matchesProvider = matchesProvider;
    private readonly CommandRunner _commandRunner = commandRunner;
    private readonly SettingProvider _settingProvider = settingProvider;

    public async Task MatchServerOutputAsync(string id, string line)
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
                    || CheckExclusions(match, id)
                )
                    continue;

                var matches = match.RegexObj.Match(line);

                if (matches.Success)
                    tasks.Add(_commandRunner.RunAsync(match.CommandObj, new(matches)));
            }
        }

        await Task.WhenAll(tasks);
    }

    public async Task MatchServerInputAsync(string id, string line)
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
                    || CheckExclusions(match, id)
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
                    || !(
                        match.FieldType == MatchFieldType.SelfMsg
                            && messagePacket.SelfId == messagePacket.UserId
                        || match.FieldType == MatchFieldType.GroupMsg
                            && messagePacket.MessageType == MessageType.Group
                        || match.FieldType == MatchFieldType.PrivateMsg
                            && messagePacket.MessageType == MessageType.Private)
                    || CheckExclusions(match, messagePacket)
                )
                    continue;

                var matches = match.RegexObj.Match(messagePacket.RawMessage);

                if (matches.Success)
                    tasks.Add(
                        _commandRunner.RunAsync(match.CommandObj, new(matches, messagePacket))
                    );
            }
        }

        await Task.WhenAll(tasks);
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
