using Serein.Base;
using Serein.Items.Motd;
using Serein.Server;
using System.Timers;
using System.Windows;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages
{
    public partial class Home : UiPage
    {
        private readonly Timer _Timer = new Timer(5000)
        {
            AutoReset = true
        };

        public Home()
        {
            InitializeComponent();
            _Timer.Elapsed += (_sender, _e) => Update();
            _Timer.Start();
        }

        private void Update()
        {
            Dispatcher.Invoke(() =>
            {
                RAM_Percent.Text = $"{(double)SystemInfo.UsedRAM / 1024:N1} / {(double)SystemInfo.TotalRAM / 1024:N1} GB   {SystemInfo.RAMUsage:N1}%";
                RAM_Percent_Ring.Progress = SystemInfo.RAMUsage;
                Server_Status.Text = ServerManager.Status ? "已启动" : "未启动";
                Server_Time.Text = ServerManager.Status ? ServerManager.GetTime() : "-";
                Server_Occupancy.Text = ServerManager.Status ? ServerManager.CPUUsage.ToString("N2") + "%" : "-";
                if (ServerManager.Status && Global.Settings.Server.Type != 0)
                {
                    Motd _Motd;
                    if (Global.Settings.Server.Type == 1)
                    {
                        _Motd = new Motdpe(NewPort: Global.Settings.Server.Port.ToString());
                    }
                    else
                    {
                        _Motd = new Motdje(NewPort: Global.Settings.Server.Port.ToString());
                    }
                    Server_Online.Text = _Motd != null && _Motd.Success ? $"{_Motd.OnlinePlayer}/{_Motd.MaxPlayer}" : "获取失败";
                }
                else
                {
                    Server_Online.Text = "-";
                }
                string CPUPercentage = SystemInfo.CPUUsage.ToString("N1");
                CPU_Percent.Text = CPUPercentage + "%";
                CPU_Percent_Bar.Value = double.TryParse(CPUPercentage, out double _Result1) ? _Result1 : 0;
                CPU_Percent_Bar.IsIndeterminate = false;
                CPU_Name.Text = SystemInfo.CPUName;
            });
        }


        private void CardAction_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as CardAction)?.Tag)
            {
                default:
                case null:
                    break;
                case "Server":
                    Catalog.MainWindow.Navigation.Navigate(1);
                    Catalog.Server.Container?.Navigation?.Navigate(0);
                    ServerManager.Start();
                    break;
                case "Regex":
                    Catalog.MainWindow.Navigation.Navigate(2);
                    Catalog.Function.Container?.Navigation?.Navigate(2);
                    break;
                case "Bot":
                    Catalog.MainWindow.Navigation.Navigate(2);
                    Catalog.Function.Container?.Navigation?.Navigate(0);
                    Websocket.Connect();
                    break;
                case "Task":
                    Catalog.MainWindow.Navigation.Navigate(2);
                    Catalog.Function.Container?.Navigation?.Navigate(3);
                    break;
                case "Plugins":
                    Catalog.MainWindow.Navigation.Navigate(2);
                    Catalog.Function.Container?.Navigation?.Navigate(4);
                    break;
                case "Settings":
                    Catalog.MainWindow.Navigation.Navigate(4);
                    break;
            }
        }
    }
}
