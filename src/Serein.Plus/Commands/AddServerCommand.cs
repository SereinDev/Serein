using System;
using System.Windows.Input;

namespace Serein.Plus.Commands;

public class AddServerCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
    }
}
