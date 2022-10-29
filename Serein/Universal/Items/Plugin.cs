using Jint;

namespace Serein.Items
{
    internal class Plugin
    {
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
        /// JS引擎
        /// </summary>
        public Engine Engine { get; set; } = null;
    }
}
