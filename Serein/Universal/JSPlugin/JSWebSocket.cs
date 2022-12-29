using Jint.Native;
using Jint.Runtime;
using Newtonsoft.Json;
using Serein.Base;
using System;
using WebSocket4Net;

namespace Serein.JSPlugin
{
    internal class JSWebSocket : IDisposable
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        private readonly string Namespace;

        /// <summary>
        /// 事件函数
        /// </summary>
        public static Delegate onopen, onclose, onerror, onmessage;

        /// <summary>
        /// WS客户端
        /// </summary>
        [JsonIgnore]
        private readonly WebSocket _WebSocket;

        public void Dispose()
        {
            if (_WebSocket != null && _WebSocket.State == WebSocketState.Open)
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
        /// <param name="Uri">ws地址</param>
        /// <param name="Namespace">命名空间</param>
        public JSWebSocket(string Uri, string Namespace = null)
        {
            this.Namespace = Namespace ?? throw new ArgumentNullException("无法找到对应的命名空间", nameof(Namespace));
            _WebSocket = new WebSocket(
                Uri,
                "",
                null,
                null
                );
            _WebSocket.Opened += (sender, e) => Trigger(onopen, "Opened");
            _WebSocket.Closed += (sender, e) => Trigger(onclose, "Closed"); ;
            _WebSocket.MessageReceived += (sender, e) => Trigger(onmessage, "MessageReceived", e);
            _WebSocket.Error += (sender, e) => Trigger(onerror, "Opened", e);
            JSPluginManager.PluginDict[Namespace].WebSockets.Add(this);
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="Event">事件</param>
        /// <param name="Name">名称</param>
        /// <param name="Args">参数</param>
        private void Trigger(Delegate Event, string Name, object Args = null)
        {
            if (JSPluginManager.PluginDict[Namespace].Engine == null)
            {
                Dispose();
                return;
            }
            try
            {
                lock (JSPluginManager.PluginDict[Namespace].Engine)
                    switch (Name)
                    {
                        case "Opened":
                        case "Closed":
                            Event?.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
                            break;
                        case "MessageReceived":
                            if (Args is MessageReceivedEventArgs e1 && e1 != null)
                            {
                                Event?.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, e1.Message) });
                            }
                            break;
                        case "Error":
                            if (Args is SuperSocket.ClientEngine.ErrorEventArgs e2 && e2 != null)
                            {
                                Event?.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, e2.Exception.Message) });
                            }
                            break;
                    }
            }
            catch (Exception e)
            {
                string Message;
                if (e.InnerException is JavaScriptException JSe)
                {
                    Message = $"{JSe.Message}\n{JSe.JavaScriptStackTrace}"; ;
                }
                else
                {
                    Message = (e.InnerException ?? e).Message;
                }
                Logger.Out(Items.LogType.Plugin_Error, $"[{Namespace}]", $"Websocket的{Name}事件调用失败：", Message);
                Logger.Out(Items.LogType.Debug, $"{Name}事件调用失败\r\n", e);
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
        /// <param name="Msg"></param>
        public void send(string Msg) => _WebSocket.Send(Msg);

#pragma warning restore IDE1006
    }
}
