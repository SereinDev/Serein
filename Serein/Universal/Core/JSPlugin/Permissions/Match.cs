using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Serein.Core.JSPlugin.Permission
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Match
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type;

        /// <summary>
        /// 目标
        /// </summary>
        public long[] Targets = { };

        /// <summary>
        /// 成员
        /// </summary>
        public long[] Members = { };

        /// <summary>
        /// 仅监听群聊
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? OnlyListened;

        private static List<string> Types = new() { "group", "private", "temp", "unknown" };

        public Match()
        {
            Targets ??= Array.Empty<long>();
            Members ??= Array.Empty<long>();
            if (string.IsNullOrEmpty(Type) || !Types.Contains(Type))
            {
                Type ??= "unknown";
            }
            if (Type != "group" && Type != "temp" && (OnlyListened ?? false))
            {
                OnlyListened = null;
            }
        }
    }
}