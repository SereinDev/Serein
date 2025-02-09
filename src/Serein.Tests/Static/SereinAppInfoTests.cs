using Serein.Core;
using Xunit;

namespace Serein.Tests.Static;

public sealed class SereinAppInfoTests
{
    [Fact]
    public void VersionShouldBeEqualToThisTestAssembly()
    {
        Assert.Equal(
            SereinApp.GetCurrentApp().Version,
            typeof(SereinAppInfoTests).Assembly.GetName().Version
        );
    }

    [Fact]
    public void AppTypeShouleBeUnknown()
    {
        Assert.Equal(AppType.Unknown, SereinApp.GetCurrentApp().Type);
    }
}
