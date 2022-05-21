using System;
using System.Windows.Forms;

namespace Serein
{
    public partial class Ui : Form
    {
        delegate void BotWebBrowser_Delegate(object[] objects);

        private void BotWebBrowser_AppendText(object[] objects)
        {
            BotWebBrowser.Document.InvokeScript("AppendText", objects);
        }
        public void BotWebBrowser_Invoke(string str)
        {
            object[] objects1 = { str };
            object[] objects2 = { objects1 };
            Invoke((PanelConsoleWebBrowser_Delegate)BotWebBrowser_AppendText, objects2);
        }
        private void BotConnect_Click(object sender, EventArgs e)
        {
            Websocket.Connect();
        }
        private void BotClose_Click(object sender, EventArgs e)
        {
            Websocket.Close();
        }
    }

}
