using System.Threading.Tasks;
using Jint;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests
{
    [Fact]
    public async Task ShouldBeAbleToSetTimeout()
    {
        _kv.Value.Engine.Execute(
            """
            var called = false;
            setTimeout(() => { called = true; }, 500);
            """
        );

        await Task.Delay(1000);
        Assert.True(_kv.Value.Engine.Evaluate("called").AsBoolean());
    }

    [Fact]
    public async Task ShouldBeAbleToSetInterval()
    {
        _kv.Value.Engine.Execute(
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
        Assert.Equal(5, _kv.Value.Engine.Evaluate("count"));
    }
}
