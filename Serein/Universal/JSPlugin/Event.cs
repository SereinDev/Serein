using Jint.Native;
using Jint.Runtime;
using Serein.Base;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Serein.JSPlugin
{
    internal class Event : IDisposable
    {
        public void Dispose()
            => Dict.Clear();

        public string Namespace = string.Empty;

        /// <summary>
        /// 设置事件
        /// </summary>
        /// <param name="Type">事件类型</param>
        /// <param name="Function">执行函数</param>
        /// <returns>设置结果</returns>
        public bool Set(EventType Type, Delegate Function)
        {
            Logger.Out(LogType.Debug, Type);
            switch (Type)
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
                case EventType.SereinStart:
                case EventType.SereinClose:
                case EventType.PluginsReload:
                    if (Dict.ContainsKey(Type))
                    {
                        Dict[Type] = Function;
                    }
                    else
                        Dict.Add(Type, Function);
                    break;
                default:
                    Logger.Out(LogType.Plugin_Error, $"{Namespace}添加了了一个不支持的事件：", Type);
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
            if (!JSPluginManager.PluginDict.ContainsKey(Namespace) || !Dict.ContainsKey(Type) || Dict[Type] == null)
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
                        case EventType.SereinStart:
                        case EventType.SereinClose:
                        case EventType.PluginsReload:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                                Dict[Type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
                            break;
                        case EventType.ServerStop:
                        case EventType.ServerOutput:
                        case EventType.ServerOriginalOutput:
                        case EventType.ServerSendCommand:
                        case EventType.ReceivePacket:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                                Dict[Type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]) });
                            break;
                        case EventType.GroupIncrease:
                        case EventType.GroupDecrease:
                        case EventType.GroupPoke:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                                Dict[Type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]) });
                            break;
                        case EventType.ReceiveGroupMessage:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                                Dict[Type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]), JsValue.FromObject(JSEngine.Converter, Args[2]), JsValue.FromObject(JSEngine.Converter, Args[3]) });
                            break;
                        case EventType.ReceivePrivateMessage:
                            lock (JSPluginManager.PluginDict[Namespace].Engine)
                                Dict[Type].DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]), JsValue.FromObject(JSEngine.Converter, Args[2]) });
                            break;
                        default:
                            Logger.Out(LogType.Plugin_Error, $"{Namespace}运行了了一个不支持的事件：", Type);
                            break;
                    }
                }
                catch (TargetInvocationException e)
                {
                    if (e.InnerException is JavaScriptException JSException)
                    {
                        Logger.Out(LogType.Plugin_Error, $"{Namespace}触发事件{Type}时出现异常：" + $"{JSException.Message} (at line {JSException.Location.Start.Line}:{JSException.Location.Start.Column})");
                    }
                    else
                    {
                        Logger.Out(LogType.Debug, $"{Namespace}触发事件{Type}时出现异常：\n", e);
                    }
                }
                catch (Exception e)
                {
                    Logger.Out(LogType.Debug, $"{Namespace}触发事件{Type}时出现异常：\n", e);
                }
            });
        }

        /// <summary>
        /// 监听字典
        /// </summary>
        private readonly Dictionary<EventType, Delegate> Dict = new Dictionary<EventType, Delegate>();
    }
}
