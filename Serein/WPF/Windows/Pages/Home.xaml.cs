using Serein.Base;
using Serein.Items.Motd;
using Serein.Server;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Wpf.Ui.Controls;

namespace Serein.Windows.Pages
{
    public partial class Home : UiPage
    {
        private Timer _Timer = new Timer(5000)
        {
            AutoReset = true
        };

        public Home()
        {
            InitializeComponent();
        }

        private void Update()
        {
            Dispatcher.Invoke(() =>
            {
                if (Catalog.MainWindow.Navigation.SelectedPageIndex != 0)
                {
                    return;
                }
                CPU_Name.Text = SystemInfo.CPUName;
                Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        string CPUPercentage = SystemInfo.CPUPercentage;
                        CPU_Percent.Text = CPUPercentage + "%";
                        CPU_Percent_Bar.Value = double.TryParse(CPUPercentage, out double _Result1) ? _Result1 : 0;
                        CPU_Percent_Bar.IsIndeterminate = false;
                    });
                });
                RAM_Percent.Text = $"{SystemInfo.UsedRAM} / {SystemInfo.TotalRAM} MB   {SystemInfo.RAMPercentage}%";
                RAM_Percent_Ring.Progress = double.TryParse(SystemInfo.RAMPercentage, out double _Result2) ? _Result2 : 0;
                RAM_Percent_Ring.IsIndeterminate = false;
                Server_Status.Text = ServerManager.Status ? "已启动" : "未启动";
                Server_Time.Text = ServerManager.Status ? ServerManager.GetTime() : "-";
                Server_Occupancy.Text = ServerManager.Status ? ServerManager.CPUPersent.ToString("N2") + "%" : "-";
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
            });
        }

        private void UiPage_Loaded(object sender, RoutedEventArgs e)
        {
            _Timer.Elapsed += (_sender, _e) => Update();
            _Timer.Start();
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
                    break;
                case "Regex":
                    Catalog.MainWindow.Navigation.Navigate(2);
                    Catalog.Function.Container?.Navigation?.Navigate(2);
                    break;
                case "Bot":
                    Catalog.MainWindow.Navigation.Navigate(2);
                    Catalog.Function.Container?.Navigation?.Navigate(0);
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
