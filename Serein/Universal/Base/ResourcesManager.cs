using System.IO;
using System.Text;

namespace Serein.Base
{
    internal static class ResourcesManager
    {
        /// <summary>
        /// 初始化控制台
        /// </summary>
        public static void InitConsole()
        {
            if (!Directory.Exists(IO.GetPath("console")) || !File.Exists(IO.GetPath("console", "console.html")))
            {
                ExtractConsoleFile(Properties.Resources.console_html, "console.html");
                ExtractConsoleFile(Properties.Resources.preset_css, "preset.css");
                Global.FirstOpen = true;
            }
        }

        /// <summary>
        /// 解压控制台文件
        /// </summary>
        /// <param name="Resource">资源</param>
        /// <param name="Name">文件名</param>
        private static void ExtractConsoleFile(string Resource, string Name)
        {
            if (!Directory.Exists(IO.GetPath("console")))
            {
                Directory.CreateDirectory(IO.GetPath("console"));
            }
            File.WriteAllText(IO.GetPath("console", Name), Resource, Encoding.UTF8);
        }
    }
}
