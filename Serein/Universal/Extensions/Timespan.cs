using System;

namespace Serein.Extensions
{
    internal static class Timespan
    {
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