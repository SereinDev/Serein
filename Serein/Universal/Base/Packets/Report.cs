using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Serein.Base.Packets
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal abstract class Report
    {
        public string? PostType;
    }
}