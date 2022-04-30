using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void ConvertToItem(string text)
        {
            string[] texts = text.Split('\t');
            if (texts.Length != 5)
            {
                return;
            }
            Regex = texts[0];
            Area = int.TryParse(texts[1], out int s) ? s : 0;
            IsAdmin = texts[2]=="True";
            Remark = texts[3];
            Command = texts[4];
        }
    }
}
