using NCrontab;
using Notification.Wpf;
using Serein.Core.JSPlugin;
using Serein.Core.Server;
using Serein.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein.Windows
{
    public partial class MainWindow : UiWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            DebugNavigationItem.Visibility = Global.Settings.Serein.DevelopmentTool.EnableDebug ? Visibility.Visible : Visibility.Hidden;
            SettingsNavigationItem.Visibility = Global.Settings.Serein.PagesDisplayed.Settings ? Visibility.Visible : Visibility.Hidden;
            if (!Global.Settings.Serein.PagesDisplayed.ServerPanel && !Global.Settings.Serein.PagesDisplayed.ServerPluginManager)
            {
                ServerNavigationItem.Visibility = Visibility.Hidden;
            }
            if (
                !Global.Settings.Serein.PagesDisplayed.Bot &&
                !Global.Settings.Serein.PagesDisplayed.Member &&
                !Global.Settings.Serein.PagesDisplayed.RegexList &&
                !Global.Settings.Serein.PagesDisplayed.Schedule &&
                !Global.Settings.Serein.PagesDisplayed.JSPlugin)
            {
                FunctionNavigationItem.Visibility = Visibility.Hidden;
            }
            Catalog.MainWindow = this;
        }

        private void UiWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Catalog.Notification ??= new();
            if (Global.Settings.Serein.ThemeFollowSystem)
            {
                Watcher.Watch(this, BackgroundType.Tabbed, true);
            }
            Theme.Apply(Global.Settings.Serein.UseDarkTheme ? ThemeType.Dark : ThemeType.Light);
        }

        private void UiWindow_ContentRendered(object sender, EventArgs e)
            => Runtime.Start();

        private void UiWindow_StateChanged(object sender, EventArgs e)
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }

        private void UiWindow_Closing(object sender, CancelEventArgs e)
        {
            if (ServerManager.Status)
            {
                e.Cancel = true;
                ShowInTaskbar = false;
                Hide();
                Catalog.Notification?.Show("Serein", "服务器进程仍在运行中\n已自动最小化至托盘，点击托盘图标即可复原窗口");
            }
            else
            {
                JSFunc.Trigger(Base.EventType.SereinClose);
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
            {
                Close();
            }
            else
            {
                Catalog.Notification?.Show("Serein", "服务器进程仍在运行中\n已自动最小化至托盘，点击托盘图标即可复原窗口");
            }
        }

        /// <summary>
        /// 显示底部通知栏
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">信息</param>
        /// <param name="icon">图标</param>
        public void OpenSnackbar(string title, string message, SymbolRegular icon)
            => Dispatcher.Invoke(() =>
            {
                Snackbar.Show(title, message, icon);
            });

        private void Help_Click(object sender, RoutedEventArgs e)
            => Process.Start(new ProcessStartInfo("https://serein.cc/") { UseShellExecute = true });

        private void UiWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
            => ShowInTaskbar = IsVisible;

        #region 成员编辑器代码

        /// <summary>
        /// 打开成员编辑器窗口
        /// </summary>
        public void OpenMemberEditor() => OpenMemberEditor(true, string.Empty, string.Empty);

        /// <summary>
        /// 打开成员编辑器窗口
        /// </summary>
        public void OpenMemberEditor(bool isNew, string id, string gameid)
        {
            MemberEditor_ID.Text = id;
            MemberEditor_GameID.Text = gameid;
            MemberEditor_ID.IsEnabled = isNew;
            MemberEditor.Show();
        }

        private void MemberEditor_ButtonRightClick(object sender, RoutedEventArgs e) => MemberEditor.Hide();
        private void MemberEditor_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            if (Catalog.Function.Member?.Confirm(MemberEditor_ID.Text, MemberEditor_GameID.Text) ?? false)
            {
                MemberEditor.Hide();
            }
        }
        #endregion

        #region 正则编辑器代码

        /// <summary>
        /// 打开正则编辑器窗口
        /// </summary>
        public void OpenRegexEditor() => OpenRegexEditor(0, false, string.Empty, string.Empty, string.Empty);

        /// <summary>
        /// 打开正则编辑器窗口
        /// </summary>
        public void OpenRegexEditor(int areaIndex, bool needAdmin, string regex, string command, string remark)
        {
            RegexEditor_Area.SelectedIndex = areaIndex;
            RegexEditor_IsAdmain.IsChecked = needAdmin;
            RegexEditor_Regex.Text = regex;
            RegexEditor_Command.Text = command;
            RegexEditor_Remark.Text = remark;
            RegexEditor.Show();
        }

        private void RegexEditor_Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RegexEditor_IsAdmain.IsEnabled = RegexEditor_Area.SelectedIndex >= 2 && RegexEditor_Area.SelectedIndex <= 3;
            RegexEditor_IsAdmain.IsChecked = RegexEditor_IsAdmain.IsEnabled ? RegexEditor_IsAdmain.IsChecked : false;
            if (RegexEditor_Area.SelectedIndex == 4)
            {
                OpenSnackbar("警告", "保存前请务必检查这条正则触发的命令是否会导致再次被所触发内容触发，\n配置错误可能导致机器人刷屏甚至被封号", SymbolRegular.Warning24);
            }
        }

        private void RegexEditor_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            if (Catalog.Function.Regex?.Confirm(
                RegexEditor_Area.SelectedIndex,
                RegexEditor_IsAdmain.IsChecked ?? false,
                RegexEditor_Regex.Text,
                RegexEditor_Command.Text,
                RegexEditor_Remark.Text) ?? false)
            {
                RegexEditor.Hide();
            }
        }

        private void RegexEditor_ButtonRightClick(object sender, RoutedEventArgs e) => RegexEditor.Hide();
        #endregion

        #region 任务编辑器代码
        public void OpenScheduleEditor() => OpenScheduleEditor(string.Empty, string.Empty, string.Empty);
        public void OpenScheduleEditor(string cronExp, string command, string remark)
        {
            ScheduleEditor_Cron.Text = cronExp;
            ScheduleEditor_Command.Text = command;
            ScheduleEditor_Remark.Text = remark;
            ScheduleEditor.Show();
        }

        private void ScheduleEditor_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            if (Catalog.Function.Schedule?.Confirm(ScheduleEditor_Cron.Text, ScheduleEditor_Command.Text, ScheduleEditor_Remark.Text) ?? false)
            {
                ScheduleEditor.Hide();
            }
        }

        private void ScheduleEditor_Cron_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                List<DateTime> occurrences = CrontabSchedule.Parse(ScheduleEditor_Cron.Text).GetNextOccurrences(DateTime.Now, DateTime.Now.AddYears(1)).ToList();
                if (occurrences.Count > 20)
                {
                    occurrences.RemoveRange(20, occurrences.Count - 20);
                }
                ScheduleEditor_NextTime.Text = $"预计执行时间： {occurrences[0]:g}";
                ScheduleEditor_NextTime.ToolTip = $"最近20次执行执行时间：\n{string.Join("\n", occurrences.Select((dateTime) => dateTime.ToString("g")))}";
            }
            catch
            {
                ScheduleEditor_NextTime.Text = "Cron表达式无效或超过时间限制";
                ScheduleEditor_NextTime.ToolTip = string.Empty;
            }
        }

        private void ScheduleEditor_ButtonRightClick(object sender, RoutedEventArgs e) => ScheduleEditor.Hide();
        #endregion

        #region 事件编辑器代码
        public void OpenEventEditor(string command)
        {
            EventEditor_Command.Text = command;
            EventEditor.Show();
        }

        private void EventEditor_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            if (Catalog.Settings.Event?.Confirm(EventEditor_Command.Text) ?? false)
            {
                EventEditor.Hide();
            }
        }

        private void EventEditor_ButtonRightClick(object sender, RoutedEventArgs e)
        {
            EventEditor.Hide();
        }
        #endregion

        private void UiWindow_Drop(object sender, DragEventArgs e)
        {
            Array data = (Array)e.Data.GetData(DataFormats.FileDrop);
            string filename = string.Empty;
            List<string> SingleList = new List<string> { ".exe", ".bat", ".json", ".tsv" };
            if (
                data.Length == 1 &&
                SingleList.Contains(
                    Path.GetExtension(
                        data.GetValue(0)?.ToString()!
                        ).ToLowerInvariant()
                    )
                )
            {
                Focus();
                filename = data.GetValue(0)?.ToString() ?? string.Empty;
                if (
                    Path.GetExtension(filename).ToLowerInvariant() == ".exe" ||
                    Path.GetExtension(filename).ToLowerInvariant() == ".bat"
                    )
                {
                    if (Logger.MsgBox(
                        $"确定要以\"{filename}\"为启动文件吗？",
                        "Serein", 1, 48))
                    {
                        if (Catalog.Settings.Server != null && Catalog.Settings.Server.Path != null)
                        {
                            Catalog.Settings.Server.Path.Text = filename;
                        }
                        Global.Settings.Server.Path = filename;
                        Catalog.Server.Plugins?.Load();
                    }
                }
                else if (Path.GetExtension(filename).ToLowerInvariant() == ".json" || Path.GetExtension(filename).ToLowerInvariant() == ".tsv")
                {
                    if (File.ReadAllText(filename).ToLowerInvariant().Contains("regex"))
                    {
                        if (Logger.MsgBox($"确定要从{Path.GetFileName(filename)}导入正则记录吗？", "Serein", 1, 48))
                        {
                            IO.ReadRegex(filename, Logger.MsgBox($"确定要合并正则记录吗？\n二者均将覆盖原有文件且不可逆", "Serein", 1, 48));
                            Catalog.Function.Regex?.Load();
                        }
                    }
                    else if (Logger.MsgBox($"确定要从{Path.GetFileName(filename)}导入定时任务吗？\n将覆盖原有文件且不可逆", "Serein", 1, 48))
                    {
                        IO.ReadSchedule(filename);
                        Catalog.Function.Schedule?.Load();
                    }
                }
            }
            else if (data.Length > 0 && PluginManager.TryImport(data))
            {
                Catalog.Server.Plugins?.Load();
            }
        }

        public void UpdateTitle(string? title)
            => Dispatcher.Invoke(() => _TitleBar.Title = string.IsNullOrEmpty(title) ? "Serein" : $"Serein - {title}");
    }
}
