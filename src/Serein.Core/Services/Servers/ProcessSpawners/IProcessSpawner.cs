using System;
using System.Diagnostics;
using Serein.Core.Models.Server;

namespace Serein.Core.Services.Servers.ProcessSpawners;

public interface IProcessSpawner
{
    Process? CurrentProcess { get; }

    public bool Status { get; }

    void Start(Configuration configuration);

    void Terminate();

    void Write(byte[] data);

    event EventHandler StatusChanged;

    event EventHandler<int> ProcessExited;
}
