using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Serein.Core.Services.Data;

public class LogWriter(ILogger<LogWriter> logger, string directory)
{
    private readonly object _lock = new();
    private readonly List<string> _buffer = [];
    private DateTime _last;

    public async Task WriteAsync(string line)
    {
        lock (_buffer)
        {
            _buffer.Add(line);
        }

        _last = DateTime.Now;
        await Task.Delay(500);

        if ((DateTime.Now - _last).TotalMilliseconds > 450)
        {
            Flush();
        }
    }

    public void Flush()
    {
        lock (_lock)
        {
            lock (_buffer)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var path = Path.Combine(directory, $"{DateTime.Now:yyyy-MM-dd}.log");

                try
                {
                    File.AppendAllLines(path, _buffer);
                    _buffer.Clear();
                }
                catch (Exception e)
                {
                    logger.LogDebug(e, "写入“{}”失败", path);
                }
            }
        }
    }
}
