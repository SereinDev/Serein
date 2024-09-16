using System;
using System.Windows;
using System.Windows.Input;

using iNKORE.UI.WPF.Modern.Controls;

using Microsoft.Win32;

using Serein.Core.Models.Server;
using Serein.Core.Services.Servers;

namespace Serein.Plus.Windows;

public partial class ServerConfigurationEditor : Window
{
    private readonly ServerManager _serverManager;

    public ServerConfigurationEditor(ServerManager serverManager, Configuration configuration, string? id = null)
    {
        DataContext = this;
        _serverManager = serverManager;
        Configuration = configuration;
        Id = id;

        InitializeComponent();
        IdTextBox.IsEnabled = string.IsNullOrEmpty(Id);
    }

    public Configuration Configuration { get; }
    public string? Id { get; set; }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (IdTextBox.IsEnabled)
                Validate();
            
            DialogResult = true;
            Hide();
        }
        catch (Exception ex)
        {
            new ContentDialog
            {
                Content = ex.Message,
                DefaultButton = ContentDialogButton.Close,
                CloseButtonText = "确定"
            }.ShowAsync();
        }
    }

    private void Validate()
    {
        ServerManager.ValidateId(Id);
        if (_serverManager.Servers.ContainsKey(Id!))
            throw new InvalidOperationException("此Id已被占用");
    }

    private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var dialog = new OpenFileDialog();
        if (dialog.ShowDialog() == true)
            Configuration.FileName = dialog.FileName;
    }
}
