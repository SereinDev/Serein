using System;
using System.Threading.Tasks;

using Serein.Core.Models.Plugins.Net;

namespace Serein.Tests.Plugin;

public class TestPlugin : PluginBase
{
    public TestPlugin()
    {
        Console.WriteLine("Loaded!");

        var dateTime = DateTime.Now;
        Task.Run(() =>
        {
            while (true)
            {
                Console.WriteLine($"[{dateTime}]");
                Task.Delay(5000).Wait();
            }
        });
    }

    public override void Dispose()
    {
        Console.WriteLine("Disposed!");
    }
}
