using System;
using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;
using Serein.Lite.Ui.Function;

namespace Serein.Lite.Services.Loggers;

public sealed class PluginLogger(IServiceProvider serviceProvider) : IPluginLogger
{
    private readonly Lazy<PluginPage> _pluginPage =
        new(serviceProvider.GetRequiredService<PluginPage>);

    private readonly object _lock = new();

    public void Log(LogLevel level, string name, string message)
    {
        Debug.WriteLine($"[Plugin::{(string.IsNullOrEmpty(name) ? "Serein" : name)}] [{level}] {message}");

        if (_pluginPage.Value.IsHandleCreated)
        {
            _pluginPage.Value.Invoke(() =>
            {
                lock (_lock)
                {
                    switch (level)
                    {
                        case LogLevel.Debug:
                            break;
                        case LogLevel.Information:
                            _pluginPage.Value.ConsoleWebBrowser.AppendInfo($"[{name}] {message}");

                            break;
                        case LogLevel.Warning:
                            _pluginPage.Value.ConsoleWebBrowser.AppendWarn($"[{name}] {message}");
                            break;
                        case LogLevel.Error:
                            _pluginPage.Value.ConsoleWebBrowser.AppendError($"[{name}] {message}");
                            break;
                        case LogLevel.Trace:
                            _pluginPage.Value.ConsoleWebBrowser.AppendNotice(message);
                            break;
                        case LogLevel.Critical:
                        case LogLevel.None:
                            break;
                    }
                }
            });
        }
    }
}
