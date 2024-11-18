using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Bindings;
using Serein.Core.Services.Bindings;

using Xunit;

using Parser = Serein.Core.Services.Commands.CommandParser;

namespace Serein.Tests.Services;

[Collection(nameof(Serein))]
public sealed class BindingTests : IDisposable
{
    private readonly IHost _host;
    private readonly BindingManager _bindingManager;
    private readonly Parser _parser;

    public BindingTests()
    {
        _host = HostFactory.BuildNew();
        _bindingManager = _host.Services.GetRequiredService<BindingManager>();
        _parser = _host.Services.GetRequiredService<Parser>();

        var context = _host.Services.GetRequiredService<BindingRecordDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _host.StopAsync();
        _host.Dispose();
    }

    [Theory]
    [InlineData("1")]
    [InlineData("-")]
    [InlineData("_")]
    [InlineData("a..bc")]
    [InlineData("00000000000000000000000000000000000000000000000000000000000000")]
    public void ShouldValidateInvalidGameId(string gameId)
    {
        Assert.Throws<BindingFailureException>(() => _bindingManager.ValidateGameId(gameId));
    }

    [Fact]
    public void ShouldCheckConflict()
    {
        _bindingManager.Add(123456, "gameId");
        Assert.True(_bindingManager.TryGetValue(123456, out var record));
        Assert.Throws<BindingFailureException>(() => _bindingManager.Add(123456, "gameId"));
    }

    [Fact]
    public void ShouldThrowWhenTryingToRemoveNonExistentRecord()
    {
        Assert.Throws<BindingFailureException>(() => _bindingManager.Remove(123456, "gameId"));
    }

    [Fact]
    public void ShouldBeAbleToAccessUsingCommandVariable()
    {
        _bindingManager.Add(123456, "test_name");
        Assert.Equal(
            "test_name",
            _parser.ApplyVariables(
                "{sender.gameid}",
                new(MessagePacket: new() { UserId = 123456, Sender = { UserId = 123456 } })
            )
        );
    }
}
