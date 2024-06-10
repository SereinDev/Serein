using System;

using Jint.Native;

using Microsoft.Extensions.Logging;

using Serein.Core.Models.Output;

namespace Serein.Core.Models.Plugins.Js;

public class Console
{
    private readonly ISereinLogger _logger;
    private readonly string _title;

    public Console(ISereinLogger logger, string title)
    {
        _logger = logger;
        _title = title ?? throw new ArgumentNullException(nameof(title));
    }

    public void Log(params JsValue[] jsValues) => Info(jsValues);

    public void Info(params JsValue[] jsValues)
    {
        _logger.LogPlugin(LogLevel.Information, _title, string.Join<JsValue>('\x20', jsValues));
    }

    public void Warn(params JsValue[] jsValues)
    {
        _logger.LogPlugin(LogLevel.Warning, _title, string.Join<JsValue>('\x20', jsValues));
    }

    public void Error(params JsValue[] jsValues)
    {
        _logger.LogPlugin(LogLevel.Error, _title, string.Join<JsValue>('\x20', jsValues));
    }

    public void Assert(bool assertion, params JsValue[] jsValues)
    {
        if (!assertion)
            return;

        _logger.LogPlugin(LogLevel.Error, _title, string.Join<JsValue>('\x20', jsValues));
    }
}
