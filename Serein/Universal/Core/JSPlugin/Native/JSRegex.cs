using Serein.Base;

namespace Serein.Core.JSPlugin.Native
{
    internal struct JSRegex
    {
        internal Regex Regex;

#pragma warning disable IDE1006
        public string regex => Regex.Expression;
        public string remark => Regex.Remark;
        public string command => Regex.Command;
        public int area => Regex.Area;
        public bool needAdmin => Regex.IsAdmin;
#pragma warning restore IDE1006

        internal JSRegex(Regex regex)
        {
            Regex = regex;
        }
    }
}