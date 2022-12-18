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
            Catalog.Function.Task = this;
        }

        public void Load(string FileName = null)
        {
            IO.ReadTask(FileName);
            TaskListView.Items.Clear();
            foreach (Items.Task Item in Global.TaskItems)
            {
                TaskListView.Items.Add(Item);
            }
        }

        private void Save()
        {
            List<Items.Task> Items = new List<Items.Task>();
            foreach (var obj in TaskListView.Items)
            {
                if (obj is Items.Task Item && Item != null)
                {
                    Items.Add(Item);
                }
            }
            Global.UpdateTaskItems(Items);
            IO.SaveTask();
        }

        public bool Confirm(string CronExp, string Command, string Remark)
        {
            if (Base.Command.GetType(Command) == CommandType.Invalid)
            {
                Catalog.MainWindow.OpenSnackbar("编辑失败", "命令不合法", SymbolRegular.Warning24);
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
                                new Items.Task()
                                {
                                    Cron = CronExp,
                                    Command = Command,
                                    Remark = Remark,
                                });
                        }
                        else
                        {
                            TaskListView.Items.Add(
                                new Items.Task()
                                {
                                    Cron = CronExp,
                                    Command = Command,
                                    Remark = Remark
                                });
                        }
                    }
                    else if (ActionType == 2 && TaskListView.SelectedItem is Items.Task SelectedItem && SelectedItem != null)
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
                    Catalog.MainWindow.OpenSnackbar("编辑失败", "Cron表达式不合法", SymbolRegular.Warning24);
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
                    Catalog.MainWindow.TaskEditor.Hide();
                    ActionType = 0;
                }
                Items.Task SelectedItem = TaskListView.SelectedIndex >= 0 ? TaskListView.SelectedItem as Items.Task : null;
                string Tag = Item.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                        Catalog.MainWindow.OpenTaskEditor();
                        ActionType = 1;
                        break;
                    case "Edit":
                        if (SelectedItem != null)
                        {
                            Catalog.MainWindow.OpenTaskEditor(SelectedItem.Cron, SelectedItem.Command, SelectedItem.Remark);
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
                        if (SelectedItem != null)
                        {
                            SelectedItem.Enable = !SelectedItem.Enable;
                            TaskListView.SelectedItem = SelectedItem;
                            Save();
                            Load();
                        }
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
            if (TaskListView.Items.Count > 0 && TaskListView.SelectedItem is Items.Task SelectedItem && SelectedItem != null)
            {
                Enable.IsEnabled = !SelectedItem.Enable;
                Disable.IsEnabled = SelectedItem.Enable;
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
