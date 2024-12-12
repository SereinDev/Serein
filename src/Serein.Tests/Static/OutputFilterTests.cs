using Serein.Core.Utils;
using Xunit;

namespace Serein.Tests.Static;

public static class OutputFilterTests
{
    [Theory]
    [InlineData("\u001b[31mHello\u001b[0m", "Hello")]
    [InlineData("\u001b[38;2;114;111;23mHello\u001b[0m", "Hello")]
    [InlineData("\u001b[?25lHello\u001b[?25h", "Hello")]
    [InlineData("\u001b[KHello", "Hello")]
    [InlineData("\u001b[0KHello", "Hello")]
    [InlineData("\u001b[0JHello", "Hello")]
    [InlineData("\u001b[=0HHello", "Hello")]
    public static void ShouldRemoveANSIEscapeSequences(string input, string expected)
    {
        Assert.Equal(expected, OutputFilter.RemoveANSIEscapeChars(input));
    }

    [Fact]
    public static void ShouldRemoveControlChars()
    {
        Assert.Empty(OutputFilter.RemoveControlChars("\x10"));
        Assert.Empty(OutputFilter.RemoveControlChars("\x11"));
        Assert.Empty(OutputFilter.RemoveControlChars("\x12"));
        Assert.Empty(OutputFilter.RemoveControlChars("\x13"));
        Assert.Empty(OutputFilter.RemoveControlChars("\x14"));
        Assert.Empty(OutputFilter.RemoveControlChars("\x15"));
    }
}
