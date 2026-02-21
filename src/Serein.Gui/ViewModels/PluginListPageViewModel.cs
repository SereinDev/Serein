using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;
using iNKORE.UI.WPF.Modern.Controls;
using Serein.Core.Models.Plugins;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Core.Utils;
using Serein.Core.Utils.Extensions;
using Serein.Gui.Commands;
using Serein.Gui.Services;
using Serein.Gui.Utils;

namespace Serein.Gui.ViewModels;

public class PluginListPageViewModel
{
    private readonly InfoBarProvider _infoBarProvider;
    private readonly PluginManager _pluginManager;
    private readonly JsPluginLoader _jsPluginLoader;
    private readonly NetPluginLoader _netPluginLoader;
    private readonly PluginConsoleViewModel _pluginConsoleViewModel;
    private readonly Dispatcher _dispatcher;

    private KeyValuePair<string, IPlugin>? _selectedPlugin;

    public PluginListPageViewModel(
        PluginConsoleViewModel pluginConsoleViewModel,
        InfoBarProvider infoBarProvider,
        PluginManager pluginManager,
        JsPluginLoader jsPluginLoader,
        NetPluginLoader netPluginLoader
    )
    {
        _pluginConsoleViewModel = pluginConsoleViewModel;
        _infoBarProvider = infoBarProvider;
        _pluginManager = pluginManager;
        _jsPluginLoader = jsPluginLoader;
        _netPluginLoader = netPluginLoader;
        _dispatcher = Dispatcher.CurrentDispatcher;

        PluginInfos = [];

        DisableCommand = new(DisableSelected);
        ReloadCommand = new(ReloadAll);
        ClearConsoleCommand = new(ClearConsole);
        OpenDocCommand = new(OpenDoc);

        UpdatePlugins();
        _pluginManager.PluginsLoaded += (_, _) => UpdatePlugins();
    }

    public ObservableCollection<KeyValuePair<string, IPlugin>> PluginInfos { get; }

    public string StatusBarText { get; set; } = string.Empty;

    public bool CanDisable { get; set; }

    public RelayCommand DisableCommand { get; }

    public RelayCommand ReloadCommand { get; }

    public RelayCommand ClearConsoleCommand { get; }

    public RelayCommand OpenDocCommand { get; }

    public void UpdateSelection(KeyValuePair<string, IPlugin>? selectedPlugin)
    {
        _selectedPlugin = selectedPlugin;
        CanDisable = selectedPlugin is not null;

        StatusBarText = selectedPlugin is { } kv
            ? $"Id={kv.Key}\r\nPath={kv.Value.FileName}"
            : string.Empty;
    }

    private void UpdatePlugins()
    {
        if (_dispatcher.CheckAccess())
        {
            UpdatePluginsCore();
        }
        else
        {
            _dispatcher.InvokeAsync(UpdatePluginsCore, DispatcherPriority.DataBind);
        }
    }

    private void UpdatePluginsCore()
    {
        PluginInfos.Clear();

        foreach (var kv in _jsPluginLoader.Plugins)
        {
            PluginInfos.Add(new(kv.Key, kv.Value));
        }

        foreach (var kv in _netPluginLoader.Plugins)
        {
            PluginInfos.Add(new(kv.Key, kv.Value));
        }
    }

    private void OpenDoc()
    {
        UrlConstants.DocsPlugins.OpenInBrowser();
    }

    private void ReloadAll()
    {
        Task.Run(_pluginManager.Reload)
            .ContinueWith(
                (task) =>
                {
                    if (task.IsFaulted && task.Exception is not null)
                    {
                        _infoBarProvider.Enqueue(
                            "重新加载插件失败",
                            task.Exception.InnerException!.Message,
                            InfoBarSeverity.Error
                        );
                    }
                }
            );
    }

    private void ClearConsole()
    {
        _pluginConsoleViewModel.ClearConsole();
    }

    private void DisableSelected()
    {
        if (_selectedPlugin is not { } kv)
        {
            return;
        }

        try
        {
            kv.Value.Disable();
            _infoBarProvider.Enqueue(
                $"插件（Id={kv.Key}）禁用成功",
                string.Empty,
                InfoBarSeverity.Success
            );
        }
        catch (Exception ex)
        {
            MessageBoxEx.ShowException(ex, "禁用失败");
        }
    }
}
