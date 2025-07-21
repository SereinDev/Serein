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

    // 手动获取服务
    public Demo(IServiceProvider serviceProvider)
    {
        _pluginLoggerBase = serviceProvider.GetRequiredService<PluginLoggerBase>();

        Call();
    }

    // 使用自动注入获取服务
    public Demo(PluginLoggerBase pluginLoggerBase)
    {
        _pluginLoggerBase = pluginLoggerBase;

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
