using System;

using Microsoft.Extensions.Hosting;

namespace Serein.Cli.Interaction;

public abstract class Command(IHost host)
{
    protected readonly IHost _host = host;
    protected IServiceProvider Services => _host.Services;

    public abstract void Invoke(string[] args);
}
