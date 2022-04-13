using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace Serein
{
     public class Server
     {
        public static bool Restart = false;
        public static string Path = @"";
        public static bool Status = false;
        static ProcessStartInfo ServerProcessInfo;
        static Process ServerProcess;
        static object[] objects = new object[1];
        static Thread UpdateStatusThread = new Thread(UpdateStatus);


        public static void Start() 
        {
            objects[0] = "Starting...";
            Global.PanelConsoleWebBrowser.Document.InvokeScript("AppendText", objects);
            if (!File.Exists(Path))
            {
                MessageBox.Show($"\"{Path}\"未找到");
            }
            else
            {
                ServerProcessInfo = new ProcessStartInfo(Path);
                ServerProcessInfo.FileName = Path;
                ServerProcessInfo.UseShellExecute = false;
                ServerProcessInfo.CreateNoWindow = true;
                ServerProcessInfo.RedirectStandardOutput = true;
                ServerProcessInfo.RedirectStandardInput = true;
                ServerProcessInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Path);
                ServerProcess = Process.Start(ServerProcessInfo);
                ServerProcess.BeginOutputReadLine();
                ServerProcess.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
                Status = true;
            }
        }
        public static void Stop()
        {
            if (Status == true)
            {
                ServerProcess.StandardInput.WriteLine("stop\n\r");
            }
        }
        private static void SortOutputHandler(object sendingProcess,DataReceivedEventArgs outLine)
        { 
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                objects[0] = Log.ColorLog(outLine.Data, 2);
                Global.serein.PanelConsoleWebBrowser_Invoke(objects);
            }
        }
        
        public static bool GetStatus()
        {
            return Status;
        }
        public static void UpdateStatus()
        {
            while (true)
            {
                try
                {
                    if (Status == true && ServerProcess.HasExited)
                    {
                        Status = false;
                    }
                }
                catch (Exception)
                {
                }
                Task.Delay(1000);
            }
            
        }
        public static void UpdateStatusThreadStart()
        {
            UpdateStatusThread.IsBackground = true;
            //UpdateStatusThread.Start();
        }
    }
}
