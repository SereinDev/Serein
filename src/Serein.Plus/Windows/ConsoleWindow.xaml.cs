using System.Windows;
using Microsoft.Extensions.Logging;

namespace Serein.Plus.Windows;

public partial class ConsoleWindow : Window
{
    public ConsoleWindow()
    {
        InitializeComponent();
        Console.EnableLogLevelHighlight();
    }

    public void WriteLine(LogLevel logLevel, string message)
    {
        switch (logLevel)
        {
            case LogLevel.Information:
                Console.AppendInfoLine(message);
                break;

            case LogLevel.Warning:
                Console.AppendWarnLine(message);
                break;

            case LogLevel.Error:
                Console.AppendErrorLine(message);
                break;

            default:
                Console.AppendLine($"[{logLevel}] " + message);
                break;
        }
    }
}
