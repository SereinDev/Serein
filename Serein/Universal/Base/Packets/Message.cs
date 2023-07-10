using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serein.Extensions;

namespace Serein.Base.Packets
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal class Message
    {
        public string MessageType = string.Empty, PostType = string.Empty, RawMessage = string.Empty;

        /// <summary>
        /// 消息 ID
        /// </summary>
        public long MessageId;

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId;

        /// <summary>
        /// 群号
        /// </summary>
        public long GroupId;

        /// <summary>
        /// 发送者信息
        /// </summary>
        public Sender? Sender;

        public override string ToString() => this.ToJson();
    }
}