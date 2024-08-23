using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Controls;

using Serein.Pro.Models;

namespace Serein.Pro.Services;

public class InfoBarProvider(MainWindow mainWindow)
{
    private readonly BlockingCollection<InfoBarTask> _infoBarTasksQueue =
        new(new ConcurrentQueue<InfoBarTask>());
    private readonly MainWindow _mainWindow = mainWindow;
    private bool _isRunning;

    public void ShowInfoBar(
        string? title,
        string? message,
        InfoBarSeverity severity,
        TimeSpan? interval = null,
        CancellationToken cancellationToken = default
    )
    {
        _infoBarTasksQueue.Add(
            new()
            {
                ResetEvent = new(false),
                Title = title,
                Message = message,
                Severity = severity,
                Interval = interval ?? TimeSpan.FromSeconds(3),
                CancellationToken = cancellationToken,
            },
            cancellationToken
        );

        if (!_isRunning)
            StartInfoBarLoop();
    }

    private void StartInfoBarLoop() =>
        Task.Run(() =>
        {
            _isRunning = true;
            while (!_infoBarTasksQueue.IsCompleted)
            {
                var task = _infoBarTasksQueue.Take();

                if (task.CancellationToken.IsCancellationRequested)
                    continue;

                _mainWindow.ShowInfoBar(task);
                task.ResetEvent.WaitOne();
            }

            _isRunning = false;
        });
}
