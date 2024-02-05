using System;

using Serein.Core.Models.Commands;

using Xunit;

using Parser = Serein.Core.Services.CommandParser;

namespace Serein.Tests.Static;

public class CommandParserTests
{
    [Theory]
    [InlineData("[cmd]114", CommandType.ExecuteShellCommand, "114")]
    [InlineData("[CmD]514", CommandType.ExecuteShellCommand, "514")]
    [InlineData("[cmD]abc", CommandType.ExecuteShellCommand, "abc")]
    // ---------
    [InlineData("[s]0000", CommandType.ServerInput, "0000")]
    [InlineData("[server]0000", CommandType.ServerInput, "0000")]
    [InlineData("[sErVer]0000", CommandType.ServerInput, "0000")]
    // ---------
    [InlineData("[g]1", CommandType.SendGroupMsg, "1")]
    [InlineData("[gRoup]2", CommandType.SendGroupMsg, "2")]
    [InlineData("[G:114514]3", CommandType.SendGroupMsg, "3", "114514")]
    [InlineData("[group:114514]4", CommandType.SendGroupMsg, "4", "114514")]
    [InlineData("[grOUp:114514]4", CommandType.SendGroupMsg, "4", "114514")]
    // ---------
    [InlineData("[p]1", CommandType.SendPrivateMsg, "1")]
    [InlineData("[pRIvate]2", CommandType.SendPrivateMsg, "2")]
    [InlineData("[p:114514]3", CommandType.SendPrivateMsg, "3", "114514")]
    [InlineData("[priVate:114514]4", CommandType.SendPrivateMsg, "4", "114514")]
    [InlineData("[priVAte:114514]4", CommandType.SendPrivateMsg, "4", "114514")]
    public static void ShouldBeAbleToParseCommand(
        string input,
        CommandType excepted,
        string exceptedBody,
        string? exceptedArgument = null
    )
    {
        var cmd = Parser.Parse(CommandOrigin.ConsoleExecute, input);
        Assert.Equal(excepted, cmd.Type);
        Assert.Equal(exceptedBody, cmd.Body);

        if (!string.IsNullOrEmpty(exceptedArgument))
            Assert.Equal(exceptedArgument, cmd.Argument);
        else
            Assert.Equal(string.Empty, cmd.Argument);
    }

    [Theory]
    [InlineData(null, "命令为空")]
    [InlineData("", "命令为空")]
    [InlineData(" ", "命令为空")]
    [InlineData("1", "缺少命令标识")]
    [InlineData("[", "缺少命令标识")]
    [InlineData("   [", "缺少命令标识")]
    [InlineData("]", "缺少命令标识")]
    [InlineData("[]", "命令语法不正确")]
    [InlineData("[1]", "命令语法不正确")]
    [InlineData("[]a", "命令语法不正确")]
    [InlineData("[]123", "命令语法不正确")]
    [InlineData("[:]123", "命令语法不正确")]
    public static void ShouldBeAbleToThrowException(string? input, string exceptedMsg)
    {
        try
        {
            Parser.Parse(CommandOrigin.ConsoleExecute, input, true);
            Assert.Fail("未抛出异常");
        }
        catch (Exception e)
        {
            Assert.True(e.Message == exceptedMsg || e.Message.Contains(exceptedMsg));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("1")]
    [InlineData("[")]
    [InlineData("   [")]
    [InlineData("]")]
    [InlineData("[]")]
    [InlineData("[1]")]
    [InlineData("[]a")]
    [InlineData("[]123")]
    [InlineData("[:]123")]
    public static void ShouldBeAbleToThrowExceptionWhenThrowsIsFalse(string? input)
    {
        Parser.Parse(CommandOrigin.ConsoleExecute, input);
    }
}
