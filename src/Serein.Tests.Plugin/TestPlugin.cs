using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Serein.Core.Models.Plugins.Net;

namespace Serein.Tests.Plugin;

public class TestPlugin : PluginBase
{
    public TestPlugin(IServiceProvider serviceProvider)
    {
        Call();
        Console.WriteLine(JsonConvert.DeserializeObject("{}"));
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

    private static void Call([CallerMemberName] string member = "")
    {
        Console.WriteLine(member + "!");
    }
}
