namespace Serein.ConnectionProtocols.Models.Satori.V1.Logins;

public class Login
{
    public long Sn { get; init; }

    public string Platform { get; init; } = string.Empty;

    public User User { get; init; } = new();

    public LoginStatus Status { get; init; }

    public string Adapter { get; init; } = string.Empty;

    public string[]? Features { get; init; }
}
