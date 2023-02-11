using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Serein.Extensions
{
    internal static partial class Extensions
    {
        /// <summary>
        /// 尝试获取JSON值
        /// </summary>
        /// <param name="jtoken">JToken</param>
        /// <param name="keys">键组</param>
        /// <returns>对应值的字符串形式。若为<see>null</see>则返回<see>string.Empty</see>。</returns>
        public static string TryGetString(this JToken jtoken, params string[] keys)
        {
            foreach (string key in keys)
            {
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }
                else if (jtoken == null)
                {
                    break;
                }
                jtoken = jtoken[key];
            }
            return (jtoken ?? string.Empty).ToString();
        }

        /// <summary>
        /// 序列化为JSON文本
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="formatting">格式化参数</param>
        /// <returns>序列化后的JSON文本</returns>
        public static string ToJson(this object obj, Formatting formatting)
            => obj != null ? JsonConvert.SerializeObject(obj, formatting) : string.Empty;

        /// <summary>
        /// 序列化为JSON文本
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>序列化后的JSON文本</returns>
        public static string ToJson(this object obj)
            => obj != null ? JsonConvert.SerializeObject(obj) : string.Empty;
    }
}