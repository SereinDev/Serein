using Microsoft.Extensions.Logging;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Core.Services.Automations.TriggerHandlers;

public sealed class PluginTriggerHandler(TaskHost taskHost, ILogger<PluginTriggerHandler> logger)
{
    public void CallPluginTriggersByKey(string key)
    {
        taskHost.EnumerateAllTriggers<PluginTrigger>(
            (_, trigger) =>
            {
                if (trigger.Key == key)
                {
                    logger.LogInformation("触发自动化任务，触发器：{}", trigger.GetType().Name);
                }
            }
        );
    }
}
