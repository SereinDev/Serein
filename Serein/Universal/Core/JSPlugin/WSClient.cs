using Jint;
using Jint.Native;
using Newtonsoft.Json;
using Serein.Utils;
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
            _WebSocket.Opened += (_, _) => Trigger(onopen, "Opened");
            _WebSocket.Closed += (_, _) => Trigger(onclose, "Closed");
            _WebSocket.MessageReceived += (_, e) => Trigger(onmessage, "MessageReceived", e);
            _WebSocket.Error += (_, e) => Trigger(onerror, "Opened", e);
            JSPluginManager.PluginDict[@namespace].WSClients.Add(this);
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="jsValue">事件</param>
        /// <param name="name">名称</param>
        /// <param name="args">参数</param>
        private void Trigger(JsValue jsValue, string name, object args = null)
        {
            if (JSPluginManager.PluginDict[Namespace].Engine == null)
            {
                Dispose();
                return;
            }
            if (jsValue == null || jsValue.IsUndefined() || jsValue.IsNull())
            {
                return;
            }
            try
            {
                lock (JSPluginManager.PluginDict[Namespace].Engine)
                {
                    switch (name)
                    {
                        case "Opened":
                        case "Closed":
                            JSPluginManager.PluginDict[Namespace].Engine.Invoke(jsValue);
                            break;
                        case "MessageReceived":
                            if (args is MessageReceivedEventArgs e1 && e1 != null)
                            {
                                JSPluginManager.PluginDict[Namespace].Engine.Invoke(jsValue, e1.Message);
                            }
                            break;
                        case "Error":
                            if (args is SuperSocket.ClientEngine.ErrorEventArgs e2 && e2 != null)
                            {
                                JSPluginManager.PluginDict[Namespace].Engine.Invoke(jsValue, e2.Exception.ToString());
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                string message = e.GetFullMsg();
                Logger.Output(Base.LogType.Plugin_Error, $"[{Namespace}]", $"WSClientt的{name}事件调用失败：", message);
                Logger.Output(Base.LogType.Debug, $"{name}事件调用失败\r\n", e);
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
    }
}
