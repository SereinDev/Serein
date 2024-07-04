using System;
using System.Windows.Input;

using Serein.Plus.Ui.Window;

namespace Serein.Plus.Ui.Commands;

#pragma warning disable CS0067

public class TaskbarIconDoubleClickCommand(MainWindow parent) : ICommand
{
    private readonly MainWindow _parent = parent;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _parent.ShowWindow();
    }

    public event EventHandler? CanExecuteChanged;
}
