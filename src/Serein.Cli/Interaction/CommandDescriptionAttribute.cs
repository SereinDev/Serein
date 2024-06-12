using System;

namespace Serein.Cli.Interaction;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandDescriptionAttribute(string rootCommnad, string description) : Attribute
{
    public string RootCommand { get; } = rootCommnad;

    public string Description { get; } = description;

    public int Priority { get; init; }
}
