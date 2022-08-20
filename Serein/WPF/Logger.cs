using System;
using System.Collections.Generic;
using System.Linq;
//using System.Windows;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace Serein
{
    internal static class Logger
    {
        public static int Type = 2;
        public static void Out(int Type, params object[] objects)
        {

        }

        /// <summary>
        /// 显示具有指定文本、标题、按钮和图标的消息框。
        /// </summary>
        /// <param name="Text">要在消息框中显示的文本。</param>
        /// <param name="Caption">要在消息框的标题栏中显示的文本。</param>
        /// <param name="Buttons">MessageBoxButtons 值之一，可指定在消息框中显示哪些按钮。</param>
        /// <param name="Icon">MessageBoxIcon 值之一，它指定在消息框中显示哪个图标。</param>
        /// <returns>按下的按钮为OK或Yes</returns>
        public static bool MsgBox(string Text, string Caption, int Buttons, int Icon)
        {
            return true;
        }
    }
}
