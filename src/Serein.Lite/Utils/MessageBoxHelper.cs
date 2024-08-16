using System.Windows.Forms;

namespace Serein.Lite.Utils;

public static class MessageBoxHelper
{
    public static bool ShowDeleteConfirmation(string message)
    {
        return ShowQuestionMsgBox(message + "\r\n这将会永远失去！（真的很久！）");
    }

    public static void ShowWarningMsgBox(string message)
    {
        MessageBox.Show(message, "Serein.Lite", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public static bool ShowQuestionMsgBox(string message)
    {
        return MessageBox.Show(
                message,
                "Serein.Lite",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question
            ) == DialogResult.OK;
    }
}
