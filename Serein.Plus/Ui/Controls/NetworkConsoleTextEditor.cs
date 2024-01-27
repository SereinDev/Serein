using System.Windows.Media;

using ICSharpCode.AvalonEdit.Highlighting;

namespace Serein.Plus.Ui.Controls;

public class NetworkConsoleTextEditor : ConsoleTextEditor
{
    public void AppendReceivedMsgLine(string line)
    {
        lock (_lock)
        {
            var text = $"[Recv] {line}\n";
            BeginChange();
            AppendText(text);
            _richTextModel.SetForeground(Text.Length - text.Length, 6, Received);
            EndChange();
            ScrollToEnd();
        }
    }

    public void AppendSentMsgLine(string line)
    {
        lock (_lock)
        {
            var text = $"[Sent] {line}\n";
            BeginChange();
            AppendText(text);
            _richTextModel.SetForeground(Text.Length - text.Length, 6, Sent);
            EndChange();
            ScrollToEnd();
        }
    }

    private static readonly SimpleHighlightingBrush Sent = new(Color.FromRgb(0x00, 0x5f, 0xd7));
    private static readonly SimpleHighlightingBrush Received = new(Color.FromRgb(0x5f, 0xd7, 0x00));
}
