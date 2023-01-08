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
            switch (type)
            {
                case Items.EventType.ServerStart:
                    commandGroup = Global.Settings.Event.ServerStart;
                    break;
                case Items.EventType.ServerStop:
                    commandGroup = Global.Settings.Event.ServerStop;
                    break;
                case Items.EventType.ServerExitUnexpectedly:
                    commandGroup = Global.Settings.Event.ServerExitUnexpectedly;
                    break;
                case Items.EventType.SereinCrash:
                    commandGroup = Global.Settings.Event.SereinCrash;
                    break;
                default:
                    Logger.Out(Items.LogType.Debug, "未知的事件名", type);
                    break;
            }
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
                    switch (type)
                    {
                        case Items.EventType.BindingSucceed:
                            commandGroup = Global.Settings.Event.BindingSucceed;
                            break;
                        case Items.EventType.BindingFailDueToOccupation:
                            commandGroup = Global.Settings.Event.BindingFailDueToOccupation;
                            break;
                        case Items.EventType.BindingFailDueToInvalid:
                            commandGroup = Global.Settings.Event.BindingFailDueToInvalid;
                            break;
                        case Items.EventType.BindingFailDueToAlreadyBinded:
                            commandGroup = Global.Settings.Event.BindingFailDueToAlreadyBinded;
                            break;
                        case Items.EventType.UnbindingSucceed:
                            commandGroup = Global.Settings.Event.UnbindingSucceed;
                            break;
                        case Items.EventType.UnbindingFail:
                            commandGroup = Global.Settings.Event.UnbindingFail;
                            break;
                        case Items.EventType.GroupIncrease:
                            commandGroup = Global.Settings.Event.GroupIncrease;
                            break;
                        case Items.EventType.GroupDecrease:
                            commandGroup = Global.Settings.Event.GroupDecrease;
                            break;
                        case Items.EventType.GroupPoke:
                            commandGroup = Global.Settings.Event.GroupPoke;
                            break;
                        case Items.EventType.PermissionDeniedFromPrivateMsg:
                            commandGroup = Global.Settings.Event.PermissionDeniedFromPrivateMsg;
                            break;
                        case Items.EventType.PermissionDeniedFromGroupMsg:
                            commandGroup = Global.Settings.Event.PermissionDeniedFromGroupMsg;
                            break;
                        default:
                            Logger.Out(Items.LogType.Debug, "未知的事件名", type);
                            break;
                    }
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
                    switch (type)
                    {
                        case Items.EventType.RequestingMotdpeSucceed:
                            commandGroup = Global.Settings.Event.RequestingMotdpeSucceed;
                            break;
                        case Items.EventType.RequestingMotdjeSucceed:
                            commandGroup = Global.Settings.Event.RequestingMotdjeSucceed;
                            break;
                        case Items.EventType.RequestingMotdFail:
                            commandGroup = Global.Settings.Event.RequestingMotdFail;
                            break;
                    }
                    foreach (string command in commandGroup)
                    {
                        string command_copy = command;
                        if (motd != null && Regex.IsMatch(command, @"%(Version|GameMode|OnlinePlayer|MaxPlayer|Description|Protocol|Original|Delay|Favicon|Exception)%", RegexOptions.IgnoreCase))
                        {
                            command_copy = Regex.Replace(command_copy, "%GameMode%", motd.GameMode, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Description%", motd.Description, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Protocol%", motd.Protocol, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%OnlinePlayer%", motd.OnlinePlayer, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%MaxPlayer%", motd.MaxPlayer, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Original%", motd.Origin, RegexOptions.IgnoreCase);
                            command_copy = Regex.Replace(command_copy, "%Delay%", motd.Delay.TotalMilliseconds.ToString("N2"), RegexOptions.IgnoreCase);
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
