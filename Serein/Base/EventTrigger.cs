using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Serein.Base
{
    internal class EventTrigger
    {
        public static void Trigger(string Type, long GroupId = -1, long UserId = -1)
        {
            Global.Debug("[EventTrigger] Trigger: " + Type);
            List<string> CommandGroup = new List<string>();
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
                        Regex.Replace(Command, "%ID%", UserId.ToString()),
                        Default: GroupId
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
                    Base.Command.Run(Command);
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
                        Regex.Replace(Command, "%ID%", UserId.ToString()),
                        Default: GroupId
                        );
                }
            }
            else if (Type == "Serein_Crash")
            {
                CommandGroup = Global.Settings.Event.Serein_Crash;
                foreach (string Command in CommandGroup)
                {
                    Base.Command.Run(Command);
                }
            }
            else
            {
                Global.Debug("[EventTrigger] 未知的事件名" + Type);
            }
        }
    }
}
