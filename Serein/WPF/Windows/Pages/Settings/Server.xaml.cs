using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Settings
{
    public partial class Server : UiPage
    {
        public Server()
        {
            InitializeComponent();
            Load();
            Catalog.Settings.Server = this;
        }

        private void Load()
        {
            StopCommands.Text = string.Join("\r\n", Global.Settings.Server.StopCommands);
            AutoStop.IsChecked = Global.Settings.Server.AutoStop;
            EnableRestart.IsChecked = Global.Settings.Server.EnableRestart;
            EnableOutputCommand.IsChecked = Global.Settings.Server.EnableOutputCommand;
            EnableUnicode.IsChecked = Global.Settings.Server.EnableUnicode;
            EnableLog.IsChecked = Global.Settings.Server.EnableLog;
            Type.SelectedIndex = Global.Settings.Server.Type;
            InputEncoding.SelectedIndex = Global.Settings.Server.InputEncoding;
            OutputEncoding.SelectedIndex = Global.Settings.Server.OutputEncoding;
            OutputStyle.SelectedIndex = Global.Settings.Server.OutputStyle;
            Path.Text = Global.Settings.Server.Path;
        }

        private void StopCommands_TextChanged(object sender, TextChangedEventArgs e) => Global.Settings.Server.StopCommands = StopCommands.Text.Replace("\r", string.Empty).Trim('\r', '\n', ' ').Split('\n');
        private void AutoStop_Click(object sender, RoutedEventArgs e) => Global.Settings.Server.AutoStop = AutoStop.IsChecked ?? false;
        private void EnableRestart_Click(object sender, RoutedEventArgs e) => Global.Settings.Server.EnableRestart = EnableRestart.IsChecked ?? false;
        private void EnableOutputCommand_Click(object sender, RoutedEventArgs e) => Global.Settings.Server.EnableOutputCommand = EnableOutputCommand.IsChecked ?? false;
        private void EnableUnicode_Click(object sender, RoutedEventArgs e) => Global.Settings.Server.EnableUnicode = EnableUnicode.IsChecked ?? false;
        private void EnableLog_Click(object sender, RoutedEventArgs e) => Global.Settings.Server.EnableLog = EnableLog.IsChecked ?? false;
        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e) => Global.Settings.Server.Type = Type.SelectedIndex;
        private void InputEncoding_SelectionChanged(object sender, SelectionChangedEventArgs e) => Global.Settings.Server.InputEncoding = InputEncoding.SelectedIndex;
        private void OutputEncoding_SelectionChanged(object sender, SelectionChangedEventArgs e) => Global.Settings.Server.OutputEncoding = OutputEncoding.SelectedIndex;
        private void OutputStyle_SelectionChanged(object sender, SelectionChangedEventArgs e) => Global.Settings.Server.OutputStyle = OutputStyle.SelectedIndex;
        private void Port_TextChanged(object sender, TextChangedEventArgs e) => Global.Settings.Server.Port = int.TryParse(Port.Text, out int i) ? i : 0;

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "支持的文件(*.exe *.bat)|*.exe;*.bat"
            };
            if (dialog.ShowDialog() ?? false)
            {
                Path.Text = dialog.FileName;
                Global.Settings.Server.Path = dialog.FileName;
                if (Catalog.Server.Plugins != null) { Catalog.Server.Plugins.Load(); }
            }
        }
    }
}
