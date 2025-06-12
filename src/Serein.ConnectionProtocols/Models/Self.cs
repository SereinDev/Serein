using System.ComponentModel;

namespace Serein.ConnectionProtocols.Models;

public class Self : INotifyPropertyChanged
{
    public string UserId { get; set; } = string.Empty;

    public string Platform { get; set; } = string.Empty;

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
