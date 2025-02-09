using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.DependencyInjection;
using Serein.Plus.Models;

namespace Serein.Plus.Services;

public sealed class InfoBarProvider(IServiceProvider serviceProvider)
{
    private readonly BlockingCollection<InfoBarTask> _tasks = new(
        new ConcurrentQueue<InfoBarTask>()
    );
    private readonly Lazy<MainWindow> _mainWindow = new(
        serviceProvider.GetRequiredService<MainWindow>
    );
    private bool _isRunning;

    public void Enqueue(
        string title,
        string message,
        InfoBarSeverity severity,
        TimeSpan? interval = null,
        CancellationToken cancellationToken = default
    )
    {
        _tasks.Add(
            new()
            {
                Title = title,
                Message = message,
                Severity = severity,
                Interval = interval,
                CancellationToken = cancellationToken,
            },
            cancellationToken
        );

        if (!_isRunning)
        {
            Task.Run(RunInfoBarLoop, CancellationToken.None);
        }
    }

    private void RunInfoBarLoop()
    {
        _isRunning = true;
        while (!_tasks.IsCompleted)
        {
            var task = _tasks.Take();

            if (task.CancellationToken.IsCancellationRequested)
            {
                continue;
            }

            _mainWindow.Value.Dispatcher.Invoke(_mainWindow.Value.ShowInfoBar, task);

            task.ResetEvent.WaitOne();
        }
    }
}
