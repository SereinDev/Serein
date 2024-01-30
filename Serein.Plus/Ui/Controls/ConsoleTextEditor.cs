using System.ComponentModel;
using System.Windows;

using ICSharpCode.AvalonEdit;

using Microsoft.Extensions.DependencyInjection;

using Serein.Core.Services.Data;
using Serein.Plus.Ui.Utils;

namespace Serein.Plus.Ui.Controls;

public class ConsoleTextEditor : TextEditor
{
    private static readonly SettingProvider SettingProvider;
    private static uint s_maxLines;

    static ConsoleTextEditor()
    {
        SettingProvider = App.Host.Services.GetRequiredService<SettingProvider>();
        SettingProvider.PropertyChanged += UpdateLines;
        SettingProvider.PropertyChanged += (_, _) =>
            SettingProvider.Value.Application.PropertyChanged += UpdateLines;

        s_maxLines = SettingProvider.Value.Application.MaxDisplayedLines;
    }

    private static void UpdateLines(object? sender, PropertyChangedEventArgs e)
    {
        s_maxLines = SettingProvider.Value.Application.MaxDisplayedLines;
    }

    protected readonly object _lock = new();

    public ConsoleTextEditor()
    {
        FontFamily = new("Consolas,微软雅黑");
        Padding = new(5);
        WordWrap = true;
        IsReadOnly = true;
        Document.UndoStack.SizeLimit = 0;
        TextArea.TextView.Options.EnableHyperlinks = false;
        TextArea.TextView.Options.EnableEmailHyperlinks = false;

        TextArea.TextView.ElementGenerators.Insert(0, new HideLogLevelPrefixElementGenerator());
        TextArea.TextView.LineTransformers.Add(new LogLevelColorizer());
    }

    public void AppendLine(string line)
    {
        lock (_lock)
        {
            AppendText(line + "\n");

            var i = (int)(LineCount - (s_maxLines == 0 ? 250 : s_maxLines));

            if (i > 0)
            {
                Document.Remove(0, Document.GetLineByNumber(i).EndOffset + 1);
            }

            ScrollToEnd();
        }
    }

    public void AppendInfoLine(string line)
    {
        var text = $"{LogLevelColorizer.Prefix}[Info] {line}";
        AppendLine(text);
    }

    public void AppendWarnLine(string line)
    {
        var text = $"{LogLevelColorizer.Prefix}[Warn] {line}";
        AppendLine(text);
    }

    public void AppendErrorLine(string line)
    {
        var text = $"{LogLevelColorizer.Prefix}[Error] {line}";
        AppendLine(text);
    }

    public void AppendReceivedMsgLine(string line)
    {
        var text = $"{LogLevelColorizer.Prefix}[Recv] {line}";
        AppendLine(text);
    }

    public void AppendSentMsgLine(string line)
    {
        var text = $"{LogLevelColorizer.Prefix}[Sent] {line}";
        AppendLine(text);
    }
}
