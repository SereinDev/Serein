using System;
using System.IO;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        private void FocusWindow()
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();
        }
        private void SereinIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            FocusWindow();
        }

        private void SereinIcon_MouseClick(object sender, MouseEventArgs e)
        {
            FocusWindow();
        }

        private void Serein_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Server.Status)
            {
                e.Cancel = true;
                Visible = false;
                ShowInTaskbar = false;
                ShowBalloonTip("服务器进程仍在运行中\n已自动最小化至托盘，点击托盘图标即可复原窗口");
            }
        }
        public void ShowBalloonTip(string text)
        {
            SereinIcon.BalloonTipTitle = "Serein";
            SereinIcon.BalloonTipText = text;
            SereinIcon.ShowBalloonTip(10000);
        }
        private void Debug_Append(string Text)
        {
            if (Global.Settings_Serein.Debug)
            {
                if (DebugTextBox.InvokeRequired)
                {
                    Action<string> actionDelegate = (_Text) => { DebugTextBox.Text = DebugTextBox.Text + "\n" + _Text; };
                    PanelInfoTime2.Invoke(actionDelegate, Text);
                }
                else
                {
                    DebugTextBox.Text = DebugTextBox.Text + "\n" + Text;
                }

            }
        }
        private void Ui_DragDrop(object sender, DragEventArgs e)
        {
            int Count = 0;
            foreach (object File in (Array)e.Data.GetData(DataFormats.FileDrop))
            {
                Count++;
            }
            if (Count <= 0)
            {
                return;
            }
            else if (Count == 1)
            {
                FocusWindow();
                string FileName = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                if (FileName.ToUpper().EndsWith(".EXE") || FileName.ToUpper().EndsWith(".BAT"))
                {
                    if ((int)MessageBox.Show(
                        this,
                        "是否以此文件为启动文件？",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        ) == 1)
                    {
                        SettingServerPath.Text = FileName;
                        Global.Settings_Server.Path = FileName;
                    }
                    LoadPlugins();
                    return;
                }
                else if (FileName.ToUpper().EndsWith("REGEX.TSV"))
                {
                    if ((int)MessageBox.Show(
                        this,
                        "是否导入正则记录？\n将覆盖原有文件且不可逆",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        ) == 1)
                    {
                        LoadRegex(FileName);
                        SaveRegex();
                    }
                    return;
                }
                else if (FileName.ToUpper().EndsWith("TASK.TSV"))
                {
                    if ((int)MessageBox.Show(
                        this,
                        "是否导入定时任务？\n将覆盖原有文件且不可逆",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        ) == 1)
                    {
                        LoadTask(FileName);
                        SaveTask();
                    }
                    return;
                }
            }
            string FirstFileName = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if ((int)MessageBox.Show(this,
                $"是否将{Path.GetFileName(FirstFileName)}等文件复制到插件文件夹内？",
                "Serein",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
                    ) == 1)
            {
                Plugins.Add((Array)e.Data.GetData(DataFormats.FileDrop));
                LoadPlugins();
            }

        }
        private void Ui_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;                                                              //重要代码：表明是所有类型的数据，比如文件路径
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void UpdateVersion()
        {
            SettingSereinVersion.Text = $"当前版本：{Global.VERSION}";
            Debug_Append(Global.VERSION);
        }
    }
}
