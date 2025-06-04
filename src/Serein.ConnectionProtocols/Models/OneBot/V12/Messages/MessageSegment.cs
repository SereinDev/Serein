using System;
using System.Collections.Generic;

namespace Serein.ConnectionProtocols.Models.OneBot.V12.Messages;

/// <summary>
/// OneBot V12 消息段
/// <see cref="https://12.onebot.dev/interface/message/segments/"/>
/// </summary>
public record MessageSegment
{
    public string Type { get; init; } = string.Empty;

    public Dictionary<string, object> Data { get; init; } = [];

    public override string ToString()
    {
        return Type switch
        {
            "text" => Data.TryGetValue("text", out var text)
                ? text.ToString() ?? string.Empty
                : string.Empty,
            "mention" => Data.TryGetValue("user_id", out var userId) ? $"@{userId}" : string.Empty,
            "mention_all " => "@所有人",
            "file" or "video" or "audio" or "voice" or "image" => Data.TryGetValue(
                "file_id",
                out var fileId
            )
                ? $"[{GetText()}:{fileId}]"
                : $"[{GetText()}]",
            "location" => Data.TryGetValue("title", out var title) ? $"[位置:{title}]" : "[位置]",
            "reply" => Data.TryGetValue("message_id", out var messageId)
                ? $"[回复:{messageId}]"
                : "[回复]",
            _ => $"[{Type}]",
        };

        string GetText()
        {
            return Type switch
            {
                "video" => "视频",
                "audio" => "音频",
                "image" => "图片",
                "voice" => "语音",
                "file" => "文件",
                _ => throw new NotSupportedException(),
            };
        }
    }
}
