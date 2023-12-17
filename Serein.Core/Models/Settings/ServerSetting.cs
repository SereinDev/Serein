using System;

using Serein.Core.Models.Server;
using Serein.Core.Utils;

namespace Serein.Core.Models.Settings;

public class ServerSetting
{
    public string? Argument;

    public bool AutoStop = true;

    public bool EnableRestart;

    public bool EnableOutputCommand = true;

    public bool EnableLog;

    public bool EnableUnicode;

    public string[] ExcludedOutputs = Array.Empty<string>();

    public EncodingMap.EncodingType InputEncoding;

    public string LineTerminator = Environment.NewLine;

    public EncodingMap.EncodingType OutputEncoding;

    public int OutputStyle = 1;

    public string Path = string.Empty;

    public int Port = 19132;

    public string[] StopCommands = { "stop" };

    public ServerType Type;
}
