using System;

using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serein.Core.Services.Loggers;

namespace Serein.Plus.Services.Loggers;

public sealed class NotificationLogger(IServiceProvider serviceProvider) : ILogger
{
    private readonly Lazy<InfoBarProvider> _infoBarProvider =
        new(serviceProvider.GetRequiredService<InfoBarProvider>);

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        var line = state + exception?.Message;
        line ??= string.Empty;

        InfoBarSeverity? severity = logLevel switch
        {
            //LogLevel.Information => InfoBarSeverity.Informational,
            LogLevel.Warning => InfoBarSeverity.Warning,
            LogLevel.Error => InfoBarSeverity.Error,
            _ => null,
        };

        if (severity is not null)
            _infoBarProvider.Value.Enqueue(
                "Serein.Plus",
                line,
                severity.Value
            );
    }
}
