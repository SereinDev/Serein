using Newtonsoft.Json;

namespace Serein.Extensions
{
    internal static partial class Extensions
    {
        /// <summary>
        /// 序列化为JSON文本
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="formatting">格式化参数</param>
        /// <returns>序列化后的JSON文本</returns>
        public static string ToJson(this object obj, Formatting formatting) =>
            obj != null ? JsonConvert.SerializeObject(obj, formatting) : string.Empty;

        /// <summary>
        /// 序列化为JSON文本
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>序列化后的JSON文本</returns>
        public static string ToJson(this object obj) =>
            obj?.ToJson(Formatting.None) ?? string.Empty;
    }
}
