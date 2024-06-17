using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

using Cronos;

using PropertyChanged;

using Serein.Core.Services;

namespace Serein.Core.Models.Commands;

public class Schedule : INotifyPropertyChanged
{
    private string? _command;
    private string? _expression;

    public string? Expression
    {
        get => _expression;
        set
        {
            _expression = value;

            try
            {
                if (string.IsNullOrEmpty(_expression))
                    throw new ArgumentException("Cron表达式不得为空", nameof(Expression));

                Cron = CronExpression.Parse(_expression);
                NextTime = Cron?.GetNextOccurrence(DateTime.Now);
                CommandTip = null;
            }
            catch (Exception e)
            {
                CommandTip = e.Message;
                NextTime = null;
            }
        }
    }

    public string? Command
    {
        get => _command;
        set
        {
            _command = value;

            try
            {
                CommandObj = CommandParser.Parse(CommandOrigin.Schedule, value, true);
                CommandTip = null;
            }
            catch (Exception e)
            {
                CommandTip = e.Message;
            }
        }
    }

    public string? Description { get; set; }

    public bool Enable { get; set; }

    [DoNotNotify]
    [JsonIgnore]
    public Command? CommandObj { get; private set; }

    [JsonIgnore]
    public DateTime? NextTime { get; internal set; }

    [JsonIgnore]
    [DoNotNotify]
    internal bool IsRunning { get; set; }

    [DoNotNotify]
    [JsonIgnore]
    internal CronExpression? Cron { get; set; }

    [AlsoNotifyFor(nameof(Tip))]
    private string? CommandTip { get; set; }

    [AlsoNotifyFor(nameof(Tip))]
    private string? CronExpTip { get; set; }

    [JsonIgnore]
    public string? Tip => CronExpTip ?? CommandTip;

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
