using System;
using System.Threading;

using Serein.Core.Services.Networks;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Models.Plugins.Js;

public partial class ScriptInstance
{
    public class WsModule
    {
        private readonly WsNetwork _wsNetwork;
        private readonly CancellationToken _token;

        public WsModule(WsNetwork wsNetwork, CancellationToken token)
        {
            _wsNetwork = wsNetwork;
            _token = token;
        }

        public bool Active => _wsNetwork.Active;

        public void Start()
        {
            _token.ThrowIfCancellationRequested();
            _wsNetwork.StartAsync().Await();
        }

        public void SendText(string text)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            _token.ThrowIfCancellationRequested();
            _wsNetwork.SendTextAsync(text).Await();
        }

        public void SendGroupMsg(string target, string message)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));

            if (message is null)
                throw new ArgumentNullException(nameof(message));

            _token.ThrowIfCancellationRequested();
            _wsNetwork.SendGroupMsgAsync(target, message).Await();
        }

        public void SendPrivateMsg(string target, string message)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));

            if (message is null)
                throw new ArgumentNullException(nameof(message));

            _token.ThrowIfCancellationRequested();
            _wsNetwork.SendPrivateMsgAsync(target, message).Await();
        }
    }
}
