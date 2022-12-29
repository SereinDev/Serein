using Jint.Native;
using Jint.Runtime;
using Serein.Base;
using Serein.Items;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Serein.JSPlugin
{
    internal class Event : IDisposable
    {
        public void Dispose()
            => Dict.Clear();

        public string Namespace = string.Empty;


        /// <summary>
        /// 监听字典
        /// </summary>
        private readonly Dictionary<EventType, Delegate> Dict = new Dictionary<EventType, Delegate>();
    }
}
