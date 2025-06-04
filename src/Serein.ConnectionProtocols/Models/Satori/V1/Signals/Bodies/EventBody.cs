using Serein.ConnectionProtocols.Models.Satori.V1.Channels;
using Serein.ConnectionProtocols.Models.Satori.V1.Logins;

namespace Serein.ConnectionProtocols.Models.Satori.V1.Signals.Bodies;

public class EventBody
{
    public long Sn { get; init; }

    public string Type { get; init; } = string.Empty;

    public double Timestamp { get; init; }

    public Login Login { get; init; } = new();

    public Channel? Channel { get; init; }

    public Message? Message { get; init; }

    public User? User { get; init; }
}
