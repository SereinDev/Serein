using Serein.Base;

namespace Serein.Core.JSPlugin.Native
{
    internal struct RegexStruct
    {
#pragma warning disable IDE1006
        public string regex;
        public string remark;
        public string command;
        public int area;
        public bool needAdmin;
#pragma warning restore IDE1006

        internal RegexStruct(Regex regex)
        {
            this.regex = regex?.Expression;
            remark = regex?.Remark;
            command = regex?.Command;
            area = regex?.Area ?? 0;
            needAdmin = regex?.IsAdmin ?? false;
        }
    }
}