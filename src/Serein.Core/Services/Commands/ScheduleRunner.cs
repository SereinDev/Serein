using System;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Commands;

public class ScheduleRunner(ScheduleProvider scheduleProvider, CommandRunner commandRunner)
    : IHostedService
{
    private readonly ScheduleProvider _scheduleProvider = scheduleProvider;
    private readonly CommandRunner _commandRunner = commandRunner;

    private readonly Timer _timer = new(5000);

    private void OnElapsed(object? sender, EventArgs e)
    {
        lock (_scheduleProvider.Value)
        {
            foreach (var schedule in _scheduleProvider.Value)
            {
                if (
                    !schedule.IsEnabled
                    || schedule.IsRunning
                    || schedule.CommandObj is null
                    || schedule.CommandObj.Type == CommandType.Invalid
                    || schedule.Cron is null
                )
                    continue;

                if (schedule.NextTime > DateTime.Now)
                {
                    schedule.IsRunning = true;
                    _commandRunner
                        .RunAsync(schedule.CommandObj)
                        .ContinueWith((_) => schedule.IsRunning = false);
                }

                schedule.NextTime = schedule.Cron.GetNextOccurrence(DateTime.Now);
            }
        }
    }

    public Task StartAsync(System.Threading.CancellationToken cancellationToken)
    {
        _timer.Elapsed += OnElapsed;
        _timer.Start();

        return Task.CompletedTask;
    }

    public Task StopAsync(System.Threading.CancellationToken cancellationToken)
    {
        _timer.Stop();
        return Task.CompletedTask;
    }
}
