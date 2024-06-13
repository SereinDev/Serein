using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serein.Lite.Ui.Servers;

public partial class ServerPage : UserControl
{
    public ServerPage()
    {
        InitializeComponent();
        ToolStripStatusLabel.Text = "当前没有服务器配置。你可以在左上角的服务器菜单栏添加或导入";
    }
}

