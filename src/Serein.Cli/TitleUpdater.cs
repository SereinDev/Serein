using System;

using Serein.Core.Services;
using Serein.Core.Services.Data;

namespace Serein.Cli;

public class TitleUpdater(SettingProvider settingProvider, CommandParser commandParser)
{
    private readonly SettingProvider _settingProvider = settingProvider;
    private readonly CommandParser _commandParser = commandParser;

    public void Init()
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            return;

        _settingProvider.Value.Application.PropertyChanged += (_, _) => Update();
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
