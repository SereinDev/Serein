using System.Linq;
using Jint.Runtime;
using Serein.Core;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests
{
    [Fact]
    public void ShouldBeAbleToGetService()
    {
        var kv = _jsPluginLoader.Plugins.First();
        var service = kv
            .Value.Engine.Evaluate("serein.getService('Serein.Core.SereinApp')")
            .ToObject();

        Assert.NotNull(service);
        Assert.True(service is SereinApp);
    }

    [Theory]
    [InlineData("114514")] // Invalid service name
    [InlineData("Serein.Core.Services.Bindings.BindingRecordDbContext")] // Internal service
    public void ShouldThrowIfFailedToGetService(string serviceName)
    {
        var kv = _jsPluginLoader.Plugins.First();
        Assert.Throws<JavaScriptException>(
            () => kv.Value.Engine.Evaluate($"serein.getService('{serviceName}')")
        );
    }
}
