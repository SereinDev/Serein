using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using PropertyChanged;
using Serein.Core.Models.Abstractions;
using Serein.Core.Services.Commands;

namespace Serein.Core.Models.Commands;

public class Match : NotifyPropertyChangedModelBase
{
    private string _regExp = string.Empty;
    private string _command = string.Empty;
    private string _exclusions = string.Empty;

    public string RegExp
    {
        get => _regExp;
        set
        {
            _regExp = value;
            UpdateRegex();
        }
    }

    private void UpdateRegex()
    {
        try
        {
            if (!string.IsNullOrEmpty(_regExp))
            {
                RegexInstance = new(_regExp);
            }
        }
        catch
        {
            RegexInstance = null;
        }
    }

    [JsonIgnore]
    public Regex? RegexInstance { get; set; }

    public MatchFieldType FieldType { get; set; }

    public bool RequireAdmin { get; set; }

    [AlsoNotifyFor(nameof(ExclusionInstance))]
    public string Exclusions
    {
        get => _exclusions;
        set
        {
            _exclusions = value;
            UpdateExclusions();
        }
    }

    private void UpdateExclusions()
    {
        ExclusionInstance = new();

        foreach (
            var item in _exclusions.Split(
                ';',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
            )
        )
        {
            var args = item.Split('=');

            if (args.Length == 2)
            {
                var arg = args[1].Trim();
                switch (args[0].Trim().ToLowerInvariant())
                {
                    case "server":
                        ExclusionInstance.Servers.Add(arg);
                        break;

                    case "group":
                        ExclusionInstance.Groups.Add(arg);
                        break;

                    case "channel":
                        ExclusionInstance.Channels.Add(arg);
                        break;

                    case "user":
                        ExclusionInstance.Users.Add(arg);
                        break;
                }
            }
        }
    }

    [JsonIgnore]
    public MatchExclusion ExclusionInstance { get; private set; } = new();

    public string Description { get; set; } = string.Empty;

    public string Command
    {
        get => _command;
        set
        {
            _command = value;
            UpdateCommand();
        }
    }

    private void UpdateCommand()
    {
        CommandInstance = CommandParser.Parse(
            FieldType switch
            {
                MatchFieldType.ServerOutput => CommandOrigin.ServerOutput,
                MatchFieldType.ServerInput => CommandOrigin.ServerInput,
                MatchFieldType.PrivateMsg
                or MatchFieldType.SelfMsg
                or MatchFieldType.GroupMsg
                or MatchFieldType.ChannelMsg
                or MatchFieldType.GuildMsg => CommandOrigin.Msg,
                _ => CommandOrigin.Null,
            },
            _command
        );
    }

    [JsonIgnore]
    public Command? CommandInstance { get; private set; }

    public void ForceUpdate()
    {
        UpdateRegex();
        UpdateCommand();
        UpdateExclusions();
    }
}
