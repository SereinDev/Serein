using Serein.Base;
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
        public static readonly string PATH = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 版本号
        /// </summary>
        public const string VERSION = "v1.3.4";

        /// <summary>
        /// 类型
        /// </summary>
        public const string TYPE =
#if CONSOLE
            "console";
#elif WINFORM
            "winform";
#elif WPF
            "wpf";
#else
            "unknown";
#endif

        /// <summary>
        /// 正则项列表
        /// </summary>
        public static List<Regex> RegexList
        {
            get
            {
                return _regexList;
            }
            set
            {
                lock (_regexList)
                {
                    _regexList = value;
                }
            }
        }

        private static List<Regex> _regexList = new();

        /// <summary>
        /// 任务项列表
        /// </summary>
        public static List<Schedule> Schedules
        {
            get
            {
                return _schedules;
            }
            set
            {
                lock (_schedules)
                {
                    _schedules = value;
                    _schedules.ForEach((schedule) => schedule.Check());
                }
            }
        }

        private static List<Schedule> _schedules = new();

        /// <summary>
        /// 成员项字典
        /// </summary>
        public static Dictionary<long, Member> MemberDict = new();

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
        /// 群组用户信息缓存
        /// </summary>
        public static Dictionary<long, Dictionary<long, Member>> GroupCache = new();
    }
}