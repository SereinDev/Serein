using System;
using System.Threading;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        private void UpdateInfo()
        {
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
                    TimeSpan t = DateTime.Now - Server.ServerProcess.StartTime;
                    if (t.TotalSeconds < 3600)
                    {
                        PanelInfoTime2_Update($"{(t.TotalSeconds / 60).ToString("N1")}m");
                    }
                    else if (t.TotalHours < 120)
                    {
                        PanelInfoTime2_Update($"{(t.TotalMinutes / 60).ToString("N1")}h");
                    }
                    else
                    {
                        PanelInfoTime2_Update($"{(t.TotalHours / 24).ToString("N2")}d");
                    }
                    PanelInfoCPU2_Update($"{Server.GetCPU():N2}%");
                    PanelInfoStatus2_Update("已启动");
                    Ui_Update($"Serein | {Server.StartFileName}");
                }
                else
                {
                    Ui_Update("Serein");
                    PanelInfoStatus2_Update("未启动");
                    PanelInfoVersion2_Update("-");
                    PanelInfoDifficulty2_Update("-");
                    PanelInfoPort2_Update("- / -");
                    PanelInfoTime2_Update("-");
                    PanelInfoCPU2_Update("-");
                }
                Thread.CurrentThread.Join(2000);
            }
        }
        private void Ui_Update(string str)
        {
            if (InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { Text = x; };
                Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoStatus2_Update(string str)
        {
            if (PanelInfoStatus2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoStatus2.Text = x; };
                PanelInfoStatus2.Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoVersion2_Update(string str)
        {
            if (PanelInfoVersion2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoVersion2.Text = x; };
                PanelInfoVersion2.Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoDifficulty2_Update(string str)
        {
            if (PanelInfoDifficulty2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoDifficulty2.Text = x; };
                PanelInfoDifficulty2.Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoPort2_Update(string str)
        {
            if (PanelInfoLevel2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoLevel2.Text = x; };
                PanelInfoLevel2.Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoTime2_Update(string str)
        {
            if (PanelInfoTime2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoTime2.Text = x; };
                PanelInfoTime2.Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoCPU2_Update(string str)
        {
            if (PanelInfoCPU2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoCPU2.Text = x; };
                PanelInfoCPU2.Invoke(actionDelegate, str);
            }
        }
    }
}
