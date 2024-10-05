using System.ComponentModel;

namespace Serein.Core.Models;

public abstract class NotifyPropertyChangedModelBase : INotifyPropertyChanged
{
#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
