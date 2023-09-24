using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages.Settings
{
    public partial class Server : UiPage
    {
        private readonly bool _loaded;

        public Server()
        {
            InitializeComponent();
            Load();
            _loaded = true;
            Catalog.Settings.Server = this;
        }

        public void Load()
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
            Port.Value = Global.Settings.Server.Port;
            LineTerminator.Text = Global.Settings.Server.LineTerminator
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        }

        private void StopCommands_TextChanged(object sender, TextChangedEventArgs e) =>
            Global.Settings.Server.StopCommands = StopCommands.Text.Split(
                new[] { ';' },
                StringSplitOptions.RemoveEmptyEntries
            );

        private void AutoStop_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Server.AutoStop = AutoStop.IsChecked ?? false;

        private void EnableRestart_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Server.EnableRestart = EnableRestart.IsChecked ?? false;

        private void EnableOutputCommand_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Server.EnableOutputCommand = EnableOutputCommand.IsChecked ?? false;

        private void EnableUnicode_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Server.EnableUnicode = EnableUnicode.IsChecked ?? false;

        private void EnableLog_Click(object sender, RoutedEventArgs e) =>
            Global.Settings.Server.EnableLog = EnableLog.IsChecked ?? false;

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            Global.Settings.Server.Type = _loaded
                ? Type.SelectedIndex
                : Global.Settings.Server.Type;

        private void InputEncoding_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            Global.Settings.Server.InputEncoding = _loaded
                ? InputEncoding.SelectedIndex
                : Global.Settings.Server.InputEncoding;

        private void OutputEncoding_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            Global.Settings.Server.OutputEncoding = _loaded
                ? OutputEncoding.SelectedIndex
                : Global.Settings.Server.OutputEncoding;

        private void OutputStyle_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            Global.Settings.Server.OutputStyle = _loaded
                ? OutputStyle.SelectedIndex
                : Global.Settings.Server.OutputStyle;

        private void Port_TextChanged(object sender, TextChangedEventArgs e) =>
            Global.Settings.Server.Port =
                _loaded && int.TryParse(Port.Text, out int i) ? i : Global.Settings.Server.Port;

        private void LineTerminator_TextChanged(object sender, TextChangedEventArgs e) =>
            Global.Settings.Server.LineTerminator = LineTerminator.Text
                .Replace("\\r", "\r")
                .Replace("\\n", "\n");

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog =
                new()
                {
                    InitialDirectory =
                        !string.IsNullOrEmpty(Global.Settings.Server.Path)
                        && File.Exists(Global.Settings.Server.Path)
                            ? Global.Settings.Server.Path
                            : Global.PATH,
                    Filter = "支持的文件(*.exe *.bat)|*.exe;*.bat"
                };
            if (dialog.ShowDialog() ?? false)
            {
                Path.Text = dialog.FileName;
                Global.Settings.Server.Path = dialog.FileName;
                if (Catalog.Server.Plugins != null)
                {
                    Catalog.Server.Plugins.Load();
                }
            }
        }
    }
}
