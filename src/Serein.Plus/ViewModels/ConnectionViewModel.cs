using System.ComponentModel;

namespace Serein.Plus.ViewModels;

public class ConnectionViewModel : INotifyPropertyChanged
{
    public string Status { get; internal set; } = "未连接";

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}