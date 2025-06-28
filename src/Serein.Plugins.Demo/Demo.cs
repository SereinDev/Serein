using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serein.Core.Models.Abstractions;
using Serein.Core.Services.Plugins.Net;

namespace Serein.Plugins.Demo;

public class Demo : PluginBase
{
    private readonly PluginLoggerBase _pluginLoggerBase;
    private readonly IServiceProvider _serviceProvider;

    public Demo(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _pluginLoggerBase = _serviceProvider.GetRequiredService<PluginLoggerBase>();

        Call();
    }

    public override void Dispose()
    {
        Call();
    }

    protected override Task OnPluginsLoaded()
    {
        Call();
        return Task.CompletedTask;
    }

    protected override Task OnPluginsUnloading()
    {
        Call();
        return Task.CompletedTask;
    }

    private void Call([CallerMemberName] string member = "")
    {
        _pluginLoggerBase.Log(LogLevel.Information, nameof(Demo), member + "!");
    }
}
