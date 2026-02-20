using Serein.ConnectionProtocols.Models;
using Serein.Core.Models.Abstractions;

namespace Serein.Core.Models.Commands;

public class CommandArguments : NotifyPropertyChangedModelBase
{
    public CommandArguments() { }

    public CommandArguments(CommandArguments commandArguments)
    {
        Target = commandArguments.Target;
        AutoEscape = commandArguments.AutoEscape;
        AsSegments = commandArguments.AsSegments;
        UseUnicode = commandArguments.UseUnicode;
        Self = commandArguments.Self is null ? null : new(commandArguments.Self);
    }

    public string? Target { get; set; }

    public bool? AutoEscape { get; set; }

    public bool? AsSegments { get; set; }

    public Self? Self { get; set; }

    public bool? UseUnicode { get; set; }
}
