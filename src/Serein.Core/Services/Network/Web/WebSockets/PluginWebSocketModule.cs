using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Network.Web;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Extensions;
using Serein.Core.Utils.Json;

namespace Serein.Core.Services.Network.Web.WebSockets;

internal sealed class PluginWebSocketModule : WebSocketModuleBase
{
    private readonly ILogger<PluginWebSocketModule> _logger;
    private readonly PluginLoggerBase _pluginLoggerBase;

    public PluginWebSocketModule(
        ILogger<PluginWebSocketModule> logger,
        SettingProvider settingProvider,
        PluginLoggerBase pluginLoggerBase
    )
        : base("/ws/plugins", settingProvider)
    {
        _logger = logger;
        _pluginLoggerBase = pluginLoggerBase;
        _pluginLoggerBase.Logging += OnLogging;
    }

    protected override void Dispose(bool disposing)
    {
        _pluginLoggerBase.Logging -= OnLogging;
        base.Dispose(disposing);
    }

    private void OnLogging(LogLevel logLevel, string title, string line)
    {
        try
        {
            BroadcastAsync(
                    JsonSerializer.Serialize(
                        new BroadcastPacket(
                            logLevel switch
                            {
                                LogLevel.Trace => BroadcastTypes.Information,
                                _ => logLevel.ToString(),
                            },
                            logLevel == LogLevel.Trace ? nameof(Serein) : title,
                            line
                        ),
                        JsonSerializerOptionsFactory.Common
                    )
                )
                .Await();
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "尝试广播插件输出时出现异常");
        }
    }
}
