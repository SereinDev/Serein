using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace Serein
{
    public partial class Ui : Form
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
        private void RegexContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {

        }
        public void AddRegex(int areaIndex,string regex,bool isAdmin,string remark, string command)
        {
            string[] areas = { "禁用", "控制台", "消息（群聊）", "消息（私聊）" };
            string isAdminText = "";
            ListViewItem Item = new ListViewItem(regex);
            if (areaIndex <= 1)
            {
                isAdminText = "-";
            }
            else if(isAdmin)
            {
                isAdminText = "是";
            }
            else
            {
                isAdminText = "否";
            }
            Item.SubItems.Add(areas[areaIndex]);
            Item.SubItems.Add(isAdminText);
            Item.SubItems.Add(remark);
            Item.SubItems.Add(command);
            if (RegexList.InvokeRequired)
            {
                Action<ListViewItem> actionDelegate = (x) => { RegexList.Items.Add(Item); };
                PanelInfoPort2.Invoke(actionDelegate, Item);
                MessageBox.Show("");
            }
            else
            {
                RegexList.Items.Add(Item);
            }
        }
    }
}
