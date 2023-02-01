using Jint;
using Jint.Native;
using Newtonsoft.Json;
using System.Linq;
using Serein.Base;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Serein.JSPlugin
{
    internal class Plugin : IDisposable
    {
        public Plugin(string @namespace, PreLoadConfig config)
        {
            Namespace = @namespace ?? throw new ArgumentOutOfRangeException();
            PreLoadConfig = config;
            List<Assembly> assemblies = new();
            if (PreLoadConfig != null)
            {
                foreach (string assembly in config.Load)
                {
                    try
                    {
                        assemblies.Add(Assembly.Load(assembly));
                    }
                    catch (Exception e)
                    {
                        Logger.Output(LogType.Plugin_Error, $"加载程序集“{assembly}”时出现异常：", e.Message);
                    }
                }
                foreach (string assemblyPath in config.LoadFrom)
                {
                    try
                    {
                        assemblies.Add(Assembly.LoadFrom(Path.Combine(Global.PATH, assemblyPath)));
                    }
                    catch (Exception e)
                    {
                        Logger.Output(LogType.Plugin_Error, $"从“{Path.Combine(Global.PATH, assemblyPath)}”加载程序集时出现异常：", e.Message);
                    }
                }
            }
            Engine = JSEngine.Init(false, Namespace, TokenSource, assemblies.ToArray());
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
        [JsonIgnore]
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
        /// 预加载配置
        /// </summary>
        public readonly PreLoadConfig PreLoadConfig;

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
            Logger.Output(LogType.Debug, type);
            @delegate = @delegate ?? throw new ArgumentNullException(nameof(@delegate));
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
                    Logger.Output(LogType.Plugin_Warn, $"{Namespace}添加了了一个不支持的事件：", type);
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="args">参数</param>
        public void Trigger(EventType type, params object[] args)
        {
            if (!JSPluginManager.PluginDict.ContainsKey(Namespace) || !EventDict.ContainsKey(type) || EventDict[type] == null)
            {
                return;
            }
            Logger.Output(LogType.Debug, $"{nameof(Namespace)}:", Namespace, $"{nameof(type)}:", type, $"{nameof(args)} Count:", args.Length);
            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    switch (type)
                    {
                        case EventType.ServerStart:
                        case EventType.SereinClose:
                        case EventType.PluginsReload:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                            {
                                EventDict[type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
                            }
                            break;
                        case EventType.ServerStop:
                        case EventType.ServerOutput:
                        case EventType.ServerOriginalOutput:
                        case EventType.ServerSendCommand:
                        case EventType.ReceivePacket:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                            {
                                EventDict[type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, args[0]) });
                            }
                            break;
                        case EventType.GroupIncrease:
                        case EventType.GroupDecrease:
                        case EventType.GroupPoke:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                            {
                                EventDict[type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, args[0]), JsValue.FromObject(JSEngine.Converter, args[1]) });
                            }
                            break;
                        case EventType.ReceiveGroupMessage:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                            {
                                EventDict[type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, args[0]), JsValue.FromObject(JSEngine.Converter, args[1]), JsValue.FromObject(JSEngine.Converter, args[2]), JsValue.FromObject(JSEngine.Converter, args[3]) });
                            }
                            break;
                        case EventType.ReceivePrivateMessage:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                            {
                                EventDict[type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, args[0]), JsValue.FromObject(JSEngine.Converter, args[1]), JsValue.FromObject(JSEngine.Converter, args[2]) });
                            }
                            break;
                        default:
                            Logger.Output(LogType.Plugin_Error, $"{Namespace}运行了了一个不支持的事件：", type);
                            break;
                    }
                }
                catch (Exception e)
                {
                    string message = e.GetFullMsg();
                    Logger.Output(LogType.Plugin_Error, $"[{Namespace}]", $"触发事件{type}时出现异常：", message);
                    Logger.Output(LogType.Debug, $"{Namespace}触发事件{type}时出现异常：\n", message);
                }
            });
        }
    }
}
