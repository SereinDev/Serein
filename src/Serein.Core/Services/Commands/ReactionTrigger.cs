using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Commands;
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
        _logger.LogDebug("触发：Type={}, Target={}", type, target);

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
            var commandArguments = command.Arguments ?? new CommandArguments();

            if (string.IsNullOrEmpty(command.Arguments?.Target) && target is not null)
            {
                if (
                    command.Type == CommandType.InputServer
                    && !string.IsNullOrEmpty(target?.ServerId)
                )
                {
                    commandArguments.Target = target.Value.ServerId;
                }
                else if (command.Type == CommandType.SendPrivateMsg)
                {
                    commandArguments.Target = target?.UserId;
                }
                else if (command.Type == CommandType.SendGroupMsg)
                {
                    commandArguments.Target = target?.GroupId;
                }
            }

            await commandRunner.RunAsync(
                new Command(command) { Arguments = commandArguments },
                context
            );
        }
    }
}
