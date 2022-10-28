using Serein.Base;
using System.IO;
using System.Windows;

namespace Serein
{
    public partial class App : Application
    {
        public App()
        {
            CrashInterception.Init();
            DispatcherUnhandledException += (sender, e) => CrashInterception.ShowException(e.Exception.StackTrace);
            if (!File.Exists(Global.Path + "console\\console.html"))
            {
                ResourcesManager.InitConsole();
                Global.FirstOpen = true;
            }
            IO.ReadAll();
            IO.StartSavingAndUpdating();
            TaskRunner.Start();
            Net.StartChecking();
        }
    }
}
