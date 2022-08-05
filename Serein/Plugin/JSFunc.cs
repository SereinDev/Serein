using Jint;
using Jint.Native;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Timers;

namespace Serein.Plugin
{
    partial class JSFunc
    {
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
            Plugins.PluginItems.Add(new PluginItem()
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
                default:
                    Global.Logger(32, $"插件注册了一个未知事件：{EventName}");
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
            Engine engine = new Engine();
            Global.Logger(999, "[JSFunc:Tigger()]", EventName);
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
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                            );
                        break;
                    case "onServerOutput":
                        Plugins.Event.onServerOutput.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(engine, Args[0])})
                            );
                        break;
                    case "onServerSendCommand":
                        Plugins.Event.onServerSendCommand.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(engine, Args[0])})
                            );
                        break;
                    case "onServerSendSpecifiedCommand":
                        ((Delegate)Args[1]).DynamicInvoke(
                            JsValue.Undefined, new[] { JsValue.FromObject(engine, Args[0]) });
                        break;
                    case "onGroupIncrease":
                        Plugins.Event.onGroupIncrease.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(engine, Args[0]),
                                JsValue.FromObject(engine, Args[1]) })
                            );
                        break;
                    case "onGroupDecrease":
                        Plugins.Event.onGroupDecrease.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(engine, Args[0]),
                                JsValue.FromObject(engine, Args[1])})
                            );
                        break;
                    case "onGroupPoke":
                        Plugins.Event.onGroupPoke.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(engine, Args[0]),
                                JsValue.FromObject(engine, Args[1])})
                            );
                        break;
                    case "onReceiveGroupMessage":
                        Plugins.Event.onReceiveGroupMessage.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(engine, Args[0]),
                                JsValue.FromObject(engine, Args[1]),
                                JsValue.FromObject(engine, Args[2]),
                                JsValue.FromObject(engine, Args[3])})
                            );
                        break;
                    case "onReceivePrivateMessage":
                        Plugins.Event.onReceivePrivateMessage.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(engine, Args[0]),
                                JsValue.FromObject(engine, Args[1]),
                                JsValue.FromObject(engine, Args[2])})
                            );
                        break;
                    case "onReceivePacket":
                        Plugins.Event.onReceivePacket.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] {
                                JsValue.FromObject(engine, Args[0])})
                            );
                        break;
                    case "onSereinStart":
                        Plugins.Event.onSereinStart.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                            );
                        break;
                    case "onSereinClose":
                        Plugins.Event.onSereinClose.ForEach(
                            (x) => x.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined })
                            );
                        break;
                }
                engine = null;
            }
            catch (Exception e)
            {
                Global.Logger(32, $"触发事件{EventName}时出现异常：{e.Message}");
                Global.Logger(999, e.ToString());
            }
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="Command">命令</param>
        /// <returns>注册结果</returns>
        public static bool RegisterCommand(string Command, Delegate Function)
        {
            if (
                Command.Contains(" ") ||
                ((IList<string>)Global.Settings.Server.StopCommand.Split(';')).Contains(Command)
                )
            {
                Global.Logger(32, $"插件注册命令\"{Command}\"失败");
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
            Timer timer = new Timer(Interval.AsNumber())
            {
                AutoReset = AutoReset,
            };
            timer.Elapsed += (e, args) =>
            {
                Function.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
                if (!AutoReset)
                {
                    timer.Dispose();
                }
            };
            timer.Start();
            int ID = timer.GetHashCode();
            Plugins.Timers.Add(ID, timer);
            return JsValue.FromObject(new Engine(), ID);
        }

        /// <summary>
        /// 清除定时器
        /// </summary>
        /// <param name="ID">哈希值</param>
        /// <returns>清除结果</returns>
        private static bool ClearTimer(int ID)
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
        public static bool ClearTimer(JsValue ID)
        {
            return ClearTimer((int)ID.AsNumber());
        }

        /// <summary>
        /// 清除所有定时器
        /// </summary>
        public static void ClearAllTimers()
        {
            IList<int> IDs = Plugins.Timers.Keys.ToArray();
            foreach (int ID in IDs)
            {
                ClearTimer(ID);
            }
        }

        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string Result = string.Empty;
            for (int i = 0; i < targetData.Length; i++)
            {
                Result += targetData[i].ToString("x2");
            }
            return Result;
        }
    }
}
