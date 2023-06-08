﻿using Jint;
using Jint.Native;
using Jint.Native.Function;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serein.Base;
using Serein.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Serein.Core.JSPlugin
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class Plugin : IDisposable
    {
        public Plugin(string @namespace, PreLoadConfig config)
        {
            Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
            Name = @namespace;
            PreLoadConfig = config;
            Engine = JSEngine.Create(false, Namespace, _tokenSource, PreLoadConfig);
        }

        public void Dispose()
        {
            if (!_tokenSource.IsCancellationRequested)
            {
                _tokenSource.Cancel();
            }
            _eventDict.Clear();
            Engine?.Dispose();
            Engine = null;
            WSClients.ForEach((ws) => ws.Dispose());
            Available = false;
        }

        /// <summary>
        /// CancellationToken
        /// </summary>
        [JsonIgnore]
        private readonly CancellationTokenSource _tokenSource = new();

        /// <summary>
        /// 命名空间
        /// </summary>
        public readonly string Namespace;

        /// <summary>
        /// 可用
        /// </summary>
        public bool Available;

        /// <summary>
        /// 显示名称
        /// </summary>
        [JsonIgnore]
        public string DisplayedName => Name == null ? "-" : Name;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示版本
        /// </summary>
        [JsonIgnore]
        public string DisplayedVersion => Version == null ? "-" : Version;

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 显示作者
        /// </summary>
        [JsonIgnore]
        public string DisplayedAuthor => Author == null ? "-" : Author;

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 显示介绍
        /// </summary>
        [JsonIgnore]
        public string DisplayedDescription => Description == null ? "-" : Description;

        /// <summary>
        /// 介绍
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string File { get; init; } = string.Empty;

        /// <summary>
        /// JS引擎
        /// </summary>
        [JsonIgnore]
        public Engine Engine { get; private set; }

        /// <summary>
        /// WebSocket列表
        /// </summary>
        public readonly List<WSClient> WSClients = new();

        /// <summary>
        /// 监听字典
        /// </summary>
        [JsonIgnore]
        private readonly Dictionary<EventType, JsValue> _eventDict = new();

        /// <summary>
        /// 预加载配置
        /// </summary>
        public readonly PreLoadConfig PreLoadConfig;

        /// <summary>
        /// 事件列表
        /// </summary>
        public EventType[] EventList => _eventDict.Keys.ToArray();

        /// <summary>
        /// 是否监听了指定的事件类型
        /// </summary>
        /// <param name="eventType">事件类型</param>
        public bool HasListenedOn(EventType eventType)
            => _eventDict.TryGetValue(eventType, out JsValue jsValue) && jsValue is FunctionInstance;

        /// <summary>
        /// 设置事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="jsValue">执行函数</param>
        /// <returns>设置结果</returns>
        public bool SetListener(EventType type, JsValue jsValue)
        {
            Logger.Output(LogType.Debug, type);
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
                case EventType.PluginsLoaded:
                    if (_eventDict.ContainsKey(type))
                    {
                        _eventDict[type] = jsValue;
                    }
                    else
                    {
                        _eventDict.Add(type, jsValue);
                    }
                    return true;
                default:
                    Logger.Output(LogType.Plugin_Warn, $"{Namespace}添加了了一个不支持的事件：", type);
                    return false;
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="args">参数</param>
        public bool Trigger(EventType type, CancellationToken token, params object[] args)
        {
            if (!Available || Engine is null || !HasListenedOn(type))
            {
                return false;
            }
            Logger.Output(LogType.Debug, $"{nameof(Namespace)}:", Namespace, $"{nameof(type)}:", type, $"{nameof(args)} Count:", args.Length);
            try
            {
                switch (type)
                {
                    case EventType.ServerStart:
                    case EventType.SereinClose:
                    case EventType.PluginsLoaded:
                    case EventType.PluginsReload:
                        lock (Engine)
                        {
                            Engine.Invoke(_eventDict[type]);
                        }
                        break;
                    case EventType.ServerStop:
                    case EventType.ServerSendCommand:
                        lock (Engine)
                        {
                            Engine.Invoke(_eventDict[type], args[0]);
                        }
                        break;
                    case EventType.ServerOutput:
                    case EventType.ServerOriginalOutput:
                    case EventType.ReceivePacket:
                        lock (Engine)
                        {
                            return !token.IsCancellationRequested && IsFalse(Engine.Invoke(_eventDict[type], args[0]));
                        }
                    case EventType.GroupIncrease:
                    case EventType.GroupDecrease:
                    case EventType.GroupPoke:
                        lock (Engine)
                        {
                            Engine.Invoke(_eventDict[type], args[0], args[1]);
                        }
                        break;
                    case EventType.ReceiveGroupMessage:
                        lock (Engine)
                        {
                            return !token.IsCancellationRequested && IsFalse(Engine.Invoke(_eventDict[type], args[0], args[1], args[2], args[3], args[4]));
                        }
                    case EventType.ReceivePrivateMessage:
                        lock (Engine)
                        {
                            return !token.IsCancellationRequested && IsFalse(Engine.Invoke(_eventDict[type], args[0], args[1], args[2], args[3]));
                        }
                    default:
                        throw new NotSupportedException($"触发了一个不支持的事件：{type}");
                }
            }
            catch (Exception e)
            {
                string message = e.ToFullMsg();
                Logger.Output(LogType.Plugin_Error, $"[{Namespace}]", $"触发事件{type}时出现异常：", message);
                Logger.Output(LogType.Debug, $"{Namespace}触发事件{type}时出现异常：\n", message);
            }
            return false;
        }

        /// <summary>
        /// 是否为false
        /// </summary>
        private static bool IsFalse(JsValue jsValue)
            => jsValue.IsBoolean() && !jsValue.AsBoolean();
    }
}
