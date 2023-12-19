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

    RequestMotdpe,

    RequestMotdje,

    ExecuteJavascriptCodes,

    Debug,

    Reload
}