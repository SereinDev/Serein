using Jint;
using System.Collections.Generic;
using Serein.JSPlugin;

namespace Serein.Items
{
    internal class Plugin
    {
        /// <summary>
        /// 被成功加载
        /// </summary>
        public bool LoadedSuccessfully = false;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// 介绍
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 文件
        /// </summary>
        public string File { get; set; } = string.Empty;

        /// <summary>
        /// JS引擎
        /// </summary>
        public Engine Engine { get; set; } = null;

        /// <summary>
        /// WebSocket列表
        /// </summary>
        public List<JSWebSocket> WebSockets { get; set; } = new List<JSWebSocket>();

        /// <summary>
        /// 事件对象
        /// </summary>
        public Event Event { get; set; } = new Event();
    }
}
