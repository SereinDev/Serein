using Serein.Base;
using System.Windows;
using Serein.Utils;
using System.Threading.Tasks;

namespace Serein
{
    /*
     *  ____ 
     * /\  _`\                        __            
     * \ \,\L\_\     __   _ __    __ /\_\    ___    
     *  \/_\__ \   /'__`\/\`'__\/'__`\/\ \ /' _ `\  
     *    /\ \L\ \/\  __/\ \ \//\  __/\ \ \/\ \/\ \ 
     *    \ `\____\ \____\\ \_\\ \____\\ \_\ \_\ \_\
     *     \/_____/\/____/ \/_/ \/____/ \/_/\/_/\/_/
     *     
     *     https://github.com/Zaitonn/Serein
     *  Copyright © 2022 Zaitonn. All Rights Reserved.
     *     
     */

    public partial class App : Application
    {
        public App()
        {
            Runtime.Init();
            DispatcherUnhandledException += (_, e) => CrashInterception.ShowException(e.Exception);
        }
    }
}
