using System;
using System.Collections.Generic;
using System.IO;

namespace Serein.Core.Services.Plugins.Js.BuiltInModules;

#pragma warning disable CA1822

public sealed class Process
{
    public string Arch => Environment.Is64BitProcess ? "x64" : "arm64";

    public string[] Argv => Environment.GetCommandLineArgs();

    public string Argv0 => Argv[0];

    public string Chdir(string directory)
    {
        Directory.SetCurrentDirectory(directory);
        return directory;
    }

    public string Cwd()
    {
        return Directory.GetCurrentDirectory();
    }

    public void Exit(int code = 0)
    {
        Environment.Exit(code);
    }

    public Dictionary<string, string?> Env
    {
        get
        {
            var dict = new Dictionary<string, string?>();
            var env = Environment.GetEnvironmentVariables();
            foreach (var key in env.Keys)
            {
                var k = key.ToString();
                if (string.IsNullOrEmpty(k) || string.IsNullOrWhiteSpace(k))
                {
                    continue;
                }
                dict[k] = env[key]?.ToString();
            }

            return dict;
        }
    }

    public string ExecPath => Environment.ProcessPath ?? string.Empty;

    public int ExitCode
    {
        get => Environment.ExitCode;
        set => Environment.ExitCode = value;
    }

    public void Kill(int pid)
    {
        System.Diagnostics.Process.GetProcessById(pid).Kill(true);
    }

    public int Pid => Environment.ProcessId;

    public string Platform => Environment.OSVersion.Platform.ToString().ToLowerInvariant();

    public string Version => Environment.Version.ToString();
}
