namespace Serein.Settings
{
    internal class Server
    {
        public bool AutoStop = true;
        public bool EnableRestart;
        public bool EnableOutputCommand = true;
        public bool EnableLog;
        public bool EnableUnicode;
        public string[] ExcludedOutputs = new string[] { };
        public int InputEncoding;
        public string LineTerminator = "\r\n";
        public int OutputEncoding;
        public int OutputStyle = 1;
        public string Path = string.Empty;
        public int Port = 19132;
        public string[] StopCommands = { "stop" };
        public int Type = 1;
    }
}
