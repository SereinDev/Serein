using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

using PropertyChanged;

using Serein.Core.Services;
using Serein.Core.Utils.Extensions;

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
            RegexObj = _regExp.TryParse(RegexOptions.None, out var regex) ? regex : null;
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

            if (CommandObj.Type == CommandType.Invalid)
                ErrorMsg = "命令格式不正确";
        }
    }

    [DoNotNotify]
    [JsonIgnore]
    public Command? CommandObj { get; private set; }

    [DoNotNotify]
    [JsonIgnore]
    public string? ErrorMsg { get; private set; }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
