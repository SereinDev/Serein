using System.Windows.Forms;

namespace Serein.Lite.Ui.Settings;

public partial class SettingPage : UserControl
{
    public SettingPage(
        ConnectionSettingPage connectionSettingPage,
        AppSettingPage appSettingPage,
        AboutPage aboutPage
    )
    {
        InitializeComponent();

        ConnectionTabPage.Controls.Add(WrapPage(connectionSettingPage));
        ApplicationTabPage.Controls.Add(WrapPage(appSettingPage));
        AboutTabPage.Controls.Add(WrapPage(aboutPage));
    }

    private static Panel WrapPage(UserControl userControl)
    {
        userControl.Dock = DockStyle.Top;
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            AutoScroll = true
        };

        panel.Controls.Add(userControl);
        return panel;
    }
}
