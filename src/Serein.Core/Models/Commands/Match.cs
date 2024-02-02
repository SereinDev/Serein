using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

using PropertyChanged;

using Serein.Core.Services;

namespace Serein.Core.Models.Commands;

public class Match : INotifyPropertyChanged
{
    private string? _regExp;
    private string? _command;

    public string? RegExp
    {
        get => _regExp;
        set
        {
            _regExp = value;
            try
            {
                if (string.IsNullOrEmpty(_regExp))
                    throw new ArgumentException("正则表达式不得为空", nameof(RegExp));

                RegexObj = new Regex(_regExp);
                RegExpTip = null;
            }
            catch (Exception e)
            {
                RegExpTip = e.Message;
            }
        }
    }

    [DoNotNotify]
    [JsonIgnore]
    public Regex? RegexObj { get; set; }

    public MatchFieldType FieldType { get; set; }

    public bool RequireAdmin { get; set; }

    public string? Restrictions { get; set; }

    public string? Description { get; set; }

    public string? Command
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
                    _ => CommandOrigin.Null
                },
                value
            );

            CommandTip =
                CommandObj.Type == CommandType.Invalid
                    ? new ArgumentException("命令格式不正确", nameof(Command)).Message
                    : null;
        }
    }

    [DoNotNotify]
    [JsonIgnore]
    public Command? CommandObj { get; private set; }

    [AlsoNotifyFor(nameof(Tip))]
    private string? CommandTip { get; set; }

    [AlsoNotifyFor(nameof(Tip))]
    private string? RegExpTip { get; set; }

    [JsonIgnore]
    public string? Tip => RegExpTip ?? CommandTip;

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
