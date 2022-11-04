using Serein.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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

        private int ActionType = 0;
        private string SelectedTag = string.Empty;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem Item && Item != null)
            {
                string Tag = Item.Tag as string ?? string.Empty;
                switch (Tag)
                {
                    case "Add":
                        ActionType = 1;
                        Catalog.MainWindow.OpenEventrEditor();
                        break;
                    case "Edit":
                        if (EventListView.SelectedItem is string SelectedItem && SelectedItem != null)
                        {
                            ActionType = 2;
                            Catalog.MainWindow.OpenEventrEditor(SelectedItem);
                        }
                        break;
                    case "Delete":
                        if (Logger.MsgBox("确定删除此行数据？\n它将会永远失去！（真的很久！）", "Serein", 1, 48) && EventListView.SelectedIndex >= 0)
                            EventListView.Items.RemoveAt(EventListView.SelectedIndex);
                        break;
                    case "LookupEvent":
                        Process.Start(new ProcessStartInfo("https://serein.cc/#/Function/Event") { UseShellExecute = true });
                        break;
                }
            }
        }

        public bool Confirm(string Command)
        {
            if (Base.Command.GetType(Command) < 0)
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
                        Command);
                }
                else
                {
                    EventListView.Items.Add(Command);
                }
                Save();
            }
            else if (ActionType == 2 && EventListView.SelectedIndex >= 0)
            {
                EventListView.Items[EventListView.SelectedIndex] = Command;
                Save();
                Load();
            }
            ActionType = 0;
            return true;
        }

        private void Save()
        {
            if (Enum.IsDefined(typeof(Items.EventType), SelectedTag))
            {
                Global.Settings.Event.Edit(GetEventCommands(), (Items.EventType)Enum.Parse(typeof(Items.EventType), SelectedTag));
                IO.SaveEventSetting();
            }
        }

        private string[] GetEventCommands()
        {
            List<string> Commands = new List<string>();
            foreach (object Item in EventListView.Items)
            {
                Commands.Add(Item as string ?? string.Empty);
            }
            return Commands.ToArray();
        }

        private void Load()
        {
            if (Events.SelectedItem is System.Windows.Controls.TreeViewItem Item && Item != null)
            {
                EventListView.Items.Clear();
                SelectedTag = Item.Tag as string ?? string.Empty;
                if (Enum.IsDefined(typeof(Items.EventType), SelectedTag))
                    Global.Settings.Event.Get((Items.EventType)Enum.Parse(typeof(Items.EventType), SelectedTag)).ToList().ForEach((Command) => { EventListView.Items.Add(Regex.Replace(Command, @"(\n|\r|\\n|\\r)+", "\\n")); });
            }
            else
                SelectedTag = string.Empty;
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
