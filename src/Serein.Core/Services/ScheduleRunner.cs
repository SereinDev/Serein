using System;
using System.Timers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;

namespace Serein.Core.Services;

public class ScheduleRunner(IHost host)
{
    private readonly IHost _host = host;
    private IServiceProvider Services => _host.Services;
    private ScheduleProvider ScheduleProvider => Services.GetRequiredService<ScheduleProvider>();
    private CommandRunner CommandRunner => Services.GetRequiredService<CommandRunner>();

    private readonly Timer _timer = new(2000);

    internal void Start()
    {
        _timer.Elapsed += OnElapsed;
        _timer.Start();
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
                    || schedule.CommandObj is null
                    || schedule.CommandObj.Type == CommandType.Invalid
                    || schedule.Cron is null
                )
                    continue;

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
