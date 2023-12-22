using System;

namespace Serein.Cli.Interaction;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandDescriptionAttribute : Attribute
{
    public CommandDescriptionAttribute(string rootCommnad, string description)
    {
        RootCommand = rootCommnad;
        Description = description;
    }

    public string RootCommand { get; }

    public string Description { get; }

    public int Priority { get; init; }
}
