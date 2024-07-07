using System;
using System.CommandLine;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serein.Cli.Interaction;
using Serein.Cli.Loggers;
using Serein.Core;
using Serein.Core.Models.Output;
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

        builder.Services.AddHostedService<TitleUpdater>();
        builder.Services.AddHostedService<InputReader>();

        builder.Services.AddSingleton<IConnectionLogger, ConnectionLogger>();
        builder.Services.AddSingleton<IPluginLogger, PluginLogger>();
        builder.Services.AddSingleton<ILogger, CliLogger>();

        var app = builder.Build();

        app.Run();
    }
}
