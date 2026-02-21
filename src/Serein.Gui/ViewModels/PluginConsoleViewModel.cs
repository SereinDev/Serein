using System;
using System.Threading.Tasks;
using iNKORE.UI.WPF.Modern.Controls;
using PropertyChanged;
using Serein.Core.Models.Abstractions;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Plugins.Js;
using Serein.Core.Services.Plugins.Net;
using Serein.Gui.Commands;
using Serein.Gui.Services;

namespace Serein.Gui.ViewModels;

public class PluginConsoleViewModel : NotifyPropertyChangedModelBase
{
    private readonly InfoBarProvider _infoBarProvider;
    private readonly PluginManager _pluginManager;
    private readonly NetPluginLoader _netPluginLoader;
    private readonly JsPluginLoader _jsPluginLoader;

    public PluginConsoleViewModel(
        InfoBarProvider infoBarProvider,
        PluginManager pluginManager,
        NetPluginLoader netPluginLoader,
        JsPluginLoader jsPluginLoader
    )
    {
        _infoBarProvider = infoBarProvider;
        _pluginManager = pluginManager;
        _netPluginLoader = netPluginLoader;
        _jsPluginLoader = jsPluginLoader;

        ReloadCommand = new(Reload);
        ClearCommand = new(ClearConsole);

        UpdateStats();
        _pluginManager.PluginsLoaded += (_, _) => UpdateStats();
        _pluginManager.PluginsReloading += (_, _) => RequestClearConsole?.Invoke();
    }

    public int Total => JavaScriptPluginCount + NetPluginCount;

    [AlsoNotifyFor(nameof(Total))]
    public int JavaScriptPluginCount { get; internal set; }

    [AlsoNotifyFor(nameof(Total))]
    public int NetPluginCount { get; internal set; }

    public RelayCommand ReloadCommand { get; }

    public RelayCommand ClearCommand { get; }

    public event Action? RequestClearConsole;

    public void ClearConsole()
    {
        RequestClearConsole?.Invoke();
    }

    private void Reload()
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

    private void UpdateStats()
    {
        JavaScriptPluginCount = _jsPluginLoader.Plugins.Count;
        NetPluginCount = _netPluginLoader.Plugins.Count;
    }
}
