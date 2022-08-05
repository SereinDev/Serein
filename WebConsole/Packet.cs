using Newtonsoft.Json;

namespace WebConsole
{
    internal class Packet
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "sub_type")]
        public string SubType { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "from")]
        public string From { get; set; } = string.Empty;

        public Packet(string type = "", string sub_type = "", string data = "", string from = "")
        {
            Type = type;
            SubType = sub_type;
            Data = data;
            From = from;
        }
    }
}
