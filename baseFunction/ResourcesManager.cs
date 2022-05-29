using System.IO;
using System.Text;

namespace Serein
{
    class ResourcesManager
    {
        public static void InitConsole()
        {
            ExtractConsoleFile(Properties.Resources.console_html, "console.html");
            ExtractConsoleFile(Properties.Resources.console_js, "console.js");
            ExtractConsoleFile(Properties.Resources.preset_css, "preset.css");
            ExtractConsoleFile(Properties.Resources.vanilla_css, "vanilla.css");
        }
        private static void ExtractConsoleFile(string Resource, string Name)
        {
            if (!Directory.Exists(Global.Path + "console"))
            {
                Directory.CreateDirectory(Global.Path + "console");
            }
            StreamWriter streamWriter = new StreamWriter(Global.Path + "console\\" + Name, false, Encoding.UTF8);
            streamWriter.Write(Resource);
            streamWriter.Close();
        }
    }
}
