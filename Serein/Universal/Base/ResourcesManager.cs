using System.IO;
using System.Text;

namespace Serein.Base
{
    internal class ResourcesManager
    {
        /// <summary>
        /// 初始化控制台
        /// </summary>
        public static void InitConsole()
        {
            ExtractConsoleFile(Properties.Resources.console_html, "console.html");
            ExtractConsoleFile(Properties.Resources.console_js, "console.js");
            ExtractConsoleFile(Properties.Resources.preset_css, "preset.css");
            ExtractConsoleFile(Properties.Resources.vanilla_css, "vanilla.css");
        }

        /// <summary>
        /// 解压控制台文件
        /// </summary>
        /// <param name="Resource">资源</param>
        /// <param name="Name">文件名</param>
        private static void ExtractConsoleFile(string Resource, string Name)
        {
            if (!Directory.Exists(Global.Path + "console"))
                Directory.CreateDirectory(Global.Path + "console");
            File.WriteAllText(Global.Path + "console\\" + Name, Resource, Encoding.UTF8);
        }
    }
}
