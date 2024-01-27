using Serein.Core.Models.Server;
using Serein.Core.Services.Server;
using Serein.Core.Utils;

namespace Serein.Core.Models.Plugins.Js.Modules;

public class ServerModule
{
    private readonly ServerManager _serverManager;

    public ServerModule(ServerManager serverManager)
    {
        _serverManager = serverManager;
    }

    public IServerInfo? ServerInfo => _serverManager.ServerInfo;
    public ServerStatus ServerStatus => _serverManager.Status;
    public int? Pid => _serverManager.Pid;

    public void Start()
    {
        _serverManager.Start();
    }

    public void Stop()
    {
        _serverManager.Stop();
    }

    public void Terminate()
    {
        _serverManager.Terminate();
    }

    public void Input(string command, EncodingMap.EncodingType? encodingType = null)
    {
        _serverManager.Input(command, encodingType);
    }

    public bool TryStart()
    {
        try
        {
            _serverManager.Start();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool TryStop()
    {
        try
        {
            _serverManager.Stop();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool TryTerminate()
    {
        try
        {
            _serverManager.Terminate();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
