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
            if (!Directory.Exists(Path.Combine("console")) || !File.Exists(Path.Combine("console", "console.html")))
            {
                ExtractConsoleFile(Properties.Resources.console_html, "console.html");
                ExtractConsoleFile(Properties.Resources.preset_css, "preset.css");
                Global.FirstOpen = true;
            }
        }

        /// <summary>
        /// 解压控制台文件
        /// </summary>
        /// <param name="resource">资源</param>
        /// <param name="name">文件名</param>
        private static void ExtractConsoleFile(string resource, string name)
        {
            if (!Directory.Exists(Path.Combine("console")))
            {
                Directory.CreateDirectory(Path.Combine("console"));
            }
            File.WriteAllText(Path.Combine("console", name), resource, Encoding.UTF8);
        }
    }
}
