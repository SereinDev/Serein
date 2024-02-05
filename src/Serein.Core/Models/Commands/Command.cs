using System;

namespace Serein.Core.Models.Commands;

public class Command : ICloneable
{
    public CommandOrigin Origin { get; init; }

    public CommandType Type { get; init; }

    public string Argument { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public object Clone()
    {
        return MemberwiseClone();
    }
}
