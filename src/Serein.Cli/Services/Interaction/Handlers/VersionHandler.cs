using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Models;
using Serein.Core;

namespace Serein.Cli.Services.Interaction.Handlers;

[CommandName("version", "版本")]
[CommandDescription(["查看版本信息", "查看版权声明"])]
public sealed class VersionHandler(ILogger<VersionHandler> logger) : CommandHandler
{
    private readonly ILogger<VersionHandler> _logger = logger;

    public override void Invoke(IReadOnlyList<string> args)
    {
        _logger.LogInformation("程序集：{}", typeof(VersionHandler).Assembly.FullName);
        _logger.LogInformation("详细版本：{}", SereinApp.FullVersion);
        _logger.LogInformation("Copyright (C) 2022 Zaitonn. All rights reserved.");
    }
}
