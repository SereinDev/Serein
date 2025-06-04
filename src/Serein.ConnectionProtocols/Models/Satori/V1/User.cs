namespace Serein.ConnectionProtocols.Models.Satori.V1;

public class User
{
    public string Id { get; init; } = string.Empty;

    public string? Name { get; init; }

    public string? Nick { get; init; }

    public string? Avatar { get; init; }

    public bool? IsBot { get; init; }
}
