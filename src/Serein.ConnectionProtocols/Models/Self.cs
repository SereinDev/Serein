using System.ComponentModel;

namespace Serein.ConnectionProtocols.Models;

public class Self : INotifyPropertyChanged
{
    public Self() { }

    public Self(Self self)
    {
        UserId = self.UserId;
        Platform = self.Platform;
    }

    public string UserId { get; set; } = string.Empty;

    public string Platform { get; set; } = string.Empty;

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
