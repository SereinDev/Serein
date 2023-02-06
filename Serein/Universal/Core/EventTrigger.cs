using Serein.Base.Motd;
using Serein.Utils;
using System;
using System.Text.RegularExpressions;

namespace Serein.Core
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
                Base.EventType.ServerExitUnexpectedly => Global.Settings.Event.ServerExitUnexpectedly,
                Base.EventType.SereinCrash => Global.Settings.Event.SereinCrash,
                _ => Array.Empty<string>()
            };
            foreach (string command in commandGroup)
            {
                Command.Run(4, command);
            }
        }

        /// <summary>
        /// 触发指定事件
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="groupID">群聊ID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="motd">Motd对象</param>
        public static void Trigger(Base.EventType type, long groupID, long userID, Motd motd = null)
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
                case Base.EventType.GroupIncrease:
                case Base.EventType.GroupDecrease:
                case Base.EventType.GroupPoke:
                case Base.EventType.PermissionDeniedFromPrivateMsg:
                case Base.EventType.PermissionDeniedFromGroupMsg:
                    commandGroup = type switch
                    {
                        Base.EventType.BindingSucceed => Global.Settings.Event.BindingSucceed,
                        Base.EventType.BindingFailDueToOccupation => Global.Settings.Event.BindingFailDueToOccupation,
                        Base.EventType.BindingFailDueToInvalid => Global.Settings.Event.BindingFailDueToInvalid,
                        Base.EventType.BindingFailDueToAlreadyBinded => Global.Settings.Event.BindingFailDueToAlreadyBinded,
                        Base.EventType.UnbindingSucceed => Global.Settings.Event.UnbindingSucceed,
                        Base.EventType.UnbindingFail => Global.Settings.Event.UnbindingFail,
                        Base.EventType.BinderDisable => Global.Settings.Event.BinderDisable,
                        Base.EventType.GroupIncrease => Global.Settings.Event.GroupIncrease,
                        Base.EventType.GroupDecrease => Global.Settings.Event.GroupDecrease,
                        Base.EventType.GroupPoke => Global.Settings.Event.GroupPoke,
                        Base.EventType.PermissionDeniedFromPrivateMsg => Global.Settings.Event.PermissionDeniedFromPrivateMsg,
                        Base.EventType.PermissionDeniedFromGroupMsg => Global.Settings.Event.PermissionDeniedFromGroupMsg,
                        _ => commandGroup,
                    };
                    foreach (string command in commandGroup)
                    {
                        Command.Run(
                            4,
                            Regex.Replace(command, "%ID%", userID.ToString(), RegexOptions.IgnoreCase),
                            groupID: groupID
                            );
                    }
                    break;
                case Base.EventType.RequestingMotdpeSucceed:
                case Base.EventType.RequestingMotdjeSucceed:
                case Base.EventType.RequestingMotdFail:
                    commandGroup = type switch
                    {
                        Base.EventType.RequestingMotdpeSucceed => Global.Settings.Event.RequestingMotdpeSucceed,
                        Base.EventType.RequestingMotdjeSucceed => Global.Settings.Event.RequestingMotdjeSucceed,
                        Base.EventType.RequestingMotdFail => Global.Settings.Event.RequestingMotdFail,
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
                        Command.Run(
                            4,
                            command_copy,
                            null,
                            null,
                            userID,
                            groupID,
                            true
                            );
                    }
                    break;
                default:
                    Logger.Output(Base.LogType.Debug, "未知的事件名", type);
                    break;
            }
        }
    }
}
