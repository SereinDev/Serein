using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Settings
{
    public partial class Bot : UiPage
    {
        public Bot()
        {
            InitializeComponent();
            Load();
            Catalog.Settings.Bot = this;
        }

        private void Load()
        {
            Uri.Text = Global.Settings.Bot.Uri;
            Authorization.Text = Global.Settings.Bot.Authorization;
            EnableLog.IsChecked = Global.Settings.Bot.EnableLog;
            GivePermissionToAllAdmin.IsChecked = Global.Settings.Bot.GivePermissionToAllAdmin;
            EnbaleOutputData.IsChecked = Global.Settings.Bot.EnbaleOutputData;
            AutoReconnect.IsChecked = Global.Settings.Bot.AutoReconnect;
            AutoEscape.IsChecked = Global.Settings.Bot.AutoEscape;
            EnbaleParseAt.IsChecked = Global.Settings.Bot.EnbaleParseAt;
            GroupList.Text = string.Join(";", Global.Settings.Bot.GroupList);
            PermissionList.Text = string.Join(";", Global.Settings.Bot.PermissionList);
        }

        private void Uri_TextChanged(object sender, TextChangedEventArgs e)
            => Global.Settings.Bot.Uri = Uri.Text;

        private void Authorization_TextChanged(object sender, TextChangedEventArgs e)
            => Global.Settings.Bot.Authorization = Authorization.Password;

        private void EnableLog_Click(object sender, RoutedEventArgs e)
            => Global.Settings.Bot.EnableLog = EnableLog.IsChecked ?? false;

        private void GivePermissionToAllAdmin_Click(object sender, RoutedEventArgs e)
            => Global.Settings.Bot.GivePermissionToAllAdmin = GivePermissionToAllAdmin.IsChecked ?? false;

        private void EnbaleOutputData_Click(object sender, RoutedEventArgs e)
            => Global.Settings.Bot.EnbaleOutputData = EnbaleOutputData.IsChecked ?? false;

        private void AutoReconnect_Click(object sender, RoutedEventArgs e)
            => Global.Settings.Bot.AutoReconnect = AutoReconnect.IsChecked ?? false;

        private void EnbaleParseAt_Click(object sender, RoutedEventArgs e)
            => Global.Settings.Bot.EnbaleParseAt = EnbaleParseAt.IsChecked ?? false;

        private void AutoEscape_Click(object sender, RoutedEventArgs e)
            => Global.Settings.Bot.AutoEscape = AutoEscape.IsChecked ?? false;

        private void GroupList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(GroupList.Text))
            {
                Global.Settings.Bot.GroupList.Clear();
            }
            else if (Regex.IsMatch(GroupList.Text, @"^[\d;]+?$"))
            {
                List<long> Groups = new List<long>();
                foreach (string Group in GroupList.Text.Split(';'))
                {
                    if (Group.Length >= 6 && Group.Length <= 16 && long.TryParse(Group, out long _Group))
                    {
                        Groups.Add(_Group);
                    }
                }
                Global.Settings.Bot.GroupList = Groups;
            }
            string Text = Regex.Replace(GroupList.Text, @"[^\d;]", ";");
            Text = Regex.Replace(Text, @";+", ";");
            Text = Regex.Replace(Text, "^;", string.Empty);
            if (Text != GroupList.Text)
            {
                GroupList.Text = Text; // 仅当有差异时更新文本
            }
        }

        private void PermissionList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(PermissionList.Text))
            {
                Global.Settings.Bot.PermissionList.Clear();
            }
            else if (Regex.IsMatch(PermissionList.Text, @"^[\d;]+?$"))
            {
                List<long> IDs = new List<long>();
                foreach (string ID in PermissionList.Text.Split(';'))
                {
                    if (ID.Length >= 5 && ID.Length <= 13 && long.TryParse(ID, out long _ID))
                    {
                        IDs.Add(_ID);
                    }
                }
                Global.Settings.Bot.PermissionList = IDs;
            }
            string Text = Regex.Replace(PermissionList.Text, @"[^\d,]", ";");
            Text = Regex.Replace(Text, @";+", ";");
            Text = Regex.Replace(Text, "^;", string.Empty);
            if (Text != PermissionList.Text)
            {
                PermissionList.Text = Text; // 仅当有差异时更新文本
            }
        }
    }
}
