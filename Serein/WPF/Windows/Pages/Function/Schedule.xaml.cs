using NCrontab;
using Serein.Base;
using Serein.Core.Generic;
using Serein.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Schedule : UiPage
    {
        private int ActionType;

        public Schedule()
        {
            InitializeComponent();
            Load();
            Catalog.Function.Schedule = this;
        }

        public void Load() => Load(null);

        public void Load(string filename)
        {
            IO.ReadSchedule(filename);
            ScheduleListView.Items.Clear();
            foreach (Base.Schedule schedule in Global.Schedules)
            {
                ScheduleListView.Items.Add(schedule);
            }
        }

        private void Save()
        {
            List<Base.Schedule> list = new();
            foreach (var obj in ScheduleListView.Items)
            {
                if (obj is Base.Schedule schedule && schedule != null)
                {
                    list.Add(schedule);
                }
            }
            Global.Schedules = list;
            IO.SaveSchedule();
        }

        public bool Confirm(string cronExp, string command, string remark)
        {
            if (Command.GetType(command) == CommandType.Invalid)
            {
                Catalog.MainWindow.OpenSnackbar("编辑失败", "命令不合法", SymbolRegular.Warning24);
            }
            else
            {
                try
                {
                    CrontabSchedule.Parse(cronExp);
                    if (ActionType == 1)
                    {
                        if (ScheduleListView.SelectedIndex >= 0)
                        {
                            ScheduleListView.Items.Insert(
                                ScheduleListView.SelectedIndex,
                                new Base.Schedule
                                {
                                    Cron = cronExp,
                                    Command = command,
                                    Remark = remark,
                                });
                        }
                        else
                        {
                            ScheduleListView.Items.Add(
                                new Base.Schedule
                                {
                                    Cron = cronExp,
                                    Command = command,
                                    Remark = remark
                                });
                        }
                    }
                    else if (ActionType == 2 && ScheduleListView.SelectedItem is Base.Schedule selectedItem && selectedItem != null)
                    {
                        selectedItem.Cron = cronExp;
                        selectedItem.Command = command;
                        selectedItem.Remark = remark;
                        ScheduleListView.SelectedItem = selectedItem;
                    }
                    Save();
                    Load();
                    ActionType = 0;
                    return true;
                }
                catch
                {
                    Catalog.MainWindow.OpenSnackbar("编辑失败", "Cron表达式不合法", SymbolRegular.Warning24);
                }
            }
            return false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem menuItem && menuItem != null)
            {
                if (ActionType != 0)
                {
                    Catalog.MainWindow.ScheduleEditor.Hide();
                    ActionType = 0;
                }
                Base.Schedule selectedItem = ScheduleListView.SelectedIndex >= 0 ? ScheduleListView.SelectedItem as Base.Schedule : null;
                string Tag = menuItem.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                        Catalog.MainWindow.OpenScheduleEditor();
                        ActionType = 1;
                        break;
                    case "Edit":
                        if (selectedItem != null)
                        {
                            Catalog.MainWindow.OpenScheduleEditor(selectedItem.Cron, selectedItem.Command, selectedItem.Remark);
                            ActionType = 2;
                        }
                        break;
                    case "Delete":
                        if (Logger.MsgBox("确定删除此行数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && ScheduleListView.SelectedIndex >= 0)
                        {
                            ScheduleListView.Items.RemoveAt(ScheduleListView.SelectedIndex);
                            Save();
                        }
                        break;
                    case "Clear":
                        if (Logger.MsgBox("确定删除所有数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && ScheduleListView.Items.Count > 0)
                        {
                            ScheduleListView.Items.Clear();
                            Save();
                        }
                        break;
                    case "Refresh":
                        Load();
                        break;
                    case "LookupCommand":
                        Process.Start(new ProcessStartInfo("https://serein.cc/docs/guide/command") { UseShellExecute = true });
                        break;
                    case "LookupVariables":
                        Process.Start(new ProcessStartInfo("https://serein.cc/docs/guide/variables") { UseShellExecute = true });
                        break;
                    case "Enable":
                    case "Disable":
                        if (selectedItem != null)
                        {
                            selectedItem.Enable = !selectedItem.Enable;
                            ScheduleListView.SelectedItem = selectedItem;
                            Save();
                            Load();
                        }
                        break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleListView.SelectedIndex != -1 && sender is Wpf.Ui.Controls.Button button && button != null)
            {
                int index = ScheduleListView.SelectedIndex;
                string tag = button.Tag as string ?? string.Empty;
                switch (tag)
                {
                    case "Up":
                        ScheduleListView.Items.Insert(index - 1, ScheduleListView.SelectedItem);
                        ScheduleListView.Items.RemoveAt(index + 1);
                        ScheduleListView.SelectedIndex = index - 1;
                        break;
                    case "Down":
                        ScheduleListView.Items.Insert(index + 2, ScheduleListView.SelectedItem);
                        ScheduleListView.Items.RemoveAt(index);
                        ScheduleListView.SelectedIndex = index + 1;
                        break;
                }
                Save();
                UpdateButton();
            }
        }

        private void ScheduleListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Edit.IsEnabled = ScheduleListView.SelectedIndex != -1;
            Delete.IsEnabled = ScheduleListView.SelectedIndex != -1;
            Clear.IsEnabled = ScheduleListView.Items.Count > 0;
            if (ScheduleListView.Items.Count > 0 && ScheduleListView.SelectedItem is Base.Schedule selectedItem && selectedItem != null)
            {
                Enable.IsEnabled = !selectedItem.Enable;
                Disable.IsEnabled = selectedItem.Enable;
            }
            else
            {
                Enable.IsEnabled = false;
                Disable.IsEnabled = false;
            }
        }


        private void ScheduleListView_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateButton();

        private void UpdateButton()
        {
            Up.IsEnabled = ScheduleListView.SelectedIndex > 0;
            Down.IsEnabled = ScheduleListView.SelectedIndex >= 0 && ScheduleListView.SelectedIndex < ScheduleListView.Items.Count - 1;
        }
    }
}
