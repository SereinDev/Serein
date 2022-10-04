using Serein.Base;
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
                        {
                            EventListView.Items.RemoveAt(EventListView.SelectedIndex);
                        }
                        break;
                    case "LookupEvent":
                        Process.Start(new ProcessStartInfo("https://serein.cc/Event.html") { UseShellExecute = true });
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
            switch (SelectedTag)
            {
                case "BindSuccess":
                    Global.Settings.Event.Bind_Success = GetEventCommands();
                    break;
                case "BindOccupied":
                    Global.Settings.Event.Bind_Occupied = GetEventCommands();
                    break;
                case "BindInvalid":
                    Global.Settings.Event.Bind_Invalid = GetEventCommands();
                    break;
                case "BindAlready":
                    Global.Settings.Event.Bind_Already = GetEventCommands();
                    break;
                case "UnbindSuccess":
                    Global.Settings.Event.Unbind_Success = GetEventCommands();
                    break;
                case "UnbindFailure":
                    Global.Settings.Event.Unbind_Failure = GetEventCommands();
                    break;
                case "ServerStart":
                    Global.Settings.Event.Server_Start = GetEventCommands();
                    break;
                case "ServerStop":
                    Global.Settings.Event.Server_Stop = GetEventCommands();
                    break;
                case "ServerError":
                    Global.Settings.Event.Server_Error = GetEventCommands();
                    break;
                case "GroupIncrease":
                    Global.Settings.Event.Group_Increase = GetEventCommands();
                    break;
                case "GroupDecrease":
                    Global.Settings.Event.Group_Decrease = GetEventCommands();
                    break;
                case "GroupPoke":
                    Global.Settings.Event.Group_Poke = GetEventCommands();
                    break;
                case "SereinCrash":
                    Global.Settings.Event.Serein_Crash = GetEventCommands();
                    break;
                case "MotdpeSuccess":
                    Global.Settings.Event.Motdpe_Success = GetEventCommands();
                    break;
                case "MotdjeSuccess":
                    Global.Settings.Event.Motdje_Success = GetEventCommands();
                    break;
                case "MotdFailure":
                    Global.Settings.Event.Motd_Failure = GetEventCommands();
                    break;
                case "PermissionDeniedPrivate":
                    Global.Settings.Event.PermissionDenied_Private = GetEventCommands();
                    break;
                case "PermissionDeniedGroup":
                    Global.Settings.Event.PermissionDenied_Group = GetEventCommands();
                    break;
            }
            IO.SaveEventSetting();
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
                string[] TargetEvent = new string[] { };
                switch (SelectedTag)
                {
                    case "BindSuccess":
                        TargetEvent = Global.Settings.Event.Bind_Success;
                        break;
                    case "BindOccupied":
                        TargetEvent = Global.Settings.Event.Bind_Occupied;
                        break;
                    case "BindInvalid":
                        TargetEvent = Global.Settings.Event.Bind_Invalid;
                        break;
                    case "BindAlready":
                        TargetEvent = Global.Settings.Event.Bind_Already;
                        break;
                    case "UnbindSuccess":
                        TargetEvent = Global.Settings.Event.Unbind_Success;
                        break;
                    case "UnbindFailure":
                        TargetEvent = Global.Settings.Event.Unbind_Failure;
                        break;
                    case "ServerStart":
                        TargetEvent = Global.Settings.Event.Server_Start;
                        break;
                    case "ServerStop":
                        TargetEvent = Global.Settings.Event.Server_Stop;
                        break;
                    case "ServerError":
                        TargetEvent = Global.Settings.Event.Server_Error;
                        break;
                    case "GroupIncrease":
                        TargetEvent = Global.Settings.Event.Group_Increase;
                        break;
                    case "GroupDecrease":
                        TargetEvent = Global.Settings.Event.Group_Decrease;
                        break;
                    case "GroupPoke":
                        TargetEvent = Global.Settings.Event.Group_Poke;
                        break;
                    case "SereinCrash":
                        TargetEvent = Global.Settings.Event.Serein_Crash;
                        break;
                    case "MotdpeSuccess":
                        TargetEvent = Global.Settings.Event.Motdpe_Success;
                        break;
                    case "MotdjeSuccess":
                        TargetEvent = Global.Settings.Event.Motdje_Success;
                        break;
                    case "MotdFailure":
                        TargetEvent = Global.Settings.Event.Motd_Failure;
                        break;
                    case "PermissionDeniedPrivate":
                        TargetEvent = Global.Settings.Event.PermissionDenied_Private;
                        break;
                    case "PermissionDeniedGroup":
                        TargetEvent = Global.Settings.Event.PermissionDenied_Group;
                        break;
                }
                TargetEvent.ToList().ForEach((Command) => { EventListView.Items.Add(Regex.Replace(Command, @"(\n|\r|\\n|\\r)+", "\\n")); });
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
