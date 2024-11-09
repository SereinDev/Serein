namespace Serein.Core.Models.Plugins.Js;

public class JsPluginConfig
{
    public static readonly JsPluginConfig Default = new();

    public string[] NetAssemblies { get; init; } = [];

    public bool AllowGetType { get; init; }

    public bool AllowOperatorOverloading { get; init; } = true;

    public bool AllowSystemReflection { get; init; }

    public bool AllowWrite { get; init; } = true;

    public bool AllowStringCompilation { get; init; } = true;

    public bool Strict { get; init; }
}
