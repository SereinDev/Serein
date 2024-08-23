using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;

using Serein.Pro.Models;
using Serein.Pro.Views;

using WinUIEx;

namespace Serein.Pro;

public sealed partial class MainWindow : WindowEx
{
    private readonly DispatcherQueue _dispatcherQueue;
    private CancellationTokenSource ?_cancellationTokenSource;

    public MainWindow(ShellPage shellPage)
    {
        InitializeComponent();

        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        ExtendsContentIntoTitleBar = true;
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/logo.ico"));
        RootFrame.Content = shellPage;
    }

    public void ShowInfoBar(InfoBarTask infoBarTask)
    {
        _dispatcherQueue.TryEnqueue(() =>
        {
            GlobalInfoBar.Title = infoBarTask.Title;
            GlobalInfoBar.Message = infoBarTask.Message;
            GlobalInfoBar.Severity = infoBarTask.Severity;
            GlobalInfoBar.Content = infoBarTask.Content;
            GlobalInfoBar.IsOpen = true;

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(infoBarTask.CancellationToken);
            
            GlobalInfoBarPopIn.Completed += Wait;
            GlobalInfoBarPopIn.Begin();


            void Wait(object? sender, object e)
            {
                GlobalInfoBarPopIn.Completed -= Wait;
                Task.Delay(infoBarTask.Interval, _cancellationTokenSource.Token)
                    .ContinueWith(
                        (t) =>
                            _dispatcherQueue.TryEnqueue(() => CloseInfoBar(infoBarTask.ResetEvent))
                    );
            }
        });
    }

    private void CloseInfoBar(ManualResetEvent manualResetEvent)
    {
        GlobalInfoBarPopOut.Completed += Wait;
        GlobalInfoBarPopOut.Begin();

        void Wait(object? sender, object e)
        {
            GlobalInfoBarPopOut.Completed -= Wait;
            manualResetEvent.Set();
            _cancellationTokenSource = null;
        }
    }

    private void GlobalInfoBar_CloseButtonClick(InfoBar sender, object args)
    {
        if (_cancellationTokenSource?.IsCancellationRequested is false)
            _cancellationTokenSource.Cancel();
    }
}
