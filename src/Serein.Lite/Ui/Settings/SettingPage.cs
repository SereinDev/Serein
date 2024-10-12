using System.Windows.Forms;

namespace Serein.Lite.Ui.Settings;

public partial class SettingPage : UserControl
{
    public SettingPage(
        ConnectionSettingPage connectionSettingPage,
        AppSettingPage appSettingPage,
        ReactionSettingPage reactionSettingPage,
        WebApiSettingPage webApiSettingPage,
        AboutPage aboutPage
    )
    {
        InitializeComponent();

        ConnectionTabPage.Controls.Add(WrapPage(connectionSettingPage));
        ApplicationTabPage.Controls.Add(WrapPage(appSettingPage));
        ReactionTabPage.Controls.Add(WrapPage(reactionSettingPage, DockStyle.Fill));
        WebApiTabPage.Controls.Add(WrapPage(webApiSettingPage));
        AboutTabPage.Controls.Add(WrapPage(aboutPage));
    }

    private static Panel WrapPage(UserControl userControl, DockStyle dockStyle = DockStyle.Top)
    {
        userControl.Dock = dockStyle;
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
