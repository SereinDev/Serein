using System.IO;
using System.Linq;
using Jint;
using Jint.Runtime;
using Serein.Core.Utils;
using Xunit;

namespace Serein.Tests.Services.JsPlugin;

public sealed partial class InteropTests
{
    [Fact]
    public void ShouldAccessToBuiltInModuleFs()
    {
        Assert.True(_kv.Value.Engine.Evaluate("fs.globSync('*.*')").IsArray());

        _kv.Value.Engine.Evaluate("fs.writeFileSync('test.txt', 'test')");
        Assert.True(File.Exists("test.txt"));
        Assert.Equal(
            File.ReadAllText("test.txt"),
            _kv.Value.Engine.Evaluate("fs.readFileSync('test.txt')")
        );
    }

    [Fact]
    public void ShouldThrowWhenAccessingNotImplementedMethods()
    {
        Assert.Throws<JavaScriptException>(() => _kv.Value.Engine.Evaluate("fs.readlinkSync('')"));
    }

    [Theory]
    [InlineData(["123.js"])]
    [InlineData(["abc/456.dll"])]
    [InlineData(["../sky.png"])]
    public void ShouldBeAbleToResolvePaths(params string[] paths)
    {
        Assert.Equal(
            Path.GetFullPath(Path.Combine([PathConstants.PluginsDirectory, .. paths])),
            _kv.Value.Engine.Evaluate(
                $"serein.resolve({string.Join(string.Empty, paths.Select(p => $"'{p}',"))})"
            )
        );
    }
}
