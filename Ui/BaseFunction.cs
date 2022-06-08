using Serein.baseFunction;
using System;
using System.Collections.Generic;
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
                    _ = PanelInfoTime2.Invoke(actionDelegate, Text);
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
            string FileName;
            foreach (object File in (Array)e.Data.GetData(DataFormats.FileDrop))
            {
                Count++;
            }
            if (Count == 1)
            {
                FocusWindow();
                FileName = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                if (FileName.ToUpper().EndsWith(".EXE") || FileName.ToUpper().EndsWith(".BAT"))
                {
                    if ((int)MessageBox.Show(
                        this,
                        $"是否以\"{FileName}\"为启动文件？",
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
                else if (Path.GetFileName(FileName) == "regex.tsv")
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
                else if (Path.GetFileName(FileName) == "task.tsv")
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
            if (Count > 0)
            {
                List<string> AcceptableList = new() { ".py", ".dll", ".js", ".go", ".jar" };
                List<string> FileList = new() { };
                string FileListText = "";
                foreach (object File in (Array)e.Data.GetData(DataFormats.FileDrop))
                {
                    if (AcceptableList.Contains(Path.GetExtension(File.ToString())))
                    {
                        FileList.Add(File.ToString());
                        FileListText = FileListText + Path.GetFileName(File.ToString()) + "\n";
                    }
                }
                if (FileList.Count > 0 &&
                    (int)MessageBox.Show(this,
                    $"是否将以下文件复制到插件文件夹内？\n{FileListText}",
                    "Serein",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                        ) == 1)
                {
                    Plugins.Add(FileList);
                    LoadPlugins();
                }
                else if (FileList.Count == 0 && Count > 0)
                {
                    _ = MessageBox.Show(this,
                        ":(\n无法识别所选文件",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        );
                }
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
