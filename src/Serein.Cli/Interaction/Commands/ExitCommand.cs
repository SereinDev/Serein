using Microsoft.Extensions.Hosting;

namespace Serein.Cli.Interaction.Commands;

[CommandDescription("exit", "关闭并退出", Priority = -114514)]
public class ExitCommand(IHost host) : Command(host)
{
    public override void Invoke(string[] args)
    {
        _host.StopAsync();
    }
}
