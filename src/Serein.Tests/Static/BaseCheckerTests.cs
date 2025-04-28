using Serein.Core.Utils;
using Xunit;

namespace Serein.Tests.Static;

public static class BaseCheckerTests
{
    [Fact]
    public static void ShouldNotBeInTempFolder()
    {
        Assert.False(BaseChecker.CheckTempFolder());
    }

    [Fact]
    public static void ShouldNotHaveConflictProcesses()
    {
        var processes = BaseChecker.CheckConflictProcesses();
        Assert.Empty(processes);
    }
}
