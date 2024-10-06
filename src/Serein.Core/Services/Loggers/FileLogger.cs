using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Serein.Core.Utils;

namespace Serein.Core.Services.Loggers;

public class FileLogger
{
    public bool IsEnable { get; } = Environment.GetCommandLineArgs().Contains("--log");
    private readonly string _file;
    private readonly List<string> _buffer;

    public FileLogger()
    {
        _file = GetFileName();
        _buffer = [];

        Task.Run(() =>
        {
            while (true)
            {
                Flush();
                Task.Delay(1000).Wait();
            }
        });
    }

    private static string GetFileName()
    {
        var id = 1;
        while (true)
        {
            var path = string.Format(PathConstants.AppLogFile, DateTime.Now.ToString("yyyy-MM-dd"), id);

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

    public void Add(string line)
    {
        if (!IsEnable)
            return;

        lock (_buffer)
            _buffer.Add($"{DateTime.Now:T} {line}");
    }
}
