﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.TrayIcons;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serein.Core;
using Serein.Core.Models;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Loggers;
using Serein.Core.Services.Network;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;
using Serein.Plus.Commands;
using Serein.Plus.Models;
using Serein.Plus.Pages;
using Serein.Plus.Pages.Settings;
using Serein.Plus.Services;
using Serein.Plus.Utils;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;

namespace Serein.Plus;

public partial class MainWindow : Window
{
    private bool _isTopMost;
    private readonly IHost _host;
    private readonly IServiceProvider _services;
    private readonly UpdateChecker _updateChecker;
    private readonly ServerManager _serverManager;
    private readonly EventDispatcher _eventDispatcher;
    private readonly ImportHandler _importHandler;
    private readonly DoubleAnimation _infoBarPopIn;
    private readonly DoubleAnimation _infoBarPopOut;

    public MainWindow(
        IHost host,
        IServiceProvider services,
        UpdateChecker updateChecker,
        ServerManager serverManager,
        SettingProvider settingProvider,
        EventDispatcher eventDispatcher,
        TitleUpdater titleUpdater,
        ImportHandler importHandler
    )
    {
        _host = host;
        _services = services;
        _updateChecker = updateChecker;
        _serverManager = serverManager;
        _eventDispatcher = eventDispatcher;
        _importHandler = importHandler;
        var powerEase = new PowerEase { EasingMode = EasingMode.EaseInOut };
        _infoBarPopIn = new(200, 0, new(TimeSpan.FromSeconds(0.5))) { EasingFunction = powerEase };
        _infoBarPopOut = new(0, 200, new(TimeSpan.FromSeconds(0.5))) { EasingFunction = powerEase };

        InitializeComponent();
        AppTrayIcon.ContextMenu!.DataContext = this;
        AppTrayIcon.DoubleClickCommand = new TaskbarIconDoubleClickCommand(this);

        DataContext = titleUpdater;
        titleUpdater.Update();

        ThemeManager.Current.ApplicationTheme = settingProvider.Value.Application.Theme switch
        {
            Theme.Light => ApplicationTheme.Light,
            Theme.Dark => ApplicationTheme.Dark,
            _ => null,
        };

        // 提前在ui线程实例化 避免在其他线程中实例化导致异常
        _services.GetRequiredService<ConnectionPage>();
        _services.GetRequiredService<PluginConsolePage>();

        _updateChecker.Updated += (_, _) =>
        {
            if (_updateChecker.Latest is not null)
            {
                Dispatcher.Invoke(() =>
                {
                    AppTrayIcon.ShowBalloonTip(
                        "发现新版本",
                        _updateChecker.Latest.TagName,
                        BalloonIcon.Info
                    );

                    if (NavView.SettingsItem is NavigationViewItem navViewItem)
                    {
                        navViewItem.InfoBadge ??= new() { Value = 1 };
                    }
                });
            }
        };
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem item)
        {
            return;
        }

        var tag = item.Tag as string;

