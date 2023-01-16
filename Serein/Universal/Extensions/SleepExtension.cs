using System.Threading.Tasks;

namespace Serein.Extensions
{
    internal static class SleepExtension
    {
        /// <summary>
        /// 睡觉觉
        /// </summary>
        /// <param name="ms">时长</param>
        public static void ToSleepFor(this int ms)
        {
            Task.Delay(ms).GetAwaiter().GetResult();
        }
    }
}