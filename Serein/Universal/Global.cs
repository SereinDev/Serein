using Serein.Items;
using Serein.Settings;
using System;
using System.Collections.Generic;

namespace Serein
{
    internal static class Global
    {
        /// <summary>
        /// 程序路径
        /// </summary>
        public static readonly string Path = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 版本号
        /// </summary>
        public const string VERSION = "v1.3.3";

        /// <summary>
        /// 正则项列表
        /// </summary>
        public static List<Regex> RegexItems = new();

        /// <summary>
        /// 任务项列表
        /// </summary>
        public static List<Task> TaskItems = new();

        /// <summary>
        /// 成员项字典
        /// </summary>
        public static Dictionary<long, Member> MemberItems = new();

        /// <summary>
        /// 设置项
        /// </summary>
        public static Category Settings = new();

        /// <summary>
        /// 首次开启
        /// </summary>
        public static bool FirstOpen;

        /// <summary>
        /// 编译信息
        /// </summary>
        public static readonly BuildInfo BuildInfo = new();

        /// <summary>
        /// 更新正则列表
        /// </summary>
        /// <param name="newList">新正则列表</param>
        public static void UpdateRegexItems(List<Regex> newList)
        {
            lock (RegexItems)
            {
                RegexItems = newList;
            }
        }

        /// <summary>
        /// 更新任务列表
        /// </summary>
        /// <param name="newList">新任务列表</param>
        public static void UpdateTaskItems(List<Task> newList)
        {
            lock (TaskItems)
            {
                TaskItems = newList;
            }
        }

        /// <summary>
        /// 更新成员字典
        /// </summary>
        /// <param name="newDict">新成员字典</param>
        public static void UpdateMemberItems(Dictionary<long, Member> newDict)
        {
            lock (MemberItems)
            {
                MemberItems = newDict;
            }
        }
    }
}