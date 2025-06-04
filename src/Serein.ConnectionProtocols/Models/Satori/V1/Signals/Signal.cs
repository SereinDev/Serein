namespace Serein.ConnectionProtocols.Models.Satori.V1.Signals;

public class Signal<T>
{
    public Opcode Op { get; init; }

    public T? Body { get; init; }
}
