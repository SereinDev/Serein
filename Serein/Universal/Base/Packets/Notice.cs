using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serein.Extensions;

namespace Serein.Base.Packets
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal class Notice : Report
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
        public string NoticeType = string.Empty;

        /// <summary>
        /// 子类型
        /// </summary>
        public string? SubType = string.Empty;

        /// <summary>
        /// 目标ID
        /// </summary>
        public long? TargetId;
    }
}