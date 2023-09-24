using System;
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

        private void Uri_TextChanged(object sender, TextChangedEventArgs e) =>
            Global.Settings.Bot.Uri = Uri.Text;

        private void Authorization_TextChanged(object sender, TextChangedEventArgs e) =>
            Global.Settings.Bot.Authorization = Authorization.Password;

        private void EnableLog_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Bot.EnableLog = EnableLog.IsChecked ?? false;

        private void GivePermissionToAllAdmin_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Bot.GivePermissionToAllAdmin =
                GivePermissionToAllAdmin.IsChecked ?? false;

        private void EnbaleOutputData_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Bot.EnbaleOutputData = EnbaleOutputData.IsChecked ?? false;

        private void AutoReconnect_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Bot.AutoReconnect = AutoReconnect.IsChecked ?? false;

        private void EnbaleParseAt_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Bot.EnbaleParseAt = EnbaleParseAt.IsChecked ?? false;

        private void AutoEscape_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Bot.AutoEscape = AutoEscape.IsChecked ?? false;

        private void GroupList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(GroupList.Text))
            {
                Global.Settings.Bot.GroupList = Array.Empty<long>();
            }
            else if (Regex.IsMatch(GroupList.Text, @"^[\d;]+?$"))
            {
                List<long> groups = new List<long>();
                foreach (string group in GroupList.Text.Split(';'))
                {
                    if (
                        group.Length >= 6
                        && group.Length <= 16
                        && long.TryParse(group, out long _group)
                    )
                    {
                        groups.Add(_group);
                    }
                }
                Global.Settings.Bot.GroupList = groups.ToArray();
            }
            string text = Regex.Replace(GroupList.Text, @"[^\d;]", ";");
            text = Regex.Replace(text, @";+", ";");
            text = Regex.Replace(text, "^;", string.Empty);
            if (text != GroupList.Text)
            {
                GroupList.Text = text; // 仅当有差异时更新文本
            }
        }

        private void PermissionList_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(PermissionList.Text))
            {
                Global.Settings.Bot.PermissionList = Array.Empty<long>();
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
                Global.Settings.Bot.PermissionList = IDs.ToArray();
            }
            string text = Regex.Replace(PermissionList.Text, @"[^\d,]", ";");
            text = Regex.Replace(text, @";+", ";");
            text = Regex.Replace(text, "^;", string.Empty);
            if (text != PermissionList.Text)
            {
                PermissionList.Text = text; // 仅当有差异时更新文本
            }
        }
    }
}
