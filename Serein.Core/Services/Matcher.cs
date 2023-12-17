using System;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Models.OneBot.Messages;
using Serein.Core.Models.OneBot.Packets;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services;

public class Matcher
{
    private readonly IHost _host;
    private readonly MatchesProvider _matchesProvider;
    private readonly CommandRunner _commandRunner;

    private IServiceProvider Service => _host.Services;
    private BotSetting BotSetting => Service.GetRequiredService<SettingProvider>().Value.Bot;

    public Matcher(IHost host, MatchesProvider matchesProvider, CommandRunner commandRunner)
    {
        _host = host;
        _matchesProvider = matchesProvider;
        _commandRunner = commandRunner;
    }

    public void MatchServerOutput(string line)
    {
        lock (_matchesProvider.Value)
        {
            foreach (var match in _matchesProvider.Value)
            {
                if (
                    string.IsNullOrEmpty(match.RegExp)
                    || string.IsNullOrEmpty(match.Command)
                    || match.FieldType != FieldType.ServerOutput
                    || !match.RegExp.TryParse(RegexOptions.None, out Regex? regex)
                )
                    continue;

                var matches = regex.Match(line);

                if (matches.Success)
                    _commandRunner.Run(CommandOrigin.ServerOutput, match.Command, matches);
            }
        }
    }

    public void MatchServerInput(string line)
    {
        lock (_matchesProvider.Value)
        {
            foreach (var match in _matchesProvider.Value)
            {
                if (
                    string.IsNullOrEmpty(match.RegExp)
                    || string.IsNullOrEmpty(match.Command)
                    || match.FieldType != FieldType.ServerInput
                    || !match.RegExp.TryParse(RegexOptions.None, out Regex? regex)
                )
                    continue;

                var matches = regex.Match(line);

                if (matches.Success)
                    _commandRunner.Run(CommandOrigin.ServerInput, match.Command, matches);
            }
        }
    }

    public void MatchMsg(MessagePacket messagePacket)
    {
        lock (_matchesProvider.Value)
        {
            foreach (var match in _matchesProvider.Value)
            {
                if (
                    string.IsNullOrEmpty(messagePacket.RawMessage)
                    || string.IsNullOrEmpty(match.RegExp)
                    || string.IsNullOrEmpty(match.Command)
                    || match.FieldType == FieldType.Disabled
                    || match.FieldType == FieldType.ServerInput
                    || match.FieldType == FieldType.ServerOutput
                    || !match.RegExp.TryParse(RegexOptions.None, out Regex? regex)
                )
                    continue;

                if (match.RequireAdmin && !IsFromAdmin(messagePacket))
                    continue;

                var matches = regex.Match(messagePacket.RawMessage);

                if (matches.Success)
                    if (
                        match.FieldType == FieldType.SelfMsg
                            && messagePacket.SelfId == messagePacket.UserId
                        || match.FieldType == FieldType.GroupMsg
                            && messagePacket.MessageType == MessageType.Group
                        || match.FieldType == FieldType.PrivateMsg
                            && messagePacket.MessageType == MessageType.Private
                    )
                        _commandRunner.Run(CommandOrigin.Msg, match.Command, matches);
            }
        }
    }

    private bool IsFromAdmin(MessagePacket messagePacket) =>
        BotSetting.PermissionList.Contains(messagePacket.UserId.ToString())
        || BotSetting.GivePermissionToAllAdmins
            && messagePacket.MessageType == MessageType.Group
            && messagePacket.Sender.Role != Role.Member;
}
