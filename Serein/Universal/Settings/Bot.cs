using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Serein.Settings
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class Bot
    {
        public string Authorization = string.Empty;

        public bool AutoEscape;

        public bool AutoReconnect;

        public bool EnableLog;

        public bool EnbaleOutputData;

        public bool EnbaleParseAt = true;

        public bool GivePermissionToAllAdmin;

        public long[] GroupList = Array.Empty<long>();

        public long[] PermissionList = Array.Empty<long>();

        public string Uri = "127.0.0.1:8080";
    }
}
