namespace Serein.Plugin
{
    partial class JSFunc
    {
        public static bool Register(
            string Name,
            string Version,
            string Author,
            string Description
            )
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name) || Plugins.PluginNames.Contains(Name))
            {
                return false;
            }
            Plugins.PluginItems.Add(new PluginItem()
            {
                Name = Name,
                Version = Version,
                Author = Author,
                Description = Description
            });
            return true;
        }
        public static bool SetListener(string Event, string FunctionName)
        {
            return false;
        }
    }
}
