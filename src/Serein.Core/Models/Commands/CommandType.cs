namespace Serein.Core.Models.Commands;

public enum CommandType
{
    Invalid,

    ExecuteShellCommand,

    ServerInput,

    SendGroupMsg,

    SendPrivateMsg,

    SendText,

    Bind,

    Unbind,

    ExecuteJavascriptCodes,

    Debug,

    Reload
}
