using System;

namespace Serein.Cli.Models;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandDescriptionAttribute(string[] lines) : Attribute
{
    public string[] Lines { get; } = lines;
}
