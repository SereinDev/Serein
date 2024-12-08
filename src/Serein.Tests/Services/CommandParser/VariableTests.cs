using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core.Models.Commands;
using Serein.Core.Models.Network.Connection.OneBot.Packets;
using Serein.Core.Services.Servers;
using Xunit;
using Parser = Serein.Core.Services.Commands.CommandParser;

namespace Serein.Tests.Services.CommandParser;

[Collection(nameof(Serein))]
public sealed class VariableTests : IDisposable
{
    private readonly IHost _app;
    private readonly Parser _commandParser;

    public VariableTests()
    {
        _app = HostFactory.BuildNew();
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
        _app.Services.GetRequiredService<ServerManager>().Add("foo", new() { Name = "5678" });
        _app.Services.GetRequiredService<ServerManager>().Add("bar", new() { Name = "1234" });
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
            Sender = { Card = "", Nickname = "nickname" },
        };
        Assert.Equal("1", _commandParser.ApplyVariables("{msg.id}", new(MessagePacket: packet)));
        Assert.Equal(
            "成员",
            _commandParser.ApplyVariables("{sender.role}", new(MessagePacket: packet))
        );
        Assert.Equal(
            "114514",
            _commandParser.ApplyVariables("{sender.id}", new(MessagePacket: packet))
        );
        Assert.Equal(
            "nickname",
            _commandParser.ApplyVariables("{sender.nickname}", new(MessagePacket: packet))
        );
        Assert.Equal(
            "nickname",
            _commandParser.ApplyVariables("{sender.shownname}", new(MessagePacket: packet))
        );
    }

    [Fact]
    public void ShouldWorkWithRegexVariable()
    {
        Assert.Equal(
            "a",
            _commandParser.Format(
                Parser.Parse(CommandOrigin.Null, "[cmd]$1"),
                new(Match: Regex.Match("a1", @"([a-z])"))
            )
        );
        Assert.Equal(
            "1",
            _commandParser.Format(
                Parser.Parse(CommandOrigin.Null, "[cmd]$a"),
                new(Match: Regex.Match("a1", @"(?<a>\d)"))
            )
        );
    }
}
