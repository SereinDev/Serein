using System;
using System.Text.Json.Nodes;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.OneBot.Packets;
using Serein.Core.Models.Output;
using Serein.Core.Services.Data;
using Serein.Plus.Ui.Pages.Server;

namespace Serein.Plus;

public class WpfOutputHandler : IOutputHandler
{
    private readonly LogLevel _logLevel;
    public IServiceProvider Services { get; }
    private readonly Lazy<PanelPage> _panelPage;

    public WpfOutputHandler(IServiceProvider services)
    {
        Services = services;

        _logLevel = Services.GetRequiredService<SettingProvider>().Value.Application.LogLevel;
        _panelPage = new(() => Services.GetRequiredService<PanelPage>());
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logLevel <= logLevel;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) { }

    public void LogBotJsonPacket(JsonNode jsonNode) { }

    public void LogBotMessage(MessagePacket packet) { }

    public void LogBotNotice(string line) { }

    public void LogPluginError(string title, string line) { }

    public void LogPluginInfomation(string title, string line) { }

    public void LogPluginNotice(string line) { }

    public void LogPluginWarn(string title, string line) { }

    public void LogServerInfo(string line)
    {
        _panelPage.Value.Dispatcher.Invoke(() => _panelPage.Value.TextEditor.AppendInfoLine(line));
    }

    public void LogServerRawOutput(string line)
    {
        _panelPage.Value.Dispatcher.Invoke(() => _panelPage.Value.TextEditor.AppendLine(line));
    }
}
