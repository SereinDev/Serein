using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using iNKORE.UI.WPF.Modern.Controls;
using iNKORE.UI.WPF.Modern.Helpers;

using Microsoft.Extensions.Hosting;

using Serein.Core.Services.Server;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Server;

public partial class PanelPage : Page
{
    private readonly ServerManager _serverManager;

    public PanelPage(ServerManager serverManager)
    {
        InitializeComponent();
        _serverManager = serverManager;
    }

    private void ControlButton_Click(object sender, RoutedEventArgs e) { }

    private void Enter_Click(object sender, RoutedEventArgs e) { }

    private void InputBox_PreviewKeyDown(object sender, KeyEventArgs e) { }
}
