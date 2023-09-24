using NCrontab;
using Serein.Base;
using Serein.Core.Common;
using Serein.Utils.IO;
using Serein.Utils.Output;
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
        private int _actionType;

        public Schedule()
        {
            InitializeComponent();
            Load();
            Catalog.Function.Schedule = this;
        }

        public void Load()
        {
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
            Data.SaveSchedule();
        }

        public bool Confirm(string cronExp, string command, string remark)
        {
            if (Command.GetType(command) == CommandType.Invalid)
            {
                Catalog.MainWindow?.OpenSnackbar("编辑失败", "命令不合法", SymbolRegular.Warning24);
                return false;
            }
            else if (CrontabSchedule.TryParse(cronExp) == null)
            {
                Catalog.MainWindow?.OpenSnackbar("编辑失败", "Cron表达式不合法", SymbolRegular.Warning24);
                return false;
            }
            if (_actionType == 1)
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
                        }
                    );
                }
                else
                {
                    ScheduleListView.Items.Add(
                        new Base.Schedule
                        {
                            Cron = cronExp,
                            Command = command,
                            Remark = remark
                        }
                    );
                }
            }
            else if (
                _actionType == 2
                && ScheduleListView.SelectedItem is Base.Schedule selectedItem
                && selectedItem != null
            )
            {
                selectedItem.Cron = cronExp;
                selectedItem.Command = command;
                selectedItem.Remark = remark;
                ScheduleListView.SelectedItem = selectedItem;
            }
            Save();
            Load();
            _actionType = 0;
            return true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem menuItem && menuItem != null)
            {
                if (_actionType != 0)
                {
                    Catalog.MainWindow?.ScheduleEditor.Hide();
                    _actionType = 0;
                }
                Base.Schedule? selectedItem =
                    ScheduleListView.SelectedIndex >= 0
                        ? ScheduleListView.SelectedItem as Base.Schedule
                        : null;
                string Tag = menuItem.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                        Catalog.MainWindow?.OpenScheduleEditor();
                        _actionType = 1;
                        break;
                    case "Edit":
                        if (selectedItem != null)
                        {
                            Catalog.MainWindow?.OpenScheduleEditor(
                                selectedItem.Cron,
                                selectedItem.Command,
                                selectedItem.Remark
                            );
                            _actionType = 2;
                        }
                        break;
                    case "Delete":
                        if (
                            MsgBox.Show("确定删除此行数据？\n它将会永远失去！（真的很久！）", true)
                            && ScheduleListView.SelectedIndex >= 0
                        )
                        {
                            ScheduleListView.Items.RemoveAt(ScheduleListView.SelectedIndex);
                            Save();
                        }
                        break;
                    case "Clear":
                        if (
                            MsgBox.Show("确定删除所有数据？\n它将会永远失去！（真的很久！）", true)
                            && ScheduleListView.Items.Count > 0
                        )
                        {
                            ScheduleListView.Items.Clear();
                            Save();
                        }
                        break;
                    case "Refresh":
                        Data.ReadSchedule();
                        Load();
                        break;
                    case "LookupCommand":
                        Process.Start(
                            new ProcessStartInfo("https://serein.cc/docs/guide/command")
                            {
                                UseShellExecute = true
                            }
                        );
                        break;
                    case "LookupVariables":
                        Process.Start(
                            new ProcessStartInfo("https://serein.cc/docs/guide/variables")
                            {
                                UseShellExecute = true
                            }
                        );
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
            if (
                ScheduleListView.SelectedIndex != -1
                && sender is Wpf.Ui.Controls.Button button
                && button != null
            )
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
            if (
                ScheduleListView.Items.Count > 0
                && ScheduleListView.SelectedItem is Base.Schedule selectedItem
                && selectedItem != null
            )
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

        private void ScheduleListView_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e
        ) => UpdateButton();

        private void UpdateButton()
        {
            Up.IsEnabled = ScheduleListView.SelectedIndex > 0;
            Down.IsEnabled =
                ScheduleListView.SelectedIndex >= 0
                && ScheduleListView.SelectedIndex < ScheduleListView.Items.Count - 1;
        }
    }
}
