using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Serein.ConnectionProtocols.Models.OneBot;
using Serein.ConnectionProtocols.Models.OneBot.Shared;
using Serein.ConnectionProtocols.Models.OneBot.V12.Messages;
using Serein.Core.Models.Network.Connection;
using Serein.Core.Services.Data;
using Serein.Core.Utils.Json;
using V11 = Serein.ConnectionProtocols.Models.OneBot.V11.Actions;
using V12 = Serein.ConnectionProtocols.Models.OneBot.V12.Actions;

namespace Serein.Core.Services.Network.Connection.Adapters.OneBot;

public class ActionBuilder(ILogger<ActionBuilder> logger, SettingProvider settingProvider)
{
    public string Build(TargetType targetType, string target, string text)
    {
        if (settingProvider.Value.Connection.OneBot.Version == OneBotVersion.V11)
        {
            return JsonSerializer.Serialize(
                new ActionRequest<V11.MessageParams>
                {
                    Action = "send_msg",
                    Params =
                        targetType is TargetType.Private
                            ? new()
                            {
                                UserId = long.Parse(target),
                                Message = text,
                                AutoEscape = settingProvider.Value.Connection.OneBot.AutoEscape,
                            }
                        : targetType is TargetType.Group
                            ? new()
                            {
                                GroupId = long.Parse(target),
                                Message = text,
                                AutoEscape = settingProvider.Value.Connection.OneBot.AutoEscape,
                            }
                        : throw new NotSupportedException(),
                },
                JsonSerializerOptionsFactory.PacketStyle
            );
        }
        else
        {
            var segments = ParseMessageSegments(text);
            var action = new ActionRequest<V12.MessageParams>()
            {
                Action = "send_message",
                Params = targetType switch
                {
                    TargetType.Private => new()
                    {
                        Message = segments,
                        DetailType = "private",
                        UserId = target,
                    },
                    TargetType.Group => new()
                    {
                        Message = segments,
                        DetailType = "group",
                        GroupId = target,
                    },
                    TargetType.Channel => new()
                    {
                        Message = segments,
                        DetailType = "channel",
                        ChannelId = target,
                    },
                    TargetType.Guild => new()
                    {
                        Message = segments,
                        DetailType = "guild",
                        GuildId = target,
                    },
                    _ => throw new NotSupportedException(),
                },
            };

            return JsonSerializer.Serialize(action, JsonSerializerOptionsFactory.PacketStyle);
        }
    }

    private MessageSegment[] ParseMessageSegments(string text, bool skipParse = true)
    {
        if (!skipParse && text.StartsWith('[') && text.EndsWith(']'))
        {
            try
            {
                return JsonSerializer.Deserialize<MessageSegment[]>(
                        text,
                        JsonSerializerOptionsFactory.PacketStyle
                    ) ?? [];
            }
            catch (JsonException ex)
            {
                logger.LogDebug(ex, "尝试解析消息段时异常: {}", text);
            }
        }

        return
        [
            new MessageSegment
            {
                Type = "text",
                Data = new() { ["text"] = text },
            },
        ];
    }
}
