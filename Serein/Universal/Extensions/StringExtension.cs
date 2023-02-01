using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Serein.Extensions
{
    internal static class StringExtension
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

        public static string ToJson(this object obj, Formatting formatting)
            => obj != null ? JsonConvert.SerializeObject(obj, formatting) : string.Empty;

        public static string ToJson(this object obj)
            => obj != null ? JsonConvert.SerializeObject(obj) : string.Empty;

        public static bool TestRegex(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    _ = new Regex(text);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public static bool TryConvert(this string text, RegexOptions options, out Regex regex)
        {
            regex = null;
            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    regex = new Regex(text, options);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}