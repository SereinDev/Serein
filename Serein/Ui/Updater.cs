using Serein.Base;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private void UpdateInfo()
        {
            TimeSpan t;
            while (true)
            {
                if (!Visible)
                {
                    Thread.Sleep(5000);
                }
                if (Global.Crash)
                {
                    Ui_Update("Serein | 崩溃啦:(");
                }
                else if (Server.Status)
                {
                    PanelInfoTime2_Update(Server.GetTime());
                    PanelInfoCPU2_Update($"{Server.CPUPersent:N2}%");
                    PanelInfoStatus2_Update("已启动");
                    Ui_Update($"Serein | {Server.StartFileName}");
                }
                else
                {
                    Ui_Update("Serein");
                    PanelInfoStatus2_Update("未启动");
                    PanelInfoVersion2_Update("-");
                    PanelInfoDifficulty2_Update("-");
                    PanelInfoLevel2_Update("-");
                    PanelInfoTime2_Update("-");
                    PanelInfoCPU2_Update("-");
                }
                if (Websocket.Status)
                {
                    BotInfoStatus2_Update("已连接");
                    BotInfoQQ2_Update(Base.Message.SelfId);
                    BotInfoMessageReceived2_Update(Base.Message.MessageReceived);
                    BotInfoMessageSent2_Update(Base.Message.MessageSent);
                    t = DateTime.Now - Websocket.StartTime;
                    if (t.TotalSeconds < 3600)
                    {
                        BotInfoTime2_Update($"{t.TotalSeconds / 60:N1}m");
                    }
                    else if (t.TotalHours < 120)
                    {
                        BotInfoTime2_Update($"{t.TotalMinutes / 60:N1}h");
                    }
                    else
                    {
                        BotInfoTime2_Update($"{t.TotalHours / 24:N2}d");
                    }
                }
                else
                {
                    BotInfoStatus2_Update("未连接");
                    BotInfoQQ2_Update("-");
                    BotInfoMessageReceived2_Update("-");
                    BotInfoMessageSent2_Update("-");
                    BotInfoTime2_Update("-");
                }
                Thread.Sleep(2000);
            }
        }
        public void UpdateServerInfo(string LevelName, string Version, string Difficulty)
        {
            PanelInfoVersion2_Update(Version);
            PanelInfoLevel2_Update(LevelName);
            PanelInfoDifficulty2_Update(Difficulty);
        }
        private void Ui_Update(string NewText)
        {
            if (InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { this.Text = Text; };
                Invoke(actionDelegate, NewText);
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
        private void PanelInfoStatus2_Update(string NewText)
        {
            if (PanelInfoStatus2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { PanelInfoStatus2.Text = Text; };
                PanelInfoStatus2.Invoke(actionDelegate, NewText);
            }
        }
        private void PanelInfoVersion2_Update(string NewText)
        {
            if (PanelInfoVersion2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { PanelInfoVersion2.Text = Text; };
                PanelInfoVersion2.Invoke(actionDelegate, NewText);
            }
        }
        private void PanelInfoDifficulty2_Update(string NewText)
        {
            if (PanelInfoDifficulty2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { PanelInfoDifficulty2.Text = Text; };
                PanelInfoDifficulty2.Invoke(actionDelegate, NewText);
            }
        }
        private void PanelInfoLevel2_Update(string NewText)
        {
            if (PanelInfoLevel2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { PanelInfoLevel2.Text = Text; };
                PanelInfoLevel2.Invoke(actionDelegate, NewText);
            }
        }
        private void PanelInfoTime2_Update(string NewText)
        {
            if (PanelInfoTime2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { PanelInfoTime2.Text = Text; };
                PanelInfoTime2.Invoke(actionDelegate, NewText);
            }
        }
        private void PanelInfoCPU2_Update(string NewText)
        {
            if (PanelInfoCPU2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { PanelInfoCPU2.Text = Text; };
                PanelInfoCPU2.Invoke(actionDelegate, NewText);
            }
        }
        private void BotInfoStatus2_Update(string NewText)
        {
            if (BotInfoStatus2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { BotInfoStatus2.Text = Text; };
                BotInfoStatus2.Invoke(actionDelegate, NewText);
            }
        }
        private void BotInfoQQ2_Update(string NewText)
        {
            if (BotInfoQQ2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { BotInfoQQ2.Text = Text; };
                BotInfoQQ2.Invoke(actionDelegate, NewText);
            }
        }
        private void BotInfoMessageReceived2_Update(string NewText)
        {
            if (BotInfoMessageReceived2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { BotInfoMessageReceived2.Text = Text; };
                BotInfoMessageReceived2.Invoke(actionDelegate, NewText);
            }
        }
        private void BotInfoMessageSent2_Update(string NewText)
        {
            if (BotInfoMessageSent2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { BotInfoMessageSent2.Text = Text; };
                BotInfoMessageSent2.Invoke(actionDelegate, NewText);
            }
        }
        private void BotInfoTime2_Update(string NewText)
        {
            if (BotInfoTime2.InvokeRequired)
            {
                Action<string> actionDelegate = (Text) => { BotInfoTime2.Text = Text; };
                BotInfoTime2.Invoke(actionDelegate, NewText);
            }
        }

    }
}
