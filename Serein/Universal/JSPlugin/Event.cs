using Jint.Native;
using Jint.Runtime;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Serein.JSPlugin
{
    internal class Event : IDisposable
    {
        public string Key = string.Empty;

        public void Dispose()
        {
            ServerStart.Clear();
            ServerStop.Clear();
            ServerSendCommand.Clear();
            ServerOutput.Clear();
            ServerOriginalOutput.Clear();
            GroupIncrease.Clear();
            GroupDecrease.Clear();
            GroupPoke.Clear();
            ReceiveGroupMessage.Clear();
            ReceivePrivateMessage.Clear();
            ReceivePacket.Clear();
            SereinStart.Clear();
            SereinClose.Clear();
            PluginsReload.Clear();
        }

        public bool Add(EventType Type, Delegate Function)
        {
            Logger.Out(LogType.Debug, Type);
            switch (Type)
            {
                case EventType.ServerStart:
                    ServerStart.Add(Function);
                    break;
                case EventType.ServerStop:
                    ServerStop.Add(Function);
                    break;
                case EventType.ServerOutput:
                    ServerOutput.Add(Function);
                    break;
                case EventType.ServerOriginalOutput:
                    ServerOriginalOutput.Add(Function);
                    break;
                case EventType.ServerSendCommand:
                    ServerSendCommand.Add(Function);
                    break;
                case EventType.GroupIncrease:
                    GroupIncrease.Add(Function);
                    break;
                case EventType.GroupDecrease:
                    GroupDecrease.Add(Function);
                    break;
                case EventType.GroupPoke:
                    GroupPoke.Add(Function);
                    break;
                case EventType.ReceiveGroupMessage:
                    ReceiveGroupMessage.Add(Function);
                    break;
                case EventType.ReceivePrivateMessage:
                    ReceivePrivateMessage.Add(Function);
                    break;
                case EventType.ReceivePacket:
                    ReceivePacket.Add(Function);
                    break;
                case EventType.SereinStart:
                    SereinStart.Add(Function);
                    break;
                case EventType.SereinClose:
                    SereinClose.Add(Function);
                    break;
                case EventType.PluginsReload:
                    PluginsReload.Add(Function);
                    break;
                default:
                    Logger.Out(LogType.Plugin_Error, $"{Key}添加了了一个不支持的事件：", Type);
                    return false;
            }
            return true;
        }

        public void Trigger(EventType Type, params object[] Args)
        {
            Logger.Out(LogType.Debug, Type);
            if (!JSPluginManager.PluginDict.ContainsKey(Key))
            {
                Dispose();
                return;
            }
            try
            {
                lock (JSPluginManager.PluginDict[Key].Engine)
                {
                    switch (Type)
                    {
                        case EventType.ServerStart:
                            ServerStart.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                                );
                            break;
                        case EventType.ServerStop:
                            ServerStop.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0])})
                                );
                            break;
                        case EventType.ServerOutput:
                            ServerOutput.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0])})
                                );
                            break;
                        case EventType.ServerOriginalOutput:
                            ServerOriginalOutput.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0])})
                                );
                            break;
                        case EventType.ServerSendCommand:
                            ServerSendCommand.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0])})
                                );
                            break;
                        case EventType.GroupIncrease:
                            GroupIncrease.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0]),
                                JsValue.FromObject(JSEngine.Converter, Args[1]) })
                                );
                            break;
                        case EventType.GroupDecrease:
                            GroupDecrease.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0]),
                                JsValue.FromObject(JSEngine.Converter, Args[1])})
                                );
                            break;
                        case EventType.GroupPoke:
                            GroupPoke.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0]),
                                JsValue.FromObject(JSEngine.Converter, Args[1])})
                                );
                            break;
                        case EventType.ReceiveGroupMessage:
                            ReceiveGroupMessage.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0]),
                                JsValue.FromObject(JSEngine.Converter, Args[1]),
                                JsValue.FromObject(JSEngine.Converter, Args[2]),
                                JsValue.FromObject(JSEngine.Converter, Args[3])})
                                );
                            break;
                        case EventType.ReceivePrivateMessage:
                            ReceivePrivateMessage.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0]),
                                JsValue.FromObject(JSEngine.Converter, Args[1]),
                                JsValue.FromObject(JSEngine.Converter, Args[2])})
                                );
                            break;
                        case EventType.ReceivePacket:
                            ReceivePacket.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0])})
                                );
                            break;
                        case EventType.SereinStart:
                            SereinStart.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                                );
                            break;
                        case EventType.SereinClose:
                            SereinClose.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                                );
                            break;
                        case EventType.PluginsReload:
                            PluginsReload.ForEach(
                                (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                                );
                            break;
                        default:
                            Logger.Out(LogType.Plugin_Error, $"{Key}运行了了一个不支持的事件：", Type);
                            break;
                    }

                }
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is JavaScriptException JSException)
                    Logger.Out(LogType.Plugin_Error, $"触发事件{Type}时出现异常：" +
                        $"{JSException.Message} (at line {JSException.Location.Start.Line}:{JSException.Location.Start.Column})");
                else
                    Logger.Out(LogType.Debug, $"触发事件{Type}时出现异常：\n", e);
            }
            catch (Exception e)
            {
                Logger.Out(LogType.Debug, $"触发事件{Type}时出现异常：\n", e);
            }
        }

        #region 事件成员
        public List<Delegate> ServerStart { get; set; } = new List<Delegate>();
        public List<Delegate> ServerStop { get; set; } = new List<Delegate>();
        public List<Delegate> ServerSendCommand { get; set; } = new List<Delegate>();
        public List<Delegate> ServerOutput { get; set; } = new List<Delegate>();
        public List<Delegate> ServerOriginalOutput { get; set; } = new List<Delegate>();
        public List<Delegate> GroupIncrease { get; set; } = new List<Delegate>();
        public List<Delegate> GroupDecrease { get; set; } = new List<Delegate>();
        public List<Delegate> GroupPoke { get; set; } = new List<Delegate>();
        public List<Delegate> ReceiveGroupMessage { get; set; } = new List<Delegate>();
        public List<Delegate> ReceivePrivateMessage { get; set; } = new List<Delegate>();
        public List<Delegate> ReceivePacket { get; set; } = new List<Delegate>();
        public List<Delegate> SereinStart { get; set; } = new List<Delegate>();
        public List<Delegate> SereinClose { get; set; } = new List<Delegate>();
        public List<Delegate> PluginsReload { get; set; } = new List<Delegate>();
        #endregion
    }
}
