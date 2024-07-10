using System;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Commands;

public class ScheduleRunner(IHost host, CommandRunner commandRunner) : IHostedService
{
    private readonly CommandRunner _commandRunner = commandRunner;

    private ScheduleProvider ScheduleProvider { get; } = host.Services.GetRequiredService<ScheduleProvider>();

    private readonly Timer _timer = new(2000);

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
