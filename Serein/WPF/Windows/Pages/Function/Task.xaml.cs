using NCrontab;
using Serein.Base;
using Serein.Items;
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
        private int ActionType = 0;
        public Task()
        {
            InitializeComponent();
            Load();
            Window.Function.Task = this;
        }

        private void Load()
        {
            Loader.ReadTask();
            TaskListView.Items.Clear();
            foreach (TaskItem Item in Global.TaskItems)
            {
                TaskListView.Items.Add(Item);
            }
        }

        private void Save()
        {
            List<TaskItem> Items = new List<TaskItem>();
            foreach (var obj in TaskListView.Items)
            {
                if (obj is TaskItem Item && Item != null)
                {
                    Items.Add(Item);
                }
            }
            Global.UpdateTaskItems(Items);
            Loader.SaveTask();
        }

        public bool Confirm(string CronExp, string Command, string Remark)
        {
            if (Base.Command.GetType(Command) == -1)
            {
                Window.MainWindow.OpenSnackbar("编辑失败", "命令不合法", SymbolRegular.Warning24);
            }
            else
            {
                try
                {
                    CrontabSchedule.Parse(CronExp);
                    if (ActionType == 1)
                    {
                        if (TaskListView.SelectedIndex >= 0)
                        {
                            TaskListView.Items.Insert(
                                TaskListView.SelectedIndex,
                                new TaskItem()
                                {
                                    Cron = CronExp,
                                    Command = Command,
                                    Remark = Remark,
                                });
                        }
                        else
                        {
                            TaskListView.Items.Add(
                                new TaskItem()
                                {
                                    Cron = CronExp,
                                    Command = Command,
                                    Remark = Remark
                                });
                        }
                    }
                    else if (ActionType == 2 && TaskListView.SelectedItem is TaskItem SelectedItem && SelectedItem != null)
                    {
                        SelectedItem.Cron = CronExp;
                        SelectedItem.Command = Command;
                        SelectedItem.Remark = Remark;
                        TaskListView.SelectedItem = SelectedItem;
                    }
                    Save();
                    Load();
                    ActionType = 0;
                    return true;
                }
                catch
                {
                    Window.MainWindow.OpenSnackbar("编辑失败", "Cron表达式不合法", SymbolRegular.Warning24);
                }
            }
            return false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.MenuItem Item && Item != null)
            {
                if (ActionType != 0)
                {
                    Window.MainWindow.TaskEditor.Hide();
                    ActionType = 0;
                }
                string Tag = Item.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                        Window.MainWindow.OpenTaskEditor();
                        ActionType = 1;
                        break;
                    case "Edit":
                        if (TaskListView.SelectedItem is TaskItem SelectedItem && SelectedItem != null)
                        {
                            Window.MainWindow.OpenTaskEditor(SelectedItem.Cron, SelectedItem.Command, SelectedItem.Remark);
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
                        Process.Start(new ProcessStartInfo("https://serein.cc/Command.html") { UseShellExecute = true });
                        break;
                    case "LookupVariables":
                        Process.Start(new ProcessStartInfo("https://serein.cc/Variables.html") { UseShellExecute = true });
                        break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedIndex != -1 && sender is Wpf.Ui.Controls.Button Item && Item != null)
            {
                int Index = TaskListView.SelectedIndex;
                string Tag = Item.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Up":
                        TaskListView.Items.Insert(Index - 1, TaskListView.SelectedItem);
                        TaskListView.Items.RemoveAt(Index + 1);
                        TaskListView.SelectedIndex = Index - 1;
                        break;
                    case "Down":
                        TaskListView.Items.Insert(Index + 2, TaskListView.SelectedItem);
                        TaskListView.Items.RemoveAt(Index);
                        TaskListView.SelectedIndex = Index + 1;
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
        }


        private void TaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateButton();

        private void UpdateButton()
        {
            Up.IsEnabled = TaskListView.SelectedIndex > 0;
            Down.IsEnabled = TaskListView.SelectedIndex >= 0 && TaskListView.SelectedIndex < TaskListView.Items.Count - 1;
        }
    }
}
