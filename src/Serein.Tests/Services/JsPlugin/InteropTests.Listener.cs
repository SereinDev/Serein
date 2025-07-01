using System.Threading.Tasks;
using Jint;
using Jint.Runtime;
using Serein.Core.Models.Plugins;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests
{
    [Fact]
    public async Task ShouldBeAbleToSetListener()
    {
        _kv.Value.Engine.Execute(
            "var called = false; serein.setListener('PluginsLoaded', () => { called = true; });"
        );
        _eventDispatcher.Dispatch(Event.PluginsLoaded);

        await Task.Delay(5000);
        Assert.True(_kv.Value.Engine.Evaluate("called").AsBoolean());
    }

    [Fact]
    public void ShouldThrowWithInvalidListenerName()
    {
        Assert.Throws<JavaScriptException>(
            () => _kv.Value.Engine.Execute("serein.setListener('InvalidEvent', () => {});")
        );
    }
}
