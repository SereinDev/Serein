using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;
using Serein.Core.Utils.Extensions;
using Serein.Plus.Dialogs;
using Serein.Plus.ViewModels;

namespace Serein.Plus.Windows;

public partial class ServerPluginManagerWindow : Window
{
    private readonly Server _server;
    public ObservableCollection<ServerPlugin> Plugins { get; }
    public ServerPluginManagerViewModel ViewModel { get; }

    public ServerPluginManagerWindow(Server server)
    {
        _server = server;
        _server.PluginManager.Update();
        Plugins = [.. _server.PluginManager.Plugins];
        ViewModel = new() { Title = "服务器插件管理 - " + _server.Configuration.Name };
        DataContext = this;

        InitializeComponent();
    }

    private void Update()
    {
        Plugins.Clear();
        foreach (var plugin in _server.PluginManager.Plugins)
        {
            Plugins.Add(plugin);
        }
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        var selectedPlugins = PluginListView.SelectedItems.OfType<ServerPlugin>();
        var count = selectedPlugins.Count();

        switch ((sender as MenuItem)?.Tag as string)
        {
            case "Refresh":
                _server.PluginManager.Update();
                Update();
                break;

            case "Import":
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "可接受的插件文件|*.dll;*.jar;*.js;*.py;*.lua",
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        _server.PluginManager.Add(openFileDialog.FileNames);
                    }
                    catch (Exception ex)
                    {
                        DialogHelper.ShowSimpleDialog("导入失败", ex.Message);
                    }
                }

                _server.PluginManager.Update();
                Update();
                break;

            case "Enable":
                foreach (var plugin in selectedPlugins)
                {
                    try
                    {
                        _server.PluginManager.Enable(plugin);
                    }
                    catch (Exception ex)
                    {
                        DialogHelper.ShowSimpleDialog(
                            $"启用插件\"{plugin.FriendlyName}\"失败",
                            ex.Message
                        );
                        break;
                    }
                }
                break;

            case "Disable":
                foreach (var plugin in selectedPlugins)
                {
                    try
                    {
                        _server.PluginManager.Disable(plugin);
                    }
                    catch (Exception ex)
                    {
                        DialogHelper.ShowSimpleDialog(
                            $"禁用插件\"{plugin.FriendlyName}\"失败",
                            ex.Message
                        );
                        break;
                    }
                }
                break;

            case "Remove":
                if (count <= 0)
                {
                    return;
                }

                DialogHelper
                    .ShowDeleteConfirmation(
                        count == 1
                            ? $"确定要删除\"{selectedPlugins.First().FriendlyName}\"吗？"
                            : $"确定要删除\"{selectedPlugins.First().FriendlyName}\"等{count}个插件吗？"
                    )
                    .ContinueWith(
                        (task) =>
                        {
                            if (task.Result)
                            {
                                foreach (var plugin in selectedPlugins)
                                {
                                    try
                                    {
                                        _server.PluginManager.Remove(plugin);
                                    }
                                    catch (Exception ex)
                                    {
                                        Dispatcher.Invoke(
                                            () =>
                                                DialogHelper.ShowSimpleDialog(
                                                    $"删除插件\"{plugin.FriendlyName}\"失败",
                                                    ex.Message
                                                )
                                        );
                                        break;
                                    }
                                }
                            }
                        }
                    );
                break;

            case "OpenInExplorer":
                selectedPlugins.FirstOrDefault()?.Path.OpenInExplorer();
                break;
        }
    }

    private void PluginListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedPlugins = PluginListView.SelectedItems.OfType<ServerPlugin>();

        ViewModel.Remove = ViewModel.OpenInExplorer = selectedPlugins.Any();
        ViewModel.Enable = ViewModel.Remove && !selectedPlugins.Any((plugin) => plugin.IsEnabled);
        ViewModel.Disable = ViewModel.Remove && !selectedPlugins.Any((plugin) => !plugin.IsEnabled);
    }
}
