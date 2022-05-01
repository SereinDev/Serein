using System;
using System.Threading;
using System.Windows.Forms;

namespace Serein
{
    class Update 
    {
        
    }

    public partial class Ui : Form
    {
        private void UpdateInfo()
        {
            while (Global.Alive)
            {
                if (!Visible)
                {
                    Thread.CurrentThread.Join(5000);
                }
                if (Server.Status && Global.Alive)
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

                }
                else if(Global.Alive)
                {
                    PanelInfoStatus2_Update("未启动");
                    PanelInfoVersion2_Update("-");
                    PanelInfoDifficulty2_Update("-");
                    PanelInfoPort2_Update("- / -");
                    PanelInfoTime2_Update("-");
                    PanelInfoCPU2_Update("-");
                }
                Thread.CurrentThread.Join(2000);
            }
            Thread.CurrentThread.Abort();
        }
        private void PanelInfoStatus2_Update(string str)
        {
            if (PanelInfoStatus2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoStatus2.Text = x.ToString(); };
                PanelInfoStatus2.Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoVersion2_Update(string str)
        {
            if (PanelInfoVersion2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoVersion2.Text = x.ToString(); };
                PanelInfoVersion2.Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoDifficulty2_Update(string str)
        {
            if (PanelInfoDifficulty2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoDifficulty2.Text = x.ToString(); };
                PanelInfoDifficulty2.Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoPort2_Update(string str)
        {
            if (PanelInfoPort2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoPort2.Text = x.ToString(); };
                PanelInfoPort2.Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoTime2_Update(string str)
        {
            if (PanelInfoTime2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoTime2.Text = x.ToString(); };
                PanelInfoTime2.Invoke(actionDelegate, str);
            }
        }
        private void PanelInfoCPU2_Update(string str)
        {
            if (PanelInfoCPU2.InvokeRequired)
            {
                Action<string> actionDelegate = (x) => { PanelInfoCPU2.Text = x.ToString(); };
                PanelInfoCPU2.Invoke(actionDelegate, str);
            }
        }
    }
}
