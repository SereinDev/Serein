using System;
using Serein.Core.Utils;

namespace Serein.Core.Models.Server;

public class Configuration : NotifyPropertyChangedModelBase
{
    public string Name { get; set; } = "未命名";

    public string FileName { get; set; } = string.Empty;

    public string Argument { get; set; } = string.Empty;

    public bool AutoStopWhenCrashing { get; set; } = true;

    public bool AutoRestart { get; set; }

    public bool OutputCommandUserInput { get; set; } = true;

    public bool SaveLog { get; set; }

    public string LineTerminator { get; set; } = Environment.NewLine;

    public EncodingMap.EncodingType InputEncoding { get; set; }

    public EncodingMap.EncodingType OutputEncoding { get; set; }

    public OutputStyle OutputStyle { get; set; } = OutputStyle.RawText;

    public short PortIPv4 { get; set; } = 19132;

    public string[] StopCommands { get; set; } = ["stop"];

    public bool StartWhenSettingUp { get; set; }

    public bool UseUnicodeChars { get; set; }

    public bool UsePty { get; set; }

    public int? TerminalWidth { get; set; } = 150;

    public int? TerminalHeight { get; set; } = 80;
}
