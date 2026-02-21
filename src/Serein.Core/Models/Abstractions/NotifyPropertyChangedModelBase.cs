using System.ComponentModel;

namespace Serein.Core.Models.Abstractions;

public abstract class NotifyPropertyChangedModelBase : INotifyPropertyChanged
{
    protected void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
