namespace Serein.Core.Models.Commands;

public enum CommandType
{
    Invalid,

    ExecuteShellCommand,

    InputServer,

    SendGroupMsg,

    SendPrivateMsg,

    SendData,

    Bind,

    Unbind,

    ExecuteJavascriptCodes,

    Debug
}
