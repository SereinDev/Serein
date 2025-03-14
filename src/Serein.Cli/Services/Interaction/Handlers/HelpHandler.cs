using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serein.Cli.Models;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandName("help", "帮助")]
[CommandDescription(["显示帮助页面"])]
public sealed class HelpHandler(ILogger<HelpHandler> logger, IServiceProvider serviceProvider)
    : CommandHandler
{
    private readonly Lazy<CommandProvider> _commandProvider = new(
        serviceProvider.GetRequiredService<CommandProvider>
    );

    public override void Invoke(IReadOnlyList<string> args)
    {
        logger.LogInformation("{}", _commandProvider.Value.HelpPage);
    }
}
