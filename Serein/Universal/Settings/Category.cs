namespace Serein.Settings
{
    internal class Category
    {
        public Server Server = new();
        public Matches Matches = new();
        public Bot Bot = new();
        public Serein Serein = new();
        public Event Event = new();
    }
}
