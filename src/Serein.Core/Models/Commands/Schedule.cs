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

    [AlsoNotifyFor(nameof(NextTime))]
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
                LastCronExpError = null;
            }
            catch (Exception e)
            {
                LastCronExpError = e.Message;
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
                LastCommandError = null;
            }
            catch (Exception e)
            {
                LastCommandError = e.Message;
            }
        }
    }

    public string? Description { get; set; }

    public bool Enable { get; set; } = true;

    [DoNotNotify]
    [JsonIgnore]
    internal Command? CommandObj { get; private set; }

    [JsonIgnore]
    public DateTime? NextTime { get; internal set; }

    [JsonIgnore]
    [DoNotNotify]
    internal bool IsRunning { get; set; }

    [DoNotNotify]
    [JsonIgnore]
    internal CrontabSchedule? Cron { get; set; }

    [AlsoNotifyFor(nameof(LastError))]
    private string? LastCommandError { get; set; }

    [AlsoNotifyFor(nameof(LastError))]
    private string? LastCronExpError { get; set; }

    [JsonIgnore]
    public string? LastError => LastCronExpError ?? LastCommandError;
}
