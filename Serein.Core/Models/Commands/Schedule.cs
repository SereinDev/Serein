using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

using NCrontab;

using PropertyChanged;

using Serein.Core.Services;

namespace Serein.Core.Models.Commands;

public class Schedule : INotifyPropertyChanged
{
    private string? _command;
    private string? _cronExp;

    public string? CronExp
    {
        get => _cronExp;
        set
        {
            _cronExp = value;

            try
            {
                if (string.IsNullOrEmpty(_cronExp))
                    throw new ArgumentException("Cron表达式不得为空", nameof(CronExp));

                CrontabSchedule = CrontabSchedule.Parse(_cronExp);
                NextTime = CrontabSchedule?.GetNextOccurrence(DateTime.Now);
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
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

            if (CommandObj.Type == CommandType.Invalid)
                ErrorMsg = "命令格式不正确";
        }
    }

    public string? Description { get; set; }

    public bool Enable { get; set; }

    [DoNotNotify]
    [JsonIgnore]
    public Command? CommandObj { get; private set; }

    public DateTime? NextTime { get; internal set; }

    internal bool IsRunning { get; set; }

    [DoNotNotify]
    [JsonIgnore]
    internal CrontabSchedule? CrontabSchedule { get; set; }

    [DoNotNotify]
    [JsonIgnore]
    public string? ErrorMsg { get; private set; }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
