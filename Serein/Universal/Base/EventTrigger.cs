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
        /// <param name="Type">类型</param>
        public static void Trigger(Items.EventType Type)
        {
            Logger.Out(Items.LogType.Debug, "Trigger:" + Type);
            string[] CommandGroup = Array.Empty<string>();
            switch (Type)
            {
                case Items.EventType.ServerStart:
                    CommandGroup = Global.Settings.Event.ServerStart;
                    break;
                case Items.EventType.ServerStop:
                    CommandGroup = Global.Settings.Event.ServerStop;
                    break;
                case Items.EventType.ServerExitUnexpectedly:
                    CommandGroup = Global.Settings.Event.ServerExitUnexpectedly;
                    break;
                case Items.EventType.SereinCrash:
                    CommandGroup = Global.Settings.Event.SereinCrash;
                    break;
                default:
                    Logger.Out(Items.LogType.Debug, "未知的事件名", Type);
                    break;
            }
            foreach (string Command in CommandGroup)
            {
                Base.Command.Run(4, Command);
            }
        }

        /// <summary>
        /// 触发指定事件
        /// </summary>
        /// <param name="Type">类型</param>
        /// <param name="GroupId">群聊ID</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="Motd">Motd对象</param>
        public static void Trigger(Items.EventType Type, long GroupId, long UserId, Motd Motd = null)
        {
            Logger.Out(Items.LogType.Debug, "Trigger:" + Type);
            string[] CommandGroup = Array.Empty<string>();
            switch (Type)
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
                    switch (Type)
                    {
                        case Items.EventType.BindingSucceed:
                            CommandGroup = Global.Settings.Event.BindingSucceed;
                            break;
                        case Items.EventType.BindingFailDueToOccupation:
                            CommandGroup = Global.Settings.Event.BindingFailDueToOccupation;
                            break;
                        case Items.EventType.BindingFailDueToInvalid:
                            CommandGroup = Global.Settings.Event.BindingFailDueToInvalid;
                            break;
                        case Items.EventType.BindingFailDueToAlreadyBinded:
                            CommandGroup = Global.Settings.Event.BindingFailDueToAlreadyBinded;
                            break;
                        case Items.EventType.UnbindingSucceed:
                            CommandGroup = Global.Settings.Event.UnbindingSucceed;
                            break;
                        case Items.EventType.UnbindingFail:
                            CommandGroup = Global.Settings.Event.UnbindingFail;
                            break;
                        case Items.EventType.GroupIncrease:
                            CommandGroup = Global.Settings.Event.GroupIncrease;
                            break;
                        case Items.EventType.GroupDecrease:
                            CommandGroup = Global.Settings.Event.GroupDecrease;
                            break;
                        case Items.EventType.GroupPoke:
                            CommandGroup = Global.Settings.Event.GroupPoke;
                            break;
                        case Items.EventType.PermissionDeniedFromPrivateMsg:
                            CommandGroup = Global.Settings.Event.PermissionDeniedFromPrivateMsg;
                            break;
                        case Items.EventType.PermissionDeniedFromGroupMsg:
                            CommandGroup = Global.Settings.Event.PermissionDeniedFromGroupMsg;
                            break;
                        default:
                            Logger.Out(Items.LogType.Debug, "未知的事件名", Type);
                            break;
                    }
                    foreach (string Command in CommandGroup)
                    {
                        Base.Command.Run(
                            4,
                            Regex.Replace(Command, "%ID%", UserId.ToString(), RegexOptions.IgnoreCase),
                            GroupId: GroupId
                            );
                    }
                    break;
                case Items.EventType.RequestingMotdpeSucceed:
                case Items.EventType.RequestingMotdjeSucceed:
                case Items.EventType.RequestingMotdFail:
                    switch (Type)
                    {
                        case Items.EventType.RequestingMotdpeSucceed:
                            CommandGroup = Global.Settings.Event.RequestingMotdpeSucceed;
                            break;
                        case Items.EventType.RequestingMotdjeSucceed:
                            CommandGroup = Global.Settings.Event.RequestingMotdjeSucceed;
                            break;
                        case Items.EventType.RequestingMotdFail:
                            CommandGroup = Global.Settings.Event.RequestingMotdFail;
                            break;
                    }
                    foreach (string Command in CommandGroup)
                    {
                        string Command_Copy = Command;
                        if (Motd != null && Regex.IsMatch(Command, @"%(Version|GameMode|OnlinePlayer|MaxPlayer|Description|Protocol|Original|Delay|Favicon|Exception)%", RegexOptions.IgnoreCase))
                        {
                            Command_Copy = Regex.Replace(Command_Copy, "%GameMode%", Motd.GameMode, RegexOptions.IgnoreCase);
                            Command_Copy = Regex.Replace(Command_Copy, "%Description%", Motd.Description, RegexOptions.IgnoreCase);
                            Command_Copy = Regex.Replace(Command_Copy, "%Protocol%", Motd.Protocol, RegexOptions.IgnoreCase);
                            Command_Copy = Regex.Replace(Command_Copy, "%OnlinePlayer%", Motd.OnlinePlayer, RegexOptions.IgnoreCase);
                            Command_Copy = Regex.Replace(Command_Copy, "%MaxPlayer%", Motd.MaxPlayer, RegexOptions.IgnoreCase);
                            Command_Copy = Regex.Replace(Command_Copy, "%Original%", Motd.Origin, RegexOptions.IgnoreCase);
                            Command_Copy = Regex.Replace(Command_Copy, "%Delay%", Motd.Delay.TotalMilliseconds.ToString("N2"), RegexOptions.IgnoreCase);
                            Command_Copy = Regex.Replace(Command_Copy, "%Version%", Motd.Version, RegexOptions.IgnoreCase);
                            Command_Copy = Regex.Replace(Command_Copy, "%Favicon%", Motd.Favicon, RegexOptions.IgnoreCase);
                            Command_Copy = Regex.Replace(Command_Copy, "%Exception%", Motd.Exception, RegexOptions.IgnoreCase);
                        }
                        Base.Command.Run(
                            4,
                            Command_Copy,
                            UserId: UserId,
                            GroupId: GroupId,
                            DisableMotd: true
                            );
                    }
                    break;
                default:
                    Logger.Out(Items.LogType.Debug, "未知的事件名", Type);
                    break;
            }
        }
    }
}
