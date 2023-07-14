using Serein.Base;
using Serein.Extensions;

#if WINFORM
using System.Windows.Forms;
#elif WPF
using Serein.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
#endif

namespace Serein.Utils.Output
{
    internal static class MsgBox
    {

        /// <summary>
        /// 显示具有指定文本、标题、按钮和图标的消息框。
        /// </summary>
        /// <param name="text">要在消息框中显示的文本。</param>
        /// <param name="buttons">MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="icon">MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>按下的按钮为OK或Yes</returns>
        public static bool Show(string text, bool canBeCanceled = false, bool isError = false)
        {
            Logger.Output(LogType.DetailDebug, new object[] { text, canBeCanceled, isError }.ToJson());
            text = text.Trim('\r', '\n');
#if CONSOLE
            Logger.WriteLine(isError ? 3 : 2, text, true);
            return true;
#elif WINFORM
            if (!canBeCanceled)
            {
                text = ":(\n" + text;
            }
            DialogResult result = MessageBox.Show(text, "Serein", canBeCanceled ? MessageBoxButtons.OKCancel : MessageBoxButtons.OK, isError ? MessageBoxIcon.Error : MessageBoxIcon.Warning);
            return result == DialogResult.OK;
#elif WPF
            if (!canBeCanceled)
            {
                if (text.Contains("\n"))
                {
                    Catalog.MainWindow?.OpenSnackbar(
                        text.Substring(0, text.IndexOf('\n')),
                        text.Substring(text.IndexOf('\n')).Trim('\n'),
                        isError ? SymbolRegular.Dismiss24 : SymbolRegular.Warning24
                        );
                }
                else
                {
                    Catalog.MainWindow?.OpenSnackbar(
                        "执行失败",
                        text,
                        isError ? SymbolRegular.Dismiss24 : SymbolRegular.Warning24
                        );
                }
                return true;
            }
            bool confirmed = false;
            MessageBox messageBox = new()
            {
                Title = "Serein",
                Content = new TextBlock
                {
                    Text = text,
                    TextWrapping = System.Windows.TextWrapping.WrapWithOverflow
                },
                ShowInTaskbar = false,
                ResizeMode = System.Windows.ResizeMode.NoResize,
                Topmost = true,
                Width = 350,
                ButtonLeftName = "确定",
                ButtonRightName = "取消"
            };
            messageBox.ButtonRightClick += (_, _) => messageBox.Close();
            messageBox.ButtonLeftClick += (_, _) =>
            {
                confirmed = true;
                messageBox.Close();
            };
            messageBox.ShowDialog();
            return confirmed;
#endif
        }
    }
}