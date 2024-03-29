using System;
using System.Text.Json.Nodes;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core.Models.Network.OneBot.Packets;
using Serein.Core.Models.Output;
using Serein.Core.Services.Data;
using Serein.Plus.Ui.Pages.Function;
using Serein.Plus.Ui.Pages.Server;

namespace Serein.Plus;

public class WpfOutputHandler : IOutputHandler
{
    private readonly LogLevel _logLevel;
    public IServiceProvider Services { get; }
    private readonly Lazy<PanelPage> _panelPage;
    private readonly Lazy<NetworkPage> _networkPage;
    private readonly Lazy<PluginConsolePage> _pluginConsolePage;

    public WpfOutputHandler(IServiceProvider services)
    {
        Services = services;

        _logLevel = Services.GetRequiredService<SettingProvider>().Value.Application.LogLevel;
        _panelPage = new(() => Services.GetRequiredService<PanelPage>());
        _networkPage = new(() => Services.GetRequiredService<NetworkPage>());
        _pluginConsolePage = new(() => Services.GetRequiredService<PluginConsolePage>());
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
    )
    { }

    public void LogBotJsonPacket(JsonNode jsonNode)
    {
        _networkPage.Value.Dispatcher.Invoke(
            () => _networkPage.Value.Console.AppendReceivedMsgLine(jsonNode.ToJsonString())
        );
    }

    public void LogBotReceivedMessage(MessagePacket packet)
    {
        _networkPage.Value.Dispatcher.Invoke(
            () =>
                _networkPage.Value.Console.AppendReceivedMsgLine(
                    $"{packet.Sender.Card ?? packet.Sender.Nickname}({packet.UserId}): {packet.Message}"
                )
        );
    }

    public void LogBotConsole(LogLevel logLevel, string line)
    {
        _networkPage.Value.Dispatcher.Invoke(() =>
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    _networkPage.Value.Console.AppendInfoLine(line);
                    break;
                case LogLevel.Warning:
                    _networkPage.Value.Console.AppendWarnLine(line);
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    _networkPage.Value.Console.AppendErrorLine(line);
                    break;
                default:
                    throw new NotSupportedException();
            }
        });
    }

    public void LogPlugin(LogLevel logLevel, string title, string line)
    {
        _pluginConsolePage.Value.Dispatcher.Invoke(() =>
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    _pluginConsolePage.Value.Console.AppendInfoLine(line);
                    break;
                case LogLevel.Information:
                    _pluginConsolePage.Value.Console.AppendInfoLine($"[{title}] {line}");
                    break;
                case LogLevel.Warning:
                    _pluginConsolePage.Value.Console.AppendWarnLine($"[{title}] {line}");
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    _pluginConsolePage.Value.Console.AppendErrorLine($"[{title}] {line}");
                    break;
                default:
                    throw new NotSupportedException();
            }
        });
    }

    public void LogServerInfo(string line)
    {
        _panelPage.Value.Dispatcher.Invoke(() => _panelPage.Value.Console.AppendInfoLine(line));
    }

    public void LogServerRawOutput(string line)
    {
        _panelPage.Value.Dispatcher.Invoke(() => _panelPage.Value.Console.AppendLine(line));
    }
}
