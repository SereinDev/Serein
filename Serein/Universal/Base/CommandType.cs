namespace Serein.Base
{
    /// <summary>
    /// 命令类型
    /// </summary>
    internal enum CommandType
    {
        /// <summary>
        /// 错误的，会谢的，栓q的，yyds的，暴风吸入的，绝绝子的，属于是的，剁jiojio的，homo特有的，现充的，一整个的，乌鱼子的，集美的，咱就是说的，退退退的，别急的，抛开事实不谈的，9敏的()
        /// </summary>
        Invalid,

        /// <summary>
        /// 执行cmd
        /// </summary>
        ExecuteCmd,

        /// <summary>
        /// 服务器输入命令
        /// </summary>
        ServerInput,

        /// <summary>
        /// 服务器输入命令（使用Unicode）
        /// </summary>
        ServerInputWithUnicode,

        /// <summary>
        /// 发送群聊消息
        /// </summary>
        SendGroupMsg,

        /// <summary>
        /// 发送群聊消息到指定群
        /// </summary>
        SendGivenGroupMsg,

        /// <summary>
        /// 发送私聊消息
        /// </summary>
        SendPrivateMsg,

        /// <summary>
        /// 发送私聊消息给指定用户
        /// </summary>
        SendGivenPrivateMsg,

        /// <summary>
        /// 发送临时会话消息
        /// </summary>
        SendTempMsg,

        /// <summary>
        /// 绑定
        /// </summary>
        Bind,

        /// <summary>
        /// 解绑
        /// </summary>
        Unbind,

        /// <summary>
        /// 查询Motdpe
        /// </summary>
        RequestMotdpe,

        /// <summary>
        /// 查询Motdje
        /// </summary>
        RequestMotdje,

        /// <summary>
        /// 执行JS代码
        /// </summary>
        ExecuteJavascriptCodes,

        /// <summary>
        /// 在指定命名空间执行JS代码
        /// </summary>
        ExecuteJavascriptCodesWithNamespace,

        /// <summary>
        /// 调试输出
        /// </summary>
        DebugOutput
    }
}
