namespace Serein.ConnectionProtocols.Models.Satori.V1.Signals;

public enum Opcode
{
    Event = 0,

    Ping = 1,

    Pong = 2,

    Identify = 3,

    Ready = 4,

    Meta = 5,
}
