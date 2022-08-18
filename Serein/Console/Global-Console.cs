using Serein.Console;

namespace Serein
{
    partial class Global
    {
        public static bool Console = true;
        public static void Logger(int Type, params object[] objects)
        {
            string Line = string.Empty;
            foreach (var o in objects)
            {
                if (o != null) { Line += o.ToString() + " "; }
            }
            Line = Line.TrimEnd();
            switch (Type)
            {
                case 999:
                    if (Settings.Serein.Debug) { Output.Logger(4, Line); }
                    break;
                case 1:
                case 11:
                case 21:
                case 31:
                    Output.Logger(1, Line);
                    break;
                case 2:
                    Output.Logger(2, Line);
                    break;
                case 3:
                case 24:
                case 32:
                    Output.Logger(3, Line);
                    break;
                case 10:
                case 20:
                case 30:
                    Output.Logger(0, Line);
                    break;
                case 22:
                    Output.Logger(0, $"\x1b[92m[↓]\x1b[0m{Line}");
                    break;
                case 23:
                    Output.Logger(0, $"\x1b[36m[↑]\x1b[0m{Line}");
                    break;
                case 33:
                    Output.Logger(0, $"\x1b[94m[插件]\x1b[0m{Line}");
                    break;
            }
        }
    }
}
