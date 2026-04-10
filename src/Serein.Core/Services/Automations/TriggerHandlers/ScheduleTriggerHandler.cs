using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Core.Services.Automations.TriggerHandlers;

internal sealed class ScheduleTriggerHandler : IHostedService
{
    private readonly TaskHost _taskHost;
    private readonly Timer _timer = new(10_000);

    public ScheduleTriggerHandler(TaskHost taskHost)
    {
        _taskHost = taskHost;
        _timer.Elapsed += OnElapsed;
    }

    public Task StartAsync(System.Threading.CancellationToken cancellationToken)
    {
        if (!_timer.Enabled)
        {
            _timer.Start();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(System.Threading.CancellationToken cancellationToken)
    {
        _timer.Stop();
        _timer.Dispose();

        return Task.CompletedTask;
    }

    private void OnElapsed(object? sender, EventArgs e)
    {
        _taskHost.EnumerateAllTriggers<ScheduleTrigger>(
            (task, trigger) =>
            {
                if (Check(trigger))
                {
                    _taskHost.RunTaskAsync(task).ContinueWith((_) => trigger.UpdateNextTime());
                }
            }
        );
    }

    private static bool Check(ScheduleTrigger scheduleTrigger)
    {
        if (!scheduleTrigger.IsEnabled || scheduleTrigger.CrontabSchedule is null)
        {
            return false;
        }

        if (scheduleTrigger.NextTime == DateTime.MinValue)
        {
            scheduleTrigger.UpdateNextTime();

            return false;
        }
        else if (DateTime.Now >= scheduleTrigger.NextTime)
        {
            return true;
        }

        return false;
    }
}
