using System;
using System.Windows.Input;

namespace Serein.Plus.Commands;

#pragma warning disable CS0067

public class TaskbarIconDoubleClickCommand(MainWindow mainWindow) : ICommand
{
    private readonly MainWindow _mainWindow = mainWindow;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _mainWindow.ShowWindow();
    }

    public event EventHandler? CanExecuteChanged;
}
