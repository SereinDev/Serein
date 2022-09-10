using Jint;
using Jint.Native;
using System;
using WebSocket4Net;

namespace Serein.Plugin
{
    internal class JSWebSocket
    {
        public static Delegate onopen = null, onclose = null, onerror = null, onmessage = null;
        private WebSocket _WebSocket = null;
        private Engine _Engine = new Engine();
        public int state => _WebSocket != null ? -1 : ((int)_WebSocket.State);

        /// <summary>
        /// 入口函数
        /// </summary>
        /// <param name="Uri">ws地址</param>
        /// <param name="Token">鉴权Token</param>
        public JSWebSocket(string Uri)
        {
            _WebSocket = new WebSocket(
                Uri,
                "",
                null,
                null
                );
            _WebSocket.Opened += (sender, e) =>
            {
                if (onopen != null)
                {
                    onopen.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
                }
            };
            _WebSocket.Closed += (sender, e) =>
            {
                if (onclose != null)
                {
                    onclose.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
                }
            };
            _WebSocket.MessageReceived += (sender, e) =>
            {
                if (onmessage != null)
                {
                    try
                    {
                        onmessage.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(_Engine, e.Message) });
                    }
                    catch { }
                }
            };
            _WebSocket.Error += (sender, e) =>
            {
                if (onerror != null)
                {
                    onerror.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(_Engine, e.Exception.Message) });
                }
            };
        }

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
    }
}
