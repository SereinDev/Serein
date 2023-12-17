namespace Serein.Core.Models.Commands;

public enum CommandType
{
    Invalid,

    ExecuteShellCmd,

    ServerInput,

    ServerInputWithUnicode,

    SendGroupMsg,

    SendGivenGroupMsg,

    SendPrivateMsg,

    SendGivenPrivateMsg,

    SendTempMsg,

    Bind,

    Unbind,

    RequestMotdpe,

    RequestMotdje,

    ExecuteJavascriptCodes,

    ExecuteJavascriptCodesWithNamespace,

    Debug,

    Reload
}
