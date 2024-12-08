using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using PropertyChanged;
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
            try
            {
                if (string.IsNullOrEmpty(_regExp))
                {
                    throw new ArgumentException("正则表达式不得为空", nameof(RegExp));
                }
                RegexObj = new Regex(_regExp);
            }
            catch
            {
                RegexObj = null;
            }
        }
    }

    [JsonIgnore]
    public Regex? RegexObj { get; set; }

    public MatchFieldType FieldType { get; set; }

    public bool RequireAdmin { get; set; }

    [AlsoNotifyFor(nameof(MatchExclusion))]
    public string Exclusions
    {
        get => _exclusions;
        set
        {
            _exclusions = value;
            MatchExclusion = new();

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
                    switch (args[0].Trim())
                    {
                        case "server":
                            MatchExclusion.Servers.Add(arg);
                            break;

                        case "group":
                            if (long.TryParse(arg, out var group))
                            {
                                MatchExclusion.Groups.Add(group);
                            }
                            break;

                        case "user":
                            if (long.TryParse(arg, out var user))
                            {
                                MatchExclusion.Users.Add(user);
                            }
                            break;
                    }
                }
            }
        }
    }

    [JsonIgnore]
    public MatchExclusion MatchExclusion { get; private set; } = new();

    public string Description { get; set; } = string.Empty;

    public string Command
    {
        get => _command;
        set
        {
            _command = value;
            CommandObj = CommandParser.Parse(
                FieldType switch
                {
                    MatchFieldType.ServerOutput => CommandOrigin.ServerOutput,
                    MatchFieldType.ServerInput => CommandOrigin.ServerInput,
                    MatchFieldType.PrivateMsg => CommandOrigin.Msg,
                    MatchFieldType.GroupMsg => CommandOrigin.Msg,
                    MatchFieldType.SelfMsg => CommandOrigin.Msg,
                    _ => CommandOrigin.Null,
                },
                value
            );
        }
    }

    [JsonIgnore]
    public Command? CommandObj { get; private set; }
}
