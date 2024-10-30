using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core;
using Serein.Core.Models.Output;
using Serein.Core.Utils;
using Serein.Tests.Loggers;

namespace Serein.Tests;

public static class HostFactory
{
    private static readonly object Lock = new();

    public static IHost BuildNew()
    {
        lock (Lock)
        {
            if (Directory.Exists(PathConstants.Root))
            {
                Array.ForEach(
                    Directory.GetFiles(PathConstants.Root, "*", SearchOption.AllDirectories),
                    File.Delete
                );
            }

            var builder = SereinAppBuilder.CreateBuilder();

            builder.Services.AddSingleton<IPluginLogger, PluginLogger>();
            builder.Services.AddSingleton<IConnectionLogger, ConnectionLogger>();

            return builder.Build();
        }
    }
}
