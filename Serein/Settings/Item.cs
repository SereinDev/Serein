using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serein.Settings
{
    internal class Item
    {
        public Server Server = new Server();
        public Matches Matches = new Matches();
        public Bot Bot = new Bot();
        public Serein Serein = new Serein();
    }
}
