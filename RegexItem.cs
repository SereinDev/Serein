using System.Text.RegularExpressions;

namespace Serein
{
    class RegexItem
    {
        public string Regex { get; set; } = "";
        public string Remark { get; set; } = "";
        public string Command { get; set; } = "";
        public int Area { get; set; } = 0;
        public bool IsAdmin { get; set; } = false;
        public string ConvertToStr()
        {
            string Text = $"{Regex}\t{Area}\t{IsAdmin}\t{Remark}\t{Command}";
            return Text;
        }
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
                if (Serein.Command.GetType(Command) == -1)
                {
                    return false;
                }
                try
                {
                    Regex m = new Regex(Regex);
                    m.Match("");
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
