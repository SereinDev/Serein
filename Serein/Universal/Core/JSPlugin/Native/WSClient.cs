using Jint.Native;
using Jint.Native.Function;
using Newtonsoft.Json;
using SuperSocket.ClientEngine;
using System;
using WebSocket4Net;

namespace Serein.Core.JSPlugin.Native
{
    internal class WSClient : PluginBase, IDisposable
    {
        /// <summary>
        /// 事件函数
        /// </summary>
        public static JsValue Onopen, Onclose, Onerror, Onmessage;

        /// <summary>
        /// WS客户端
        /// </summary>
        [JsonIgnore]
        private readonly WebSocket _webSocket;

        public void Dispose()
        {
            if (_webSocket == null)
            {
                return;
            }
            if (_webSocket.State == WebSocketState.Open)
            {
                _webSocket.Close();
            }
            if (!Disposed)
            {
                _webSocket?.Dispose();
                Disposed = true;
            }
        }

        /// <summary>
        /// ws地址
        /// </summary>
        public string Uri { get; init; }

        public bool Disposed { get; private set; }

        /// <summary>
        /// 入口函数
        /// </summary>
        /// <param name="uri">ws地址</param>
        /// <param name="namespace">命名空间</param>
        public WSClient(string uri, string @namespace = null) : base(@namespace)
        {
            Uri = uri;
            _webSocket = new(uri);
            _webSocket.Opened += (_, _) => Trigger(Onopen, EventType.Opened);
            _webSocket.Closed += (_, _) => Trigger(Onclose, EventType.Closed);
            _webSocket.MessageReceived += (_, e) => Trigger(Onmessage, EventType.MessageReceived, e);
            _webSocket.Error += (_, e) => Trigger(Onerror, EventType.Error, e);

            JSPluginManager.PluginDict[@namespace].WSClients.Add(this);
        }

        /// <summary>
        /// 判断事件是否有效
        /// </summary>
        /// <param name="jsValue">事件函数</param>
        private bool Check(JsValue jsValue)
        {
            if (JSPluginManager.PluginDict[_namespace].Engine is null || !JSPluginManager.PluginDict[_namespace].Available)
            {
                Dispose();
                return false;
            }
            return jsValue is FunctionInstance && !Disposed;
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
                lock (JSPluginManager.PluginDict[_namespace].Engine)
                {
                    switch (eventType)
                    {
                        case EventType.Opened:
                        case EventType.Closed:
                            JSPluginManager.PluginDict[_namespace].Engine.Invoke(jsValue);
                            break;
                        case EventType.MessageReceived:
                            if (args is MessageReceivedEventArgs e1 && e1 != null)
                            {
                                JSPluginManager.PluginDict[_namespace].Engine.Invoke(jsValue, e1.Message);
                            }
                            break;
                        case EventType.Error:
                            if (args is ErrorEventArgs e2 && e2 != null)
                            {
                                JSPluginManager.PluginDict[_namespace].Engine.Invoke(jsValue, e2.Exception.ToFullMsg());
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
                Utils.Logger.Output(Base.LogType.Plugin_Error, $"[{_namespace}]", $"WSClientt的{eventType}事件调用失败：", message);
                Utils.Logger.Output(Base.LogType.Debug, $"{eventType}事件调用失败\r\n", e);
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int State => _webSocket == null ? -1 : ((int)_webSocket.State);

        /// <summary>
        /// 开启ws
        /// </summary>
        public void Open() => _webSocket.Open();

        /// <summary>
        /// 关闭ws
        /// </summary>
        public void Close() => _webSocket.Close();

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void Send(string msg) => _webSocket.Send(msg);

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

            public string uri;

            public ReadonlyWSClient(WSClient wsclient)
            {
                disposed = wsclient.Disposed;
                state = wsclient.State;
                uri = wsclient.Uri;
            }
        }
    }
}
