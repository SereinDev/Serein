using Serein.Items.Motd;
using System.Text.RegularExpressions;

namespace Serein.Base
{
    internal class EventTrigger
    {
        /// <summary>
        /// 触发指定事件
        /// </summary>
        /// <param name="Type">类型</param>
        /// <param name="GroupId">群聊ID</param>
        /// <param name="UserId">用户ID</param>
        /// <param name="motd">Motd对象</param>
        public static void Trigger(
            string Type,
            long GroupId = -1,
            long UserId = -1,
            Motd Motd = null
            )
        {
            Logger.Out(Items.LogType.Debug, "[EventTrigger]", "Trigger:" + Type);
            string[] CommandGroup = new string[] { };
            if (
                Type.StartsWith("Bind_") ||
                Type.StartsWith("Unbind_")
                )
            {
                switch (Type)
                {
                    case "Bind_Success":
                        CommandGroup = Global.Settings.Event.Bind_Success;
                        break;
                    case "Bind_Occupied":
                        CommandGroup = Global.Settings.Event.Bind_Occupied;
                        break;
                    case "Bind_Invalid":
                        CommandGroup = Global.Settings.Event.Bind_Invalid;
                        break;
                    case "Bind_Already":
                        CommandGroup = Global.Settings.Event.Bind_Already;
                        break;
                    case "Unbind_Success":
                        CommandGroup = Global.Settings.Event.Unbind_Success;
                        break;
                    case "Unbind_Failure":
                        CommandGroup = Global.Settings.Event.Unbind_Failure;
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
            }
            else if (Type.StartsWith("Server_"))
            {
                switch (Type)
                {
                    case "Server_Start":
                        CommandGroup = Global.Settings.Event.Server_Start;
                        break;
                    case "Server_Stop":
                        CommandGroup = Global.Settings.Event.Server_Stop;
                        break;
                    case "Server_Error":
                        CommandGroup = Global.Settings.Event.Server_Error;
                        break;
                }
                foreach (string Command in CommandGroup)
                {
                    Base.Command.Run(4, Command);
                }
            }
            else if (Type.StartsWith("Group_"))
            {
                switch (Type)
                {
                    case "Group_Increase":
                        CommandGroup = Global.Settings.Event.Group_Increase;
                        break;
                    case "Group_Decrease":
                        CommandGroup = Global.Settings.Event.Group_Decrease;
                        break;
                    case "Group_Poke":
                        CommandGroup = Global.Settings.Event.Group_Poke;
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
            }
            else if (Type == "Serein_Crash")
            {
                CommandGroup = Global.Settings.Event.Serein_Crash;
                foreach (string Command in CommandGroup)
                {
                    Base.Command.Run(
                        4,
                        Command
                        );
                }
            }
            else if (Type.StartsWith("Motd"))
            {
                switch (Type)
                {
                    case "Motdpe_Success":
                        CommandGroup = Global.Settings.Event.Motdpe_Success;
                        break;
                    case "Motdje_Success":
                        CommandGroup = Global.Settings.Event.Motdje_Success;
                        break;
                    case "Motd_Failure":
                        foreach (string Command in Global.Settings.Event.Motd_Failure)
                        {
                            Base.Command.Run(
                                4,
                                Regex.Replace(Command, "%Exception%", Motd.Exception, RegexOptions.IgnoreCase),
                                GroupId: GroupId,
                                DisableMotd: true
                                );
                        }
                        break;
                }
                foreach (string Command in CommandGroup)
                {
                    string Command_Copy = Command;
                    if (Regex.IsMatch(Command, @"%(Version|GameMode|OnlinePlayer|MaxPlayer|Description|Protocol|Original|Delay|Favicon)%", RegexOptions.IgnoreCase))
                    {
                        Command_Copy = Regex.Replace(Command_Copy, "%GameMode%", Motd.GameMode, RegexOptions.IgnoreCase);
                        Command_Copy = Regex.Replace(Command_Copy, "%Description%", Motd.Description, RegexOptions.IgnoreCase);
                        Command_Copy = Regex.Replace(Command_Copy, "%Protocol%", Motd.Protocol, RegexOptions.IgnoreCase);
                        Command_Copy = Regex.Replace(Command_Copy, "%OnlinePlayer%", Motd.OnlinePlayer, RegexOptions.IgnoreCase);
                        Command_Copy = Regex.Replace(Command_Copy, "%MaxPlayer%", Motd.MaxPlayer, RegexOptions.IgnoreCase);
                        Command_Copy = Regex.Replace(Command_Copy, "%Original%", Motd.Original, RegexOptions.IgnoreCase);
                        Command_Copy = Regex.Replace(Command_Copy, "%Delay%", Motd.Delay.TotalMilliseconds.ToString("N2"), RegexOptions.IgnoreCase);
                        Command_Copy = Regex.Replace(Command_Copy, "%Version%", Motd.Version, RegexOptions.IgnoreCase);
                        Command_Copy = Regex.Replace(Command_Copy, "%Favicon%", Motd.Favicon, RegexOptions.IgnoreCase);
                    }
                    Base.Command.Run(
                        4,
                        Command_Copy,
                        GroupId: GroupId,
                        DisableMotd: true
                        );
                }
            }
            else if (Type.StartsWith("PermissionDenied_"))
            {
                switch (Type)
                {
                    case "PermissionDenied_Private":
                        CommandGroup = Global.Settings.Event.PermissionDenied_Private;
                        break;
                    case "PermissionDenied_Group":
                        CommandGroup = Global.Settings.Event.PermissionDenied_Group;
                        break;
                }
                foreach (string Command in CommandGroup)
                {
                    Base.Command.Run(
                        4,
                        Regex.Replace(Command, "%ID%", UserId.ToString(), RegexOptions.IgnoreCase),
                        UserId: UserId,
                        GroupId: GroupId
                        );
                }
            }
            else
            {
                Logger.Out(Items.LogType.Debug, "[EventTrigger]", "未知的事件名" + Type);
            }
        }
    }
}
