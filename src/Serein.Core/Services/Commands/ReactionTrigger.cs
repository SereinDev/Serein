using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Services.Commands;

public sealed class ReactionTrigger(
    ILogger<ReactionTrigger> logger,
    SettingProvider settingProvider,
    CommandRunner commandRunner
)
{
    private readonly ILogger _logger = logger;

    internal void Trigger(
        ReactionType type,
        ReactionTarget? target = null,
        IReadOnlyDictionary<string, string?>? variables = null
    )
    {
        TriggerAsync(type, target, variables).Await();
    }

    internal async Task TriggerAsync(
        ReactionType type,
        ReactionTarget? target = null,
        IReadOnlyDictionary<string, string?>? variables = null
    )
    {
        _logger.LogDebug("触发：type={}", type);

        if (!settingProvider.Value.Reactions.TryGetValue(type, out var values))
        {
            settingProvider.Value.Reactions[type] = [];
            settingProvider.SaveAsyncWithDebounce();
            return;
        }

        List<Command> commands;
        lock (values)
        {
            commands =
            [
                .. values.Select((cmd) => CommandParser.Parse(CommandOrigin.Reaction, cmd)),
            ];
        }

        if (commands.Count == 0)
        {
            return;
        }

        var context = new CommandContext { Variables = variables, ServerId = target?.ServerId };

        foreach (var command in commands)
        {
            var command1 = new Command(command);

            if (string.IsNullOrEmpty(command1.Argument) && target is not null)
            {
                if (
                    command1.Type == CommandType.InputServer
                    && !string.IsNullOrEmpty(target.ServerId)
                )
                {
                    command1.Argument = target.ServerId;
                }
                else if (command1.Type == CommandType.SendPrivateMsg)
                {
                    command1.Argument = target.UserId;
                }
            }

            await commandRunner.RunAsync(command1, context);
        }
    }
}
