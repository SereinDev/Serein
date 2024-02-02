using System;

namespace Serein.Cli.Interaction;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CommandUsageAttribute : Attribute
{
    public CommandUsageAttribute(string example, string description)
    {
        Example = example;
        Description = description;
    }

    public string Example { get; }

    public string Description { get; }
}
