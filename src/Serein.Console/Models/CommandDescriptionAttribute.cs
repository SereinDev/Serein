using System;

namespace Serein.Console.Models;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandDescriptionAttribute(string[] lines) : Attribute
{
    public string[] Lines { get; } = lines;
}
