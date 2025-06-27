using System;

namespace Serein.Cli.Models;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class SubCommandAttribute(string command, string description) : Attribute
{
    public string Command { get; } = command;
    public string Description { get; } = description;
}
