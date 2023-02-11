using System;

namespace Serein.Extensions
{
    internal static partial class Extensions
    {
        /// <summary>
        /// 转化为时间文本
        /// </summary>
        /// <param name="timeSpan">时间间隔</param>
        /// <returns>时间文本</returns>
        public static string ToCustomString(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalSeconds < 3600)
            {
                return $"{timeSpan.TotalSeconds / 60:N1}m";
            }
            else if (timeSpan.TotalHours < 120)
            {
                return $"{timeSpan.TotalMinutes / 60:N1}h";
            }
            else
            {
                return $"{timeSpan.TotalHours / 24:N1}d";
            }
        }
    }
}