using System;
using System.Threading;
using System.Windows;

using iNKORE.UI.WPF.Modern.Controls;

namespace Serein.Plus.Models;

public record InfoBarTask
{
    public required string Title { get; init; }

    public required string Message { get; init; }

    public InfoBarSeverity Severity { get; init; }

    public TimeSpan? Interval { get; init; }

    public UIElement? Content { get; init; }

    public CancellationToken CancellationToken { get; init; }

    public ManualResetEvent ResetEvent { get; } = new(false);
}
