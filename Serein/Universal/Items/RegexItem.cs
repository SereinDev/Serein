using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Serein.Items
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class RegexItem
    {
        public string Regex { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public int Area { get; set; } = 0;
        public bool IsAdmin { get; set; } = false;

        public void ConvertToItem(string Text)
        {
            string[] Texts = Text.Split('\t');
            if (Texts.Length != 5)
            {
                return;
            }
            Regex = Texts[0];
            Area = int.TryParse(Texts[1], out int s) ? s : 0;
            IsAdmin = Texts[2] == "True";
            Remark = Texts[3];
            Command = Texts[4];
        }

        public bool CheckItem()
        {
            if (
                !(string.IsNullOrWhiteSpace(Regex) || string.IsNullOrEmpty(Regex) ||
                string.IsNullOrWhiteSpace(Command) || string.IsNullOrEmpty(Command)
                ))
            {
                if (Base.Command.GetType(Command) == -1)
                {
                    return false;
                }
                try
                {
                    new Regex(Regex).Match(string.Empty);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
