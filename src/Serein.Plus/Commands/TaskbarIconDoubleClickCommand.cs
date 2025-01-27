using System;
using System.Windows.Input;

namespace Serein.Plus.Commands;

#pragma warning disable CS0067

public class TaskbarIconDoubleClickCommand(MainWindow mainWindow) : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        mainWindow.ShowWindow();
    }

    public event EventHandler? CanExecuteChanged;
}
