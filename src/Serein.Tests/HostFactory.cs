using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core;
using Serein.Core.Models.Abstractions;
using Serein.Core.Utils;
using Serein.Tests.Loggers;
using Serein.Tests.Utils;

namespace Serein.Tests;

public static class HostFactory
{
    static HostFactory()
    {
        if (Directory.Exists("tests"))
        {
            Directory.Delete("tests", true);
        }

        if (Directory.Exists("Serein"))
        {
            Directory.Delete("Serein", true);
        }
    }

    public static IHost BuildNew(bool enableFileLogger = false, [CallerFilePath] string caller = "")
    {
        Task.Delay(1000).Wait();
        TestHelper.Initialize(caller);
        Task.Delay(1000).Wait();

        Directory.CreateDirectory(PathConstants.Root);
        Directory.CreateDirectory(PathConstants.PluginsDirectory);

        var builder = SereinAppBuilder.CreateBuilder(enableFileLogger);

        builder.Services.AddSingleton<PluginLoggerBase, PluginLogger>();
        builder.Services.AddSingleton<ConnectionLoggerBase, ConnectionLogger>();

        return builder.Build();
    }
}
