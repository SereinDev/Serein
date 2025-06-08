using Serein.ConnectionProtocols.Models;

namespace Serein.Core.Models.Commands;

public record CommandArguments
{
    public string? Target { get; set; }

    public bool? AutoEscape { get; set; }

    public bool? AsSegments { get; set; }

    public Self? Self { get; set; }

    public bool? UseUnicode { get; set; }
}
