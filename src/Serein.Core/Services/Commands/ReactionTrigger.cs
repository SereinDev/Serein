using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Commands;

public class ReactionTrigger(SettingProvider settingProvider, CommandRunner commandRunner)
{
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly CommandRunner _commandRunner = commandRunner;

    public Task TriggerAsync(
        ReactionType type,
        ReactionTarget? target = null,
        IReadOnlyDictionary<string, string?>? variables = null
    ) => Task.Run(() => Trigger(type, target, variables));

    public void Trigger(
        ReactionType type,
        ReactionTarget? target = null,
        IReadOnlyDictionary<string, string?>? variables = null
    )
    {
        if (!_settingProvider.Value.Reactions.TryGetValue(type, out var values))
        {
            _settingProvider.Value.Reactions[type] = [];
            _settingProvider.SaveAsyncWithDebounce();
            return;
        }

        IEnumerable<Command> commands;
        lock (values)
            commands = values.Select((cmd) => CommandParser.Parse(CommandOrigin.Reaction, cmd));

        if (!commands.Any())
            return;

        var context = new CommandContext
        {
            Variables = variables,
            ServerId = target?.ServerId
        };

        var tasks = new List<Task>();
        foreach (var command in commands)
        {
            if (command.Argument is null && target is not null)
                if (command.Type == CommandType.InputServer && !string.IsNullOrEmpty(target.ServerId))
                    command.Argument = target.ServerId;
                else if (command.Type == CommandType.SendPrivateMsg && target.UserId.HasValue)
                    command.Argument = target.UserId.ToString() ?? string.Empty;


            tasks.Add(_commandRunner.RunAsync(command, context));
        }

        Task.WaitAll([.. tasks], 1000);
    }
}
