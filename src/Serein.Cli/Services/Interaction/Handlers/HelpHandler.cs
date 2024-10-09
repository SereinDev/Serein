using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Models;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandName("help", "帮助")]
[CommandDescription(["显示帮助页面"])]
public class HelpHandler(CommandProvider commandProvider, IHost host) : CommandHandler
{
    private readonly CommandProvider _commandProvider = commandProvider;
    private readonly ILogger _logger = host.Services.GetRequiredService<ILogger<HelpHandler>>();

    public override void Invoke(IReadOnlyList<string> args)
    {
        _logger.LogInformation("{}", _commandProvider.HelpPage);
    }
}