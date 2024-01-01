using System;
using System.CommandLine;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Interaction;
using Serein.Core;
using Serein.Core.Models;
using Serein.Core.Services.Data;
using Serein.Core.Utils;

namespace Serein.Cli;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.InputEncoding = EncodingMap.UTF8;
        Console.OutputEncoding = EncodingMap.UTF8;
        Console.ResetColor();

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            Console.Title = $"Serein.Cli {SereinApp.Version}";

        return CreateCommand().Invoke(args);
    }

    private static RootCommand CreateCommand()
    {
        var rootCommand = new RootCommand();

        rootCommand.SetHandler(BuildApp);
        return rootCommand;
    }

    private static void BuildApp()
    {
        var builder = new SereinAppBuilder();
        builder.ConfigureService();

        builder.Services.AddSingleton<InputReader>();
        builder.Services.AddSingleton<IOutputHandler, CliLogger>(
            (services) =>
                new(
                    "Serein",
                    services.GetRequiredService<SettingProvider>().Value.Application.LogLevel
                )
        );

        var app = builder.Build();
        app.OnStarted += () =>
            app.Services.GetRequiredService<InputReader>().Start(app.CancellationToken);

        app.Run();
    }
}
