using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ookii.Dialogs.Wpf;
using Serein.Base;
using Serein.JSPlugin;
using Serein.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private bool IsDragging;
        private ListViewItem ItemDraged;
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl.SelectedIndex)
            {
                case 5:
                    LoadMember();
                    break;
                case 6:
                    LoadSereinPlugin();
                    break;
            }
        }

        private void SettingSereinShowWelcomePage_Click(object sender, EventArgs e)
        {
            AutoRun.ShowWelcomePage();
        }

        private void Ui_Shown(object sender, EventArgs e)
        {
            AutoRun.Check();
        }

        private void Ui_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ServerManager.Status)
            {
                e.Cancel = true;
                Visible = false;
                ShowInTaskbar = false;
                ShowBalloonTip("服务器进程仍在运行中\n已自动最小化至托盘，点击托盘图标即可复原窗口");
            }
            else
            {
                SereinIcon.Dispose();
                JSFunc.Trigger(Items.EventType.SereinClose);
            }
        }

        public void ShowBalloonTip(string text)
        {
            SereinIcon.BalloonTipTitle = "Serein";
            SereinIcon.BalloonTipText = text;
            SereinIcon.ShowBalloonTip(10000);
        }

        public void Debug_Append(string text)
        {
            if (Global.Settings.Serein.DevelopmentTool.EnableDebug)
            {
                Action<string> action = (_text) =>
                {
                    if (DebugTextBox.Text.Length > 50000)
                    {
                        DebugTextBox.Text = "";
                    }
                    DebugTextBox.Text = DebugTextBox.Text + _text + "\r\n";
                };
                PanelInfoTime2.Invoke(action, text);
            }
        }

        private void Ui_DragDrop(object sender, DragEventArgs e)
        {
            Array data = (Array)e.Data.GetData(DataFormats.FileDrop);
            string fileName;
            List<string> extensionsList = new List<string> { ".EXE", ".BAT", ".JSON", ".TSV" };
            if (
                data.Length == 1 &&
                extensionsList.Contains(
                    Path.GetExtension(
                        data.GetValue(0).ToString()
                        ).ToUpper()
                    )
                )
            {
                FocusWindow();
                fileName = data.GetValue(0).ToString();
                if (
                    Path.GetExtension(fileName).ToUpper() == ".EXE" ||
                    Path.GetExtension(fileName).ToUpper() == ".BAT"
                    )
                {
                    if ((int)MessageBox.Show(
                        this,
                        $"是否以\"{fileName}\"为启动文件？",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        ) == 1)
                    {
                        SettingServerPath.Text = fileName;
                        Global.Settings.Server.Path = fileName;
                    }
                    LoadPlugins();
                }
                else if (Path.GetExtension(fileName).ToUpper() == ".JSON")
                {
                    StreamReader streamReader = new StreamReader(
                        File.Open(
                            fileName,
                            FileMode.Open
                        ),
                        Encoding.UTF8
                        );
                    JObject jsonObject = (JObject)JsonConvert.DeserializeObject(streamReader.ReadToEnd());
                    streamReader.Close();
                    if (
                        jsonObject["type"].ToString().ToUpper() == "REGEX"
                        &&
                        (int)MessageBox.Show(
                            this,
                            "是否导入正则记录？\n将覆盖原有文件且不可逆",
                            "Serein",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Warning
                        ) == 1)
                    {
                        LoadRegex(fileName);
                        SaveRegex();
                    }
                    else if (jsonObject["type"].ToString().ToUpper() == "TASK"
                        &&
                        (int)MessageBox.Show(
                            this,
                            "是否导入定时任务？\n将覆盖原有文件且不可逆",
                            "Serein",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Warning
                        ) == 1)
                    {
                        LoadTask(fileName);
                        SaveTask();
                    }
                }
                else if (
                    Path.GetFileName(fileName).ToUpper() == "REGEX.TSV" &&
                    (int)MessageBox.Show(
                        this,
                        "是否导入正则记录？\n将覆盖原有文件且不可逆",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        ) == 1)
                {
                    LoadRegex(fileName);
                    SaveRegex();
                }
                else if (
                    Path.GetFileName(fileName).ToUpper() == "TASK.TSV" &&
                    (int)MessageBox.Show(
                        this,
                        "是否导入定时任务？\n将覆盖原有文件且不可逆",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        ) == 1)
                {
                    LoadTask(fileName);
                    SaveTask();
                }
                return;
            }
            if (data.Length > 0)
            {
                List<string> pluginExtensionsList = new List<string>() { ".py", ".dll", ".js", ".ts", ".jar" };
                List<string> fileList = new List<string>();
                string filePath = string.Empty;
                foreach (object file in data)
                {
                    if (pluginExtensionsList.Contains(Path.GetExtension(file.ToString().ToLower())))
                    {
                        fileList.Add(file.ToString());
                        filePath = filePath + Path.GetFileName(file.ToString()) + "\n";
                    }
                }
                if (fileList.Count > 0 &&
                    (int)MessageBox.Show(this,
                    $"是否将以下文件复制到插件文件夹内？\n{filePath}",
                    "Serein",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                        ) == 1)
                {
                    PluginManager.Add(fileList);
                    LoadPlugins();
                }
                else if (fileList.Count == 0 && data.Length > 0)
                {
                    MessageBox.Show(this,
                        ":(\n无法识别所选文件",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        );
                }
            }
        }


        private void UpdateVersion()
        {
            SettingSereinVersion.Text = $"当前版本：{Global.VERSION}";
            UpdateStatusLabel(Global.VERSION);
            DebugTextBox.Text = Global.VERSION + "\r\n";
        }

        private void FocusWindow()
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void SereinIcon_BalloonTipClicked(object sender, EventArgs e) => FocusWindow();
        private void SereinIcon_MouseClick(object sender, MouseEventArgs e) => FocusWindow();
        private void UpdateStatusLabel(string text) => StripStatusLabel.Text = text;
        private void Ui_DragEnter(object sender, DragEventArgs e) => e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
    }
}
