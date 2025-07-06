using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Commands;

internal sealed class ScheduleRunner(
    ILogger<ScheduleRunner> logger,
    ScheduleProvider scheduleProvider,
    CommandRunner commandRunner
) : IHostedService
{
    private readonly Timer _timer = new(5000);

    private void OnElapsed(object? sender, EventArgs e)
    {
        lock (scheduleProvider.Value)
        {
            foreach (var schedule in scheduleProvider.Value)
            {
                if (
                    !schedule.IsEnabled
                    || schedule.IsRunning
                    || schedule.CommandInstance is null
                    || schedule.CommandInstance.Type == CommandType.Invalid
                    || schedule.Crontab is null
                )
                {
                    continue;
                }

                if (schedule.NextTime < DateTime.Now)
                {
                    schedule.NextTime = schedule.Crontab.GetNextOccurrence(DateTime.Now);
                    schedule.IsRunning = true;

                    logger.LogDebug(
                        "正在运行定时任务（{},cron={},command={}）",
                        schedule.GetHashCode(),
                        schedule.Crontab,
                        schedule.Command
                    );
                    commandRunner
                        .RunAsync(schedule.CommandInstance)
                        .ContinueWith((_) => schedule.IsRunning = false);
                }
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
