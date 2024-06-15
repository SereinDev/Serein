using System;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Lite.Ui.Function;
using Serein.Lite.Ui.Servers;

namespace Serein.Lite.Ui;

public partial class MainForm : Form
{
    private readonly IHost _host;
    private readonly ServerManager _serverManager;

    private IServiceProvider Services => _host.Services;

    public MainForm(IHost host, ServerManager serverManager)
    {
        _host = host;
        _serverManager = serverManager;

        InitializeComponent();
        SwitchPage<ServerPage>();
    }

    private void SwitchPage<T>()
        where T : Control
    {
        var page = Services.GetRequiredService<T>();
        page.Dock = DockStyle.Fill;

        ChildrenPanel.Controls.Clear();
        ChildrenPanel.Controls.Add(page);
    }

    private void ServerConsoleToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<ServerPage>();
    }

    private void MatchToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<MatchPage>();
    }

    private void ConnectionToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<ConnectionPage>();
    }

    private void ScheduleToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<SchedulePage>();
    }

    private void SettingToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<SettingPage>();
    }

    private void PluginToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<PluginPage>();
    }

    private void ServerToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
        var isServerPage =
            ChildrenPanel.Controls.Count == 1
            && ChildrenPanel.Controls[0].GetType() == typeof(ServerPage);
        ServerEditToolStripMenuItem.Enabled = isServerPage && _serverManager.Servers.Count > 0;
        ServerRemoveToolStripMenuItem.Enabled = isServerPage && _serverManager.Servers.Count > 0;
    }

    private void ServerAddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<ServerPage>();

        var configuration = new Configuration();

        var editor = new ConfigurationEditor(_serverManager, configuration);
        if (editor.ShowDialog() == DialogResult.OK)
            _serverManager.Add(editor.Id, configuration);
    }

    private void ServerImportToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<ServerPage>();

        var openFileDialog = new OpenFileDialog { Title = "选择服务器配置", Filter = "服务器配置文件|*.json" };

        if (
            openFileDialog.ShowDialog() != DialogResult.OK
            || string.IsNullOrEmpty(openFileDialog.FileName)
        )
            return;

        Configuration configuration;

        try
        {
            configuration = ServerManager.LoadFrom(openFileDialog.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Serein.Lite", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var editor = new ConfigurationEditor(_serverManager, configuration);

        if (editor.ShowDialog() == DialogResult.OK)
            _serverManager.Add(editor.Id, configuration);
    }

    private void ServerEditToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var serverPage = Services.GetRequiredService<ServerPage>();

        if (serverPage.MainTabControl.Controls.Count == 0)
            return;

        var panel = serverPage.MainTabControl.Controls[serverPage.MainTabControl.SelectedIndex];
        var id = panel.Tag?.ToString();

        if (string.IsNullOrEmpty(id) || !_serverManager.Servers.TryGetValue(id, out var server))
            return;

        var editor = new ConfigurationEditor(_serverManager, server.Configuration, id);
        editor.ShowDialog();

        panel.Text = string.IsNullOrEmpty(server.Configuration.Name)
            ? $"未命名-{id}"
            : server.Configuration.Name;
        _serverManager.SaveAll();
    }

    private void ServerRemoveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var serverPage = Services.GetRequiredService<ServerPage>();

        if (
            serverPage.MainTabControl.Controls.Count == 0
            || serverPage.MainTabControl.SelectedIndex == -1
        )
            return;

        var id = serverPage.MainTabControl.Controls[
            serverPage.MainTabControl.SelectedIndex
        ].Tag?.ToString();

        if (
            !string.IsNullOrEmpty(id)
            && MessageBox.Show(
                $"是否删除服务器配置（{id}）",
                "Serein.Lite",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            ) == DialogResult.Yes
        )
            _serverManager.Remove(id);
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ShowInTaskbar = true;
        Visible = true;
        Close();
    }

    private void HideToolStripMenuItem_Click(object sender, EventArgs e)
    {
        HideToolStripMenuItem.Checked = !HideToolStripMenuItem.Checked;
        TopMostToolStripMenuItem.Enabled = !HideToolStripMenuItem.Checked;
        ShowInTaskbar = !HideToolStripMenuItem.Checked;
        Visible = !HideToolStripMenuItem.Checked;
    }

    private void TopMostToolStripMenuItem_Click(object sender, EventArgs e)
    {
        TopMostToolStripMenuItem.Checked = !TopMostToolStripMenuItem.Checked;
        HideToolStripMenuItem.Enabled = !TopMostToolStripMenuItem.Checked;
        TopMost = TopMostToolStripMenuItem.Checked;
    }
}
