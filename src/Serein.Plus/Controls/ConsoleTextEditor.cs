using System.ComponentModel;

using ICSharpCode.AvalonEdit;

using Microsoft.Extensions.DependencyInjection;

using Serein.Core;
using Serein.Core.Services.Data;
using Serein.Plus.Models.ConsoleTextEditor;

namespace Serein.Plus.Controls;

public class ConsoleTextEditor : TextEditor
{
    private static readonly SettingProvider SettingProvider;
    private static uint s_maxLines;

    static ConsoleTextEditor()
    {
        SettingProvider = SereinApp.Current!.Services.GetRequiredService<SettingProvider>();
        SettingProvider.PropertyChanged += UpdateLines;
        SettingProvider.PropertyChanged += (_, _) =>
            SettingProvider.Value.Application.PropertyChanged += UpdateLines;

        s_maxLines = SettingProvider.Value.Application.MaxDisplayedLines;
    }

    public void EnableAnsiColor()
    {
        TextArea.TextView.ElementGenerators.Insert(0, new HideAnsiElementGenerator());
        TextArea.TextView.LineTransformers.Add(new AnsiColorizer());
    }

    public void EnableLogLevelHighlight(bool onlySereinHeader = false)
    {
        TextArea.TextView.LineTransformers.Add(new LineHeaderColorizer(onlySereinHeader));
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
        AppendLine($"[Info] {line}");
    }

    public void AppendWarnLine(string line)
    {
        AppendLine($"[Warn] {line}");
    }

    public void AppendErrorLine(string line)
    {
        var text = $"[Error] {line}";
        AppendLine(text);
    }

    public void AppendReceivedMsgLine(string line)
    {
        AppendLine($"[Recv] {line}");
    }

    public void AppendSentMsgLine(string line)
    {
        AppendLine($"[Sent] {line}");
    }

    public void AppendNoticeLine(string line)
    {
        AppendLine($"[Serein] {line}");
    }
}
