using System;

using Serein.Core.Services;
using Serein.Core.Services.Data;
using Serein.Core.Services.Server;

namespace Serein.Cli;

public class TitleUpdater
{
    private readonly ServerManager _serverManager;
    private readonly SettingProvider _settingProvider;
    private readonly CommandParser _commandParser;

    public TitleUpdater(
        ServerManager serverManager,
        SettingProvider settingProvider,
        CommandParser commandParser
    )
    {
        _serverManager = serverManager;
        _settingProvider = settingProvider;
        _commandParser = commandParser;
    }

    public void Init()
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            return;

        _settingProvider.Value.Application.PropertyChanged += (_, _) => Update();
        _serverManager.ServerStatusChanged += (_, _) => Update();
        Update();
    }

    private void Update()
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            return;

        var text = _commandParser.ApplyVariables(_settingProvider.Value.Application.Title, null);

        Console.Title = !string.IsNullOrEmpty(text.Trim())
            ? $"Serein.Plus - {text}"
            : "Serein.Plus";
    }
}
