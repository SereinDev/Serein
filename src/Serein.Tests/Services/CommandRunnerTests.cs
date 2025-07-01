using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serein.Core.Models.Commands;
using Xunit;
using Parser = Serein.Core.Services.Commands.CommandParser;
using Runner = Serein.Core.Services.Commands.CommandRunner;

namespace Serein.Tests.Services;

[Collection(nameof(Serein))]
public sealed class CommandTests
{
    private readonly Runner _commandRunner;

    public CommandTests()
    {
        var app = HostFactory.BuildNew();
        app.StartAsync();

        _commandRunner = app.Services.GetRequiredService<Runner>();
    }

    [Fact]
    public async Task ShouldExecuteShellCommand()
    {
        await _commandRunner.RunAsync(
            Parser.Parse(CommandOrigin.Null, "[cmd]echo \"1\">Serein/1.txt")
        );
        Assert.True(File.Exists("Serein/1.txt"));
    }
}
