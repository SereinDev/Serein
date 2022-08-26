using Serein.Server;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein.Windows
{
    public partial class MainWindow : UiWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Window.MainWindow = this;
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
        }

        /// <summary>
        /// 显示底部通知栏
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="Message">信息</param>
        /// <param name="Icon">图标</param>
        public void OpenSnackbar(string Title, string Message, SymbolRegular Icon)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                PopupEx.VerticalOffset = Height * 0.5 - 60; // 自动调整底部距离，使之刚好与底部重合
                Snackbar.Show(Title, Message, Icon);
            }));
        }

        private void UiWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Snackbar.IsShown)
            {
                PopupEx.VerticalOffset = Height * 0.5 - 60;
            }
        }

        private void Help_Click(object sender, RoutedEventArgs e) => Process.Start(new ProcessStartInfo("https://serein.cc/") { UseShellExecute = true });
        private void UiWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => ShowInTaskbar = IsVisible;

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
            if (Window.Function.Member.Confirm(MemberEditor_ID.Text, MemberEditor_GameID.Text))
            {
                MemberEditor.Hide();
            }
        }
        #endregion

        #region 正则编辑器代码

        /// <summary>
        /// 打开正则编辑器窗口
        /// </summary>
        public void OpenrRegexEditor(int AreaIndex = 0, bool IsAdmin = false, string Regex = "", string Command = "", string Remark = "")
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
            {
                OpenSnackbar("警告", "保存前请务必检查这条正则触发的命令是否会导致再次被所触发内容触发，\n配置错误可能导致机器人刷屏甚至被封号", SymbolRegular.Warning24);
            }
        }

        private void RegexEditor_ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            if (Window.Function.Regex.Confirm(
                RegexEditor_Area.SelectedIndex,
                RegexEditor_IsAdmain.IsChecked ?? false,
                RegexEditor_Regex.Text,
                RegexEditor_Command.Text,
                RegexEditor_Remark.Text))
            {
                RegexEditor.Hide();
            }
        }

        private void RegexEditor_ButtonRightClick(object sender, RoutedEventArgs e) => RegexEditor.Hide();
        #endregion
    }
}
