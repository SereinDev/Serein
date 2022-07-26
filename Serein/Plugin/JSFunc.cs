using System.Collections.Generic;
using System;
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
        public static bool SetListener(string EventName, Delegate Function)
        {
            if (string.IsNullOrEmpty(EventName) || string.IsNullOrWhiteSpace(EventName))
                return false;
            switch (EventName)
            {
                case "onServerStart":
                    Plugins.Event.onServerStart.Add(Function);
                    break;
                case "onServerStop":
                    Plugins.Event.onServerStop.Add(Function);
                    break;
                case "onServerSendCommand":
                    Plugins.Event.onServerSendCommand.Add(Function);
                    break;
                case "onGroupIncrease":
                    Plugins.Event.onGroupIncrease.Add(Function);
                    break;
                case "onGroupDecrease":
                    Plugins.Event.onGroupDecrease.Add(Function);
                    break;
                case "onGroupPoke":
                    Plugins.Event.onGroupPoke.Add(Function);
                    break;
                case "onReceiveGroupMessage":
                    Plugins.Event.onReceiveGroupMessage.Add(Function);
                    break;
                case "onReceivePrivateMessage":
                    Plugins.Event.onReceivePrivateMessage.Add(Function);
                    break;
                case "onSereinStart":
                    Plugins.Event.onSereinStart.Add(Function);
                    break;
                case "onSereinClose":
                    Plugins.Event.onSereinClose.Add(Function);
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 触发插件事件
        /// </summary>
        /// <param name="EventName">事件名称</param>
        /// <param name="Args">参数</param>
        public static void Trigger(string EventName, params object[] Args)
        {
            Global.Debug("[JSFunc:Tigger()] " + EventName);
            try
            {
                switch (EventName)
                {
                    case "onServerStart":
                        Plugins.Event.onServerStart.ForEach((x) => x.DynamicInvoke());
                        break;
                    case "onServerStop":
                        Plugins.Event.onServerStop.ForEach((x) => x.DynamicInvoke());
                        break;
                    case "onServerSendCommand":
                        Plugins.Event.onServerSendCommand.ForEach((x) => x.DynamicInvoke(Args[0]));
                        break;
                    case "onGroupIncrease":
                        Plugins.Event.onGroupIncrease.ForEach((x) => x.DynamicInvoke(Args));
                        break;
                    case "onGroupDecrease":
                        Plugins.Event.onGroupDecrease.ForEach((x) => x.DynamicInvoke(Args));
                        break;
                    case "onGroupPoke":
                        Plugins.Event.onGroupPoke.ForEach((x) => x.DynamicInvoke(Args));
                        break;
                    case "onReceiveGroupMessage":
                        Plugins.Event.onReceiveGroupMessage.ForEach((x) => x.DynamicInvoke(Args));
                        break;
                    case "onReceivePrivateMessage":
                        Plugins.Event.onReceivePrivateMessage.ForEach((x) => x.DynamicInvoke(Args));
                        break;
                    case "onSereinStart":
                        Plugins.Event.onSereinStart.ForEach((x) => x.DynamicInvoke());
                        break;
                    case "onSereinClose":
                        Plugins.Event.onSereinClose.ForEach((x) => x.DynamicInvoke());
                        break;
                    default:
                        return;
                }
            }
            catch (Exception e)
            {
                Global.Debug(e.ToString());
            }
        }
        public static bool RegisterCommand(string Command)
        {
            if (Command.Contains(" ") || Plugins.Commands.Contains(Command))
                return false;
            else
            {
                Plugins.Commands.Add(Command);
                return true;
            }
        }
    }
}
