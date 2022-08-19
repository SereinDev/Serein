namespace Serein.Settings
{
    internal class Server
    {
        public string Path { get; set; } = string.Empty;
        public bool EnableRestart { get; set; } = false;
        public bool EnableOutputCommand { get; set; } = true;
        public bool EnableLog { get; set; } = false;
        public int OutputStyle { get; set; } = 1;
        public string StopCommand { get; set; } = "stop";
        public bool AutoStop { get; set; } = true;
        public int EncodingIndex { get; set; } = 0;
        public bool EnableUnicode { get; set; } = false;
        public int Type { get; set; } = 1;
        public int Port { get; set; } = 19132;
    }
}
