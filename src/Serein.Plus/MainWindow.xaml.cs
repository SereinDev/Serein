﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using Hardcodet.Wpf.TaskbarNotification;

using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.Modern.Media.Animation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serein.Core;
using Serein.Core.Models.Plugins;
using Serein.Core.Models.Settings;
using Serein.Core.Services.Data;
using Serein.Core.Services.Loggers;
using Serein.Core.Services.Plugins;
using Serein.Core.Services.Servers;
using Serein.Core.Utils;
using Serein.Plus.Commands;
using Serein.Plus.Dialogs;
using Serein.Plus.Models;
using Serein.Plus.Pages;
using Serein.Plus.Services;

namespace Serein.Plus;

public partial class MainWindow : Window
{
    private bool _isTopMost;
    private readonly IHost _host;
    private readonly IServiceProvider _services;
    private readonly ServerManager _serverManager;
    private readonly SettingProvider _settingProvider;
    private readonly EventDispatcher _eventDispatcher;
    private readonly TitleUpdater _titleUpdater;
    private readonly DoubleAnimation _infoBarPopIn;
    private readonly DoubleAnimation _infoBarPopOut;

    public MainWindow(
        IHost host,
        IServiceProvider services,
        ServerManager serverManager,
        SettingProvider settingProvider,
        EventDispatcher eventDispatcher,
        TitleUpdater titleUpdater
    )
    {
        _host = host;
        _services = services;
        _serverManager = serverManager;
        _settingProvider = settingProvider;
        _eventDispatcher = eventDispatcher;
        _titleUpdater = titleUpdater;

        var powerEase = new PowerEase { EasingMode = EasingMode.EaseInOut };
        _infoBarPopIn = new(200, 0, new(TimeSpan.FromSeconds(0.5))) { EasingFunction = powerEase };
        _infoBarPopOut = new(0, 200, new(TimeSpan.FromSeconds(0.5))) { EasingFunction = powerEase };

        InitializeComponent();
        AppTaskbarIcon.ContextMenu.DataContext = this;
        AppTaskbarIcon.DoubleClickCommand = new TaskbarIconDoubleClickCommand(this);

        RootFrame.Navigate(_services.GetRequiredService<NotImplPage>());

        DataContext = _titleUpdater;
        _titleUpdater.Update();

        ThemeManager.Current.ApplicationTheme = _settingProvider.Value.Application.Theme switch
        {
            Theme.Light => ApplicationTheme.Light,
            Theme.Dark => ApplicationTheme.Dark,
            _ => null,
        };

        _services.GetRequiredService<PluginConsolePage>(); // 提前在ui线程实例化
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem item)
            return;

        var tag = item.Tag?.ToString();

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
                    Hide();
                else
                    ShowWindow();

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

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Task.Delay(1000)
            .ContinueWith(
                (_) =>
                    Dispatcher.Invoke(
                        () =>
                            RootFrame.Navigate(
                                _services.GetRequiredService<ShellPage>(),
                                new SuppressNavigationTransitionInfo()
                            )
                    )
            );
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
        if (SereinApp.StartForTheFirstTime)
            new WelcomeDialog().ShowAsync();

        if (FileLoggerProvider.IsEnable)
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
            Topmost = false;
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        Hide();

        if (!_serverManager.AnyRunning)
        {
            AppTaskbarIcon.Visibility = Visibility.Collapsed;
            _eventDispatcher.Dispatch(Event.SereinClosed);
            return;
        }

        e.Cancel = true;
        HideMenuItem.IsEnabled = HideMenuItem.IsChecked = true;
        Topmost = _isTopMost = TopMostMenuItem.IsEnabled = TopMostMenuItem.IsChecked = false;

        ShowBalloonTip(
            "仍有服务器进程在运行中",
            "已自动最小化至托盘，点击托盘图标即可复原窗口",
            BalloonIcon.None
        );
    }

    private void AppTaskbarIcon_TrayBalloonTipClicked(object sender, RoutedEventArgs e)
    {
        ShowWindow();
    }

    public void ShowBalloonTip(string title, string message, BalloonIcon icon)
    {
        AppTaskbarIcon.ShowBalloonTip(title, message, icon);
    }

    public void ShowInfoBar(InfoBarTask infoBarTask)
    {
        GlobalInfoBar.Title = infoBarTask.Title;
        GlobalInfoBar.Content = infoBarTask.Content;
        GlobalInfoBar.Message = string.IsNullOrEmpty(infoBarTask.Message) ? " " : infoBarTask.Message;
        GlobalInfoBar.Severity = infoBarTask.Severity;

        var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            infoBarTask.CancellationToken
        );

        if (GlobalInfoBar.RenderTransform is not TranslateTransform translateTransform)
            return;

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
                    Task.Delay(500).ContinueWith((_) => infoBarTask.ResetEvent.Set());
                }
            );

        void Cancel(object sender, EventArgs e)
        {
            infoBarTask.ResetEvent.Set();
            if (!cancellationTokenSource.IsCancellationRequested)
                cancellationTokenSource.Cancel();
        }
    }
}
