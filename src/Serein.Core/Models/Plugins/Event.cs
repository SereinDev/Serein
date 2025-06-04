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
    ConnectionDataReceived,
    PacketReceived,

    SereinClosed,
    SereinCrashed,

    PluginsLoaded,
    PluginsUnloading,
}
