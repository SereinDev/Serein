using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Serein.Core.JSPlugin.Permission
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Condition
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; init; }

        /// <summary>
        /// 群号
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long[]? Groups { get; init; }

        /// <summary>
        /// 用户
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long[]? Users { get; init; }

        /// <summary>
        /// 仅监听群聊
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? OnlyListened { get; init; }

        /// <summary>
        /// 仅管理
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? RequireAdmin { get; init; }

        private static List<string> _acceptableTypes = new() { "group", "private", "temp", "unknown" };

        public Condition()
        {
            if (Type is null || !_acceptableTypes.Contains(Type!))
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