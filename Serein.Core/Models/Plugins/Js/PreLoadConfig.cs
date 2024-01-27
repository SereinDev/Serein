using System;

namespace Serein.Core.Models.Plugins.Js;

/// <remarks>
/// https://github.com/sebastienros/jint/blob/main/Jint/Options.cs
/// </remarks>
public class PreloadConfig
{
    public string? Name { get; init; }
    public string[] CSharpAssemblies { get; init; } = Array.Empty<string>();

    public bool AllowGetType { get; init; }
    public bool AllowOperatorOverloading { get; init; } = true;
    public bool AllowSystemReflection { get; init; }
    public bool AllowWrite { get; init; } = true;
    public bool Strict { get; init; }
    public bool StringCompilationAllowed { get; init; } = true;
}
