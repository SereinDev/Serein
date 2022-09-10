using Jint;
using Jint.Native;
using System;
using WebSocket4Net;

namespace Serein.Plugin
{
    internal class JSWebSocket
    {
        public static Delegate onopen = null, onclose = null, onerror = null, onmessage = null;
        private WebSocket webSocket = null;
        private Engine engine = new Engine();
        public int state => webSocket != null ? -1 : ((int)webSocket.State);

        /// <summary>
        /// 入口函数
        /// </summary>
        /// <param name="Uri">ws地址</param>
        /// <param name="Token">鉴权Token</param>
        public JSWebSocket(string Uri)
        {
            webSocket = new WebSocket(
                Uri,
                "",
                null,
                null
                );
            webSocket.Opened += (sender, e) =>
            {
                if (onopen != null)
                {
                    onopen.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
                }
            };
            webSocket.Closed += (sender, e) =>
            {
                if (onclose != null)
                {
                    onclose.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
                }
            };
            webSocket.MessageReceived += (sender, e) =>
            {
                if (onmessage != null)
                {
                    try
                    {
                        onmessage.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(engine, e.Message) });
                    }
                    catch { }
                }
            };
            webSocket.Error += (sender, e) =>
            {
                if (onerror != null)
                {
                    onerror.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(engine, e.Exception.Message) });
                }
            };
            webSocket.Open();
        }

        /// <summary>
        /// 关闭ws
        /// </summary>
        public void close()
        {
            webSocket.Close();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="Msg"></param>
        public void send(string Msg)
        {
            webSocket.Send(Msg);
        }
    }
}
