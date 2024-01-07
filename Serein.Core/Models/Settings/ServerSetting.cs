using System;

using Serein.Core.Models.Server;
using Serein.Core.Utils;

namespace Serein.Core.Models.Settings;

public partial class ServerSetting
{
    public string FileName { get; set; } = string.Empty;

    public string Argument { get; set; } = string.Empty;

    public bool AutoStopWhenCrashing { get; set; } = true;

    public bool AutoRestart { get; set; }

    public bool OutputCommandUserInput { get; set; } = true;

    public bool SaveLog { get; set; }

    public bool UseUnicodeChars { get; set; }

    public string[] ExcludedOutputs { get; set; } = Array.Empty<string>();

    public EncodingMap.EncodingType InputEncoding { get; set; }

    public string LineTerminator { get; set; } = Environment.NewLine;

    public EncodingMap.EncodingType OutputEncoding { get; set; }

    public OutputStyle OutputStyle { get; set; }

    public ushort Port { get; set; } = 19132;

    public string[] StopCommands { get; set; } = { "stop" };

    public ServerType Type { get; set; }
}
