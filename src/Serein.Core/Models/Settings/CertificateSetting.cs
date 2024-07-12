using System.ComponentModel;

namespace Serein.Core.Models.Settings;

public class CertificateSetting : INotifyPropertyChanged
{
    public bool Enable { get; set; }

    public bool AutoRegisterCertificate { get; set; }

    public bool AutoLoadCertificate { get; set; }

    public string? Path { get; set; }

    public string? Password { get; set; }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
