namespace Serein.Core.Models.Settings;

public class DevelopmentSetting
{
    public bool EnableDebug { get; set; }

    public bool DetailDebug { get; set; }

    public string NOTE { get; } = "以上设置内容为开发专用选项，请在指导下修改";
}