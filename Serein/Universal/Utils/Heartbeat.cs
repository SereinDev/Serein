using System.Timers;

namespace Serein.Utils
{
    internal static class Heartbeat
    {
        /// <summary>
        /// 在线统计计时器
        /// </summary>
        private static readonly Timer HeartbeatTimer = new(20000) { AutoReset = true };

        /// <summary>
        /// 开始心跳事件
        /// </summary>
        public static void Start()
        {
            HeartbeatTimer.Elapsed += (_, _) => _ = Net.Get("http://count.ongsat.com/api/online/heartbeat?uri=127469ef347447698dd74c449881b877").GetAwaiter().GetResult();
            HeartbeatTimer.Start();
        }

    }
}