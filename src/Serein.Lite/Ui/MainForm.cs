using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Json;
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


    private void SwitchPage<T>() where T : Control
    {
        var page = Services.GetRequiredService<T>();
        page.Dock = DockStyle.Fill;

        if (Controls.Count == 2)
            Controls.RemoveAt(1);

        Controls.Add(page);
    }

    private void ServerImportToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "选择服务器配置",
            Filter = "服务器配置文件|*.json"
        };
        var result = openFileDialog.ShowDialog();

        if (result != DialogResult.OK || string.IsNullOrEmpty(openFileDialog.FileName))
            return;

        var configuration = JsonSerializer.Deserialize<Configuration>(
               File.ReadAllText(openFileDialog.FileName),
               JsonSerializerOptionsFactory.CamelCase
           );

        if (configuration == null)
        {
            MessageBox.Show("配置文件为空", "Serein.Lite");
            return;
        }

        var editor = new ConfigurationEditor(configuration);
        result = editor.ShowDialog();

        if (result == DialogResult.OK)
            _serverManager.Add(editor.Id, configuration);
    }


    private void ServerConsoleToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<ServerPage>();
    }

    private void MatchToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SwitchPage<MatchPage>();
    }

    private void ServerAddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var configuration = new Configuration();

        var editor = new ConfigurationEditor(configuration);
        if (editor.ShowDialog() == DialogResult.OK)
            _serverManager.Add(editor.Id, configuration);
    }

    private void ServerToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
        ServerEditToolStripMenuItem.Enabled = _serverManager.Servers.Count > 0;
        ServerRemoveToolStripMenuItem.Enabled = _serverManager.Servers.Count > 0;
    }
}
