using Serein.Utils;
using Serein.Core;
using Serein.Core.Server;
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
            _Timer.Elapsed += (_, _) => Update();
            _Timer.Start();
        }

        private void Update()
        {
            Dispatcher.Invoke(() =>
            {
                RAM_Percent.Text = $"{(double)SystemInfo.UsedRAM / 1024:N1} / {(double)SystemInfo.TotalRAM / 1024:N1} GB   {SystemInfo.RAMUsage:N1}%";
                RAM_Percent_Ring.Progress = SystemInfo.RAMUsage;
                Server_Status.Text = ServerManager.Status ? "已启动" : "未启动";
                Server_Time.Text = ServerManager.Status ? ServerManager.Time : "-";
                Server_Occupancy.Text = ServerManager.Status ? ServerManager.CPUUsage.ToString("N1") + "%" : "-";
                if (ServerManager.Status)
                {
                    Server_Online.Text = ServerManager.Motd != null && ServerManager.Motd.IsSuccessful ? $"{ServerManager.Motd.OnlinePlayer}/{ServerManager.Motd.MaxPlayer}" : "获取失败";
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
                case "Server":
                    if (Global.Settings.Serein.PagesDisplayed.ServerPanel)
                    {

                        Catalog.MainWindow.Navigation.Navigate(1);
                        Catalog.Server.Container?.Navigation?.Navigate(0);
                        ServerManager.Start();
                    }
                    break;
                case "Regex":
                    if (Global.Settings.Serein.PagesDisplayed.RegexList)
                    {
                        Catalog.MainWindow.Navigation.Navigate(2);
                        Catalog.Function.Container?.Navigation?.Navigate(2);
                    }
                    break;
                case "Bot":
                    if (Global.Settings.Serein.PagesDisplayed.Bot)
                    {
                        Catalog.MainWindow.Navigation.Navigate(2);
                        Catalog.Function.Container?.Navigation?.Navigate(0);
                        Websocket.Open();
                    }
                    break;
                case "Schedule":
                    if (Global.Settings.Serein.PagesDisplayed.Schedule)
                    {
                        Catalog.MainWindow.Navigation.Navigate(2);
                        Catalog.Function.Container?.Navigation?.Navigate(3);
                    }
                    break;
                case "Plugins":
                    if (Global.Settings.Serein.PagesDisplayed.JSPlugin)
                    {
                        Catalog.MainWindow.Navigation.Navigate(2);
                        Catalog.Function.Container?.Navigation?.Navigate(4);
                    }
                    break;
                case "Settings":
                    if (Global.Settings.Serein.PagesDisplayed.Settings)
                    {
                        Catalog.MainWindow.Navigation.Navigate(4);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
