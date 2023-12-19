namespace Serein.Core.Models.Commands;

public enum CommandOrigin
{
    Null,

    Msg,

    ServerOutput,

    ServerInput,

    Schedule,

    EventTrigger,

    Plugin,

    ConsoleExecute
}