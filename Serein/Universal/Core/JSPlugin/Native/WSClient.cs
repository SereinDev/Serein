using Jint.Native;
using Jint.Native.Function;
using Newtonsoft.Json;
using SuperSocket.ClientEngine;
using System;
using WebSocket4Net;

namespace Serein.Core.JSPlugin
{
    internal class WSClient : IDisposable
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        private readonly string Namespace;

        /// <summary>
        /// 事件函数
        /// </summary>
        public static JsValue onopen, onclose, onerror, onmessage;

        /// <summary>
        /// WS客户端
        /// </summary>
        [JsonIgnore]
        private readonly WebSocket _WebSocket;

        public void Dispose()
        {
            if (_WebSocket == null)
            {
                return;
            }
            if (_WebSocket.State == WebSocketState.Open)
            {
                _WebSocket.Close();
            }
            if (!Disposed)
            {
                _WebSocket?.Dispose();
                Disposed = true;
            }
        }

        /// <summary>
        /// ws地址
        /// </summary>
        public string Uri { get; private set; }

        public bool Disposed { get; private set; }

        /// <summary>
        /// 入口函数
        /// </summary>
        /// <param name="uri">ws地址</param>
        /// <param name="namespace">命名空间</param>
        public WSClient(string uri, string @namespace = null)
        {
            Uri = uri;
            Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
            if (!JSPluginManager.PluginDict.ContainsKey(Namespace))
            {
                throw new ArgumentException("无法找到对应的命名空间", nameof(@namespace));
            }
            _WebSocket = new(uri);
            _WebSocket.Opened += (_, _) => Trigger(onopen, EventType.Opened);
            _WebSocket.Closed += (_, _) => Trigger(onclose, EventType.Closed);
            _WebSocket.MessageReceived += (_, e) => Trigger(onmessage, EventType.MessageReceived, e);
            _WebSocket.Error += (_, e) => Trigger(onerror, EventType.Error, e);
            JSPluginManager.PluginDict[@namespace].WSClients.Add(this);
        }

        /// <summary>
        /// 判断事件是否有效
        /// </summary>
        /// <param name="jsValue">事件函数</param>
        private bool Check(JsValue jsValue)
        {
            if (JSPluginManager.PluginDict[Namespace].Engine == null || !JSPluginManager.PluginDict[Namespace].Available)
            {
                Dispose();
                return false;
            }
            return jsValue is FunctionInstance;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="jsValue">事件</param>
        /// <param name="eventType">名称</param>
        /// <param name="args">参数</param>
        private void Trigger(JsValue jsValue, EventType eventType, object args = null)
        {
            if (!Check(jsValue))
            {
                return;
            }
            try
            {
                lock (JSPluginManager.PluginDict[Namespace].Engine)
                {
                    switch (eventType)
                    {
                        case EventType.Opened:
                        case EventType.Closed:
                            JSPluginManager.PluginDict[Namespace].Engine.Invoke(jsValue);
                            break;
                        case EventType.MessageReceived:
                            if (args is MessageReceivedEventArgs e1 && e1 != null)
                            {
                                JSPluginManager.PluginDict[Namespace].Engine.Invoke(jsValue, e1.Message);
                            }
                            break;
                        case EventType.Error:
                            if (args is ErrorEventArgs e2 && e2 != null)
                            {
                                JSPluginManager.PluginDict[Namespace].Engine.Invoke(jsValue, e2.Exception.ToString());
                            }
                            break;
                        default:
                            throw new NotSupportedException("未知的事件类型");
                    }
                }
            }
            catch (Exception e)
            {
                string message = e.ToFullMsg();
                Utils.Logger.Output(Base.LogType.Plugin_Error, $"[{Namespace}]", $"WSClientt的{eventType}事件调用失败：", message);
                Utils.Logger.Output(Base.LogType.Debug, $"{eventType}事件调用失败\r\n", e);
            }
        }

#pragma warning disable IDE1006
        /// <summary>
        /// 释放对象
        /// </summary>
        public void dispose() => Dispose();

        /// <summary>
        /// 状态
        /// </summary>
        public int state => _WebSocket == null ? -1 : ((int)_WebSocket.State);

        /// <summary>
        /// 开启ws
        /// </summary>
        public void open() => _WebSocket.Open();

        /// <summary>
        /// 关闭ws
        /// </summary>
        public void close() => _WebSocket.Close();

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void send(string msg) => _WebSocket.Send(msg);

#pragma warning restore IDE1006

        private enum EventType
        {
            Opened,
            Closed,
            MessageReceived,
            Error
        }

        internal struct ReadonlyWSClient
        {
            public bool disposed;

            public int state;

            public ReadonlyWSClient(WSClient wsclient)
            {
                disposed = wsclient.Disposed;
                state = wsclient.state;
            }
        }
    }
}
