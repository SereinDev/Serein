using Jint;
using Newtonsoft.Json;
using Serein.JSPlugin;
using System.Collections.Generic;

namespace Serein.Items
{
    internal class Plugin
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace;

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
        /// 文件名
        /// </summary>
        public string File = string.Empty;

        /// <summary>
        /// JS引擎
        /// </summary>
        [JsonIgnore]
        public Engine Engine;

        /// <summary>
        /// WebSocket列表
        /// </summary>
        public List<JSWebSocket> WebSockets = new List<JSWebSocket>();

        /// <summary>
        /// 事件对象
        /// </summary>
        public Event Event = new Event();

        /// <summary>
        /// 计时器ID列表
        /// </summary>
        public List<long> TimerIDs = new List<long>();
    }
}
