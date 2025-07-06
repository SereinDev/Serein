using System;
using System.Text.Json.Serialization;
using NCrontab;
using PropertyChanged;
using Serein.Core.Models.Abstractions;
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
            UpdateExpression();
        }
    }

    private void UpdateExpression()
    {
        try
        {
            if (string.IsNullOrEmpty(_expression))
            {
                throw new ArgumentException("Cron表达式不得为空", nameof(Expression));
            }
            Crontab = CrontabSchedule.Parse(_expression);
            NextTime = Crontab?.GetNextOccurrence(DateTime.Now);
        }
        catch
        {
            NextTime = null;
        }
    }

    public string? Command
    {
        get => _command;
        set
        {
            _command = value;
            CommandInstance = CommandParser.Parse(CommandOrigin.Schedule, value);
        }
    }

    public string? Description { get; set; }

    public bool IsEnabled { get; set; } = true;

    [JsonIgnore]
    public Command? CommandInstance { get; private set; }

    [JsonIgnore]
    public DateTime? NextTime { get; internal set; }

    [JsonIgnore]
    public bool IsRunning { get; internal set; }

    [AlsoNotifyFor(nameof(NextTime))]
    [JsonIgnore]
    public CrontabSchedule? Crontab { get; internal set; }

    public void ForceUpdate()
    {
        CommandInstance = CommandParser.Parse(CommandOrigin.Schedule, _command);
        UpdateExpression();
    }
}
