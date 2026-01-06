using System;

namespace Serein.Console.Models;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CommandNameAttribute(string rootCommnad, string name) : Attribute
{
    public string RootCommand { get; } = rootCommnad;

    public string Name { get; } = name;
}
