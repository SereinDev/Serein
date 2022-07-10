using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Serein.Base
{
    internal class EventTrigger
    {
        public static void Run(string Type, string Value, JObject JsonObject = null)
        {
            if (Type == "Bind_Success")
            {
            }
        }
    }
}
