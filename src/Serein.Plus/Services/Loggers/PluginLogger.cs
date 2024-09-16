using System;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Plus.Pages;

namespace Serein.Plus.Services.Loggers;

public class PluginLogger(IServiceProvider serviceProvider) : IPluginLogger
{
    private readonly Lazy<PluginConsolePage> _pluginConsolePage =
        new(serviceProvider.GetRequiredService<PluginConsolePage>, LazyThreadSafetyMode.ExecutionAndPublication);

    public void Log(LogLevel level, string name, string message)
    {
        var line = level != LogLevel.Trace ? $"[{name}] {message}" : message;
        _pluginConsolePage.Value.Dispatcher.Invoke(() =>
        {
            switch (level)
            {
                case LogLevel.Information:
                    _pluginConsolePage.Value.Console.AppendInfoLine(line);
                    break;
                case LogLevel.Warning:
                    _pluginConsolePage.Value.Console.AppendWarnLine(line);
                    break;
                case LogLevel.Error:
                    _pluginConsolePage.Value.Console.AppendErrorLine(line);
                    break;
                case LogLevel.Trace:
                    _pluginConsolePage.Value.Console.AppendNoticeLine(line);
                    break;
            }
        });
    }
}