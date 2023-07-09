using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Serein.Base.Packets
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal class Notice
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId;

        /// <summary>
        /// 群号
        /// </summary>
        public long GroupId;

        /// <summary>
        /// 通知类型
        /// </summary>
        public string? NoticeType;

        /// <summary>
        /// 子类型
        /// </summary>
        public string? SubType;

        /// <summary>
        /// 目标ID
        /// </summary>
        public long? TargetId;
    }
}