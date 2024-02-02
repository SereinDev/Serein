using Microsoft.Extensions.Hosting;

namespace Serein.Cli.Interaction.Commands;

[CommandDescription("exit", "关闭并退出", Priority = -114514)]
public class ExitCommand : Command
{
    public ExitCommand(IHost host)
        : base(host) { }

    public override void Parse(string[] args)
    {
        _host.StopAsync();
    }
}
