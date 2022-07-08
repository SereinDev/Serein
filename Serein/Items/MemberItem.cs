using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serein
{
    internal class MemberItem
    {
        public long ID { get; set; } = 0;
        public string Card { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public int Role { get; set; } = 2;
        public string GameID { get; set; } = string.Empty;
    }
}
