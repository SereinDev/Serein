namespace Serein.Core.Models.Plugins;

public enum Event
{
    ServerStarting,
    ServerStarted,
    ServerStopping,
    ServerExited,
    ServerOutput,
    ServerRawOutput,
    ServerInput,

    GroupMessageReceived,
    PrivateMessageReceived,
    WsDataReceived,
    PacketReceived,

    SereinClosed,
    SereinCrashed,

    PluginsLoaded,
    PluginsUnloading,
}
