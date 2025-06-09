using System;
using System.IO;
using System.Linq;
using Jint;
using Jint.Runtime;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests : IDisposable
{
    [Fact]
    public void ShouldAccessToBuiltInModuleFs()
    {
        var kv = _jsPluginLoader.Plugins.First();

        Assert.True(kv.Value.Engine.Evaluate("fs.globSync('*.*')").IsArray());

        kv.Value.Engine.Evaluate("fs.writeFileSync('test.txt', 'test')");
        Assert.True(File.Exists("test.txt"));
        Assert.Equal(
            File.ReadAllText("test.txt"),
            kv.Value.Engine.Evaluate("fs.readFileSync('test.txt')")
        );
    }

    [Fact]
    public void ShouldThrowWhenAccessingNotImplementedMethods()
    {
        var kv = _jsPluginLoader.Plugins.First();

        Assert.Throws<JavaScriptException>(() => kv.Value.Engine.Evaluate("fs.readlinkSync('')"));
    }
}
