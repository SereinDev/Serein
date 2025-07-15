using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;
using Serein.Core.Models.Commands;
using Serein.Core.Services.Bindings;
using Serein.Core.Services.Servers;
using Xunit;
using Parser = Serein.Core.Services.Commands.CommandParser;
using V11 = Serein.ConnectionProtocols.Models.OneBot.V11.Packets;
using V12 = Serein.ConnectionProtocols.Models.OneBot.V12.Packets;

#pragma warning disable SYSLIB1045

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
                new() { Variables = new Dictionary<string, string?> { ["shit"] = "114514" } }
            )
        );
    }

    [Theory]
    [InlineData("{}", false)]
    [InlineData("{1}", false)]
    [InlineData("{_}", false)]
    [InlineData("{\\}", false)]
    [InlineData("{a}", true)]
    [InlineData("{a.}", true)]
    [InlineData("{a.b}", true)]
    [InlineData("{a.111}", true)]
    [InlineData("{a.b.}", true)]
    [InlineData("{a.b.c}", true)]
    [InlineData("{a.b@}", false)]
    [InlineData("{a.b@c}", true)]
    public static void ShouldAnalyzeWhetherToBeVariablePattern(string input, bool result)
    {
        Assert.Equal(result, Parser.Variable.IsMatch(input));
    }

    [Fact]
    public void ShouldApplyServerVariable()
    {
        var serverManager = _app.Services.GetRequiredService<ServerManager>();

        serverManager.Add("foo", new() { Name = "5678" });
        serverManager.Add("bar", new() { Name = "1234" });
        serverManager.Add("UPPERCASE", new() { Name = "upper" });

        Assert.Equal("myserver", _commandParser.ApplyVariables("{server.id}", null));
        Assert.Equal("foo", _commandParser.ApplyVariables("{server.id@foo}", null));
        Assert.Equal("bar", _commandParser.ApplyVariables("{server.id@bar}", null));
        Assert.Equal("UPPERCASE", _commandParser.ApplyVariables("{server.id@UPPERCASE}", null));

        Assert.Equal("未命名", _commandParser.ApplyVariables("{server.name}", null));
        Assert.Equal("5678", _commandParser.ApplyVariables("{server.name@foo}", null));

        Assert.Equal("未启动", _commandParser.ApplyVariables("{server.status}", null));
        Assert.Equal("未启动", _commandParser.ApplyVariables("{server.status@foo}", null));

        Assert.Equal(
            "bar",
            _commandParser.ApplyVariables("{server.id}", new() { ServerId = "bar" })
        );
    }

    [Fact]
    public void ShouldApplyOneBotV11MsgPacketVariable()
    {
        var packet = new V11.MessagePacket
        {
            UserId = 114514,
            MessageId = 1,
            Sender = { Card = "", Nickname = "nickname" },
        };

        Assert.Equal(
            "1",
            _commandParser.ApplyVariables(
                "{MSG.ID}",
                new() { Packets = new() { OneBotV11 = packet } }
            )
        );
        Assert.Equal(
            "1",
            _commandParser.ApplyVariables(
                "{msg.id}",
                new() { Packets = new() { OneBotV11 = packet } }
            )
        );
        Assert.Equal(
            "成员",
            _commandParser.ApplyVariables(
                "{sender.role}",
                new() { Packets = new() { OneBotV11 = packet } }
            )
        );
        Assert.Equal(
            "114514",
            _commandParser.ApplyVariables(
                "{sender.id}",
                new() { Packets = new() { OneBotV11 = packet } }
            )
        );
        Assert.Equal(
            "nickname",
            _commandParser.ApplyVariables(
                "{sender.nickname}",
                new() { Packets = new() { OneBotV11 = packet } }
            )
        );
        Assert.Equal(
            "nickname",
            _commandParser.ApplyVariables(
                "{sender.shownname}",
                new() { Packets = new() { OneBotV11 = packet } }
            )
        );
    }

    [Fact]
    public void ShouldApplyOneBotV12MsgPacketVariable()
    {
        var packet = new V12.MessagePacket { UserId = "114514", MessageId = "1" };
        Assert.Equal(
            "1",
            _commandParser.ApplyVariables(
                "{msg.id}",
                new() { Packets = new() { OneBotV12 = packet } }
            )
        );
        Assert.Equal(
            "114514",
            _commandParser.ApplyVariables(
                "{sender.id}",
                new() { Packets = new() { OneBotV12 = packet } }
            )
        );
    }

    [Fact]
    public void ShouldApplySatoriMsgPacketVariable()
    {
        var eventBody = new EventBody
        {
            Message = new() { Id = "1" },
            User = new()
            {
                Id = "114514",
                Nick = "nickname",
                Avatar = "http://example.com",
            },
        };

        Assert.Equal(
            "1",
            _commandParser.ApplyVariables(
                "{msg.id}",
                new() { Packets = new() { SatoriV1 = eventBody } }
            )
        );
        Assert.Equal(
            "114514",
            _commandParser.ApplyVariables(
                "{sender.id}",
                new() { Packets = new() { SatoriV1 = eventBody } }
            )
        );
        Assert.Equal(
            "http://example.com",
            _commandParser.ApplyVariables(
                "{sender.avatar}",
                new() { Packets = new() { SatoriV1 = eventBody } }
            )
        );
        Assert.Equal(
            "nickname",
            _commandParser.ApplyVariables(
                "{sender.nickname}",
                new() { Packets = new() { SatoriV1 = eventBody } }
            )
        );
    }

    [Fact]
    public void ShouldApplyRegexVariable()
    {
        Assert.Equal(
            "a",
            _commandParser.Format(
                Parser.Parse(CommandOrigin.Null, "[cmd]$1"),
                new() { Match = Regex.Match("a1", @"([a-z])") }
            )
        );
        Assert.Equal(
            "1",
            _commandParser.Format(
                Parser.Parse(CommandOrigin.Null, "[cmd]$a"),
                new() { Match = Regex.Match("a1", @"(?<a>\d)") }
            )
        );
    }

    [Fact]
    public void ShouldApplyBindingVariable()
    {
        Assert.Equal(
            "{user.id}",
            _commandParser.Format(Parser.Parse(CommandOrigin.Null, "[cmd]{user.id}"), new())
        );

        Assert.Equal(
            string.Empty,
            _commandParser.Format(Parser.Parse(CommandOrigin.Null, "[cmd]{user.id@steve}"), new())
        );

        _app.Services.GetRequiredService<BindingManager>().Add("123456", "steve");

        Assert.Equal(
            "123456",
            _commandParser.Format(Parser.Parse(CommandOrigin.Null, "[cmd]{user.id@steve}"), new())
        );
    }
}
