using System;
using System.Collections.Generic;
using System.IO;

namespace Serein.Core.Services.Plugins.Js.BuiltInModules;

public static class Process
{
    public static string Arch => Environment.Is64BitProcess ? "x64" : "arm64";

    public static string[] Argv => Environment.GetCommandLineArgs();

    public static string Argv0 => Argv[0];

    public static string Chdir(string directory)
    {
        Directory.SetCurrentDirectory(directory);
        return directory;
    }

    public static string Cwd()
    {
        return Directory.GetCurrentDirectory();
    }

    public static void Exit(int code = 0)
    {
        Environment.Exit(code);
    }

    public static Dictionary<string, string?> Env
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

    public static string ExecPath => Environment.ProcessPath ?? string.Empty;

    public static int ExitCode
    {
        get => Environment.ExitCode;
        set => Environment.ExitCode = value;
    }

    public static void Kill(int pid)
    {
        System.Diagnostics.Process.GetProcessById(pid).Kill(true);
    }

    public static int Pid => Environment.ProcessId;

    public static string Platform => Environment.OSVersion.Platform.ToString().ToLowerInvariant();

    public static string Version => Environment.Version.ToString();
}
