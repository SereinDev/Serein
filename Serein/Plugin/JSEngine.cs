using Jint;
using Serein.Base;
using Serein.Items.Motd;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Serein.Plugin
{
    internal class JSEngine
    {
        private static Engine engine = new Engine();
        public static void Init()
        {
            engine = new Engine(cfg => cfg.AllowClr(
                typeof(File).Assembly,
                typeof(Path).Assembly,
                typeof(Directory).Assembly,
                typeof(DirectoryInfo).Assembly,
                typeof(StreamReader).Assembly,
                typeof(StreamWriter).Assembly,
                typeof(Encoding).Assembly,
                typeof(Process).Assembly,
                typeof(ProcessStartInfo).Assembly
                )
            .TimeoutInterval(new TimeSpan(0, 1, 0))
            .CatchClrExceptions()
            );
            engine.SetValue("Serein_Global_Path", Global.Path);
            engine.SetValue("Serein_Global_Debug", new Action<object>(Global.Debug));
            engine.SetValue("Serein_Base_Command_Run", new Action<string>((command) => Command.Run(5, command)));
            engine.SetValue("Serein_Plugin_JSFunc_Register", new Func<string, string, string, string, bool>(JSFunc.Register));
            engine.SetValue("Serein_Plugin_JSFunc_SetListener", new Func<string, string, bool>(JSFunc.SetListener));
            engine.SetValue("Serein_Motdpe", new Func<string, string>((IP) => { return new Motdpe(IP).Original; }));
            engine.SetValue("Serein_Motdje", new Func<string, string>((IP) => { return new Motdje(IP).Original; }));
            engine.Execute("var serein={" +
                "path:Serein_Global_Path," +
                "debug:Serein_Global_Debug," +
                "run:Serein_Base_Command_Run," +
                "reg:Serein_Plugin_JSFunc_Register," +
                "register:Serein_Plugin_JSFunc_Register," +
                "setl:Serein_Plugin_JSFunc_SetListener," +
                "setlistener:Serein_Plugin_JSFunc_SetListener," +
                "motdpe:Serein_Motdpe," +
                "motdje:Serein_Motdje" +
                "};");
        }
        public static bool Run(string Code)
        {
            try
            {
                engine.Execute(Code);
                return true;
            }
            catch (Exception e)
            {
                Global.Debug("[JSEngine] " + e.Message);
                return false;
            }
        }
    }
}
