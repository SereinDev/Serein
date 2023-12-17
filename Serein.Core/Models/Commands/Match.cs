namespace Serein.Core.Models.Commands;

public class Match
{
    public string? RegExp { get; set; }

    public FieldType FieldType { get; set; }

    public bool RequireAdmin { get; set; }

    public string? Restrictions { get; set; }

    public string? Command { get; set; }

    public string? Description { get; set; }
}
