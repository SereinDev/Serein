using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Lite.Ui.Servers;

public partial class ServerPage : UserControl
{
    private readonly ServerManager _serverManager;
    private readonly ResourcesManager _resourcesManager;

    public Dictionary<string, TabPage> Panels { get; }

    public ServerPage(ServerManager serverManager, ResourcesManager resourcesManager)
    {
        InitializeComponent();

        Panels = [];
        _serverManager = serverManager;
        _resourcesManager = resourcesManager;
        _serverManager.ServersUpdated += Update;

        foreach (var (id, server) in _serverManager.Servers)
        {
            Add(id, server);
        }

        StatusStrip.Visible = Panels.Count == 0;
    }

    private void Add(string id, Server server)
    {
        if (!File.Exists(ResourcesManager.IndexPath))
            _resourcesManager.WriteConsoleHtml();

        var tabPage = new TabPage
        {
            Text = string.IsNullOrEmpty(server.Configuration.Name)
                ? $"未命名-{id}"
                : server.Configuration.Name,
            Tag = id
        };

        tabPage.Controls.Add(new Panel(server));
        Panels[id] = tabPage;
        MainTabControl.Controls.Add(tabPage);
    }

    private void Update(object? sender, ServersUpdatedEventArgs e)
    {
        Invoke(() =>
        {
            if (
                e.Type == ServersUpdatedType.Added
                && _serverManager.Servers.TryGetValue(e.Id, out var server)
            )
                Add(e.Id, server);
            else if (Panels.TryGetValue(e.Id, out var page))
            {
                MainTabControl.Controls.Remove(page);
                Panels.Remove(e.Id);
            }

            StatusStrip.Visible = Panels.Count == 0;
        });
    }
}
