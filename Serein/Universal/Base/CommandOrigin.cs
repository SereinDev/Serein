namespace Serein.Base
{
    /// <summary>
    /// 命令来源类型
    /// </summary>
    internal enum CommandOrigin
    {
        Msg,
        Console,
        Schedule,
        EventTrigger,
        Javascript
    }
}