using System;
using System.ComponentModel;

using Serein.Core.Utils;

namespace Serein.Core.Models.Server;

public class Configuration : INotifyPropertyChanged
{
    public string Name { get; set; } = "Î´ÃüÃû";

    public string FileName { get; set; } = string.Empty;

    public string Argument { get; set; } = string.Empty;

    public bool AutoStopWhenCrashing { get; set; } = true;

    public bool AutoRestart { get; set; }

    public bool OutputCommandUserInput { get; set; } = true;

    public bool SaveLog { get; set; }

    public bool UseUnicodeChars { get; set; }

    public string LineTerminator { get; set; } = Environment.NewLine;

    public EncodingMap.EncodingType InputEncoding { get; set; }

    public EncodingMap.EncodingType OutputEncoding { get; set; }

    public OutputStyle OutputStyle { get; set; }

    public short IPv4Port { get; set; } = 19132;

    public string[] StopCommands { get; set; } = { "stop" };

    public bool StartWhenSettingUp { get; set; }

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
}
