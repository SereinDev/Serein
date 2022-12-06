using Jint.Native;
using Jint.Runtime;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Timers;

namespace Serein.JSPlugin
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
            if (!JSPluginManager.PluginDict.ContainsKey(JSPluginManager.LatestFile))
                return false;
            lock (JSPluginManager.PluginDict)
            {
                JSPluginManager.PluginDict[JSPluginManager.LatestFile].Name = Name;
                JSPluginManager.PluginDict[JSPluginManager.LatestFile].Version = Version;
                JSPluginManager.PluginDict[JSPluginManager.LatestFile].Author = Author;
                JSPluginManager.PluginDict[JSPluginManager.LatestFile].Description = Description;
            }
            return true;
        }

        /// <summary>
        /// 注册事件监听器
        /// </summary>
        /// <param name="EventName">事件名称</param>
        /// <param name="Function">函数</param>
        /// <returns>注册结果</returns>
        public static bool SetListener(string Target, string EventName, Delegate Function)
        {
            Logger.Out(LogType.Debug, EventName);
            lock (JSPluginManager.PluginDict)
            {
                return
                    Enum.IsDefined(typeof(EventType), EventName ?? string.Empty) &&
                    JSPluginManager.PluginDict.ContainsKey(Target) &&
                    JSPluginManager.PluginDict[Target].Event.Add((EventType)Enum.Parse(typeof(EventType), EventName), Function);
            }
        }

        /// <summary>
        /// 触发插件事件
        /// </summary>
        /// <param name="EventName">事件名称</param>
        /// <param name="Args">参数</param>
        public static void Trigger(string EventName, params object[] Args)
        {
            Logger.Out(LogType.Debug, EventName);
            if (Enum.IsDefined(typeof(EventType), EventName ?? string.Empty))
                lock (JSPluginManager.PluginDict)
                {
                    JSPluginManager.PluginDict.Keys.ToList().ForEach((Key) =>
                        JSPluginManager.PluginDict[Key].Event.Trigger((EventType)Enum.Parse(typeof(EventType), EventName), Args)
                    );
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
            Logger.Out(LogType.Debug, "Interval:", Interval.ToString(), "AutoReset:", AutoReset, "ID:", TimerID);
            Timer _Timer = new Timer((double)Interval.ToObject())
            {
                AutoReset = AutoReset,
            };
            _Timer.Elapsed += (sender, args) =>
            {
                try
                {
                    Function.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
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
            JSPluginManager.Timers.Add(TimerID, _Timer);
            return JsValue.FromObject(JSEngine.Converter, TimerID);
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
        /// <param name="ID">哈希值</param>
        /// <returns>清除结果</returns>
        public static bool ClearTimer(JsValue ID) => ClearTimer((long)ID.ToObject());

        /// <summary>
        /// 清除所有定时器
        /// </summary>
        public static void ClearAllTimers()
        {
            IList<long> IDs = JSPluginManager.Timers.Keys.ToArray();
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
