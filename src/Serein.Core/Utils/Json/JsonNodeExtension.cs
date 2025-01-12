using System.Text.Json;
using System.Text.Json.Nodes;

namespace Serein.Core.Utils.Json;

public static class JsonNodeExtension
{
    /// <summary>
    /// 转换为对象
    /// </summary>
    public static T? ToObject<T>(this JsonNode jsonNode, JsonSerializerOptions? options = null)
        where T : notnull
    {
        return JsonSerializer.Deserialize<T>(
            jsonNode,
            options ?? JsonSerializerOptionsFactory.Common
        );
    }
}
