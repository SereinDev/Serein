namespace Serein.Core.Models.Commands;

public enum CommandType
{
    Invalid,

    ExecuteShellCommand,

    InputServer,

    SendGroupMsg,

    SendPrivateMsg,

    SendChannelMsg,

    SendGuildMsg,

    SendReply,

    SendData,

    Bind,

    Unbind,

    ExecuteJavascriptCodes,

    Debug,
}
