using System.Text.Json.Serialization;

namespace Serein.Core.Models.Plugins.Info;

[JsonConverter(typeof(JsonStringEnumConverter<PluginType>))]
public enum PluginType
{
    Unknown,

    Js,

    Net,
}
