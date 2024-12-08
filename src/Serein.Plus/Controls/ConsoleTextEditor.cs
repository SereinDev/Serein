using ICSharpCode.AvalonEdit;
using Serein.Plus.Models.ConsoleTextEditor;

namespace Serein.Plus.Controls;

public class ConsoleTextEditor : TextEditor
{
    public void EnableAnsiColor()
    {
        TextArea.TextView.ElementGenerators.Insert(0, new HideAnsiElementGenerator());
        TextArea.TextView.LineTransformers.Add(new AnsiColorizer());
    }

    public void EnableLogLevelHighlight(bool onlySereinHeader = false)
    {
        TextArea.TextView.LineTransformers.Add(new LineHeaderColorizer(onlySereinHeader));
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

            var i = LineCount - 250;

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
