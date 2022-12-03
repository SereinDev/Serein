﻿using Jint.Native;
using Jint.Runtime;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Timers;

namespace Serein.Plugin
{
    partial class JSFunc
    {
        public static long ID = 0;

        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="Version">版本</param>
        /// <param name="Author">作者</param>
        /// <param name="Description">介绍</param>
        /// <returns>注册结果</returns>
        public static bool Register(
            string Name,
            string Version,
            string Author,
            string Description
            )
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            Plugins.PluginItems.Add(new Items.Plugin()
            {
                Name = Name,
                Version = Version,
                Author = Author,
                Description = Description
            });
            return true;
        }

        /// <summary>
        /// 注册事件监听器
        /// </summary>
        /// <param name="EventName">事件名称</param>
        /// <param name="Function">函数</param>
        /// <returns>注册结果</returns>
        public static bool SetListener(string EventName, Delegate Function)
        {
            Logger.Out(LogType.Debug, EventName);
            if (string.IsNullOrEmpty(EventName) || string.IsNullOrWhiteSpace(EventName))
                return false;
            switch (EventName)
            {
                case "onServerStart":
                    Plugins.Event.onServerStart.Add(Function);
                    break;
                case "onServerStop":
                    Plugins.Event.onServerStop.Add(Function);
                    break;
                case "onServerOutput":
                    Plugins.Event.onServerOutput.Add(Function);
                    break;
                case "onServerOriginalOutput":
                    Plugins.Event.onServerOriginalOutput.Add(Function);
                    break;
                case "onServerSendCommand":
                    Plugins.Event.onServerSendCommand.Add(Function);
                    break;
                case "onGroupIncrease":
                    Plugins.Event.onGroupIncrease.Add(Function);
                    break;
                case "onGroupDecrease":
                    Plugins.Event.onGroupDecrease.Add(Function);
                    break;
                case "onGroupPoke":
                    Plugins.Event.onGroupPoke.Add(Function);
                    break;
                case "onReceiveGroupMessage":
                    Plugins.Event.onReceiveGroupMessage.Add(Function);
                    break;
                case "onReceivePrivateMessage":
                    Plugins.Event.onReceivePrivateMessage.Add(Function);
                    break;
                case "onReceivePacket":
                    Plugins.Event.onReceivePacket.Add(Function);
                    break;
                case "onSereinStart":
                    Plugins.Event.onSereinStart.Add(Function);
                    break;
                case "onSereinClose":
                    Plugins.Event.onSereinClose.Add(Function);
                    break;
                case "onPluginsReload":
                    Plugins.Event.onPluginsReload.Add(Function);
                    break;
                default:
                    Logger.Out(LogType.Plugin_Error, $"插件注册了一个未知事件：{EventName}");
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 触发插件事件
        /// </summary>
        /// <param name="EventName">事件名称</param>
        /// <param name="Args">参数</param>
        public static void Trigger(string EventName, params object[] Args)
        {
            Logger.Out(LogType.Debug, EventName);
            try
            {
                switch (EventName)
                {
                    case "onServerStart":
                        Plugins.Event.onServerStart.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                            );
                        break;
                    case "onServerStop":
                        Plugins.Event.onServerStop.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0])})
                            );
                        break;
                    case "onServerOutput":
                        Plugins.Event.onServerOutput.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0])})
                            );
                        break;
                    case "onServerOriginalOutput":
                        Plugins.Event.onServerOriginalOutput.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0])})
                            );
                        break;
                    case "onServerSendCommand":
                        Plugins.Event.onServerSendCommand.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0])})
                            );
                        break;
                    case "onServerSendSpecifiedCommand":
                        ((Delegate)Args[1]).DynamicInvoke(
                            JsValue.Undefined, new[] { JsValue.FromObject(JSEngine.Converter, Args[0]) });
                        break;
                    case "onGroupIncrease":
                        Plugins.Event.onGroupIncrease.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0]),
                                JsValue.FromObject(JSEngine.Converter, Args[1]) })
                            );
                        break;
                    case "onGroupDecrease":
                        Plugins.Event.onGroupDecrease.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0]),
                                JsValue.FromObject(JSEngine.Converter, Args[1])})
                            );
                        break;
                    case "onGroupPoke":
                        Plugins.Event.onGroupPoke.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0]),
                                JsValue.FromObject(JSEngine.Converter, Args[1])})
                            );
                        break;
                    case "onReceiveGroupMessage":
                        Plugins.Event.onReceiveGroupMessage.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0]),
                                JsValue.FromObject(JSEngine.Converter, Args[1]),
                                JsValue.FromObject(JSEngine.Converter, Args[2]),
                                JsValue.FromObject(JSEngine.Converter, Args[3])})
                            );
                        break;
                    case "onReceivePrivateMessage":
                        Plugins.Event.onReceivePrivateMessage.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0]),
                                JsValue.FromObject(JSEngine.Converter, Args[1]),
                                JsValue.FromObject(JSEngine.Converter, Args[2])})
                            );
                        break;
                    case "onReceivePacket":
                        Plugins.Event.onReceivePacket.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(JSEngine.Converter, Args[0])})
                            );
                        break;
                    case "onSereinStart":
                        Plugins.Event.onSereinStart.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                            );
                        break;
                    case "onSereinClose":
                        Plugins.WebSockets.ForEach((WSC) => WSC.Dispose());
                        Plugins.Event.onSereinClose.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                            );
                        break;
                    case "onPluginsReload":
                        Plugins.Event.onPluginsReload.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                            );
                        break;
                }
            }
            catch (Exception e)
            {
                string Message;
                if (e.InnerException is JavaScriptException JSe)
                    Message = $"{JSe.Message} (at line {JSe.Location.Start.Line}:{JSe.Location.Start.Column})";
                else
                    Message = e.Message;
                Logger.Out(LogType.Plugin_Error, $"触发事件{EventName}时出现异常：{Message}");
                Logger.Out(LogType.Debug, $"触发事件{EventName}时出现异常：\n", e);
            }
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="Command">命令</param>
        /// <returns>注册结果</returns>
        public static bool RegisterCommand(string Command, Delegate Function)
        {
            Logger.Out(LogType.Debug, "Register:", Command);
            if (
                Command.Contains(" ") ||
                ((IList<string>)Global.Settings.Server.StopCommands).Contains(Command)
                )
            {
                Logger.Out(LogType.Plugin_Error, $"插件注册命令\"{Command}\"失败");
                return false;
            }
            else
            {
                Plugins.CommandItems.Add(
                    new CommandItem
                    {
                        Command = Command,
                        Function = Function
                    });
                return true;
            }
        }

        /// <summary>
        /// 设置定时器
        /// </summary>
        /// <param name="Function">函数</param>
        /// <param name="Interval">间隔</param>
        /// <param name="AutoReset"自动重置></param>
        /// <returns>定时器哈希值</returns>
        public static JsValue SetTimer(Delegate Function, JsValue Interval, bool AutoReset)
        {
            long TimerID = ID;
            ID++;
            Timer _Timer = new Timer(Interval.AsNumber())
            {
                AutoReset = AutoReset,
            };
            _Timer.Elapsed += (sender, args) =>
            {
                try
                {
                    Function?.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
                }
                catch (Exception e)
                {
                    string Message;
                    if (e.InnerException is JavaScriptException JSe)
                        Message = $"{JSe.Message} (at line {JSe.Location.Start.Line}:{JSe.Location.Start.Column})";
                    else if (e.InnerException is ArgumentException || e.InnerException is InvalidOperationException)
                        return;
                    else
                        Message = e.Message;
                    Logger.Out(LogType.Plugin_Error, $"触发定时器[ID:{TimerID}]时出现异常：{Message}");
                    Logger.Out(LogType.Debug, $"触发定时器[ID:{TimerID}]时出现异常：\n", e);
                }
                if (!AutoReset)
                    _Timer.Dispose();
            };
            _Timer.Start();
            Plugins.Timers.Add(TimerID, _Timer);
            return JsValue.FromObject(JSEngine.Converter, TimerID);
        }

        /// <summary>
        /// 清除定时器
        /// </summary>
        /// <param name="ID">哈希值</param>
        /// <returns>清除结果</returns>
        private static bool ClearTimer(long ID)
        {
            if (Plugins.Timers.ContainsKey(ID))
            {
                Plugins.Timers[ID].Stop();
                Plugins.Timers[ID].Dispose();
                return Plugins.Timers.Remove(ID);
            }
            return false;
        }

        /// <summary>
        /// 清除定时器
        /// </summary>
        /// <param name="ID">哈希值</param>
        /// <returns>清除结果</returns>
        public static bool ClearTimer(JsValue ID) => ClearTimer((long)ID.AsNumber());

        /// <summary>
        /// 清除所有定时器
        /// </summary>
        public static void ClearAllTimers()
        {
            IList<long> IDs = Plugins.Timers.Keys.ToArray();
            foreach (long ID in IDs)
            {
                ClearTimer(ID);
            }
        }


        /// <summary>
        /// 获取MD5
        /// </summary>
        /// <param name="Text">文本</param>
        /// <returns>MD5文本</returns>
        public static string GetMD5(string Text)
        {
            byte[] Datas = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Text));
            string Result = string.Empty;
            for (int i = 0; i < Datas.Length; i++)
            {
                Result += Datas[i].ToString("x2");
            }
            return Result;
        }
    }
}