using System.Drawing;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Serein.Windows
{
    internal static class BalloonTip
    {
        private static NotifyIcon notifyIcon = new NotifyIcon()
        {
            Icon = Icon.FromHandle(new WindowInteropHelper(Window.MainWindow).Handle),
            Visible = false
        };

        /// <summary>
        /// 在指定时间段内，在任务栏中显示具有指定标题、文本和图标的气球状提示。
        /// </summary>
        /// <param name="TipTitle">要在气球状提示上显示的标题。</param>
        /// <param name="TipText">要在气球状提示上显示的文本。</param>
        public static void Show(string TipTitle, string TipText) => notifyIcon.ShowBalloonTip(5000, TipTitle, TipText, ToolTipIcon.None);
    }
}
