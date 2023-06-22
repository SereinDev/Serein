using System.Text.RegularExpressions;

namespace Serein.Extensions
{
    internal static partial class Extensions
    {
        /// <summary>
        /// 测试正则
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <returns>是否合法</returns>
        public static bool TestRegex(this string pattern)
        {
            if (!string.IsNullOrEmpty(pattern))
            {
                try
                {
                    _ = new Regex(pattern);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 尝试转换正则
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <param name="options">正则表达式选项</param>
        /// <param name="regex">转换后的正则对象</param>
        /// <returns>转换结果</returns>
        public static bool TryParse(this string pattern, RegexOptions options, out Regex? regex)
        {
            regex = null;
            try
            {
                regex = new Regex(pattern, options);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
