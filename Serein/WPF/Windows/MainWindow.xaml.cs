using NCrontab;
using Serein.Base;
using Serein.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Notification.Wpf;

namespace Serein.Windows
{
    public partial class MainWindow : UiWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Catalog.MainWindow = this;
            Task.Run(() => Logger.Out(Items.LogType.Debug, "[Serein] Welcome. ", SystemInfo.CPUPercentage.Replace('.', 'w')));
            if (Global.Args.Contains("auto_connect"))
                Task.Run(() => Websocket.Connect(false));
            if (Global.Args.Contains("auto_start"))
                Task.Run(() => ServerManager.Start(true));
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void UiWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Catalog.Notification = Catalog.Notification ?? new NotificationManager();
            DebugNavigationItem.Visibility = Global.Settings.Serein.Debug ? Visibility.Visible : Visibility.Hidden;
            if (Global.Settings.Serein.ThemeFollowSystem)
                Watcher.Watch(this, BackgroundType.Tabbed, true);
            Theme.Apply(Global.Settings.Serein.UseDarkTheme ? ThemeType.Dark : ThemeType.Light);
        }

        private void UiWindow_StateChanged(object sender, EventArgs e)
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            PopupEx.VerticalOffset = Height * 0.5 - 60;
        }

        private void UiWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = ServerManager.Status;
            if (ServerManager.Status)
            {
                ShowInTaskbar = false;
                Hide();
                Catalog.Notification.Show("Serein", "服务器进程仍在运行中\n已自动最小化至托盘，点击托盘图标即可复原窗口");
            }
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = false;
            Hide();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (!ServerManager.Status)
                Close();
            else
                Catalog.Notification.Show("Serein", "服务器进程仍在运行中\n已自动最小化至托盘，点击托盘图标即可复原窗口");
        }

        /// <summary>
        /// 显示底部通知栏
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="Message">信息</param>
        /// <param name="Icon">图标</param>
        public void OpenSnackbar(string Title, string Message, SymbolRegular Icon)
            => Dispatcher.Invoke(() =>
            {
                PopupEx.VerticalOffset = Height * 0.5 - 60; // 自动调整底部距离，使之刚好与底部重合
                Snackbar.Show(Title, Message, Icon);
            });

        private void UiWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Snackbar.IsShown)
                PopupEx.VerticalOffset = Height * 0.5 - 60;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
            => Process.Start(new ProcessStartInfo("https://serein.cc/") { UseShellExecute = true });

        private void UiWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
            => ShowInTaskbar = IsVisible;

        #region 成员编辑器代码

        /// <summary>
        /// 打开成员编辑器窗口
        /// </summary>
        public void OpenMemberEditor(bool New = true, string ID = "", string GameID = "")
        {
            MemberEditor_ID.Text = ID;
            MemberEditor_GameID.Text = GameID;
            MemberEditor_ID.IsEnabled = New;
            MemberEditor.Show();
        }

        private void MemberEditor_ButtonRightClick(object sender, RoutedEventArgs e) => MemberEditor.Hide();
        private void MemberEditor_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            if (Catalog.Function.Member.Confirm(MemberEditor_ID.Text, MemberEditor_GameID.Text))
                MemberEditor.Hide();
        }
        #endregion

        #region 正则编辑器代码

        /// <summary>
        /// 打开正则编辑器窗口
        /// </summary>
        public void OpenRegexEditor(int AreaIndex = 0, bool IsAdmin = false, string Regex = "", string Command = "", string Remark = "")
        {
            RegexEditor_Area.SelectedIndex = AreaIndex;
            RegexEditor_IsAdmain.IsChecked = IsAdmin;
            RegexEditor_Regex.Text = Regex;
            RegexEditor_Command.Text = Command;
            RegexEditor_Remark.Text = Remark;
            RegexEditor.Show();
        }

        private void RegexEditor_Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RegexEditor_IsAdmain.IsEnabled = RegexEditor_Area.SelectedIndex >= 2 && RegexEditor_Area.SelectedIndex <= 3;
            RegexEditor_IsAdmain.IsChecked = RegexEditor_IsAdmain.IsEnabled ? RegexEditor_IsAdmain.IsChecked : false;
            if (RegexEditor_Area.SelectedIndex == 4)
                OpenSnackbar("警告", "保存前请务必检查这条正则触发的命令是否会导致再次被所触发内容触发，\n配置错误可能导致机器人刷屏甚至被封号", SymbolRegular.Warning24);
        }

        private void RegexEditor_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            if (Catalog.Function.Regex.Confirm(
                RegexEditor_Area.SelectedIndex,
                RegexEditor_IsAdmain.IsChecked ?? false,
                RegexEditor_Regex.Text,
                RegexEditor_Command.Text,
                RegexEditor_Remark.Text))
                RegexEditor.Hide();
        }

        private void RegexEditor_ButtonRightClick(object sender, RoutedEventArgs e) => RegexEditor.Hide();
        #endregion

        #region 任务编辑器代码
        public void OpenTaskEditor(string CronExp = "", string Command = "", string Remark = "")
        {
            TaskEditor_Cron.Text = CronExp;
            TaskEditor_Command.Text = Command;
            TaskEditor_Remark.Text = Remark;
            TaskEditor.Show();
        }

        private void TaskEditor_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            if (Catalog.Function.Task.Confirm(TaskEditor_Cron.Text, TaskEditor_Command.Text, TaskEditor_Remark.Text))
                TaskEditor.Hide();
        }

        private void TaskEditor_Cron_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TaskEditor_NextTime.Text = $"预计执行时间: {CrontabSchedule.Parse(TaskEditor_Cron.Text).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList()[0].ToString("g")}";
            }
            catch
            {
                TaskEditor_NextTime.Text = "Cron表达式不合法";
            }
        }

        private void TaskEditor_ButtonRightClick(object sender, RoutedEventArgs e) => TaskEditor.Hide();
        #endregion

        #region 事件编辑器代码
        public void OpenEventrEditor(string Command = "")
        {
            EventEditor_Command.Text = Command;
            EventEditor.Show();
        }

        private void EventEditor_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            if (Catalog.Settings.Event.Confirm(EventEditor_Command.Text))
                EventEditor.Hide();
        }

        private void EventEditor_ButtonRightClick(object sender, RoutedEventArgs e)
        {
            EventEditor.Hide();
        }

        #endregion

        private void UiWindow_Drop(object sender, DragEventArgs e)
        {
            Array data = (Array)e.Data.GetData(DataFormats.FileDrop);
            string FileName = string.Empty;
            List<string> SingleList = new List<string> { ".exe", ".bat", ".json", ".tsv" };
            if (
                data.Length == 1 &&
                SingleList.Contains(
                    Path.GetExtension(
                        data.GetValue(0)?.ToString()
                        ).ToLower()
                    )
                )
            {
                Focus();
                FileName = data.GetValue(0)?.ToString();
                if (
                    Path.GetExtension(FileName).ToUpper() == ".EXE" ||
                    Path.GetExtension(FileName).ToUpper() == ".BAT"
                    )
                {
                    if (Logger.MsgBox(
                        $"是否以\"{FileName}\"为启动文件？",
                        "Serein", 1, 48))
                    {
                        if (Catalog.Settings.Server != null && Catalog.Settings.Server.Path != null)
                            Catalog.Settings.Server.Path.Text = FileName;
                        Global.Settings.Server.Path = FileName;
                        Catalog.Server.Plugins?.Load();
                    }
                }
                else if (Path.GetExtension(FileName).ToLower() == ".json" || Path.GetExtension(FileName).ToLower() == ".tsv")
                {
                    if (Logger.MsgBox($"是否导入{Path.GetFileName(FileName)}？\n将覆盖原有文件且不可逆",
                            "Serein",
                            1,
                            48))
                    {
                        Catalog.Function.Regex?.Load(FileName);
                        Catalog.Function.Task?.Load(FileName);
                        IO.SaveRegex();
                        IO.SaveTask();
                    }
                }
            }
            else if (data.Length > 0)
            {
                List<string> AcceptableList = new List<string>() { ".py", ".dll", ".js", ".jar" };
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
                if (FileList.Count > 0 && Logger.MsgBox($"是否将以下文件复制到插件文件夹内？\n{FileListText}", "Serein", 1, 48))
                {
                    PluginManager.Add(FileList);
                    Catalog.Server.Plugins?.Load();
                }
                else if (FileList.Count == 0 && data.Length > 0)
                    Logger.MsgBox("无法识别所选文件", "Serein", 0, 48);
            }
        }

        public void UpdateTitle(string Title = null)
            => Dispatcher.Invoke(() => TitleBar.Title = string.IsNullOrEmpty(Title) ? "Serein" : $"Serein - {Title}");
    }
}
