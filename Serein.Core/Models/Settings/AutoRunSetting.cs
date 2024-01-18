using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class AutoRunSetting : INotifyPropertyChanged
{
    public bool Server { get; set; }

    public bool WebSocket { get; set; }

    public int Delay { get; set; } = 5000;

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
