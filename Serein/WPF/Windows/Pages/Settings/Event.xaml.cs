using Serein.Base;
using Serein.Core.Generic;
using Serein.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Settings
{
    public partial class Event : UiPage
    {
        public Event()
        {
            InitializeComponent();
            Catalog.Settings.Event = this;
        }

        private int ActionType;
        private string SelectedTag = string.Empty;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem != null)
            {
                string tag = menuItem.Tag as string ?? string.Empty;
                switch (tag)
                {
                    case "Add":
                        ActionType = 1;
                        Catalog.MainWindow.OpenEventEditor(string.Empty);
                        break;
                    case "Edit":
                        if (EventListView.SelectedItem is string selectedItem && selectedItem != null)
                        {
                            ActionType = 2;
                            Catalog.MainWindow.OpenEventEditor(selectedItem);
                        }
                        break;
                    case "Delete":
                        if (Logger.MsgBox("确定删除此行数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && EventListView.SelectedIndex >= 0)
                        {
                            EventListView.Items.RemoveAt(EventListView.SelectedIndex);
                        }
                        Save();
                        break;
                    case "LookupEvent":
                        Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/Event") { UseShellExecute = true });
                        break;
                }
            }
        }

        public bool Confirm(string command)
        {
            if (Command.GetType(command) < 0)
            {
                Catalog.MainWindow.OpenSnackbar("编辑失败", "命令不合法", SymbolRegular.Warning24);
                return false;
            }
            else if (ActionType == 1)
            {
                if (EventListView.SelectedIndex >= 0)
                {
                    EventListView.Items.Insert(
                        EventListView.SelectedIndex,
                        command);
                }
                else
                {
                    EventListView.Items.Add(command);
                }
                Save();
                Load();
            }
            else if (ActionType == 2 && EventListView.SelectedIndex >= 0)
            {
                EventListView.Items[EventListView.SelectedIndex] = command;
                Save();
                Load();
            }
            ActionType = 0;
            return true;
        }

        private void Save()
        {
            if (Enum.IsDefined(typeof(EventType), SelectedTag))
            {
                Global.Settings.Event.Edit(GetEventCommands(), (EventType)Enum.Parse(typeof(EventType), SelectedTag));
                IO.SaveEventSetting();
            }
        }

        private string[] GetEventCommands()
        {
            List<string> list = new List<string>();
            foreach (object line in EventListView.Items)
            {
                list.Add(line as string ?? string.Empty);
            }
            return list.ToArray();
        }

        private void Load()
        {
            if (Events.SelectedItem is System.Windows.Controls.TreeViewItem treeViewItemItem && treeViewItemItem != null)
            {
                EventListView.Items.Clear();
                SelectedTag = treeViewItemItem.Tag as string ?? string.Empty;
                if (Enum.IsDefined(typeof(EventType), SelectedTag))
                {
                    Global.Settings.Event.Get(
                        (EventType)Enum.Parse(typeof(EventType), SelectedTag)).ToList()
                        .ForEach((Command) => { EventListView.Items.Add(System.Text.RegularExpressions.Regex.Replace(Command, @"(\n|\r|\\n|\\r)+", "\\n")); }
                        );
                }
            }
            else
            {
                SelectedTag = string.Empty;
            }
        }

        private void Events_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => Load();

        private void EventListView_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            Add.IsEnabled = !string.IsNullOrEmpty(SelectedTag);
            Edit.IsEnabled = !string.IsNullOrEmpty(SelectedTag) && EventListView.SelectedIndex >= 0;
            Delete.IsEnabled = !string.IsNullOrEmpty(SelectedTag) && EventListView.SelectedIndex >= 0;
        }
    }
}
