using System;

using Jint.Native;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;

namespace Serein.Core.Services.Plugins.Js;

public class Console(IPluginLogger logger, string title)
{
    private readonly IPluginLogger _logger = logger;
    private readonly string _title = title ?? throw new ArgumentNullException(nameof(title));

    public void Log(params JsValue[] jsValues) => Info(jsValues);

    public void Info(params JsValue[] jsValues)
    {
        _logger.Log(LogLevel.Information, _title, string.Join<JsValue>('\x20', jsValues));
    }

    public void Warn(params JsValue[] jsValues)
    {
        _logger.Log(LogLevel.Warning, _title, string.Join<JsValue>('\x20', jsValues));
    }

    public void Error(params JsValue[] jsValues)
    {
        _logger.Log(LogLevel.Error, _title, string.Join<JsValue>('\x20', jsValues));
    }
}
