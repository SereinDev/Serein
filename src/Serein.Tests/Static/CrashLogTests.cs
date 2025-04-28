using Serein.Core.Utils;
using Xunit;

namespace Serein.Tests.Static;

public static class CrashLogTests
{
    [Fact]
    public static void ShouldNotThrowException()
    {
        CrashHelper.CreateLog(new());
    }
}
