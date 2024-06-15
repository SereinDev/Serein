using System;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Plus.Ui.Pages.Function;

namespace Serein.Plus.Loggers;

public class PluginLogger(PluginConsolePage pluginConsolePage) : IPluginLogger
{
    private readonly object _lock = new();
    private readonly PluginConsolePage _pluginConsolePage = pluginConsolePage;

    public void Log(LogLevel level, string name, string message)
    {
        _pluginConsolePage.Dispatcher.Invoke(() =>
        {
            switch (level)
            {
                case LogLevel.Trace:
                    _pluginConsolePage.Console.AppendInfoLine(message);
                    break;
                case LogLevel.Information:
                    _pluginConsolePage.Console.AppendInfoLine($"[{name}] {message}");
                    break;
                case LogLevel.Warning:
                    _pluginConsolePage.Console.AppendWarnLine($"[{name}] {message}");
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    _pluginConsolePage.Console.AppendErrorLine($"[{name}] {message}");
                    break;
                default:
                    throw new NotSupportedException();
            }
        });
    }
}
