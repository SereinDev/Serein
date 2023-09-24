using Serein.Base.Motd;
using Serein.Base.Packets;
using Serein.Utils.Output;
using System;
using System.Text.RegularExpressions;

namespace Serein.Core.Common
{
    internal static class EventTrigger
    {
        /// <summary>
        /// 触发指定事件
        /// </summary>
        /// <param name="type">类型</param>
        public static void Trigger(Base.EventType type)
        {
            Logger.Output(Base.LogType.Debug, "Trigger:" + type);
            string[] commandGroup = type switch
            {
                Base.EventType.ServerStart => Global.Settings.Event.ServerStart,
                Base.EventType.ServerStop => Global.Settings.Event.ServerStop,
                Base.EventType.ServerExitUnexpectedly
                    => Global.Settings.Event.ServerExitUnexpectedly,
                Base.EventType.SereinCrash => Global.Settings.Event.SereinCrash,
                _ => Array.Empty<string>()
            };
            foreach (string command in commandGroup)
            {
                Command.Run(Base.CommandOrigin.EventTrigger, command);
            }
        }

        /// <summary>
        /// 触发指定事件
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="notice">通知数据包</param>
        public static void Trigger(Base.EventType type, Notice notice)
        {
            Logger.Output(Base.LogType.Debug, "Trigger:" + type);
            string[] commandGroup = type switch
            {
                Base.EventType.GroupIncrease => Global.Settings.Event.GroupIncrease,
                Base.EventType.GroupDecrease => Global.Settings.Event.GroupDecrease,
                Base.EventType.GroupPoke => Global.Settings.Event.GroupPoke,
                _ => Array.Empty<string>(),
            };
            foreach (string command in commandGroup)
            {
                Command.Run(
                    Base.CommandOrigin.EventTrigger,
                    Regex.Replace(
                        command,
                        "%ID%",
                        notice.UserId.ToString(),
                        RegexOptions.IgnoreCase
                    )
                );
            }
        }

        /// <summary>
        /// 触发指定事件
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="message">消息数据包</param>
        /// <param name="motd">Motd对象</param>
        public static void Trigger(Base.EventType type, Message message, Motd? motd = null)
        {
            Logger.Output(Base.LogType.Debug, "Trigger:" + type);
            string[] commandGroup = Array.Empty<string>();
            switch (type)
            {
                case Base.EventType.BindingSucceed:
                case Base.EventType.BindingFailDueToOccupation:
                case Base.EventType.BindingFailDueToInvalid:
                case Base.EventType.BindingFailDueToAlreadyBinded:
                case Base.EventType.BinderDisable:
                case Base.EventType.UnbindingSucceed:
                case Base.EventType.UnbindingFail:
                case Base.EventType.PermissionDeniedFromPrivateMsg:
                case Base.EventType.PermissionDeniedFromGroupMsg:
                    commandGroup = type switch
                    {
                        Base.EventType.BindingSucceed => Global.Settings.Event.BindingSucceed,
                        Base.EventType.BindingFailDueToOccupation
                            => Global.Settings.Event.BindingFailDueToOccupation,
                        Base.EventType.BindingFailDueToInvalid
                            => Global.Settings.Event.BindingFailDueToInvalid,
                        Base.EventType.BindingFailDueToAlreadyBinded
                            => Global.Settings.Event.BindingFailDueToAlreadyBinded,
                        Base.EventType.UnbindingSucceed => Global.Settings.Event.UnbindingSucceed,
                        Base.EventType.UnbindingFail => Global.Settings.Event.UnbindingFail,
                        Base.EventType.BinderDisable => Global.Settings.Event.BinderDisable,
                        Base.EventType.PermissionDeniedFromPrivateMsg
                            => Global.Settings.Event.PermissionDeniedFromPrivateMsg,
                        Base.EventType.PermissionDeniedFromGroupMsg
                            => Global.Settings.Event.PermissionDeniedFromGroupMsg,
                        _ => commandGroup,
                    };
                    foreach (string command in commandGroup)
                    {
                        Command.Run(Base.CommandOrigin.EventTrigger, command, message);
                    }
                    break;

                case Base.EventType.RequestingMotdpeSucceed:
                case Base.EventType.RequestingMotdjeSucceed:
                case Base.EventType.RequestingMotdFail:
                    commandGroup = type switch
                    {
                        Base.EventType.RequestingMotdpeSucceed
                            => Global.Settings.Event.RequestingMotdpeSucceed,
                        Base.EventType.RequestingMotdjeSucceed
                            => Global.Settings.Event.RequestingMotdjeSucceed,
                        Base.EventType.RequestingMotdFail
                            => Global.Settings.Event.RequestingMotdFail,
                        _ => commandGroup
                    };

                    foreach (string command in commandGroup)
                    {
                        if (
                            motd is not null
                            && Regex.IsMatch(
                                command,
                                @"%(Version|GameMode|OnlinePlayer|MaxPlayer|Description|Protocol|Original|Latency|Favicon|Exception)%",
                                RegexOptions.IgnoreCase
                            )
                        )
                        {
                            string command_copy = Regex.Replace(
                                command,
                                @"%(\w+)%",
                                (match) =>
                                    match.Groups[1].Value.ToLowerInvariant() switch
                                    {
                                        "gamemode" => motd.GameMode,
                                        "description" => motd.Description,
                                        "protocol" => motd.Protocol,
                                        "onlineplayer" => motd.OnlinePlayer.ToString(),
                                        "maxplayer" => motd.MaxPlayer.ToString(),
                                        "original" => motd.Origin,
                                        "latency" => motd.Latency.ToString("N1"),
                                        "version" => motd.Version,
                                        "favicon" => motd.FaviconCQCode,
                                        "exception" => motd.Exception,
                                        _ => match.Groups[1].Value
                                    } ?? string.Empty
                            );
                            Command.Run(
                                Base.CommandOrigin.EventTrigger,
                                command_copy,
                                message,
                                true
                            );
                        }
                    }
                    break;

                default:
                    Logger.Output(Base.LogType.Debug, "未知的事件名", type);
                    break;
            }
        }
    }
}
