using Serein.Core;

using Xunit;

namespace Serein.Tests.Static;

public class SereinAppInfoTests
{
    [Fact]
    public void VersionShouldBeEqualToThisTestAssembly()
    {
        Assert.Equal(
            SereinApp.Version,
            typeof(SereinAppInfoTests).Assembly.GetName().Version!.ToString()
        );
    }

    [Fact]
    public void AppTypeShouleBeUnknown()
    {
        Assert.Equal(AppType.Unknown, SereinApp.Type);
    }
}
