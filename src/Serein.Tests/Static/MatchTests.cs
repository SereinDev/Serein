using Serein.Core.Models.Commands;
using Xunit;

namespace Serein.Tests.Static;

public static class MatchTests
{
    [Fact]
    public static void RegexObjShouldBeNullIfRegExpIsEmptyOrInvalid()
    {
        var match = new Match { RegExp = "{1,0}" };
        Assert.Null(match.RegexObj);

        match.RegExp = string.Empty;
        Assert.Null(match.RegexObj);

        match.RegExp = "1";
        Assert.NotNull(match.RegexObj);
    }

    [Fact]
    public static void ShouldWorkOutCorrectMatchExclusion()
    {
        var match = new Match { Exclusions = "a=1; b=2" };
        Assert.Empty(match.MatchExclusion.Servers);
        Assert.Empty(match.MatchExclusion.Groups);
        Assert.Empty(match.MatchExclusion.Users);

        match.Exclusions = "server=1; group=2 ; user = 3;;;;;";
        Assert.Single(match.MatchExclusion.Servers);
        Assert.Single(match.MatchExclusion.Groups);
        Assert.Single(match.MatchExclusion.Users);

        match.Exclusions = "server =  1; group=  2; user=3; server=4; group=5; user=6";
        Assert.Contains("1", match.MatchExclusion.Servers);
        Assert.Contains("4", match.MatchExclusion.Servers);

        Assert.Contains("2", match.MatchExclusion.Groups);
        Assert.Contains("5", match.MatchExclusion.Groups);

        Assert.Contains("3", match.MatchExclusion.Users);
        Assert.Contains("6", match.MatchExclusion.Users);
    }

    [Fact]
    public static void ShouldModifyCommandObjWhenSettingCommand()
    {
        var match = new Match { Command = "[g]1" };
        Assert.NotNull(match.CommandObj);

        match.Command = "111?";
        Assert.NotNull(match.CommandObj);
    }

    [Theory]
    [InlineData(MatchFieldType.ServerOutput, CommandOrigin.ServerOutput)]
    [InlineData(MatchFieldType.ServerInput, CommandOrigin.ServerInput)]
    [InlineData(MatchFieldType.PrivateMsg, CommandOrigin.Msg)]
    [InlineData(MatchFieldType.GroupMsg, CommandOrigin.Msg)]
    [InlineData(MatchFieldType.SelfMsg, CommandOrigin.Msg)]
    [InlineData(MatchFieldType.Disabled, CommandOrigin.Null)]
    public static void ShouldChooseCorrectCommandOrigin(
        MatchFieldType matchFieldType,
        CommandOrigin commandOrigin
    )
    {
        var match = new Match { FieldType = matchFieldType, Command = "[debug]1" };
        Assert.Equal(commandOrigin, match.CommandObj?.Origin);
    }
}
