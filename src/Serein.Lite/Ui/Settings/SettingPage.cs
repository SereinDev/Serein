using System.Windows.Forms;

namespace Serein.Lite.Ui.Settings;

public partial class SettingPage : UserControl
{
    public SettingPage(ConnectionSettingPage connectionSettingPage, AppSettingPage appSettingPage)
    {
        InitializeComponent();

        ConnectionTabPage.Controls.Add(WrapPage(connectionSettingPage));
        ApplicationTabPage.Controls.Add(WrapPage(appSettingPage));
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
