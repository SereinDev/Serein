namespace Serein.Core.Models.Commands;

public enum CommandOrigin
{
    Msg,

    ServerOutput,

    ServerInput,

    Schedule,

    EventTrigger,

    Javascript,

    ConsoleExecute
}
