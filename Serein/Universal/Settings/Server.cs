namespace Serein.Settings
{
    internal class Server
    {
        public bool AutoStop { get; set; } = true;
        public bool EnableRestart { get; set; } = false;
        public bool EnableOutputCommand { get; set; } = true;
        public bool EnableLog { get; set; } = false;
        public bool EnableUnicode { get; set; } = false;
        public int InputEncoding { get; set; } = 0;
        public int OutputEncoding { get; set; } = 0;
        public int OutputStyle { get; set; } = 1;
        public string Path { get; set; } = string.Empty;
        public int Port { get; set; } = 19132;
        public string[] StopCommands { get; set; } = new[] { "stop" };
        public int Type { get; set; } = 1;
    }
}
