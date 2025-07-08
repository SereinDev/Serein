using System;
using Serein.Core.Models.Commands;
using Xunit;
using Parser = Serein.Core.Services.Commands.CommandParser;

namespace Serein.Tests.Services.CommandParser;

[Collection(nameof(Serein))]
public static class CommandTests
{
    [Theory]
    [InlineData("[cmd]114", CommandType.ExecuteShellCommand, "114")]
    [InlineData("[CmD]514", CommandType.ExecuteShellCommand, "514")]
    [InlineData("[cmD]abc", CommandType.ExecuteShellCommand, "abc")]
    // ---------
    [InlineData("[s]0000", CommandType.InputServer, "0000")]
    [InlineData("[server]0000", CommandType.InputServer, "0000")]
    [InlineData("[sErVer]0000", CommandType.InputServer, "0000")]
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
    public static void CanParseCommand(
        string input,
        CommandType exceptedType,
        string exceptedBody,
        string? exceptedArgument = null
    )
    {
        var cmd = Parser.Parse(CommandOrigin.Null, input);
        Assert.Equal(exceptedType, cmd.Type);
        Assert.Equal(exceptedBody, cmd.Body);

        if (!string.IsNullOrEmpty(exceptedArgument))
        {
            Assert.Equal(exceptedArgument, cmd.Arguments?.Target);
        }
        else
        {
            Assert.True(string.IsNullOrEmpty(cmd.Arguments?.Target));
        }
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
    public static void ShouldThrowExceptionWithInvalidInput(string? input, string exceptedMsg)
    {
        Parser.Parse(CommandOrigin.Null, input, false);

        try
        {
            Parser.Parse(CommandOrigin.Null, input, true);
            Assert.Fail("未抛出异常");
        }
        catch (Exception e)
        {
            Assert.True(e.Message == exceptedMsg || e.Message.Contains(exceptedMsg));
        }
    }

    [Fact]
    public static void ShouldParseCommandWithMultiArguments()
    {
        var cmd = Parser.Parse(
            CommandOrigin.Null,
            "[g:target=114514,autoEscape=true,self=qq.121313]hello world",
            true
        );

        Assert.Equal(CommandType.SendGroupMsg, cmd.Type);
        Assert.Equal("hello world", cmd.Body);

        Assert.NotNull(cmd.Arguments);
        Assert.Equal("114514", cmd.Arguments.Target);
        Assert.True(cmd.Arguments.AutoEscape);
        Assert.Equal("qq", cmd.Arguments.Self?.Platform);
        Assert.Equal("121313", cmd.Arguments.Self?.UserId);
    }

    [Theory]
    [InlineData("[g:self=qq.121313]hello world")]
    [InlineData("[g:self_id=121313,self_platform=qq]hello world")]
    [InlineData("[g:selfId=121313,selfPlatform=qq]hello world")]
    [InlineData("[g:self_id=123131313123,self_platform=awdadwdasdwa,self=qq.121313]hello world")]
    public static void ShouldParseCommandWithSenderArguments(string command)
    {
        var cmd = Parser.Parse(CommandOrigin.Null, command, true);

        Assert.NotNull(cmd.Arguments);
        Assert.True(string.IsNullOrEmpty(cmd.Arguments.Target));
        Assert.Equal("qq", cmd.Arguments.Self?.Platform);
        Assert.Equal("121313", cmd.Arguments.Self?.UserId);
    }
}
