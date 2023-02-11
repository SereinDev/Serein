using System.Threading.Tasks;

namespace Serein.Extensions
{
    internal static partial class Extensions
    {
        /// <summary>
        /// 睡觉觉
        /// </summary>
        /// <param name="ms">时长</param>
        public static void ToSleep(this int ms)
        {
            Task.Delay(ms).GetAwaiter().GetResult();
        }
    }
}