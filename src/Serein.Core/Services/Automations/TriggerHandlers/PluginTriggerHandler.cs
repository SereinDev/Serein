using Microsoft.Extensions.Logging;
using Serein.Core.Models.Automations.Triggers;

namespace Serein.Core.Services.Automations.TriggerHandlers;

public sealed class PluginTriggerHandler(TaskHost taskHost, ILogger<PluginTriggerHandler> logger)
{
    public void CallPluginTriggersByKey(string key)
    {
        taskHost.EnumerateAllTriggers(
            (_, trigger) =>
            {
                if (trigger is PluginTrigger pluginTrigger && pluginTrigger.Key == key)
                {
                    logger.LogInformation(
                        "触发自动化任务，触发器：{}",
                        pluginTrigger.GetType().Name
                    );
                }
            }
        );
    }
}
