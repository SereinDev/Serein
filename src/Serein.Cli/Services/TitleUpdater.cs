using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serein.Core.Services;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;

namespace Serein.Cli.Services;

public sealed class TitleUpdater : IHostedService
{
    private readonly SettingProvider _settingProvider;
    private readonly CommandParser _commandParser;
    private readonly System.Timers.Timer _timer;

    public TitleUpdater(
        SettingProvider settingProvider,
        CommandParser commandParser,
        CancellationTokenProvider cancellationTokenProvider
    )
    {
        _settingProvider = settingProvider;
        _commandParser = commandParser;

        _timer = new(2000) { AutoReset = true };
        _timer.Elapsed += (_, _) => Update();

        cancellationTokenProvider.Token.Register(() =>
        {
            _timer.Stop();
            _timer.Dispose();
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            _settingProvider.Value.Application.PropertyChanged += (_, _) => Update();
            Update();
            _timer.Start();
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

        var text = _commandParser.ApplyVariables(
            _settingProvider.Value.Application.CustomTitle,
            null
        );

        Console.Title = !string.IsNullOrEmpty(text.Trim()) ? $"Serein.Cli - {text}" : "Serein.Cli";
    }
}
