namespace Serein.Items
{
    internal enum EventType
    {
        ServerStart,
        ServerStop,
        ServerExitUnexpectedly,
        ServerOutput,
        ServerOriginalOutput,
        ServerSendCommand,

        GroupIncrease,
        GroupDecrease,
        GroupPoke,
        ReceiveGroupMessage,
        ReceivePrivateMessage,
        ReceivePacket,
        PermissionDeniedFromPrivateMsg,
        PermissionDeniedFromGroupMsg,

        SereinStart,
        SereinClose,
        SereinCrash,

        PluginsReload,

        BindingSucceed,
        BindingFailDueToOccupation,
        BindingFailDueToInvalid,
        BindingFailDueToAlreadyBinded,
        UnbindingSucceed,
        UnbindingFail,

        RequestingMotdpeSucceed,
        RequestingMotdjeSucceed,
        RequestingMotdFail
    }
}
