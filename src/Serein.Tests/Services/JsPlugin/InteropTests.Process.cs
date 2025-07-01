using System;
using Jint.Runtime;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests
{
    [Fact]
    public void ShouldAccessToBuiltInModuleProcess()
    {
        Assert.Equal(Environment.ProcessId, _kv.Value.Engine.Evaluate("process.pid"));
        Assert.Equal(Environment.ExitCode, _kv.Value.Engine.Evaluate("process.exitCode"));
        Assert.Equal(Environment.Version.ToString(), _kv.Value.Engine.Evaluate("process.version"));
        Assert.Equal(Environment.CurrentDirectory, _kv.Value.Engine.Evaluate("process.cwd()"));
        Assert.Equal(Environment.ProcessPath, _kv.Value.Engine.Evaluate("process.execPath"));
        Assert.Equal("win32nt", _kv.Value.Engine.Evaluate("process.platform"));
        Assert.Equal(Types.Object, _kv.Value.Engine.Evaluate("process.env").Type);
    }

    [Fact]
    public void ShouldBeAbleToSetExitCode()
    {
        var code = Environment.ExitCode;

        _kv.Value.Engine.Evaluate("process.exitCode = 114514");
        Assert.Equal(114514, _kv.Value.Engine.Evaluate("process.exitCode"));

        Environment.ExitCode = code;
    }
}
