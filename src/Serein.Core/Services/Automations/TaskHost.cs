using System;
using System.Threading.Tasks;
using Serein.Core.Models.Automations;
using Serein.Core.Models.Automations.Triggers;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Automations;

public sealed class TaskHost(
    CommandRunner commandRunner,
    AutomationTaskProvider automationTaskProvider
)
{
    public void EnumerateAllTriggers<T>(Action<AutomationTask, T> action)
        where T : TriggerBase
    {
        lock (automationTaskProvider.Value)
        {
            foreach (var task in automationTaskProvider.Value)
            {
                foreach (var trigger in task.Triggers)
                {
                    if (trigger is not T t)
                    {
                        continue;
                    }

                    action(task, t);
                }
            }
        }
    }

    public async Task RunTaskAsync(AutomationTask task, CommandContext? context = null)
    {
        foreach (var command in task.Commands)
        {
            await commandRunner.RunAsync(command, context).ConfigureAwait(false);
        }
    }
}
