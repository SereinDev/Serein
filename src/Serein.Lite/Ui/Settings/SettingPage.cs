using System.Windows.Forms;

namespace Serein.Lite.Ui.Settings;

public partial class SettingPage : UserControl
{
    public SettingPage(
        ConnectionSettingPage connectionSettingPage,
        ReactionSettingPage reactionSettingPage,
        WebApiSettingPage webApiSettingPage,
        AppSettingPage appSettingPage,
        AboutPage aboutPage
    )
    {
        InitializeComponent();

        _connectionTabPage.Controls.Add(WrapPage(connectionSettingPage));
        _reactionTabPage.Controls.Add(WrapPage(reactionSettingPage, DockStyle.Fill));
        _webTabPage.Controls.Add(WrapPage(webApiSettingPage));
        _applicationTabPage.Controls.Add(WrapPage(appSettingPage));
        _aboutTabPage.Controls.Add(WrapPage(aboutPage));
    }

    private static Panel WrapPage(UserControl userControl, DockStyle dockStyle = DockStyle.Top)
    {
        userControl.Dock = dockStyle;
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            AutoScroll = true,
        };

        panel.Controls.Add(userControl);
        return panel;
    }
}
