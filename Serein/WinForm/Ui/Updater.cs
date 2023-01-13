using Serein.Base;
using Serein.Server;
using Serein.Items.Motd;
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
                Name = $"Serein | {ServerManager.StartFileName}";
                PanelInfoTime2.Text = ServerManager.GetTime();
                PanelInfoCPU2.Text = $"{ServerManager.CPUUsage:N1}%";
                PanelInfoStatus2.Text = "已启动";
                Motd motd;
                if (Global.Settings.Server.Type == 1)
                {
                    motd = new Motdpe(newPort: Global.Settings.Server.Port.ToString());
                }
                else
                {
                    motd = new Motdje(newPort: Global.Settings.Server.Port.ToString());
                }
                PanelInfoVersion2.Text = motd.Version;
                PanelInfoLevel2.Text = ServerManager.LevelName;
                PanelInfoDifficulty2.Text = ServerManager.Difficulty;
            }
            else
            {
                Name = "Serein";
                PanelInfoStatus2.Text = "未启动";
                PanelInfoVersion2.Text = "-";
                PanelInfoDifficulty2.Text = "-";
                PanelInfoLevel2.Text = "-";
                PanelInfoTime2.Text = "-";
                PanelInfoCPU2.Text = "-";
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
