using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Automations;
using Serein.Core.Models.Automations.Triggers;
using Serein.Core.Models.Commands;

namespace Serein.Core.Services.Automations.TriggerHandlers;

public sealed class EventTriggerHandler(TaskHost taskHost, ILogger<EventTriggerHandler> logger)
{
    private async Task TriggerAsync(Events eventType, CommandContext? context = null)
    {
        var tasks = new List<Task>();

        taskHost.EnumerateAllTriggers(
            (task, trigger) =>
            {
                if (
                    trigger is EventTrigger eventTrigger
                    && eventTrigger.Events.Contains(eventType)
                )
                {
                    tasks.Add(taskHost.RunTaskAsync(task, context));
                }
            }
        );

        await Task.WhenAll(tasks);
    }

    internal async Task CallAsync(
        Events type,
        EventTarget? target = null,
        IReadOnlyDictionary<string, string?>? variables = null
    )
    {
        logger.LogDebug("触发：Type={}, Target={}", type, target);

        await TriggerAsync(
            type,
            new CommandContext { Variables = variables, ServerId = target?.ServerId }
        );
    }
}
