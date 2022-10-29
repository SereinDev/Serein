using System;

namespace Serein.Plugin
{
    internal class CommandItem
    {
        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// 触发执行函数
        /// </summary>
        public Delegate Function { get; set; }
    }
}
