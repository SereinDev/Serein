using System.IO;
using Serein.Core.Utils;
using Xunit;

namespace Serein.Tests.Static;

public static class CrashTests
{
    [Fact]
    public static void ShouldCreateLogFile()
    {
        var path = CrashHelper.CreateLog(new());
        Assert.True(File.Exists(path));
    }
}
