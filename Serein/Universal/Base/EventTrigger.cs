using Serein.Items.Motd;
using System;
using System.Text.RegularExpressions;

namespace Serein.Base
{
    internal static class EventTrigger
    {
        /// <summary>
        /// 触发指定事件
        /// </summary>
        /// <param name="type">类型</param>
        public static void Trigger(Items.EventType type)
        {
            Logger.Out(Items.LogType.Debug, "Trigger:" + type);
            string[] commandGroup = Array.Empty<string>();
            commandGroup = type switch
            {
                Items.EventType.ServerStart => Global.Settings.Event.ServerStart,
                Items.EventType.ServerStop => Global.Settings.Event.ServerStop,
                Items.EventType.ServerExitUnexpectedly => Global.Settings.Event.ServerExitUnexpectedly,
                Items.EventType.SereinCrash => Global.Settings.Event.SereinCrash,
                _ => commandGroup
            };
            foreach (string command in commandGroup)
            {
                Base.Command.Run(4, command);
            }
        }

        /// <summary>
        /// 触发指定事件
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="groupId">群聊ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="motd">Motd对象</param>
        public static void Trigger(Items.EventType type, long groupId, long userId, Motd motd = null)
        {
            Logger.Out(Items.LogType.Debug, "Trigger:" + type);
            string[] commandGroup = Array.Empty<string>();
            switch (type)
            {
                case Items.EventType.BindingSucceed:
                case Items.EventType.BindingFailDueToOccupation:
                case Items.EventType.BindingFailDueToInvalid:
                case Items.EventType.BindingFailDueToAlreadyBinded:
                case Items.EventType.UnbindingSucceed:
                case Items.EventType.UnbindingFail:
                case Items.EventType.GroupIncrease:
                case Items.EventType.GroupDecrease:
                case Items.EventType.GroupPoke:
                case Items.EventType.PermissionDeniedFromPrivateMsg:
                case Items.EventType.PermissionDeniedFromGroupMsg:
                    commandGroup = type switch
                    {
                        Items.EventType.BindingSucceed => Global.Settings.Event.BindingSucceed,
                        Items.EventType.BindingFailDueToOccupation => Global.Settings.Event.BindingFailDueToOccupation,
                        Items.EventType.BindingFailDueToInvalid => Global.Settings.Event.BindingFailDueToInvalid,
                        Items.EventType.BindingFailDueToAlreadyBinded => Global.Settings.Event.BindingFailDueToAlreadyBinded,
                        Items.EventType.UnbindingSucceed => Global.Settings.Event.UnbindingSucceed,
                        Items.EventType.UnbindingFail => Global.Settings.Event.UnbindingFail,
                        Items.EventType.GroupIncrease => Global.Settings.Event.GroupIncrease,
                        Items.EventType.GroupDecrease => Global.Settings.Event.GroupDecrease,
                        Items.EventType.GroupPoke => Global.Settings.Event.GroupPoke,
                        Items.EventType.PermissionDeniedFromPrivateMsg => Global.Settings.Event.PermissionDeniedFromPrivateMsg,
                        Items.EventType.PermissionDeniedFromGroupMsg => Global.Settings.Event.PermissionDeniedFromGroupMsg,
                        _ => commandGroup,
                    };
                    foreach (string command in commandGroup)
                    {
                        Base.Command.Run(
                            4,
                            Regex.Replace(command, "%ID%", userId.ToString(), RegexOptions.IgnoreCase),
                            groupId: groupId
                            );
                    }
                    break;
                case Items.EventType.RequestingMotdpeSucceed:
                case Items.EventType.RequestingMotdjeSucceed:
                case Items.EventType.RequestingMotdFail:
                    commandGroup = type switch
                    {
                        Items.EventType.RequestingMotdpeSucceed => Global.Settings.Event.RequestingMotdpeSucceed,
                        Items.EventType.RequestingMotdjeSucceed => Global.Settings.Event.RequestingMotdjeSucceed,
                        Items.EventType.RequestingMotdFail => Global.Settings.Event.RequestingMotdFail,
                        _ => commandGroup
                    };
                    foreach (string command in commandGroup)
                    {
                        string command_copy = command;
                        if (motd != null && Regex.IsMatch(command, @"%(Version|GameMode|OnlinePlayer|MaxPlayer|Description|Protocol|Original|Delay|Favicon|Exception)%", RegexOptions.IgnoreCase))
                        {
                            command_copy = Regex.Replace(command_copy, "%GameMode%", motd.GameMode, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Description%", motd.Description, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Protocol%", motd.Protocol, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%OnlinePlayer%", motd.OnlinePlayer.ToString(), RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%MaxPlayer%", motd.MaxPlayer.ToString(), RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Original%", motd.Origin, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Delay%", motd.Delay.TotalMilliseconds.ToString("N1"), RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Version%", motd.Version, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Favicon%", motd.Favicon, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Exception%", motd.Exception, RegexOptions.IgnoreCase);
                        }
                        Base.Command.Run(
                            4,
                            command_copy,
                            userId: userId,
                            groupId: groupId,
                            disableMotd: true
                            );
                    }
                    break;
                default:
                    Logger.Out(Items.LogType.Debug, "未知的事件名", type);
                    break;
            }
        }
    }
}
