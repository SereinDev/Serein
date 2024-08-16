using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core;
using Serein.Core.Models.Server;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Commands;
using Serein.Core.Services.Data;
using Serein.Core.Services.Servers;
using Serein.Lite.Ui.Function;
using Serein.Lite.Ui.Members;
using Serein.Lite.Ui.Servers;
using Serein.Lite.Ui.Settings;
using Serein.Lite.Utils;

namespace Serein.Lite.Ui;

public partial class MainForm : Form
{
    public static readonly Color PrimaryColor = Color.FromArgb(0x4B, 0x73, 0x8D); // #4B738D

    private readonly IHost _host;
    private readonly ServerManager _serverManager;
    private readonly CommandParser _commandParser;
    private readonly SettingProvider _settingProvider;
    private readonly System.Timers.Timer _timer;

    private IServiceProvider Services => _host.Services;

    public MainForm(
        IHost host,
        ServerManager serverManager,
        CommandParser commandParser,
        SettingProvider settingProvider
    )
    {
        _host = host;
        _serverManager = serverManager;
        _commandParser = commandParser;
        _settingProvider = settingProvider;
        _timer = new(2000);
        _timer.Elapsed += (_, _) => Invoke(UpdateTitle);
        _settingProvider.Value.Application.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(ApplicationSetting.CustomTitle))
                if (InvokeRequired)
                    Invoke(UpdateTitle);
                else
                    UpdateTitle();
        };

        InitializeComponent();
        UpdateTitle();
    }

    private void UpdateTitle()
    {
        var text = _commandParser.ApplyVariables(_settingProvider.Value.Application.CustomTitle, null);

        Text = !string.IsNullOrEmpty(text.Trim())
            ? $"Serein.Lite - {text}"
            : "Serein.Lite";
    }

    private void SwitchPage<T>()
        where T : UserControl
    {
        var page = Services.GetRequiredService<T>();
        page.Dock = DockStyle.Fill;

        ChildrenPanel.Controls.Clear();
        ChildrenPanel.Controls.Add(page);

        if (page is IUpdateablePage updateablePage)
            updateablePage.UpdatePage();
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

    private void BindingToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<BindingPage>();
    }

    private void PermissionGroupToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<PermissionGroupPage>();
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
            MessageBoxHelper.ShowWarningMsgBox(ex.Message);
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

        var id = serverPage
            .MainTabControl.Controls[serverPage.MainTabControl.SelectedIndex]
            .Tag?.ToString();

        if (
            !string.IsNullOrEmpty(id)
            && MessageBoxHelper.ShowDeleteConfirmation($"确定要删除此服务器配置（{id}）吗？")
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

    protected override void OnClosing(CancelEventArgs e)
    {
        if (_serverManager.AnyRunning)
        {
            e.Cancel = true;

            TopMost = false;
            Visible = false;
            ShowInTaskbar = false;

            TopMostToolStripMenuItem.Checked = false;
            TopMostToolStripMenuItem.Enabled = false;
            HideToolStripMenuItem.Checked = true;
            HideToolStripMenuItem.Enabled = true;

            NotifyIcon.ShowBalloonTip(
                5000,
                "还有服务器尚未停止运行",
                "Serein.Lite 已隐藏窗口并在后台运行。你可以右键托盘图标取消隐藏或直接双击托盘图标以取消隐藏。",
                ToolTipIcon.None
            );
        }
        else
        {
            NotifyIcon.Visible = false;
            _host.StopAsync();
            _timer.Stop();
        }
        base.OnClosing(e);
    }

    protected override void OnShown(EventArgs e)
    {
        SwitchPage<PluginPage>();
        SwitchPage<ConnectionPage>();
        // 初始化并创建控件

        SwitchPage<ServerPage>();

        if (SereinApp.StartForTheFirstTime)
            DialogFactory.ShowWelcomeDialog();

        _host.StartAsync();
        _timer.Start();
        base.OnShown(e);
    }

    private void NotifyIcon_DoubleClick(object sender, EventArgs e)
    {
        FocusWindow();

        TopMostToolStripMenuItem.Enabled = true;
        HideToolStripMenuItem.Checked = false;
    }

    public void FocusWindow()
    {
        Visible = true;
        ShowInTaskbar = true;
        WindowState = FormWindowState.Normal;
        Activate();
    }
}
