using Hardcodet.Wpf.TaskbarNotification;

namespace Serein.Plus.Services;

public sealed class BalloonTipProvider(MainWindow mainWindow)
{
    private readonly MainWindow _mainWindow = mainWindow;

    public void ShowBalloonTip(string title, string message, BalloonIcon icon)
    {
        _mainWindow.ShowBalloonTip(title, message, icon);
    }
}
