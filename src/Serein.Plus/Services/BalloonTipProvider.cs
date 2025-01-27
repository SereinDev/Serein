using Hardcodet.Wpf.TaskbarNotification;

namespace Serein.Plus.Services;

public sealed class BalloonTipProvider(MainWindow mainWindow)
{
    public void ShowBalloonTip(string title, string message, BalloonIcon icon)
    {
        mainWindow.ShowBalloonTip(title, message, icon);
    }
}
