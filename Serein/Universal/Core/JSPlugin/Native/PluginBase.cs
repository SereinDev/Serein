using System;

namespace Serein.Core.JSPlugin.Native
{
    internal abstract class PluginBase
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        protected string _namespace { get; init; }

        protected PluginBase(string? @namespace)
        {
            _namespace = @namespace ?? throw new ArgumentNullException();
            if (string.IsNullOrEmpty(_namespace) && !JSPluginManager.PluginDict.ContainsKey(_namespace))
            {
                throw new ArgumentException("无法找到对应的命名空间");
            }
        }
    }
}