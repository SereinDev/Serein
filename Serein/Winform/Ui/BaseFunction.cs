using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Utils;
using Serein.Core.JSPlugin;
using Serein.Core.Server;
using System;
using System.Collections.Generic;
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
                    LoadJSPlugin();
                    break;
            }
        }

        private void SettingSereinShowWelcomePage_Click(object sender, EventArgs e)
            => Runtime.ShowWelcomePage();

        private void Ui_Shown(object sender, EventArgs e)
            => Runtime.Start();

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
                JSFunc.Trigger(Base.EventType.SereinClose);
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
                ServerPanelInfoTime2.Invoke(action, text);
            }
        }

        private void Ui_DragDrop(object sender, DragEventArgs e)
        {
            Array data = (Array)e.Data.GetData(DataFormats.FileDrop);
            string filename;
            List<string> extensionsList = new List<string> { ".exe", ".bat", ".json", ".tsv" };
            if (
                data.Length == 1 &&
                extensionsList.Contains(
                    Path.GetExtension(
                        data.GetValue(0).ToString()
                        ).ToLowerInvariant()
                    )
                )
            {
                FocusWindow();
                filename = data.GetValue(0).ToString();
                if (
                    Path.GetExtension(filename).ToLowerInvariant() == ".exe" ||
                    Path.GetExtension(filename).ToLowerInvariant() == ".bat"
                    )
                {
                    if ((int)MessageBox.Show(
                        this,
                        $"是否以\"{filename}\"为启动文件？",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        ) == 1)
                    {
                        SettingServerPath.Text = filename;
                        Global.Settings.Server.Path = filename;
                    }
                    LoadPlugins();
                }
                else if (Path.GetExtension(filename).ToLowerInvariant() == ".json")
                {
                    StreamReader streamReader = new StreamReader(
                        File.Open(
                            filename,
                            FileMode.Open
                        ),
                        Encoding.UTF8
                        );
                    JObject jsonObject = (JObject)JsonConvert.DeserializeObject(streamReader.ReadToEnd());
                    streamReader.Close();
                    if (
                        jsonObject["type"].ToString().ToUpperInvariant() == "REGEX")
                    {
                        DialogResult dialogResult = MessageBox.Show(
                            this,
                            $"确定要从{filename}导入正则记录并合并吗？\n否则将覆盖原有文件\n二者均将覆盖原有文件且不可逆",
                            "Serein",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Warning
                        );
                        if (dialogResult != DialogResult.Cancel)
                        {
                            LoadRegex(filename, dialogResult == DialogResult.Yes);
                            SaveRegex();
                        }
                    }
                    else if (jsonObject["type"].ToString().ToUpperInvariant() == "TASK"
                        &&
                        (int)MessageBox.Show(
                            this,
                            "确定从{filename}导入定时任务吗？\n此操作将覆盖原有文件且不可逆",
                            "Serein",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Warning
                        ) == 1)
                    {
                        LoadTask(filename);
                        SaveTask();
                    }
                }
                else if (
                    Path.GetFileName(filename).ToLowerInvariant() == "regex.tsv" &&
                    (int)MessageBox.Show(
                        this,
                        "是否导入正则记录？\n将覆盖原有文件且不可逆",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        ) == 1)
                {
                    LoadRegex(filename);
                    SaveRegex();
                }
                else if (
                    Path.GetFileName(filename).ToLowerInvariant() == "task.tsv" &&
                    (int)MessageBox.Show(
                        this,
                        "是否导入定时任务？\n将覆盖原有文件且不可逆",
                        "Serein",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                        ) == 1)
                {
                    LoadTask(filename);
                    SaveTask();
                }
            }
            else if (data.Length > 0 && PluginManager.TryImport(data))
            {
                LoadPlugins();
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
