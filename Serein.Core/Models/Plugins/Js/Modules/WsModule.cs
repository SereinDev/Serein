using System;

using Serein.Core.Services.Networks;
using Serein.Core.Utils.Extensions;

namespace Serein.Core.Models.Plugins.Js.Modules;

public class WsModule
{
    private readonly WsNetwork _wsNetwork;

    public WsModule(WsNetwork wsNetwork)
    {
        _wsNetwork = wsNetwork;
    }

    public bool Active => _wsNetwork.Active;

    public void Start()
    {
        _wsNetwork.Start();
    }

    public void Stop()
    {
        _wsNetwork.Stop();
    }

    public void SendText(string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        _wsNetwork.SendTextAsync(text).Await();
    }

    public void SendGroupMsg(string target, string message)
    {
        if (target is null)
            throw new ArgumentNullException(nameof(target));

        if (message is null)
            throw new ArgumentNullException(nameof(message));

        _wsNetwork.SendGroupMsgAsync(target, message).Await();
    }

    public void SendPrivateMsg(string target, string message)
    {
        if (target is null)
            throw new ArgumentNullException(nameof(target));

        if (message is null)
            throw new ArgumentNullException(nameof(message));

        _wsNetwork.SendPrivateMsgAsync(target, message).Await();
    }
}
