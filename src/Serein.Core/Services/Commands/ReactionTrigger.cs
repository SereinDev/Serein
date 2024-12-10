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
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly CommandRunner _commandRunner = commandRunner;

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
        if (!_settingProvider.Value.Reactions.TryGetValue(type, out var values))
        {
            _settingProvider.Value.Reactions[type] = [];
            _settingProvider.SaveAsyncWithDebounce();
            return;
        }

        IEnumerable<Command> commands;
        lock (values)
        {
            commands = values.Select((cmd) => CommandParser.Parse(CommandOrigin.Reaction, cmd));
        }
        if (!commands.Any())
        {
            return;
        }

        var context = new CommandContext { Variables = variables, ServerId = target?.ServerId };

        foreach (var command in commands)
        {
            if (command.Argument is null && target is not null)
            {
                if (
                    command.Type == CommandType.InputServer
                    && !string.IsNullOrEmpty(target.ServerId)
                )
                {
                    command.Argument = target.ServerId;
                }
                else if (command.Type == CommandType.SendPrivateMsg && target.UserId.HasValue)
                {
                    command.Argument = target.UserId.ToString() ?? string.Empty;
                }
            }

            await _commandRunner.RunAsync(command, context);
        }
    }
}
