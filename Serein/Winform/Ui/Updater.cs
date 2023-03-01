using Serein.Core.Generic;
using Serein.Core.Server;
using Serein.Extensions;
using System;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        /// <summary>
        /// 更新计时器
        /// </summary>
        private readonly System.Timers.Timer UpdateInfoTimer = new(5000) { AutoReset = true };

        private void UpdateInfo()
        {
            if (!Visible)
            {
                return;
            }
            else if (ServerManager.Status)
            {
                Text = $"Serein | {(string.IsNullOrEmpty(ServerManager.StartFileName) ? "unknown" : ServerManager.StartFileName)}";
                ServerPanelInfoTime2.Text = ServerManager.Time;
                ServerPanelInfoCPU2.Text = $"{ServerManager.CPUUsage:N1}%";
                ServerPanelInfoStatus2.Text = "已启动";
                ServerPanelInfoVersion2.Text = ServerManager.Motd != null && !string.IsNullOrEmpty(ServerManager.Motd.Version) ? ServerManager.Motd.Version : "-";
                ServerPanelInfoPlayerCount2.Text = ServerManager.Motd != null ? $"{ServerManager.Motd.OnlinePlayer}/{ServerManager.Motd.MaxPlayer}" : "-";
                ServerPanelInfoDifficulty2.Text = ServerManager.Status && !string.IsNullOrEmpty(ServerManager.Difficulty) ? ServerManager.Difficulty : "-";
            }
            else
            {
                Text = "Serein";
                ServerPanelInfoStatus2.Text = "未启动";
                ServerPanelInfoVersion2.Text = "-";
                ServerPanelInfoDifficulty2.Text = "-";
                ServerPanelInfoPlayerCount2.Text = "-";
                ServerPanelInfoTime2.Text = "-";
                ServerPanelInfoCPU2.Text = "-";
            }
            if (Websocket.Status)
            {
                BotInfoStatus2.Text = "已连接";
                BotInfoQQ2.Text = Matcher.SelfId;
                BotInfoMessageReceived2.Text = Matcher.MessageReceived;
                BotInfoMessageSent2.Text = Matcher.MessageSent;
                BotInfoTime2.Text = (DateTime.Now - Websocket.StartTime).ToCustomString();
            }
            else
            {
                BotInfoStatus2.Text = "未连接";
                BotInfoQQ2.Text = "-";
                BotInfoMessageReceived2.Text = "-";
                BotInfoMessageSent2.Text = "-";
                BotInfoTime2.Text = "-";
            }
        }

        public void SettingSereinVersion_Update(string NewText)
        {
            if (SettingSereinVersion.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { SettingSereinVersion.Text = Text; };
                SettingSereinVersion.Invoke(actionDelegate, NewText);
            }
        }
    }
}
