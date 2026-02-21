using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Win32;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Gui.Commands;
using Serein.Gui.Services;
using Serein.Gui.Windows;

namespace Serein.Gui.ViewModels;

public class ServerPageViewModel
{
    private readonly MainWindow _mainWindow;
    private readonly InfoBarProvider _infoBarProvider;
    private readonly ServerManager _serverManager;

    private CancellationTokenSource? _cancellationTokenSource;

    public ServerPageViewModel(
        MainWindow mainWindow,
        InfoBarProvider infoBarProvider,
        ServerManager serverManager
    )
    {
        _mainWindow = mainWindow;
        _infoBarProvider = infoBarProvider;
        _serverManager = serverManager;

        Panels = [];

        foreach (var (id, server) in _serverManager.Servers)
        {
            AddPanel(id, server);
        }

        AddCommand = new(Add);
        ImportCommand = new(Import);

        _serverManager.ServersUpdated += ServerManager_ServersUpdated;
    }

    public ObservableCollection<PanelViewModel> Panels { get; }
    public RelayCommand AddCommand { get; }
    public RelayCommand ImportCommand { get; }

    public void OnLoaded()
    {
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new();

        if (_serverManager.Servers.Count == 0)
        {
            _infoBarProvider.Enqueue(
                "当前没有服务器配置",
                "你可以点击上方的加号进行添加",
                InfoBarSeverity.Informational,
                TimeSpan.FromSeconds(5),
                _cancellationTokenSource.Token
            );
        }
    }

    public void OnUnloaded()
    {
        if (_cancellationTokenSource?.IsCancellationRequested == false)
        {
            _cancellationTokenSource.Cancel();
        }
    }

    private void AddPanel(string id, Server server)
    {
        Panels.Add(new(id, server, _serverManager, _mainWindow));
    }

    private void ServerManager_ServersUpdated(object? sender, ServersUpdatedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (
                e.Type == ServersUpdatedType.Added
                && _serverManager.Servers.TryGetValue(e.Id, out var server)
            )
            {
                AddPanel(e.Id, server);
                return;
            }

            var panel = Panels.FirstOrDefault(item => item.Server.Id == e.Id);
            if (panel == null)
            {
                return;
            }

            Panels.Remove(panel);

            _infoBarProvider.Enqueue(
                $"服务器（Id: {e.Id}）删除成功",
                string.Empty,
                InfoBarSeverity.Success
            );
        });
    }

    private void Import()
    {
        try
        {
            var dialog = new OpenFileDialog { Filter = "服务器配置文件|*.json" };
            if (dialog.ShowDialog() != true)
            {
                return;
            }

            var config = ServerManager.LoadFrom(dialog.FileName);
            var editor = new ServerConfigurationEditor(_serverManager, config)
            {
                Owner = _mainWindow,
            };

            if (editor.ShowDialog() != true || string.IsNullOrEmpty(editor.Id))
            {
                return;
            }

            _serverManager.Add(editor.Id, editor.Configuration);
        }
        catch (Exception ex)
        {
            _infoBarProvider.Enqueue("导入失败", ex.Message, InfoBarSeverity.Error);
        }
    }

    private void Add()
    {
        try
        {
            var names = _serverManager.Servers.Select(kv => kv.Value.Configuration.Name);
            var configuration = new Configuration();

            if (names.Any(name => name == Configuration.DefaultName))
            {
                var i = 0L;

                while (true)
                {
                    i++;

                    if (names.All(name => name != $"{Configuration.DefaultName}-{i}"))
                    {
                        break;
                    }
                }

                configuration.Name = $"{Configuration.DefaultName}-{i}";
            }

            var editor = new ServerConfigurationEditor(_serverManager, configuration)
            {
                Owner = _mainWindow,
            };

            if (editor.ShowDialog() != true || string.IsNullOrEmpty(editor.Id))
            {
                return;
            }

            _serverManager.Add(editor.Id, editor.Configuration);
        }
        catch (Exception ex)
        {
            _infoBarProvider.Enqueue("添加失败", ex.Message, InfoBarSeverity.Error);
        }
    }
}
