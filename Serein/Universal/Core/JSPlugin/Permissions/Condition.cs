using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Serein.Core.JSPlugin.Permission
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Condition
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type;

        /// <summary>
        /// 群号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long[] Groups;

        /// <summary>
        /// 用户
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long[] Users;

        /// <summary>
        /// 仅监听群聊
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? OnlyListened;

        /// <summary>
        /// 仅管理
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? RequireAdmin;

        private static List<string> Types = new() { "group", "private", "temp", "unknown" };

        public Condition()
        {
            if (string.IsNullOrEmpty(Type) || !Types.Contains(Type))
            {
                Type = "unknown";
                Groups = null;
                Users = null;
                OnlyListened = null;
                RequireAdmin = null;
            }
            if (Type != "group" && Type != "temp" && (OnlyListened ?? false))
            {
                OnlyListened = null;
                Groups = null;
            }
        }
    }
}