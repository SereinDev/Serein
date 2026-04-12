using System;
using System.Text.Json.Serialization;
using NCrontab;

namespace Serein.Core.Models.Automations.Triggers;

public sealed class ScheduleTrigger : TriggerBase
{
    [JsonIgnore]
    public override TriggerType Type => TriggerType.Schedule;

    private string _cronExpression = string.Empty;

    public string CronExpression
    {
        get => _cronExpression;
        set
        {
            _cronExpression = value;
            CrontabSchedule = CrontabSchedule.TryParse(_cronExpression);
        }
    }

    [JsonIgnore]
    public CrontabSchedule? CrontabSchedule { get; private set; }

    [JsonIgnore]
    public DateTime? NextTime { get; private set; }

    internal void UpdateNextTime()
    {
        NextTime = CrontabSchedule?.GetNextOccurrence(DateTime.Now);
    }
}
