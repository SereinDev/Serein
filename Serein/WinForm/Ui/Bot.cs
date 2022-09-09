using Serein.Base;
using System;
using System.Windows.Forms;

namespace Serein.Ui
{
    public partial class Ui : Form
    {
        private delegate void BotWebBrowser_Delegate(object[] objects);

        public void BotWebBrowser_Invoke(string str)
        {
            object[] objects1 = { str };
            object[] objects2 = { objects1 };
            Invoke((BotWebBrowser_Delegate)BotWebBrowser_AppendText, objects2);
        }

        private void BotWebBrowser_AppendText(object[] objects) => BotWebBrowser.Document.InvokeScript("AppendText", objects);
        private void BotConnect_Click(object sender, EventArgs e) => Websocket.Connect();
        private void BotClose_Click(object sender, EventArgs e) => Websocket.Close();
    }
}
