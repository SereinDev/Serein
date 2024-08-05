using System.Windows.Forms;

namespace Serein.Lite.Utils;

public static class MessageBoxFactory
{
    public static bool ShowDeleteConfirmation(string message)
    {
        return MessageBox.Show(message + "\r\n这将会永远失去！（真的很久！）", "Serein.Lite",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question) == DialogResult.Yes;
    }
}