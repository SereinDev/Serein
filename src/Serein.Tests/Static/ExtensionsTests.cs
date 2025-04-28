using System;
using System.Threading.Tasks;
using Serein.Core.Utils.Extensions;
using Xunit;

namespace Serein.Tests.Static;

public static class ExtensionsTests
{
    [Fact]
    public static void ShouldBeAbleToGetUnicodeChar()
    {
        Assert.Equal("\\u00a7", "\u00a7".ToUnicode());
    }

    [Theory]
    [InlineData(0, "0 B")]
    [InlineData(1, "1 B")]
    [InlineData(1023, "1023 B")]
    [InlineData(1024, "1.0 KB")]
    [InlineData(1025, "1.0 KB")]
    [InlineData(1024 * 1024, "1.0 MB")]
    [InlineData(1024 * 1024 + 1, "1.0 MB")]
    [InlineData(1024 * 1024 * 1024, "1.0 GB")]
    public static void ShouldBeAbleToGetSizeString(long size, string expected)
    {
        Assert.Equal(expected, size.ToSizeString());
    }

    [Theory]
    [InlineData("0:00:00", 0, 0, 0)]
    [InlineData("3:02:01", 3, 2, 1)]
    [InlineData("24:00:00", 24, 0, 0)]
    public static void ShouldBeAbleToGetTimeSpanString(string expected, params int[] value)
    {
        TimeSpan? timeSpan = new TimeSpan(value[0], value[1], value[2]);
        Assert.Equal(expected, timeSpan.ToCommonString());
    }

    [Fact]
    public static void ShouldBeAbleToWaitForTask()
    {
        var task = Task.Run(() => 1);
        Assert.Equal(1, task.Await());
        Task.Delay(50).Await();
    }
}
