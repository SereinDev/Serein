using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;

namespace Serein.Cli.Services;

public class TitleUpdater(SettingProvider settingProvider, CommandParser commandParser)
    : IHostedService
{
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly CommandParser _commandParser = commandParser;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            _settingProvider.Value.Application.PropertyChanged += (_, _) => Update();
            Update();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void Update()
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            return;

        var text = _commandParser.ApplyVariables(
            _settingProvider.Value.Application.CustomTitle,
            null
        );

        Console.Title = !string.IsNullOrEmpty(text.Trim()) ? $"Serein.Cli - {text}" : "Serein.Cli";
    }
}
