using Serein.Base;
using Serein.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Serein.Core.JSPlugin.Native
{
    internal struct MemberStruct
    {
#pragma warning disable IDE1006
        [JsonProperty(PropertyName = "ID")]
        public long id;
        [JsonProperty(PropertyName = "Card")]
        public string card;
        [JsonProperty(PropertyName = "Nickname")]
        public string nickname;
        [JsonProperty(PropertyName = "Role")]
        public int role;
        [JsonProperty(PropertyName = "GameID")]
        public string gameId;
#pragma warning restore IDE1006

        internal static Dictionary<string, Dictionary<string, MemberStruct>> Create(Dictionary<long, Dictionary<long, Member>> groupCache)
            => JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, MemberStruct>>>(groupCache.ToJson());
    }
}