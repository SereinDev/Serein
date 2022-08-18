using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ookii.Dialogs.Wpf;
using Serein.Base;
using Serein.Plugin;
using Serein.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private bool isdrag = false;
        private ListViewItem itemDraged;
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl.SelectedIndex)
            {
                case 5:
                    Members.Load();
                    LoadMembers();
                    break;
                case 6:
                    LoadSereinPlugin();
                    break;
            }
        }
        private void SettingSereinShowWelcomePage_Click(object sender, EventArgs e)
        {
            Global.FirstOpen = true;
            Ui_Shown(sender, e);
        }
        private void TaskDialog_HyperLinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Href) { UseShellExecute = true });
        }
        private void Ui_Shown(object sender, EventArgs e)
        {
            if (Global.FirstOpen)
            {
                Ookii.Dialogs.Wpf.TaskDialog TaskDialog = new Ookii.Dialogs.Wpf.TaskDialog
                {
                    Buttons = {
                        new Ookii.Dialogs.Wpf.TaskDialogButton(ButtonType.Ok)
                    },
                    MainInstruction = "欢迎使用Sereinヾ(≧▽≦*)o",
                    WindowTitle = "Serein",
                    Content = "" +
                    "如果你是第一次使用Serein，那么一定要仔细阅读以下内容，相信这些会对你有所帮助(๑•̀ㅂ•́)و✧\n" +
                    "◦ 实用教程：<a href=\"https://serein.cc/Tutorial.html\">https://serein.cc/Tutorial.html</a>\n" +
                    "◦ 帮助页面：<a href=\"https://serein.cc/Help.html\">https://serein.cc/Help.html</a>\n" +
                    "◦ 交流群：<a href=\"https://jq.qq.com/?_wv=1027&k=XNZqPSPv\">954829203</a>",
                    Footer = "此面板已发布在<a href=\"https://www.minebbs.com/resources/serein.4169/\">Minebbs</a>上，欢迎支持~",
                    FooterIcon = Ookii.Dialogs.Wpf.TaskDialogIcon.Information,
                    EnableHyperlinks = true,
                    ExpandedInformation = "此软件与Mojang Studio、网易、Microsoft没有从属关系.\n" +
                        "Serein is licensed under <a href=\"https://github.com/Zaitonn/Serein/blob/main/LICENSE\">GPL-v3.0</a>\n" +
                        "Copyright © 2022 <a href=\"https://github.com/Zaitonn\">Zaitonn</a>. All Rights Reserved.",
                };
                TaskDialog.HyperlinkClicked += new EventHandler<HyperlinkClickedEventArgs>(TaskDialog_HyperLinkClicked);
                TaskDialog.ShowDialog();
            }
            new Task(() => { if (Global.Args.Contains("auto_connect")) { Websocket.Connect(false); } }).Start();
            new Task(() => { if (Global.Args.Contains("auto_start")) { ServerManager.Start(true); } }).Start();
            new Task(
                () =>
                {
                    Plugins.Load();
                    JSFunc.Trigger("onSereinStart");
                }).Start();
        }
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
                JSFunc.Trigger("onSereinClose");
            }
        }
        public void ShowBalloonTip(string text)
        {
            SereinIcon.BalloonTipTitle = "Serein";
            SereinIcon.BalloonTipText = text;
            SereinIcon.ShowBalloonTip(10000);
        }
        public void Debug_Append(string Text)
        {
            if (Global.Settings.Serein.Debug)
            {
                if (DebugTextBox.InvokeRequired)
                {
                    Action<string> actionDelegate = (_Text) =>
                    {
                        if (DebugTextBox.Text.Length > 50000)
                        {
                            DebugTextBox.Text = "";
                        }
                        DebugTextBox.Text = DebugTextBox.Text + _Text + "\r\n";
                    };
                    PanelInfoTime2.Invoke(actionDelegate, Text);
                }
                else
                {
                    if (DebugTextBox.Text.Length > 50000)
                    {
                        DebugTextBox.Text = "";
                    }
                    DebugTextBox.Text = DebugTextBox.Text + Text + "\r\n";
                }
                if (!Directory.Exists(Global.Path + "\\logs\\debug"))
                {
                    Directory.CreateDirectory(Global.Path + "\\logs\\debug");
                }
                try
                {
                    File.AppendAllText(
                        Global.Path + $"\\logs\\debug\\{DateTime.Now:yyyy-MM-dd}.log",
                        Text + "\n",
                        Encoding.UTF8
                        );
                }
                catch { }
            }
        }
        private void Ui_DragDrop(object sender, DragEventArgs e)
        {
            Array data = (Array)e.Data.GetData(DataFormats.FileDrop);
            string FileName;
            List<string> SingleList = new List<string> { ".EXE", ".BAT", ".JSON", ".TSV" };
            if (
                data.Length == 1 &&
                SingleList.Contains(
                    Path.GetExtension(
                        data.GetValue(0).ToString()
                        ).ToUpper()
                    )
                )
            {
                FocusWindow();
                FileName = data.GetValue(0).ToString();
                if (
                    Path.GetExtension(FileName).ToUpper() == ".EXE" ||
                    Path.GetExtension(FileName).ToUpper() == ".BAT"
                    )
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
                        Global.Settings.Server.Path = FileName;
                    }
                    LoadPlugins();
                }
                else if (Path.GetExtension(FileName).ToUpper() == ".JSON")
                {
                    StreamReader Reader = new StreamReader(
                        File.Open(
                            FileName,
                            FileMode.Open
                        ),
                        Encoding.UTF8
                        );
                    JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Reader.ReadToEnd());
                    Reader.Close();
                    if (
                        JsonObject["type"].ToString().ToUpper() == "REGEX"
                        &&
                        (int)MessageBox.Show(
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
                    else if (JsonObject["type"].ToString().ToUpper() == "TASK"
                        &&
                        (int)MessageBox.Show(
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
                }
                else if (
                    Path.GetFileName(FileName).ToUpper() == "REGEX.TSV" &&
                    (int)MessageBox.Show(
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
                else if (
                    Path.GetFileName(FileName).ToUpper() == "TASK.TSV" &&
                    (int)MessageBox.Show(
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
            if (data.Length > 0)
            {
                List<string> AcceptableList = new List<string>() { ".py", ".dll", ".js", ".go", ".jar" };
                List<string> FileList = new List<string>();
                string FileListText = string.Empty;
                foreach (object File in data)
                {
                    if (AcceptableList.Contains(Path.GetExtension(File.ToString().ToLower())))
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
                    PluginManager.Add(FileList);
                    LoadPlugins();
                }
                else if (FileList.Count == 0 && data.Length > 0)
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
        private void Ui_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void UpdateVersion()
        {
            SettingSereinVersion.Text = $"当前版本：{Global.VERSION}";
            UpdateStatusLabel(Global.VERSION);
            DebugTextBox.Text = Global.VERSION + "\r\n";
        }
        private void UpdateStatusLabel(string Text)
        {
            StripStatusLabel.Text = Text;
        }
    }
}
