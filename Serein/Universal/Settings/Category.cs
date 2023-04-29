using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Serein.Settings
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class Category
    {
        public Server Server = new();
        public Matches Matches = new();
        public Bot Bot = new();
        public Serein Serein = new();
        public Event Event = new();
    }
}
