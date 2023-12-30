using System;
using System.Text.Json.Serialization;

using NCrontab;

using Serein.Core.Services;

namespace Serein.Core.Models.Commands;

public class Schedule
{
    private string? _command;
    private string? _cronExp;

    public string? CronExp
    {
        get => _cronExp;
        set
        {
            _cronExp = value;

            if (!string.IsNullOrEmpty(_cronExp))
                CrontabSchedule = CrontabSchedule.TryParse(_cronExp);
            NextTime = CrontabSchedule?.GetNextOccurrence(DateTime.Now);
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

    public bool Enable { get; set; }

    [JsonIgnore]
    public Command? CommandObj { get; set; }

    public DateTime? NextTime { get; internal set; }

    internal bool IsRunning { get; set; }

    internal CrontabSchedule? CrontabSchedule { get; set; }
}
