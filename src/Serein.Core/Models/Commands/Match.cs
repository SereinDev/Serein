using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

using PropertyChanged;

using Serein.Core.Services.Commands;

namespace Serein.Core.Models.Commands;

public class Match : INotifyPropertyChanged, ICloneable
{
    private string _regExp = string.Empty;
    private string _command = string.Empty;
    private string _exclusions = string.Empty;

    public Match()
    {
        ExclusionDict ??= new Dictionary<ExclusionType, List<string>>();
    }

    public string RegExp
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
    internal Regex? RegexObj { get; set; }

    public MatchFieldType FieldType { get; set; }

    public bool RequireAdmin { get; set; }

    [AlsoNotifyFor(nameof(ExclusionDict))]
    public string Exclusions
    {
        get => _exclusions;
        set
        {
            _exclusions = value;
            ExclusionDict = new Dictionary<ExclusionType, List<string>>();

            foreach (var item in _exclusions.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var args = item.Split('=');

                if (args.Length == 2 && Enum.TryParse<ExclusionType>(args[0].Trim(), true, out var type))
                    if (!ExclusionDict.TryGetValue(type, out var list))
                        ExclusionDict.Add(type, [args[1].Trim()]);
                    else
                        list.Add(args[1].Trim());
            }
        }
    }

    [JsonIgnore]
    internal IDictionary<ExclusionType, List<string>> ExclusionDict { get; private set; }

    public string Description { get; set; } = string.Empty;

    public string Command
    {
        get => _command;
        set
        {
            _command = value;

            try
            {
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
                    value,
                    true
                );
                CommandTip = null;
            }
            catch (Exception e)
            {
                CommandTip = e.Message;
            }
        }
    }

    [DoNotNotify]
    [JsonIgnore]
    internal Command? CommandObj { get; private set; }

    [AlsoNotifyFor(nameof(Tip))]
    private string? CommandTip { get; set; }

    [AlsoNotifyFor(nameof(Tip))]
    private string? RegExpTip { get; set; }

    [JsonIgnore]
    public string? Tip => RegExpTip ?? CommandTip;

    public object Clone()
    {
        return MemberwiseClone();
    }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
