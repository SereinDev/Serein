using System.Threading;

using Serein.Core.Models.Server;
using Serein.Core.Services.Server;
using Serein.Core.Utils;

namespace Serein.Core.Models.Plugins.Js;

public partial class ScriptInstance
{
    public class ServerModule
    {
        private readonly CancellationToken _token;
        private readonly ServerManager _serverManager;

        public ServerModule(ServerManager serverManager, CancellationToken token)
        {
            _token = token;
            _serverManager = serverManager;
        }

        public IServerInfo? ServerInfo => _serverManager.ServerInfo;
        public ServerStatus ServerStatus => _serverManager.Status;
        public int? Pid => _serverManager.Pid;

        public void Start()
        {
            _token.ThrowIfCancellationRequested();
            _serverManager.Start();
        }

        public void Stop()
        {
            _token.ThrowIfCancellationRequested();
            _serverManager.Stop();
        }

        public void Terminate()
        {
            _token.ThrowIfCancellationRequested();
            _serverManager.Terminate();
        }

        public void Input(string command, EncodingMap.EncodingType? encodingType = null)
        {
            _token.ThrowIfCancellationRequested();
            _serverManager.Input(command, encodingType);
        }

        public bool TryStart()
        {
            try
            {
                _token.ThrowIfCancellationRequested();
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
                _token.ThrowIfCancellationRequested();
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
                _token.ThrowIfCancellationRequested();
                _serverManager.Terminate();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
