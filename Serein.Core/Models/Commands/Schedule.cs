using System;
using System.Text.Json.Serialization;

using NCrontab;

using Serein.Core.Services;

namespace Serein.Core.Models.Commands;

public class Schedule
{
    private string? _command;

    public string? CronExp { get; set; }

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

    internal DateTime? NextTime { get; set; }

    internal bool IsRunning { get; set; }

    internal CrontabSchedule? CrontabSchedule { get; set; }
}
