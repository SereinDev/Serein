using System.Linq;
using System.Threading.Tasks;
using Jint;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests
{
    [Fact]
    public async Task ShouldBeAbleToSetTimeout()
    {
        var kv = _jsPluginLoader.Plugins.First();
        kv.Value.Engine.Execute(
            """
            var called = false;
            setTimeout(() => { called = true; }, 500);
            """
        );

        await Task.Delay(1000);
        Assert.True(kv.Value.Engine.Evaluate("called").AsBoolean());
    }

    [Fact]
    public async Task ShouldBeAbleToSetInterval()
    {
        var kv = _jsPluginLoader.Plugins.First();
        kv.Value.Engine.Execute(
            """
            var count = 0;
            var interval = setInterval(() => {
                count++;
                if (count >= 5) {
                    clearInterval(interval);
                }
            }, 100);
            """
        );

        await Task.Delay(1000);
        Assert.Equal(5, kv.Value.Engine.Evaluate("count"));
    }
}
