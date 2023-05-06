using System.Linq;

namespace Serein.Core.JSPlugin.Native
{
    internal struct PluginStruct
    {
        public string @namespace;
        public string name;
        public string version;
        public string author;
        public string description;
        public string file;
        public PreLoadConfig preLoadConfig;
        public string[] eventList;
        public WSClient.ReadonlyWSClient[] wsclients;
        public bool available;

        public PluginStruct(Plugin plugin)
        {
            @namespace = plugin.Namespace;
            name = plugin.Name;
            version = plugin.Version;
            author = plugin.Author;
            description = plugin.Description;
            file = plugin.File;
            eventList = plugin.EventList.Select((eventType) => eventType.ToString()).ToArray();
            available = plugin.Available;
            preLoadConfig = plugin.PreLoadConfig;
            wsclients = plugin.WSClients.Select((wsclient) => new WSClient.ReadonlyWSClient(wsclient)).ToArray();
        }
    }
}