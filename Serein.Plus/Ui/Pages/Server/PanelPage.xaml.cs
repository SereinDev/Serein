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
using Microsoft.Extensions.Logging;

using Serein.Core.Models;
using Serein.Core.Models.Exceptions;
using Serein.Core.Services.Server;

using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace Serein.Plus.Ui.Pages.Server;

public partial class PanelPage : Page
{
    private readonly ServerManager _serverManager;
    private readonly IOutputHandler _logger;

    public PanelPage(ServerManager serverManager, IOutputHandler logger)
    {
        InitializeComponent();

        _serverManager = serverManager;
        _logger = logger;
    }

    private void ControlButton_Click(object sender, RoutedEventArgs e)
    {
        var tag = (sender as Control)?.Tag?.ToString();

        try
        {
            switch (tag)
            {
                case "start":
                    _serverManager.Start();
                    break;

                case "stop":
                    _serverManager.Stop();
                    break;

                case "restart":
                    break;

                case "terminate":
                    _serverManager.Terminate();
                    break;
            }
        }
        catch (ServerException ex)
        {
            var document = ConsoleControl.ConsoleRichTextBox.Document ?? new();
            document.Blocks.Add(new Paragraph(new Run(ex.ToString())));
        }
    }

    private void Enter_Click(object sender, RoutedEventArgs e)
    {
        Input();
        InputBox.Focus();
    }

    private void InputBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            Input();
    }

    private void Input()
    {
        _serverManager.Input(InputBox.Text);
        InputBox.Text = string.Empty;
    }
}