        switch (tag)
        {
            case "TopMost":
                Topmost = _isTopMost = item.IsChecked;
                HideMenuItem.IsEnabled = !item.IsChecked;
                break;

            case "Exit":
                Close();
                break;

            case "Hide":
                if (item.IsChecked)
                {
                    Hide();
                }
                else
                {
                    ShowWindow();
                }

                TopMostMenuItem.IsEnabled = !item.IsChecked;
                break;
        }
    }

    public void ShowWindow()
    {
        Show();
        Activate();
        WindowState = WindowState.Normal;
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
        if (SereinAppBuilder.StartForTheFirstTime)
        {
            DialogFactory.ShowWelcomeDialog();
        }

        var processes = BaseChecker.CheckConflictProcesses();
        if (processes.Count > 0)
        {
            DialogFactory.ShowConflictWarning(processes);
        }

        if (FileLoggerProvider.IsEnabled)
        {
            new ContentDialog
            {
                Title = "嘿！你开启了日志模式",
                Content = new TextBlock
                {
                    Text =
                        $"在此模式下，应用程序会将完整的调试日志保存在\"{PathConstants.LogDirectory}/app\"目录下（可能很大很大很大，并对硬盘的读写速度产生一定影响）\r\n"
                        + "除非你知道你在干什么 / 是开发者要求的，请不要在此模式下运行Serein！！\r\n\r\n"
                        + "当然你也不需要太担心，若要退出此模式只需要重新启动就行啦 :D",
                },
                CloseButtonText = "我知道了",
                DefaultButton = ContentDialogButton.Close,
            }.ShowAsync();
        }

        _host.StartAsync();
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
        Topmost = _isTopMost;
    }

    private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        ShowInTaskbar = IsVisible;
        HideMenuItem.IsChecked = !IsVisible;
        TopMostMenuItem.IsEnabled = IsVisible;

        if (Topmost && !IsVisible)
        {
            Topmost = false;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        NavView.SelectedItem = NavView.MenuItems[0];
        ContentFrame.Navigate(_services.GetRequiredService<ServerPage>());
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        Hide();

        if (!_serverManager.AnyRunning)
        {
            AppTrayIcon.Visibility = Visibility.Collapsed;
            _eventDispatcher.Dispatch(Event.SereinClosed);
            return;
        }

        e.Cancel = true;
        HideMenuItem.IsEnabled = HideMenuItem.IsChecked = true;
        Topmost = _isTopMost = TopMostMenuItem.IsEnabled = TopMostMenuItem.IsChecked = false;

        AppTrayIcon.ShowBalloonTip(
            "仍有服务器进程在运行中",
            "已自动最小化至托盘，点击托盘图标即可复原窗口",
            BalloonIcon.None
        );
    }

    private void AppTrayIcon_TrayBalloonTipClicked(object sender, RoutedEventArgs e)
    {
        ShowWindow();
    }

    public void ShowInfoBar(InfoBarTask infoBarTask)
    {
        GlobalInfoBar.Title = infoBarTask.Title;
        GlobalInfoBar.Content = infoBarTask.Content;
        GlobalInfoBar.Message = string.IsNullOrEmpty(infoBarTask.Message)
            ? " "
            : infoBarTask.Message;
        GlobalInfoBar.Severity = infoBarTask.Severity;

        var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            infoBarTask.CancellationToken
        );

        if (GlobalInfoBar.RenderTransform is not TranslateTransform translateTransform)
        {
            return;
        }

        GlobalInfoBar.IsOpen = true;
        translateTransform.BeginAnimation(TranslateTransform.YProperty, _infoBarPopIn);

        GlobalInfoBar.Closed += Cancel;

        var interval =
            infoBarTask.Interval is not null && infoBarTask.Interval.Value.TotalSeconds > 3
                ? infoBarTask.Interval.Value
                : TimeSpan.FromSeconds(3);

        Task.Delay(interval, infoBarTask.CancellationToken)
            .ContinueWith(
                (_) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        translateTransform.BeginAnimation(
                            TranslateTransform.YProperty,
                            _infoBarPopOut
                        );
                        GlobalInfoBar.Closed -= Cancel;
                    });
                    Task.Delay(500, infoBarTask.CancellationToken)
                        .ContinueWith(
                            (_) => infoBarTask.ResetEvent.Set(),
                            infoBarTask.CancellationToken
                        );
                },
                infoBarTask.CancellationToken
            );

        void Cancel(object sender, EventArgs e)
        {
            infoBarTask.ResetEvent.Set();
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }
        }
    }

    private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        e.Handled = true;
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer.Tag is not Type type)
        {
            if (args.IsSettingsInvoked)
            {
                type = typeof(CategoriesPage);
                if (args.InvokedItem is NavigationViewItem navigationViewItem)
                {
                    navigationViewItem.InfoBadge = null;
                }
            }
            else
            {
                type = typeof(NotImplPage);
            }
        }

        var page = _services.GetRequiredService(type);

        ContentFrame.Navigate(page, args.RecommendedNavigationTransitionInfo);
    }

    private void NavView_DisplayModeChanged(
        NavigationView sender,
        NavigationViewDisplayModeChangedEventArgs args
    )
    {
        var currMargin = AppTitleBar.Margin;
        AppTitleBar.Margin = new Thickness(
            sender.CompactPaneLength,
            currMargin.Top,
            currMargin.Right,
            currMargin.Bottom
        );
    }

    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        NavView.IsBackEnabled = ContentFrame.CanGoBack;
        ContentFrame.RemoveBackEntry();
    }

    private void Window_DragLeave(object sender, DragEventArgs e)
    {
        DropBorder.Visibility = Visibility.Collapsed;
    }

    private void Window_DragEnter(object sender, DragEventArgs e)
    {
        DropBorder.Visibility = Visibility.Visible;
    }

    private void Window_Drop(object sender, DragEventArgs e)
    {
        DropBorder.Visibility = Visibility.Collapsed;
        ShowWindow();

        if (e.Data.GetData(DataFormats.FileDrop) is not string[] datas)
        {
            return;
        }

        try
        {
            var type = ImportActionType.Invalid;
            var result = new Lazy<bool?>(
                () => DialogFactory.ShowImportConfirmWithMergeOption(type)
            );
            _importHandler.Import(
                datas,
                (actionType) =>
                {
                    if (actionType == ImportActionType.Server)
                    {
                        return DialogFactory.ShowImportServerConfigurationConfirm();
                    }
                    else
                    {
                        type = actionType;

                        return result.Value.HasValue;
                    }
                },
                (actionType) => result.Value.HasValue && result.Value.Value
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "导入失败", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
