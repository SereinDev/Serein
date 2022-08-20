using Serein.Base;
using System;
using System.IO;
using System.Windows;
using Wpf.Ui.Controls;

namespace Serein.Windows
{
    public partial class MainWindow : UiWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(Global.Path + "console\\console.html"))
            {
                ResourcesManager.InitConsole();
                Global.FirstOpen = true;
            }
        }

        private void UiWindow_StateChanged(object sender, EventArgs e)
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }
    }
}
