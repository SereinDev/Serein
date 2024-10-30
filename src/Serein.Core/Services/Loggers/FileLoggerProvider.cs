using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Serein.Core.Utils;

namespace Serein.Core.Services.Loggers;

internal class FileLoggerProvider : ILoggerProvider
{
    public static bool IsEnabled { get; } =
        Environment.GetCommandLineArgs().Contains("--log")
        || Environment.GetEnvironmentVariable("SEREIN_LOG") is not null;
    private readonly Dictionary<string, FileLogger> _loggers;
    private readonly List<string> _buffer;
    private readonly string _file;

    public FileLoggerProvider()
    {
        _file = GetFileName();
        _buffer = [];
        _loggers = [];

        Task.Run(() =>
        {
            while (true)
            {
                Flush();
                Task.Delay(1000).Wait();
            }
        });
    }

    public ILogger CreateLogger(string categoryName)
    {
        lock (_loggers)
        {
            if (!_loggers.TryGetValue(categoryName, out var logger))
                logger = _loggers[categoryName] = new FileLogger(categoryName, _buffer);

            return logger;
        }
    }

    public void Dispose()
    {
        _loggers.Clear();
    }

    private static string GetFileName()
    {
        var id = 1;
        while (true)
        {
            var path = string.Format(
                PathConstants.AppLogFile,
                DateTime.Now.ToString("yyyy-MM-dd"),
                id
            );

            if (!File.Exists(path))
                return path;

            id++;
        }
    }

    private void Flush()
    {
        if (_buffer.Count > 0)
            lock (_buffer)
            {
                Directory.CreateDirectory(Path.Combine(PathConstants.LogDirectory, "app"));
                File.AppendAllText(
                    _file,
                    string.Join(Environment.NewLine, _buffer) + Environment.NewLine
                );
                _buffer.Clear();
            }
    }
}
