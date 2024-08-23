using System;
using System.Threading;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Serein.Pro.Models;

public readonly record struct InfoBarTask
{
    public required ManualResetEvent ResetEvent { get; init; }

    public string? Title { get; init; }

    public string? Message { get; init; }

    public InfoBarSeverity Severity { get; init; }

    public TimeSpan Interval { get; init; }

    public UIElement? Content { get; init; }

    public CancellationToken CancellationToken { get; init; }
}
