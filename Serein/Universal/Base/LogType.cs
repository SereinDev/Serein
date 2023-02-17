namespace Serein.Base
{
    internal enum LogType
    {
        Undefined,
        Info,
        Warn,
        Error,
        Debug,
        DetailDebug,

        Server_Input,
        Server_Notice,
        Server_Output,
        Server_Clear,

        Bot_Notice,
        Bot_Error,
        Bot_Receive,
        Bot_Send,
        Bot_Clear,

        Plugin_Notice,
        Plugin_Info,
        Plugin_Warn,
        Plugin_Error,
        Plugin_Clear,

        Version_New,
        Version_Latest,
        Version_Failure,
        Version_Downloading,
        Version_DownloadFailed,
        Version_Ready
    }
}
