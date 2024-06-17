using System;
using System.Timers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Cronos;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;

namespace Serein.Core.Services;

public class ScheduleRunner
{
    private readonly IHost _host;
    private IServiceProvider Services => _host.Services;
    private ScheduleProvider ScheduleProvider => Services.GetRequiredService<ScheduleProvider>();
    private CommandRunner CommandRunner => Services.GetRequiredService<CommandRunner>();

    private readonly Timer _timer;

    public ScheduleRunner(IHost host)
    {
        _host = host;
        _timer = new(2000) { Enabled = true };
    }

    internal void Start()
    {
        _timer.Elapsed += OnElapsed;
    }

    private void OnElapsed(object? sender, EventArgs e)
    {
        lock (ScheduleProvider.Value)
        {
            foreach (var schedule in ScheduleProvider.Value)
            {
                if (
                    !schedule.Enable
                    || schedule.IsRunning
                    || string.IsNullOrEmpty(schedule.Command)
                    || string.IsNullOrEmpty(schedule.Expression)
                    || schedule.CommandObj is null
                    || schedule.CommandObj.Type == CommandType.Invalid
                )
                    continue;

                if (schedule.Cron is null)
                {
                    ;
                    if (CronExpression.TryParse(schedule.Expression, out var cron))
                        schedule.Cron = cron;

                    schedule.Enable = false;
                    continue;
                }

                if (schedule.NextTime > DateTime.Now)
                {
                    schedule.IsRunning = true;
                    CommandRunner
                        .RunAsync(schedule.CommandObj)
                        .ContinueWith((_) => schedule.IsRunning = false);
                }

                schedule.NextTime = schedule.Cron.GetNextOccurrence(DateTime.Now);
            }
        }
    }
}
