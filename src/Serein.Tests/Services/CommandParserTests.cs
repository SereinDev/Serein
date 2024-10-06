using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection.OneBot.Packets;
using Serein.Core.Services.Servers;

using Xunit;

using Parser = Serein.Core.Services.Commands.CommandParser;

namespace Serein.Tests.Services;

[Collection(nameof(SereinApp))]
public class CommandParserTests : IDisposable
{
    private readonly IHost _app;
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
    public static void ShouldBeAbleToParseCommand(
        string input,
        CommandType exceptedType,
        string exceptedBody,
        string? exceptedArgument = null
    )
    {
        var cmd = Parser.Parse(CommandOrigin.ConsoleExecute, input);
        Assert.Equal(exceptedType, cmd.Type);
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
    public static void ShouldThrowException(string? input, string exceptedMsg)
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
    public static void ShouldNotThrowExceptionWhenThrowsIsFalse(string? input)
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

    [Fact]
    public void ShouldWorkWithServerVariable()
    {
        _app.Services.GetRequiredService<ServerManager>().Add("foo", new()
        {
            Name = "5678"
        });
        _app.Services.GetRequiredService<ServerManager>().Add("bar", new()
        {
            Name = "5678"
        });
        Assert.Equal("foo", _commandParser.ApplyVariables("{server.id}", null));
        Assert.Equal("foo", _commandParser.ApplyVariables("{server.id@foo}", null));
        Assert.Equal("bar", _commandParser.ApplyVariables("{server.id@bar}", null));

        Assert.Equal("5678", _commandParser.ApplyVariables("{server.name}", null));
        Assert.Equal("5678", _commandParser.ApplyVariables("{server.name@foo}", null));

        Assert.Equal("未启动", _commandParser.ApplyVariables("{server.status}", null));
        Assert.Equal("未启动", _commandParser.ApplyVariables("{server.status@foo}", null));

        Assert.Equal("bar", _commandParser.ApplyVariables("{server.id}", new(ServerId: "bar")));
    }

    [Fact]
    public void ShouldWorkWithMsgPacketVariable()
    {
        var packet = new MessagePacket
        {
            UserId = 114514,
            MessageId = 1,
            Sender =
            {
                Card = "",
                Nickname = "nickname",
            }
        };
        Assert.Equal("1", _commandParser.ApplyVariables("{msg.id}", new(MessagePacket: packet)));
        Assert.Equal("成员", _commandParser.ApplyVariables("{sender.role}", new(MessagePacket: packet)));
        Assert.Equal("114514", _commandParser.ApplyVariables("{sender.id}", new(MessagePacket: packet)));
        Assert.Equal("nickname", _commandParser.ApplyVariables("{sender.nickname}", new(MessagePacket: packet)));
        Assert.Equal("nickname", _commandParser.ApplyVariables("{sender.shownname}", new(MessagePacket: packet)));
    }
}
