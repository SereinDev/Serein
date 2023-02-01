namespace Serein.JSPlugin
{
    internal class PreLoadConfig
    {
        public string NOTE { get; } = "参考文档：[https://learn.microsoft.com/zh-cn/dotnet/api/system.reflection.assembly.load]；其中“Load”项应填系统程序集的不带扩展名的文件名，且该文件需位于“{NET安装目录}/{运行库类型}/{版本号}”的文件夹下；“LoadFrom”项应填写程序集文件（如“Newtonsoft.Json.dll”）相对于Serein文件的相对目录";
        public string[] Load = { };
        public string[] LoadFrom = { };
    }
}