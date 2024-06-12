using System;

namespace Serein.Cli.Interaction;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CommandUsageAttribute(string example, string description) : Attribute
{
    public string Example { get; } = example;

    public string Description { get; } = description;
}
