namespace Serein.Settings
{
    internal class Server
    {
        public bool AutoStop = true;
        public bool EnableRestart = false;
        public bool EnableOutputCommand = true;
        public bool EnableLog = false;
        public bool EnableUnicode = false;
        public int InputEncoding = 0;
        public string LineTerminator = "\r\n";
        public int OutputEncoding = 0;
        public int OutputStyle = 1;
        public string Path = string.Empty;
        public int Port = 19132;
        public string[] StopCommands = new[] { "stop" };
        public int Type = 1;
    }
}
