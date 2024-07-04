using System.Text.Json.Serialization;

namespace Serein.Core.Models.Plugins.Info;

[JsonConverter(typeof(JsonStringEnumConverter<PluginTag>))]
public enum PluginTag
{
    Entertainment,

    Development,

    Tool,

    Information,

    Management,

    Api
}
