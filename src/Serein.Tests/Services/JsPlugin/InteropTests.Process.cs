using System;
using System.Linq;
using Jint.Runtime;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests : IDisposable
{
    [Fact]
    public void ShouldAccessToBuiltInModuleProcess()
    {
        var kv = _jsPluginLoader.Plugins.First();

        Assert.Equal(Environment.ProcessId, kv.Value.Engine.Evaluate("process.pid"));
        Assert.Equal(Environment.ExitCode, kv.Value.Engine.Evaluate("process.exitCode"));
        Assert.Equal(Environment.Version.ToString(), kv.Value.Engine.Evaluate("process.version"));
        Assert.Equal(Environment.CurrentDirectory, kv.Value.Engine.Evaluate("process.cwd()"));
        Assert.Equal(Environment.ProcessPath, kv.Value.Engine.Evaluate("process.execPath"));
        Assert.Equal("win32nt", kv.Value.Engine.Evaluate("process.platform"));
        Assert.Equal(Types.Object, kv.Value.Engine.Evaluate("process.env").Type);
    }

    [Fact]
    public void ShouldBeAbleToSetExitCode()
    {
        var kv = _jsPluginLoader.Plugins.First();

        var code = Environment.ExitCode;

        kv.Value.Engine.Evaluate("process.exitCode = 114514");
        Assert.Equal(114514, kv.Value.Engine.Evaluate("process.exitCode"));

        Environment.ExitCode = code;
    }
}
