using Jint.Native;
using Jint.Runtime;
using Serein.Base;
using Serein.Extensions;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Serein.JSPlugin
{
    internal static class JSFunc
    {
        public static long CurrentID;

        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="namespace">命名空间</param>
        /// <param name="name">名称</param>
        /// <param name="version">版本</param>
        /// <param name="author">作者</param>
        /// <param name="description">介绍</param>
        /// <returns>注册结果</returns>
        public static string Register(
            string @namespace,
            string name,
            string version,
            string author,
            string description
            )
        {
            if (@namespace == null || !JSPluginManager.PluginDict.ContainsKey(@namespace))
            {
                throw new ArgumentException("无法找到对应的命名空间", nameof(@namespace));
            }
            lock (JSPluginManager.PluginDict)
            {
                JSPluginManager.PluginDict[@namespace].Name = name;
                JSPluginManager.PluginDict[@namespace].Version = version;
                JSPluginManager.PluginDict[@namespace].Author = author;
                JSPluginManager.PluginDict[@namespace].Description = description;
            }
            return @namespace;
        }

        /// <summary>
        /// 注册事件监听器
        /// </summary>
        /// <param name="namespace">命名空间</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="delegate">函数</param>
        /// <returns>注册结果</returns>
        public static bool SetListener(string @namespace, string eventName, JsValue @delegate)
        {
            Logger.Output(LogType.Debug, "Namespace:", @namespace, "EventName:", eventName);
            eventName = System.Text.RegularExpressions.Regex.Replace(eventName ?? string.Empty, "^on", string.Empty);
            if (!Enum.IsDefined(typeof(EventType), eventName))
            {
                throw new ArgumentException("未知的事件：" + eventName);
            }
            lock (JSPluginManager.PluginDict)
            {
                return
                    JSPluginManager.PluginDict.ContainsKey(@namespace) &&
                    JSPluginManager.PluginDict[@namespace].SetListener((EventType)Enum.Parse(typeof(EventType), eventName), @delegate);
            }

        }

        /// <summary>
        /// 事件触发异步锁
        /// </summary>
        private static readonly object EventLock = new();

        /// <summary>
        /// 触发插件事件
        /// </summary>
        /// <param name="type">事件名称</param>
        /// <param name="args">参数</param>
        public static bool Trigger(EventType type, params object[] args)
        {
            if (JSPluginManager.PluginDict.Count == 0)
            {
                return false;
            }
            bool interdicted = false;
            lock (EventLock)
            {
                Logger.Output(LogType.Debug, type);
                lock (JSPluginManager.PluginDict)
                {
                    List<System.Threading.Tasks.Task<bool>> tasks = new();
                    CancellationTokenSource tokenSource = new();
                    foreach (Plugin plugin in JSPluginManager.PluginDict.Values)
                    {
                        tasks.Add(System.Threading.Tasks.Task.Run(() => plugin.Trigger(type, tokenSource.Token, args)));
                    }
                    if (tasks.Count > 0)
                    {
                        System.Threading.Tasks.Task.WaitAll(tasks.ToArray(), 500);
                        tasks.Select((task) => task.IsCompleted && task.Result).ToList().ForEach((b) => interdicted = interdicted || b);
                    }
                    tokenSource.Cancel();
                    if (tasks.Count > 0)
                    {
                        Global.Settings.Serein.DevelopmentTool.JSEventCoolingDownTime.ToSleep();
                    }
                }
            }
            return interdicted;
        }

        /// <summary>
        /// 设置定时器
        /// </summary>
        /// <param name="namespace">命名空间</param>
        /// <param name="delegate">函数</param>
        /// <param name="interval">间隔</param>
        /// <param name="autoReset"自动重置></param>
        /// <returns>定时器哈希值</returns>
        public static JsValue SetTimer(string @namespace, JsValue @delegate, JsValue interval, bool autoReset)
        {
            if (@namespace == null && !JSPluginManager.PluginDict.ContainsKey(@namespace))
            {
                throw new ArgumentOutOfRangeException("无法找到对应的命名空间", nameof(@namespace));
            }
            long timerID = CurrentID;
            CurrentID++;
            Logger.Output(LogType.Debug, "Interval:", interval.ToString(), "AutoReset:", autoReset, "ID:", timerID);
            System.Timers.Timer timer = new((double)interval.ToObject())
            {
                AutoReset = autoReset,
            };
            timer.Elapsed += (_, _) =>
            {
                try
                {
                    if (@namespace == null && !JSPluginManager.PluginDict.ContainsKey(@namespace))
                    {
                        throw new ArgumentOutOfRangeException("无法找到对应的命名空间", nameof(@namespace));
                    }
                    else if (JSPluginManager.PluginDict[@namespace].Engine == null)
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                    else
                    {
                        lock (JSPluginManager.PluginDict[@namespace].Engine)
                        {
                            JSPluginManager.PluginDict[@namespace].Engine.Invoke(JsValue.Undefined);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Output(LogType.Plugin_Error, $"[{@namespace}]", $"触发定时器[ID:{timerID}]时出现异常：{e.GetFullMsg()}");
                    Logger.Output(LogType.Debug, $"触发定时器[ID:{timerID}]时出现异常：", e);
                }
                if (!autoReset)
                {
                    timer.Dispose();
                }
            };
            timer.Start();
            JSPluginManager.Timers.Add(timerID, timer);
            return JsValue.FromObject(JSEngine.Converter, timerID);
        }

        /// <summary>
        /// 清除定时器
        /// </summary>
        /// <param name="ID">哈希值</param>
        /// <returns>清除结果</returns>
        private static bool ClearTimer(long ID)
        {
            if (JSPluginManager.Timers.ContainsKey(ID))
            {
                JSPluginManager.Timers[ID].Stop();
                JSPluginManager.Timers[ID].Dispose();
                return JSPluginManager.Timers.Remove(ID);
            }
            return false;
        }

        /// <summary>
        /// 清除定时器
        /// </summary>
        /// <param name="ID">定时器ID</param>
        /// <returns>清除结果</returns>
        public static bool ClearTimer(JsValue ID)
            => ClearTimer((long)(double)ID.ToObject());

        /// <summary>
        /// 清除所有定时器
        /// </summary>
        public static void ClearAllTimers()
        {
            IList<long> list = JSPluginManager.Timers.Keys.ToArray();
            foreach (long ID in list)
            {
                ClearTimer(ID);
            }
        }

        /// <summary>
        /// 获取MD5
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>MD5文本</returns>
        public static string GetMD5(string text)
        {
            byte[] datas = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder stringBuilder = new();
            for (int i = 0; i < datas.Length; i++)
            {
                stringBuilder.Append(datas[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取完整报错信息
        /// </summary>
        public static string GetFullMsg(this Exception e)
        {
            if (e.InnerException is JavaScriptException javaScriptException)
            {
                return $"{javaScriptException.Message}\n{javaScriptException.JavaScriptStackTrace}";
            }
            else
            {
                return (e.InnerException ?? e).Message;
            }
        }

        /// <summary>
        /// 添加正则
        /// </summary>
        /// <returns>结果</returns>
        public static bool AddRegex(string exp, int area, bool needAdmin, string command, string remark)
        {
            if (exp.TestRegex() &&
                area <= 4 && area >= 0 &&
                Command.GetType(command) != CommandType.Invalid)
            {
                lock (Global.RegexList)
                {
                    Global.RegexList.Add(new()
                    {
                        Expression = exp,
                        Area = area,
                        IsAdmin = needAdmin,
                        Command = command,
                        Remark = remark ?? string.Empty
                    });
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改正则
        /// </summary>
        /// <returns>结果</returns>
        public static bool EditRegex(int index, string exp, int area, bool needAdmin, string command, string remark)
        {
            if (index < Global.RegexList.Count &&
                index >= 0 &&
                exp.TestRegex() &&
                area <= 4 && area >= 0 &&
                Command.GetType(command) != CommandType.Invalid)
            {
                lock (Global.RegexList)
                {
                    Regex selected = Global.RegexList[index];
                    Global.RegexList[index] = new()
                    {
                        Expression = exp ?? selected.Expression,
                        Area = area,
                        IsAdmin = needAdmin,
                        Command = command ?? selected.Command,
                        Remark = remark ?? selected.Remark
                    };
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除正则
        /// </summary>
        /// <returns>结果</returns>
        public static bool RemoveRegex(int index)
        {
            if (index < Global.RegexList.Count && index >= 0)
            {
                lock (Global.RegexList)
                {
                    Global.RegexList.RemoveAt(index);
                }
                return true;
            }
            return false;
        }

        public static bool SetVariable(string key, JsValue jsValue)
        {
            string value = jsValue.ToString();
            if (!string.IsNullOrEmpty(value) && System.Text.RegularExpressions.Regex.IsMatch(key ?? string.Empty, @"\w+"))
            {
                if (JSPluginManager.VariablesDict.ContainsKey(key))
                {
                    JSPluginManager.VariablesDict[key] = value;
                }
                else
                {
                    JSPluginManager.VariablesDict.Add(key, value);
                }
                return true;
            }
            return false;
        }
    }
}
