using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using Serein.Core;
using Serein.Core.Models.Commands;

using Xunit;

using Parser = Serein.Core.Services.CommandParser;

namespace Serein.Tests.Services;

public class CommandParserTests : IDisposable
{
    private readonly SereinApp _app;
    private readonly Parser _commandParser;

    public CommandParserTests()
    {
        _app = AppFactory.BuildNew();
        _app.StartAsync();
        _commandParser = _app.Services.GetRequiredService<Parser>();
    }

    public void Dispose()
    {
        _app.StopAsync();
        _app.Dispose();
    }

    [Theory]
    [InlineData("{aaa.bb@ccc}")]
    [InlineData("{aasafjefeiooivniv}")]
    [InlineData("{aaa.bbbb}")]
    public void ShouldPreserveOrRemoveInvalidVariable(string input)
    {
        Assert.Equal(input, _commandParser.ApplyVariables(input, null));
        Assert.Equal(string.Empty, _commandParser.ApplyVariables(input, null, true));
    }

    [Fact]
    public void ShouldApplyCustomVariables()
    {
        Assert.Equal(
            "[114514]",
            _commandParser.ApplyVariables(
                "[{shit}]",
                new(Variables: new Dictionary<string, string?> { ["shit"] = "114514" })
            )
        );
    }

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

    [Theory]
    [InlineData("{}", false)]
    [InlineData("{1}", false)]
    [InlineData("{_}", false)]
    [InlineData("{\\}", false)]
    [InlineData("{a}", true)]
    [InlineData("{a.}", false)]
    [InlineData("{a.b}", true)]
    [InlineData("{a.111}", false)]
    [InlineData("{a.b.}", false)]
    [InlineData("{a.b.c}", false)]
    [InlineData("{a.b@}", false)]
    [InlineData("{a.b@c}", true)]
    public static void ShouldAnalyzeWhetherToBeVariablePattern(string input, bool result)
    {
        Assert.Equal(result, Parser.Variable.IsMatch(input));
    }
}
