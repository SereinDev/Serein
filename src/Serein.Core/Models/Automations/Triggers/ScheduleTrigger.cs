using System;
using System.Text.Json.Serialization;
using NCrontab;

namespace Serein.Core.Models.Automations.Triggers;

public sealed class ScheduleTrigger : TriggerBase
{
    private string _cronExpression = string.Empty;

    public override TriggerType Type => TriggerType.Schedule;

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
    public DateTime NextTime { get; private set; } = DateTime.MinValue;

    internal void UpdateNextTime()
    {
        NextTime = CrontabSchedule is not null
            ? CrontabSchedule.GetNextOccurrence(DateTime.Now)
            : DateTime.MinValue;
    }
}
