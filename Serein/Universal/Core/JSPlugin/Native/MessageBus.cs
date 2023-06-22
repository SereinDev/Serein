using Jint.Native;
using Jint.Native.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serein.Core.JSPlugin.Native
{
    internal class MessageBus : PluginBase, IDisposable
    {
        /// <summary>
        /// 消息总线列表
        /// </summary>
        internal static readonly List<MessageBus> MessageBuses = new();

        /// <summary>
        /// 释放所有消息总线
        /// </summary>
        internal static void DisposeAll()
        {
            MessageBuses.ForEach((bus) => bus.Dispose());
            MessageBuses.Clear();
        }

        private readonly Dictionary<string, JsValue> _dictionary = new();

        public JsValue? Onerror;

        public MessageBus(string? @namespace) : base(@namespace)
        {
            MessageBuses.Add(this);
        }

        /// <summary>
        /// 是否存活
        /// </summary>
        public bool Alive { get; private set; } = true;

        public void Dispose()
        {
            Alive = false;
            Onerror = null;
            _dictionary.Clear();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="msg">消息主体</param>
        public void PostMessage(string channel, object msg)
        {
            if (Alive && !string.IsNullOrEmpty(channel))
            {
                MessageBuses
                    .Where((bus) => bus._namespace != _namespace && bus.Alive)
                    .ToList()
                    .ForEach((bus) => Task.Run(() => bus.Receive(channel, msg)));
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="msg">消息主体</param>
        internal void Receive(string channel, object msg)
        {
            if (Alive &&
                _dictionary.TryGetValue(channel, out JsValue? jsValue) &&
                JSPluginManager.PluginDict[_namespace].Engine is not null &&
                JSPluginManager.PluginDict[_namespace].Available)
            {
                try
                {
                    lock (JSPluginManager.PluginDict[_namespace].Engine!)
                    {
                        JSPluginManager.PluginDict[_namespace].Engine!.Invoke(jsValue, msg);
                    }
                }
                catch (Exception e)
                {
                    if (Onerror is FunctionInstance)
                    {
                        lock (JSPluginManager.PluginDict[_namespace].Engine!)
                        {
                            JSPluginManager.PluginDict[_namespace].Engine!.Invoke(Onerror, e.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置监听器
        /// </summary>
        /// <param name="categories">子类</param>
        /// <param name="callback">回调函数</param>
        /// <returns>设置结果</returns>
        public bool SetListener(string categories, JsValue callback)
        {
            if (string.IsNullOrEmpty(categories) || callback is not FunctionInstance)
            {
                return false;
            }
            if (_dictionary.ContainsKey(categories))
            {
                _dictionary[categories] = callback;
            }
            else
            {
                _dictionary.Add(categories, callback);
            }
            return true;
        }
    }
}