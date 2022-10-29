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
        /// 设置文件夹路径
        /// </summary>
        public static readonly string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "settings";

        /// <summary>
        /// 版本号
        /// </summary>
        public const string VERSION = "v1.3.2";

        /// <summary>
        /// 正则项列表
        /// </summary>
        public static List<Regex> RegexItems = new List<Regex>();

        /// <summary>
        /// 任务项列表
        /// </summary>
        public static List<Task> TaskItems = new List<Task>();

        /// <summary>
        /// 成员项字典
        /// </summary>
        public static Dictionary<long, Member> MemberItems = new Dictionary<long, Member>();

        /// <summary>
        /// 设置项
        /// </summary>
        public static Item Settings = new Item();

        /// <summary>
        /// 首次开启
        /// </summary>
        public static bool FirstOpen = false;

        /// <summary>
        /// 启动参数
        /// </summary>
        public static IList<string> Args = null;

        /// <summary>
        /// 编译信息
        /// </summary>
        public static readonly BuildInfo BuildInfo = new BuildInfo();

        /// <summary>
        /// 更新正则列表
        /// </summary>
        /// <param name="New">新正则列表</param>
        public static void UpdateRegexItems(List<Regex> New)
        {
            lock (RegexItems)
            {
                RegexItems = New;
            }
        }

        /// <summary>
        /// 更新任务列表
        /// </summary>
        /// <param name="New">新任务列表</param>
        public static void UpdateTaskItems(List<Task> New)
        {
            lock (TaskItems)
            {
                TaskItems = New;
            }
        }

        /// <summary>
        /// 更新成员字典
        /// </summary>
        /// <param name="New">新成员字典</param>
        public static void UpdateMemberItems(Dictionary<long, Member> New)
        {
            lock (MemberItems)
            {
                MemberItems = New;
            }
        }
    }
}