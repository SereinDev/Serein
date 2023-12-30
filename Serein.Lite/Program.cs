using System;
using System.Windows.Forms;

using Serein.Lite.Ui;

namespace Serein.Lite;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Window());
    }
}
