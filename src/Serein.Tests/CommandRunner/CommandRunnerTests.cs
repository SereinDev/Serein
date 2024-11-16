using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Commands;

using Xunit;

using Runner = Serein.Core.Services.Commands.CommandRunner;
using Parser = Serein.Core.Services.Commands.CommandParser;

namespace Serein.Tests.CommandRunner;

[Collection(nameof(Serein))]
public sealed class CommandTests
{
    private readonly IHost _app;
    private readonly Runner _commandRunner;

    public CommandTests()
    {
        _app = HostFactory.BuildNew();
        _app.StartAsync();

        _commandRunner = _app.Services.GetRequiredService<Runner>();
    }

    [Fact]
    public async Task ShouldExecuteShellCommand()
    {
        await _commandRunner.RunAsync(Parser.Parse(CommandOrigin.Null, "[cmd]echo.1>Serein/1.txt"));
        Assert.True(File.Exists("Serein/1.txt"));
    }
}