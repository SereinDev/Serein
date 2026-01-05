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
    public void EnumerateAllTriggers(Action<AutomationTask, TriggerBase> action)
    {
        lock (automationTaskProvider.Value)
        {
            foreach (var task in automationTaskProvider.Value)
            {
                foreach (var trigger in task.Triggers)
                {
                    action(task, trigger);
                }
            }
        }
    }

    public async Task RunTaskAsync(AutomationTask task, CommandContext? context = null)
    {
        foreach (var command in task.Commands)
        {
            await commandRunner.RunAsync(command, context);
        }
    }
}
