using System;
using System.ComponentModel;
using System.Windows.Forms;

using Ookii.Dialogs.WinForms;

using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Lite.Utils.Native;

namespace Serein.Lite.Ui.Settings;

public partial class SshSettingPage : UserControl
{
    private readonly SettingProvider _settingProvider;

    public SshSettingPage(SettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
        InitializeComponent();

        UsersListView.SetExploreTheme();
        IpAddressTextBox.DataBindings.Add(
            nameof(IpAddressTextBox.Text),
            _settingProvider.Value.Ssh,
            nameof(SshSetting.IpAddress)
            );
        PortNumericUpDown.DataBindings.Add(
            nameof(PortNumericUpDown.Value),
            _settingProvider.Value.Ssh,
            nameof(SshSetting.Port)
            );
        LoadData();
    }

    private void LoadData()
    {
        UsersListView.BeginUpdate();
        UsersListView.Items.Clear();

        foreach (var kv in _settingProvider.Value.Ssh.Users)
        {
            var item = new ListViewItem(kv.Key) { Tag = kv };
            item.SubItems.Add(string.Empty.PadRight(kv.Value.Length, '*'));
            UsersListView.Items.Add(item);
        }

        UsersListView.EndUpdate();
    }

    private static (bool Result, string Password) InputPassword()
    {
        var dialog = new InputDialog
        {
            UsePasswordMasking = true,
            WindowTitle = "Serein.Lite",
            MainInstruction = "输入新的密码",
            Content = "· 长度大于或等于6\r\n" + "· 由字母、数字和特殊符号组成\r\n" + "· 不与此服务器登录密码一致",
        };

        return (dialog.ShowDialog() == DialogResult.OK, dialog.Input);
    }

    private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        DeleteToolStripMenuItem.Enabled = UpdatePasswordToolStripMenuItem.Enabled =
            UsersListView.SelectedItems.Count == 1;
    }

    private void AddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialog = new InputDialog
        {
            UsePasswordMasking = true,
            WindowTitle = "Serein.Lite",
            MainInstruction = "输入新的用户名",
        };

        if (dialog.ShowDialog() == DialogResult.OK)
            if (string.IsNullOrWhiteSpace(dialog.Input))
                MessageBox.Show("用户名为空", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (_settingProvider.Value.Ssh.Users.ContainsKey(dialog.Input))
                MessageBox.Show("用户名重复", "Serein", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                var (result, password) = InputPassword();
                if (result)
                {
                    _settingProvider.Value.Ssh.Users[dialog.Input] = password;
                    _settingProvider.SaveAsyncWithDebounce();
                    LoadData();
                }
            }
    }

    private void UpdatePasswordToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (UsersListView.SelectedItems.Count == 1)
        {
            var (result, password) = InputPassword();
            if (result)
            {
                _settingProvider.Value.Ssh.Users[UsersListView.SelectedItems[0].Text] = password;
                _settingProvider.SaveAsyncWithDebounce();
                LoadData();
            }
        }
    }

    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (UsersListView.SelectedItems.Count == 1)
        {
            _settingProvider.Value.Ssh.Users.Remove(UsersListView.SelectedItems[0].Text);
            _settingProvider.SaveAsyncWithDebounce();
            LoadData();
        }
    }

    private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    private void OnPropertyChanged(object sender, EventArgs e)
    {
        _settingProvider.SaveAsyncWithDebounce();
    }
}
