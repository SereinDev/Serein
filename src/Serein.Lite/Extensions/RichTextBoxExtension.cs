using System.Drawing;
using System.Windows.Forms;

namespace Serein.Lite.Extensions;

public static class RichTextBoxExtension
{
    public static void AppendTextWithColor(this RichTextBox richTextBox, string text, Color color)
    {
        richTextBox.SelectionStart = richTextBox.TextLength;
        richTextBox.SelectionLength = 0;
        richTextBox.SelectionColor = color;
        richTextBox.AppendText(text);
        richTextBox.SelectionColor = richTextBox.ForeColor;
    }

    public static void ScrollToEnd(this RichTextBox richTextBox)
    {
        richTextBox.SelectionStart = richTextBox.Text.Length;
        richTextBox.ScrollToCaret();
    }
}
