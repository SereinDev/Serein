using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Serein.Plugin
{
    partial class JSFunc
    {
        public static bool Register(
            string Name,
            string Version,
            string Author,
            string Description
            )
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name) || Plugins.PluginNames.Contains(Name))
            {
                return false;
            }
            Plugins.PluginItems.Add(new PluginItem()
            {
                Name = Name,
                Version = Version,
                Author = Author,
                Description = Description
            });
            return true;
        }
        public static bool SetListener(string EventName, string FunctionName)
        {
            if (string.IsNullOrEmpty(EventName) || string.IsNullOrWhiteSpace(EventName) ||
                string.IsNullOrEmpty(FunctionName) || string.IsNullOrWhiteSpace(FunctionName) ||
                !Regex.IsMatch(FunctionName, @"^[A-Za-z_]+?$"))
                return false;
            switch (EventName)
            {
                case "onServerStart":
                    Plugins.Event.onServerStart.Add(FunctionName);
                    break;
                case "onServerStop":
                    Plugins.Event.onServerStop.Add(FunctionName);
                    break;
                case "onServerSendCommand":
                    Plugins.Event.onServerSendCommand.Add(FunctionName);
                    break;
                case "onGroupIncrease":
                    Plugins.Event.onGroupIncrease.Add(FunctionName);
                    break;
                case "onGroupDecrease":
                    Plugins.Event.onGroupDecrease.Add(FunctionName);
                    break;
                case "onGroupPoke":
                    Plugins.Event.onGroupPoke.Add(FunctionName);
                    break;
                case "onReceiveGroupMessage":
                    Plugins.Event.onReceiveGroupMessage.Add(FunctionName);
                    break;
                case "onReceivePrivateMessage":
                    Plugins.Event.onReceivePrivateMessage.Add(FunctionName);
                    break;
                case "onSereinStart":
                    Plugins.Event.onSereinStart.Add(FunctionName);
                    break;
                case "onSereinClose":
                    Plugins.Event.onSereinClose.Add(FunctionName);
                    break;
                default:
                    return false;
            }
            return true;
        }
        public static void Trigger(string EventName,params object[] Args)
        {
            Global.Debug("[JSFunc:Tigger()] "+EventName);
            List<string> TartgetEventGroup = new List<string>();
            switch (EventName)
            {
                case "onServerStart":
                    TartgetEventGroup = Plugins.Event.onServerStart;
                    break;
                case "onServerStop":
                    TartgetEventGroup = Plugins.Event.onServerStop;
                    break;
                case "onServerSendCommand":
                    TartgetEventGroup = Plugins.Event.onServerSendCommand;
                    break;
                case "onGroupIncrease":
                    TartgetEventGroup = Plugins.Event.onGroupIncrease;
                    break;
                case "onGroupDecrease":
                    TartgetEventGroup = Plugins.Event.onGroupDecrease;
                    break;
                case "onGroupPoke":
                    TartgetEventGroup = Plugins.Event.onGroupPoke;
                    break;
                case "onReceiveGroupMessage":
                    TartgetEventGroup = Plugins.Event.onReceiveGroupMessage;
                    break;
                case "onReceivePrivateMessage":
                    TartgetEventGroup = Plugins.Event.onReceivePrivateMessage;
                    break;
                case "onSereinStart":
                    TartgetEventGroup = Plugins.Event.onSereinStart;
                    break;
                case "onSereinClose":
                    TartgetEventGroup = Plugins.Event.onSereinClose;
                    break;
                default:
                    return;
            }
            foreach(string FunctionName in TartgetEventGroup)
            {
                JSEngine.Invoke(FunctionName, Args);
            }
        }
    }
}
