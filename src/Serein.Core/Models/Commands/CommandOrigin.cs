namespace Serein.Core.Models.Commands;

public enum CommandOrigin
{
    Null,

    Msg,

    ServerOutput,

    ServerInput,

    Schedule,

    Reaction,

    Plugin,

    ConsoleExecute
}
