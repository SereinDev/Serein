using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Serein.Base.Motd
{
    internal class MotdjePacket
    {
        internal class Packet
        {
            /// <summary>
            /// 图标
            /// </summary>
            public string? Favicon;

            /// <summary>
            /// 版本
            /// </summary>
            public Version Version;

            /// <summary>
            /// 玩家信息
            /// </summary>
            public Players Players;

            /// <summary>
            /// 介绍
            /// </summary>
            public Description? Description;
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        internal struct Players
        {
            /// <summary>
            /// 最大人数
            /// </summary>
            public long Max = -1;

            /// <summary>
            /// 在线人数
            /// </summary>
            public long Online = -1;

            public Players() { }
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        internal class Description
        {
            /// <summary>
            /// 文本
            /// </summary>
            public string Text;

            public List<ExtraDescription>? Extra;

            public Description()
            {
                if (string.IsNullOrEmpty(Text) && Extra?.Count > 0)
                {
                    Text = string.Join(string.Empty, Extra);
                }
                else
                {
                    Text = string.Empty;
                }
            }
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        internal struct ExtraDescription
        {
            /// <summary>
            /// 文本
            /// </summary>
            public string? Text;

            public override readonly string ToString() => Text ?? string.Empty;
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        internal struct Version
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string? Name = string.Empty;

            /// <summary>
            /// 协议
            /// </summary>
            public long Protocol = -1;

            public Version() { }
        }
    }
}
