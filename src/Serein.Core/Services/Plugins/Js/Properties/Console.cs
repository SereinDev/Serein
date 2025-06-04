using Jint.Native;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;

namespace Serein.Core.Services.Plugins.Js.Properties;

public sealed class Console
{
    private readonly IPluginLogger _logger;
    private readonly string _title;

    internal Console(IPluginLogger logger, string title)
    {
        _logger = logger;
        _title = title;
    }

    public void Log(JsValue jsValue) => Info(jsValue);

    public void Log(params JsValue[] jsValues) => Info(jsValues);

    public void Info(JsValue jsValue)
    {
        _logger.Log(LogLevel.Information, _title, jsValue.ToString());
    }

    public void Info(params JsValue[] jsValues)
    {
        _logger.Log(LogLevel.Information, _title, string.Join<JsValue>('\x20', jsValues));
    }

    public void Warn(JsValue jsValue)
    {
        _logger.Log(LogLevel.Warning, _title, jsValue.ToString());
    }

    public void Warn(params JsValue[] jsValues)
    {
        _logger.Log(LogLevel.Warning, _title, string.Join<JsValue>('\x20', jsValues));
    }

    public void Error(JsValue jsValue)
    {
        _logger.Log(LogLevel.Error, _title, jsValue.ToString());
    }

    public void Error(params JsValue[] jsValues)
    {
        _logger.Log(LogLevel.Error, _title, string.Join<JsValue>('\x20', jsValues));
    }
}
