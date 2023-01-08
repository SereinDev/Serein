using Jint;
using Jint.Native;
using Jint.Runtime;
using Newtonsoft.Json;
using System.Linq;
using Serein.Base;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Serein.JSPlugin
{
    internal class Plugin : IDisposable
    {
        public Plugin(string @namespace = null)
        {
            Namespace = @namespace ?? throw new ArgumentOutOfRangeException();
            Engine = JSEngine.Init(false, Namespace, TokenSource);
            Name = @namespace;
        }

        public void Dispose()
        {
            if (!TokenSource.IsCancellationRequested)
            {
                TokenSource.Cancel();
            }
            EventDict.Clear();
            Engine?.Dispose();
            Engine = null;
            WebSockets.ForEach((ws) => ws.Dispose());
            TokenSource.Dispose();
            Available = false;
        }

        /// <summary>
        /// CancellationToken
        /// </summary>
        private readonly CancellationTokenSource TokenSource = new();

        /// <summary>
        /// 命名空间
        /// </summary>
        public readonly string Namespace;

        /// <summary>
        /// 可用
        /// </summary>
        public bool Available;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "-";

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "-";

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; } = "-";

        /// <summary>
        /// 介绍
        /// </summary>
        public string Description { get; set; } = "-";

        /// <summary>
        /// 文件名
        /// </summary>
        public string File = string.Empty;

        /// <summary>
        /// JS引擎
        /// </summary>
        [JsonIgnore]
        public Engine Engine;

        /// <summary>
        /// WebSocket列表
        /// </summary>
        public List<JSWebSocket> WebSockets = new();

        /// <summary>
        /// 监听字典
        /// </summary>
        [JsonIgnore]
        private readonly Dictionary<EventType, Delegate> EventDict = new();

        /// <summary>
        /// 事件列表
        /// </summary>
        public EventType[] EventList => EventDict.Keys.ToArray();

        /// <summary>
        /// 设置事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="delegate">执行函数</param>
        /// <returns>设置结果</returns>
        public bool SetListener(EventType type, Delegate @delegate)
        {
            Logger.Out(LogType.Debug, type);
            switch (type)
            {
                case EventType.ServerStart:
                case EventType.ServerStop:
                case EventType.ServerOutput:
                case EventType.ServerOriginalOutput:
                case EventType.ServerSendCommand:
                case EventType.GroupIncrease:
                case EventType.GroupDecrease:
                case EventType.GroupPoke:
                case EventType.ReceiveGroupMessage:
                case EventType.ReceivePrivateMessage:
                case EventType.ReceivePacket:
                case EventType.SereinClose:
                case EventType.PluginsReload:
                    if (EventDict.ContainsKey(type))
                    {
                        EventDict[type] = @delegate;
                    }
                    else
                    {
                        EventDict.Add(type, @delegate);
                    }
                    break;
                default:
                    Logger.Out(LogType.Plugin_Warn, $"{Namespace}添加了了一个不支持的事件：", type);
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="Type">事件类型</param>
        /// <param name="Args">参数</param>
        public void Trigger(EventType Type, params object[] Args)
        {
            if (!JSPluginManager.PluginDict.ContainsKey(Namespace) || !EventDict.ContainsKey(Type) || EventDict[Type] == null)
            {
                return;
            }
            Logger.Out(LogType.Debug, $"{nameof(Namespace)}:", Namespace, $"{nameof(Type)}:", Type, $"{nameof(Args)} Count:", Args.Length);
            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    switch (Type)
                    {
                        case EventType.ServerStart:
                        case EventType.SereinClose:
                        case EventType.PluginsReload:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                            {
                                EventDict[Type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
                            }
                            break;
                        case EventType.ServerStop:
                        case EventType.ServerOutput:
                        case EventType.ServerOriginalOutput:
                        case EventType.ServerSendCommand:
                        case EventType.ReceivePacket:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                            {
                                EventDict[Type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]) });
                            }
                            break;
                        case EventType.GroupIncrease:
                        case EventType.GroupDecrease:
                        case EventType.GroupPoke:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                            {
                                EventDict[Type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]) });
                            }
                            break;
                        case EventType.ReceiveGroupMessage:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                            {
                                EventDict[Type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]), JsValue.FromObject(JSEngine.Converter, Args[2]), JsValue.FromObject(JSEngine.Converter, Args[3]) });
                            }
                            break;
                        case EventType.ReceivePrivateMessage:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                            {
                                EventDict[Type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]), JsValue.FromObject(JSEngine.Converter, Args[2]) });
                            }
                            break;
                        default:
                            Logger.Out(LogType.Plugin_Error, $"{Namespace}运行了了一个不支持的事件：", Type);
                            break;
                    }
                }
                catch (TargetInvocationException e)
                {
                    if (e.InnerException is JavaScriptException javaScriptException)
                    {
                        Logger.Out(LogType.Plugin_Error, $"[{Namespace}]", $"触发事件{Type}时出现异常： {javaScriptException.Message}\n{javaScriptException.JavaScriptStackTrace}");
                    }
                    else
                    {
                        Logger.Out(LogType.Plugin_Error, $"[{Namespace}]", $"触发事件{Type}时出现异常：", (e.InnerException ?? e).Message);
                    }
                }
                catch (Exception e)
                {
                    Logger.Out(LogType.Debug, $"{Namespace}触发事件{Type}时出现异常：\n", e);
                }
            });
        }
    }
}
