using System;
using System.Text.Json.Serialization;

using NCrontab;

using PropertyChanged;

using Serein.Core.Services.Commands;

namespace Serein.Core.Models.Commands;

public class Schedule : NotifyPropertyChangedModelBase
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

                Cron = CrontabSchedule.Parse(_expression);
                NextTime = Cron?.GetNextOccurrence(DateTime.Now);
            }
            catch
            {
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
            CommandObj = CommandParser.Parse(CommandOrigin.Schedule, value);

        }
    }

    public string? Description { get; set; }

    public bool IsEnabled { get; set; } = true;

    [DoNotNotify]
    [JsonIgnore]
    public Command? CommandObj { get; private set; }

    [JsonIgnore]
    public DateTime? NextTime { get; internal set; }

    [JsonIgnore]
    [DoNotNotify]
    public bool IsRunning { get; internal set; }

    [AlsoNotifyFor(nameof(NextTime))]
    [JsonIgnore]
    public CrontabSchedule? Cron { get; internal set; }
}
