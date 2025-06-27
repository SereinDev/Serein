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

    ChannelMessageReceived,
    GroupMessageReceived,
    PrivateMessageReceived,
    ConnectionDataReceived,
    PacketReceived,

    HttpRequestReceived,

    SereinClosed,
    SereinCrashed,

    PluginsLoaded,
    PluginsUnloading,
}
