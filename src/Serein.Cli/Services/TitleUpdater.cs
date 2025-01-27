using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;

namespace Serein.Cli.Services;

public sealed class TitleUpdater(SettingProvider settingProvider, CommandParser commandParser)
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            settingProvider.Value.Application.PropertyChanged += (_, _) => Update();
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
        {
            return;
        }
        var text = commandParser.ApplyVariables(
            settingProvider.Value.Application.CustomTitle,
            null
        );

        Console.Title = !string.IsNullOrEmpty(text.Trim()) ? $"Serein.Cli - {text}" : "Serein.Cli";
    }
}
