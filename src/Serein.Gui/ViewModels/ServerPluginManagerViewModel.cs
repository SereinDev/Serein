using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Win32;
using Serein.Core.Models.Abstractions;
using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;
using Serein.Gui.Commands;
using Serein.Gui.Utils;

namespace Serein.Gui.ViewModels;

public class ServerPluginManagerViewModel : NotifyPropertyChangedModelBase
{
    private readonly Server _server;
    private ServerPlugin[] _selectedPlugins;

    public ServerPluginManagerViewModel(Server server)
    {
        _server = server;
        _selectedPlugins = [];

        _server.PluginManager.Update();
        Plugins = [.. _server.PluginManager.Plugins];
        Title = "服务器插件管理 - " + _server.Configuration.Name;

        ImportCommand = new(Import);
        RemoveCommand = new(RemoveSelected);
        EnableCommand = new(EnableSelected);
        DisableCommand = new(DisableSelected);
        OpenInExplorerCommand = new(OpenSelectedInExplorer);
        RefreshCommand = new(Refresh);
    }

    public string Title { get; }

    public ObservableCollection<ServerPlugin> Plugins { get; }

    public bool Remove { get; set; }

    public bool Enable { get; set; }

    public bool Disable { get; set; }

    public bool OpenInExplorer { get; set; }

    public RelayCommand ImportCommand { get; }

    public RelayCommand RemoveCommand { get; }

    public RelayCommand EnableCommand { get; }

    public RelayCommand DisableCommand { get; }

    public RelayCommand OpenInExplorerCommand { get; }

    public RelayCommand RefreshCommand { get; }

    public void UpdateSelection(IEnumerable<ServerPlugin> selectedPlugins)
    {
        _selectedPlugins = [.. selectedPlugins];

        Remove = OpenInExplorer = _selectedPlugins.Length > 0;
        Enable = Remove && !_selectedPlugins.Any((plugin) => plugin.IsEnabled);
        Disable = Remove && _selectedPlugins.All((plugin) => plugin.IsEnabled);
    }

    private void Refresh()
    {
        _server.PluginManager.Update();
        UpdatePlugins();
    }

    private void Import()
    {
        var openFileDialog = new OpenFileDialog { Filter = "插件文件|*.dll;*.jar;*.js;*.py;*.lua" };
        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                _server.PluginManager.Add(openFileDialog.FileNames);
            }
            catch (Exception ex)
            {
                DialogFactory.ShowSimpleDialog("导入失败", ex.Message);
            }
        }

        _server.PluginManager.Update();
        UpdatePlugins();
    }

    private void EnableSelected()
    {
        foreach (var plugin in _selectedPlugins)
        {
            try
            {
                _server.PluginManager.Enable(plugin);
            }
            catch (Exception ex)
            {
                DialogFactory.ShowSimpleDialog(
                    $"启用插件\"{plugin.FriendlyName}\"失败",
                    ex.Message
                );
                break;
            }
        }
    }

    private void DisableSelected()
    {
        foreach (var plugin in _selectedPlugins)
        {
            try
            {
                _server.PluginManager.Disable(plugin);
            }
            catch (Exception ex)
            {
                DialogFactory.ShowSimpleDialog(
                    $"禁用插件\"{plugin.FriendlyName}\"失败",
                    ex.Message
                );
                break;
            }
        }
    }

    private void RemoveSelected()
    {
        var count = _selectedPlugins.Length;
        if (count <= 0)
        {
            return;
        }

        DialogFactory
            .ShowDeleteConfirmation(
                count == 1
                    ? $"确定要删除\"{_selectedPlugins.First().FriendlyName}\"吗？"
                    : $"确定要删除\"{_selectedPlugins.First().FriendlyName}\"等{count}个插件吗？"
            )
            .ContinueWith(
                (task) =>
                {
                    if (task.Result)
                    {
                        foreach (var plugin in _selectedPlugins)
                        {
                            try
                            {
                                _server.PluginManager.Remove(plugin);
                            }
                            catch (Exception ex)
                            {
                                DialogFactory.ShowSimpleDialog(
                                    $"删除插件\"{plugin.FriendlyName}\"失败",
                                    ex.Message
                                );
                                break;
                            }
                        }
                    }
                }
            );
    }

    private void OpenSelectedInExplorer()
    {
        _selectedPlugins.FirstOrDefault()?.Path.OpenInExplorer();
    }

    private void UpdatePlugins()
    {
        Plugins.Clear();
        foreach (var plugin in _server.PluginManager.Plugins)
        {
            Plugins.Add(plugin);
        }
    }
}
