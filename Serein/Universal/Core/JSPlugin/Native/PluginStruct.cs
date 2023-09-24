using System.Linq;

namespace Serein.Core.JSPlugin.Native
{
    internal struct PluginStruct
    {
        public string? Namespace;
        public string? Name;
        public string? Version;
        public string? Author;
        public string? Description;
        public string? File;
        public PreLoadConfig PreLoadConfig;
        public string[] EventList;
        public WSClient.ReadonlyWSClient[] Wsclients;
        public bool Available;

        public PluginStruct(Plugin plugin)
        {
            Namespace = plugin.Namespace;
            Name = plugin.Name;
            Version = plugin.Version;
            Author = plugin.Author;
            Description = plugin.Description;
            File = plugin.File;
            EventList = plugin.EventList.Select((eventType) => eventType.ToString()).ToArray();
            Available = plugin.Available;
            PreLoadConfig = plugin.PreLoadConfig;
            Wsclients = plugin.WSClients
                .Select((wsclient) => new WSClient.ReadonlyWSClient(wsclient))
                .ToArray();
        }
    }
}
