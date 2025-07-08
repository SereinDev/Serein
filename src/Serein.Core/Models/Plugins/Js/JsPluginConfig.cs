namespace Serein.Core.Models.Plugins.Js;

public sealed class JsPluginConfig
{
    public static readonly JsPluginConfig Default = new();

    public string[] NetAssemblies { get; init; } = [];

    public bool AllowOperatorOverloading { get; init; } = true;

    public bool AllowWrite { get; init; } = true;

    public bool Strict { get; init; }

    public bool UseJintJsonSerializer { get; init; } = false;
}
