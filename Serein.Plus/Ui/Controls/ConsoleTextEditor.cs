using System.Windows.Media;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;

namespace Serein.Plus.Ui.Controls;

public class ConsoleTextEditor : TextEditor
{
    protected readonly RichTextModel _richTextModel = new();

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
        TextArea.TextView.LineTransformers.Add(new RichTextColorizer(_richTextModel));
    }

    public void AppendLine(string line)
    {
        lock (_lock)
        {
            AppendText(line + "\n");
            ScrollToEnd();
        }
    }

    public void AppendInfoLine(string line)
    {
        lock (_lock)
        {
            var text = $"[Info] {line}\n";
            BeginChange();
            AppendText(text);
            _richTextModel.SetForeground(Text.Length - text.Length, 6, Info);
            EndChange();
            ScrollToEnd();
        }
    }

    public void AppendWarnLine(string line)
    {
        lock (_lock)
        {
            var text = $"[Warn] {line}\n";
            BeginChange();
            AppendText(text);
            _richTextModel.SetForeground(Text.Length - text.Length, 6, Warn);
            EndChange();
            ScrollToEnd();
        }
    }

    public void AppendErrorLine(string line)
    {
        lock (_lock)
        {
            var text = $"[Error] {line}\n";
            BeginChange();
            AppendText(text);
            _richTextModel.SetForeground(Text.Length - text.Length, 7, Error);
            EndChange();
            ScrollToEnd();
        }
    }

    private static readonly SimpleHighlightingBrush Info = new(Color.FromRgb(0x00, 0xaf, 0xff));
    private static readonly SimpleHighlightingBrush Warn = new(Color.FromRgb(0xaf, 0x5f, 0x00));
    private static readonly SimpleHighlightingBrush Error = new(Color.FromRgb(0xd7, 0x00, 0x00));
}
