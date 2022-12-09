using Jint.Native;
using Jint.Runtime;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Serein.JSPlugin
{
    internal class Event : IDisposable
    {
        public string Namespace = string.Empty;

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


        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="Type">事件类型</param>
        /// <param name="Function">执行函数</param>
        /// <returns>添加结果</returns>
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
                    Logger.Out(LogType.Plugin_Error, $"{JSPluginManager.PluginDict[Namespace]}添加了了一个不支持的事件：", Type);
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
            Logger.Out(LogType.Debug, Type);
            if (!JSPluginManager.PluginDict.ContainsKey(Namespace))
            {
                Dispose();
                return;
            }
            try
            {
                lock (JSPluginManager.PluginDict[Namespace].Engine)
                    switch (Type)
                    {
                        case EventType.ServerStart:
                            ServerStart.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined }));
                            break;
                        case EventType.ServerStop:
                            ServerStop.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]) }));
                            break;
                        case EventType.ServerOutput:
                            ServerOutput.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]) }));
                            break;
                        case EventType.ServerOriginalOutput:
                            ServerOriginalOutput.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]) }));
                            break;
                        case EventType.ServerSendCommand:
                            ServerSendCommand.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]) }));
                            break;
                        case EventType.GroupIncrease:
                            GroupIncrease.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]) }));
                            break;
                        case EventType.GroupDecrease:
                            GroupDecrease.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]) }));
                            break;
                        case EventType.GroupPoke:
                            GroupPoke.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]) }));
                            break;
                        case EventType.ReceiveGroupMessage:
                            ReceiveGroupMessage.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]), JsValue.FromObject(JSEngine.Converter, Args[2]), JsValue.FromObject(JSEngine.Converter, Args[3]) }));
                            break;
                        case EventType.ReceivePrivateMessage:
                            ReceivePrivateMessage.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]), JsValue.FromObject(JSEngine.Converter, Args[1]), JsValue.FromObject(JSEngine.Converter, Args[2]) }));
                            break;
                        case EventType.ReceivePacket:
                            ReceivePacket.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]) }));
                            break;
                        case EventType.SereinStart:
                            SereinStart.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined }));
                            break;
                        case EventType.SereinClose:
                            SereinClose.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined }));
                            break;
                        case EventType.PluginsReload:
                            PluginsReload.ForEach((x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined }));
                            break;
                        default:
                            Logger.Out(LogType.Plugin_Error, $"{Namespace}运行了了一个不支持的事件：", Type);
                            break;
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
        [JsonIgnore]
        public List<Delegate> ServerStart = new List<Delegate>();

        [JsonProperty(PropertyName = "ServerStartCount")]
        public int ServerStartCount => ServerStart.Count;

        [JsonIgnore]
        public List<Delegate> ServerStop = new List<Delegate>();

        [JsonProperty(PropertyName = "ServerStop")]
        public int ServerStopCount => ServerStop.Count;

        [JsonIgnore]
        public List<Delegate> ServerSendCommand = new List<Delegate>();

        [JsonProperty(PropertyName = "ServerSendCommand")]
        public int ServerSendCommandCount => ServerSendCommand.Count;

        [JsonIgnore]
        public List<Delegate> ServerOutput = new List<Delegate>();

        [JsonProperty(PropertyName = "ServerOutput")]
        public int ServerOutputCount => ServerOutput.Count;

        [JsonIgnore]
        public List<Delegate> ServerOriginalOutput = new List<Delegate>();

        [JsonProperty(PropertyName = "ServerOriginalOutput")]
        public int ServerOriginalOutputCount => ServerOriginalOutput.Count;

        [JsonIgnore]
        public List<Delegate> GroupIncrease = new List<Delegate>();

        [JsonProperty(PropertyName = "GroupIncrease")]
        public int GroupIncreaseCount => GroupIncrease.Count;

        [JsonIgnore]
        public List<Delegate> GroupDecrease = new List<Delegate>();

        [JsonProperty(PropertyName = "GroupDecrease")]
        public int GroupDecreaseCount => GroupDecrease.Count;

        [JsonIgnore]
        public List<Delegate> GroupPoke = new List<Delegate>();

        [JsonProperty(PropertyName = "GroupPoke")]
        public int GroupPokeCount => GroupPoke.Count;

        [JsonIgnore]
        public List<Delegate> ReceiveGroupMessage = new List<Delegate>();

        [JsonProperty(PropertyName = "ReceiveGroupMessage")]
        public int ReceiveGroupMessageCount => ReceiveGroupMessage.Count;

        [JsonIgnore]
        public List<Delegate> ReceivePrivateMessage = new List<Delegate>();

        [JsonProperty(PropertyName = "ReceivePrivateMessage")]
        public int ReceivePrivateMessageCount => ReceivePrivateMessage.Count;

        [JsonIgnore]
        public List<Delegate> ReceivePacket = new List<Delegate>();

        [JsonProperty(PropertyName = "ReceivePacket")]
        public int ReceivePacketCount => ReceivePacket.Count;

        [JsonIgnore]
        public List<Delegate> SereinStart = new List<Delegate>();

        [JsonProperty(PropertyName = "SereinStart")]
        public int SereinStartCount => SereinStart.Count;

        [JsonIgnore]
        public List<Delegate> SereinClose = new List<Delegate>();

        [JsonProperty(PropertyName = "SereinClose")]
        public int SereinCloseCount => SereinClose.Count;

        [JsonIgnore]
        public List<Delegate> PluginsReload = new List<Delegate>();

        [JsonProperty(PropertyName = "PluginsReload")]
        public int PluginsReloadCount => PluginsReload.Count;

        #endregion
    }
}
