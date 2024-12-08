using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Serein.Core.Utils.Json;

public static class JsonSerializerOptionsFactory
{
    /// <summary>
    /// 驼峰命名；允许尾随逗号；跳过注释
    /// </summary>
    public static readonly JsonSerializerOptions Common =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

    /// <summary>
    /// 蛇形命名；允许尾随逗号；跳过注释；允许从字符串读取数字
    /// </summary>
    public static readonly JsonSerializerOptions PacketStyle =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };
}
