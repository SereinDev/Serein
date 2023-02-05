using NCrontab;
using Serein.Base;
using Serein.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Function
{
    public partial class Task : UiPage
    {
        private int ActionType;

        public Task()
        {
            InitializeComponent();
            Load();
            Catalog.Function.Task = this;
        }

        public void Load() => Load(null);

        public void Load(string filename)
        {
            IO.ReadTask(filename);
            TaskListView.Items.Clear();
            foreach (Base.Task task in Global.TaskList)
            {
                TaskListView.Items.Add(task);
            }
        }

        private void Save()
        {
            List<Base.Task> list = new();
            foreach (var obj in TaskListView.Items)
            {
                if (obj is Base.Task task && task != null)
                {
                    list.Add(task);
                }
            }
            Global.TaskList = list;
            IO.SaveTask();
        }

        public bool Confirm(string cronExp, string command, string remark)
        {
            if (Core.Command.GetType(command) == CommandType.Invalid)
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
                        if (TaskListView.SelectedIndex >= 0)
                        {
                            TaskListView.Items.Insert(
                                TaskListView.SelectedIndex,
                                new Base.Task
                                {
                                    Cron = cronExp,
                                    Command = command,
                                    Remark = remark,
                                });
                        }
                        else
                        {
                            TaskListView.Items.Add(
                                new Base.Task
                                {
                                    Cron = cronExp,
                                    Command = command,
                                    Remark = remark
                                });
                        }
                    }
                    else if (ActionType == 2 && TaskListView.SelectedItem is Base.Task selectedItem && selectedItem != null)
                    {
                        selectedItem.Cron = cronExp;
                        selectedItem.Command = command;
                        selectedItem.Remark = remark;
                        TaskListView.SelectedItem = selectedItem;
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
                    Catalog.MainWindow.TaskEditor.Hide();
                    ActionType = 0;
                }
                Base.Task selectedItem = TaskListView.SelectedIndex >= 0 ? TaskListView.SelectedItem as Base.Task : null;
                string Tag = menuItem.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                        Catalog.MainWindow.OpenTaskEditor();
                        ActionType = 1;
                        break;
                    case "Edit":
                        if (selectedItem != null)
                        {
                            Catalog.MainWindow.OpenTaskEditor(selectedItem.Cron, selectedItem.Command, selectedItem.Remark);
                            ActionType = 2;
                        }
                        break;
                    case "Delete":
                        if (Logger.MsgBox("确定删除此行数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && TaskListView.SelectedIndex >= 0)
                        {
                            TaskListView.Items.RemoveAt(TaskListView.SelectedIndex);
                            Save();
                        }
                        break;
                    case "Clear":
                        if (Logger.MsgBox("确定删除所有数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && TaskListView.Items.Count > 0)
                        {
                            TaskListView.Items.Clear();
                            Save();
                        }
                        break;
                    case "Refresh":
                        Load();
                        break;
                    case "LookupCommand":
                        Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/Command") { UseShellExecute = true });
                        break;
                    case "LookupVariables":
                        Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/Variables") { UseShellExecute = true });
                        break;
                    case "Enable":
                    case "Disable":
                        if (selectedItem != null)
                        {
                            selectedItem.Enable = !selectedItem.Enable;
                            TaskListView.SelectedItem = selectedItem;
                            Save();
                            Load();
                        }
                        break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedIndex != -1 && sender is Wpf.Ui.Controls.Button button && button != null)
            {
                int index = TaskListView.SelectedIndex;
                string tag = button.Tag as string ?? string.Empty;
                switch (tag)
                {
                    case "Up":
                        TaskListView.Items.Insert(index - 1, TaskListView.SelectedItem);
                        TaskListView.Items.RemoveAt(index + 1);
                        TaskListView.SelectedIndex = index - 1;
                        break;
                    case "Down":
                        TaskListView.Items.Insert(index + 2, TaskListView.SelectedItem);
                        TaskListView.Items.RemoveAt(index);
                        TaskListView.SelectedIndex = index + 1;
                        break;
                }
                Save();
                UpdateButton();
            }
        }

        private void TaskListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Edit.IsEnabled = TaskListView.SelectedIndex != -1;
            Delete.IsEnabled = TaskListView.SelectedIndex != -1;
            Clear.IsEnabled = TaskListView.Items.Count > 0;
            if (TaskListView.Items.Count > 0 && TaskListView.SelectedItem is Base.Task selectedItem && selectedItem != null)
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


        private void TaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateButton();

        private void UpdateButton()
        {
            Up.IsEnabled = TaskListView.SelectedIndex > 0;
            Down.IsEnabled = TaskListView.SelectedIndex >= 0 && TaskListView.SelectedIndex < TaskListView.Items.Count - 1;
        }
    }
}
