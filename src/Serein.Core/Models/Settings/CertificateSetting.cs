namespace Serein.Core.Models.Settings;

public class CertificateSetting : NotifyPropertyChangedModelBase
{
    public bool IsEnabled { get; set; }

    public bool AutoRegisterCertificate { get; set; }

    public bool AutoLoadCertificate { get; set; }

    public string? Path { get; set; }

    public string? Password { get; set; }
}
