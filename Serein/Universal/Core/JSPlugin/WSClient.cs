using Jint;
using Jint.Native;
using Newtonsoft.Json;
using Serein.Utils;
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

        private bool Disposed;

        /// <summary>
        /// 入口函数
        /// </summary>
        /// <param name="uri">ws地址</param>
        /// <param name="namespace">命名空间</param>
        public WSClient(string uri, string @namespace = null)
        {
            Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
            if (!JSPluginManager.PluginDict.ContainsKey(Namespace))
            {
                throw new ArgumentException("无法找到对应的命名空间");
            }
            _WebSocket = new WebSocket(
                uri,
                "",
                null,
                null
                );
            _WebSocket.Opened += (_, _) => Trigger(onopen, WSEventType.Opened);
            _WebSocket.Closed += (_, _) => Trigger(onclose, WSEventType.Closed);
            _WebSocket.MessageReceived += (_, e) => Trigger(onmessage, WSEventType.MessageReceived, e);
            _WebSocket.Error += (_, e) => Trigger(onerror, WSEventType.Error, e);
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
            if (jsValue == null || jsValue.IsUndefined() || jsValue.IsNull())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="jsValue">事件</param>
        /// <param name="eventType">名称</param>
        /// <param name="args">参数</param>
        private void Trigger(JsValue jsValue, WSEventType eventType, object args = null)
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
                        case WSEventType.Opened:
                        case WSEventType.Closed:
                            JSPluginManager.PluginDict[Namespace].Engine.Invoke(jsValue);
                            break;
                        case WSEventType.MessageReceived:
                            if (args is MessageReceivedEventArgs e1 && e1 != null)
                            {
                                JSPluginManager.PluginDict[Namespace].Engine.Invoke(jsValue, e1.Message);
                            }
                            break;
                        case WSEventType.Error:
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
                string message = e.GetFullMsg();
                Logger.Output(Base.LogType.Plugin_Error, $"[{Namespace}]", $"WSClientt的{eventType}事件调用失败：", message);
                Logger.Output(Base.LogType.Debug, $"{eventType}事件调用失败\r\n", e);
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

        private enum WSEventType
        {
            Opened,
            Closed,
            MessageReceived,
            Error
        }
    }
}
