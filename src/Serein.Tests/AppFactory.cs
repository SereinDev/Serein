using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Core.Utils;
using Serein.Tests.Loggers;

namespace Serein.Tests;

public static class AppFactory
{
    public static SereinApp BuildNew()
    {
        if (Directory.Exists(PathConstants.Root))
        {
            Directory.Delete(PathConstants.Root, true);
            Directory.CreateDirectory(PathConstants.Root);
        }

        var builder = new SereinAppBuilder();

        builder.ConfigureService();
        builder.Services.AddSingleton<ILogger, MainTestLogger>();
        builder.Services.AddSingleton<IPluginLogger, PluginLogger>();
        builder.Services.AddSingleton<IConnectionLogger, ConnectionLogger>();

        return builder.Build();
    }
}
