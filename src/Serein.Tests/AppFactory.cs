using Microsoft.Extensions.DependencyInjection;

using Serein.Core;
using Serein.Core.Models.Output;

namespace Serein.Tests;

public static class AppFactory
{
    public static SereinApp BuildNew()
    {
        var builder = new SereinAppBuilder();
        builder.ConfigureService();
        builder.Services.AddTransient<ISereinLogger, TestLogger>();

        return builder.Build();
    }
}
