namespace Serein.Core.Models.Commands;

public enum CommandType
{
    Invalid,

    ExecuteShellCommand,

    InputServer,

    SendGroupMsg,

    SendPrivateMsg,

    SendText,

    Bind,

    Unbind,

    ExecuteJavascriptCodes,

    Debug,

    Reload
}
