using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;

namespace Serein.Core.Services.Commands;

public class ReactionManager(IHost host)
{
    private readonly IHost _host = host;
    private IServiceProvider Services => _host.Services;
    private SettingProvider SettingProvider => Services.GetRequiredService<SettingProvider>();
    private CommandRunner CommandRunner => Services.GetRequiredService<CommandRunner>();

    public Task TriggerAsync(
        ReactionType type,
        string? target = null,
        IReadOnlyDictionary<string, string?>? variables = null
    ) => Task.Run(() => Trigger(type, target, variables));

    public void Trigger(
        ReactionType type,
        string? target = null,
        IReadOnlyDictionary<string, string?>? variables = null
    )
    {
        if (!SettingProvider.Value.Reactions.TryGetValue(type, out var values))
            return;

        IEnumerable<Command> commands;
        lock (values)
            commands = values.Select(
                (cmd) => (CommandParser.Parse(CommandOrigin.Reaction, cmd).Clone() as Command)!
            );

        if (!commands.Any())
            return;

        var context = new CommandContext { Variables = variables };

        var tasks = new List<Task>();
        foreach (var command in commands)
        {
            if (
                string.IsNullOrEmpty(command.Argument)
                && !string.IsNullOrEmpty(target)
                && (
                    command.Type == CommandType.SendGroupMsg
                    || command.Type == CommandType.SendPrivateMsg
                )
            )
                command.Argument = target;

            tasks.Add(CommandRunner.RunAsync(command, context));
        }

        Task.WaitAll(tasks.ToArray(), 1000);
    }
}
