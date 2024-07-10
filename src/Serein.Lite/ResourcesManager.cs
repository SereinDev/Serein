using System;
using System.IO;

using Microsoft.Extensions.Logging;

namespace Serein.Lite;

public class ResourcesManager(ILogger logger)
{
    private readonly ILogger _logger = logger;

    private const string IndexHtml = "index.html";
    private const string CustomCss = "custom.css";

    private readonly Stream? _consoleHtml =
        typeof(ResourcesManager).Assembly.GetManifestResourceStream($"Serein.Lite.{IndexHtml}");

    private readonly Stream? _consoleCss =
        typeof(ResourcesManager).Assembly.GetManifestResourceStream($"Serein.Lite.{CustomCss}");

    public void WriteConsoleHtml()
    {
        try
        {
            WriteHtml();
            WriteCss();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "写入控制台网页时出现异常");
        }
    }

    private void WriteHtml()
    {
        if (_consoleHtml is null)
            throw new InvalidDataException($"{IndexHtml} 未找到");

        Directory.CreateDirectory("Serein/console");
        using var stream = new FileStream($"Serein/console/{IndexHtml}", FileMode.Create);
        _consoleHtml.CopyTo(stream);
        stream.Flush();
    }

    private void WriteCss()
    {
        if (_consoleCss is null)
            throw new InvalidDataException($"{CustomCss} 未找到");

        Directory.CreateDirectory("Serein/console");
        using var stream = new FileStream($"Serein/console/{CustomCss}", FileMode.Create);
        _consoleCss.CopyTo(stream);
        stream.Flush();
    }
}
