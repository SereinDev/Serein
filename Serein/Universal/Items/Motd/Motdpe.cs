using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Serein.Items.Motd
{
    internal class Motdpe : Motd
    {
        public Motdpe(string NewIp = "127.0.0.1", string NewPort = "19132")
        {
            if (!Init(NewIp, NewPort))
            {
                return;
            }
            int Lenth = 0;
            string Data = string.Empty;
            DateTime StartTime = DateTime.Now;
            try
            {
                Socket Client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                Client.Bind(new IPEndPoint(IPAddress.Any, 0));
                string[] Guids = Guid.NewGuid().ToString().ToUpper().Split('-');
                int TickCount = Environment.TickCount;
                string Text = $"01{TickCount.ToString("X").PadLeft(16, '0')}00FFFF00FEFEFEFEFDFDFDFD12345678{Guids[2]}{Guids[4]}";
                byte[] SendBytes = new byte[Text.Length / 2];
                for (int i = 0; i < Text.Length / 2; i++)
                {
                    SendBytes[i] = Convert.ToByte(Text.Substring(i * 2, 2), 16);
                }
                StartTime = DateTime.Now;
                Client.SendTo(SendBytes, new IPEndPoint(IP, Port));
                EndPoint Point = new IPEndPoint(IPAddress.Any, 0);
                byte[] Buffer = new byte[1024];
                Lenth = Client.ReceiveFrom(Buffer, ref Point);
                Delay = DateTime.Now - StartTime;
                Data = Lenth > 35 ?
                    Encoding.UTF8.GetString(Buffer, 35, Lenth - 35) :
                    Encoding.UTF8.GetString(Buffer, 0, Lenth);
                Client.Close();
            }
            catch (Exception e)
            {
                Logger.Out(LogType.Debug, "[Motdpe]", e.ToString());
                Exception = e.Message;
                return;
            }
            if (Lenth > 35)
            {
                Original = Data;
                string[] Datas = Data.Split(';');
                if (Datas.Length >= 12)
                {
                    Description = System.Text.RegularExpressions.Regex.Replace(Datas[1], "§.", string.Empty);
                    Protocol = Datas[2];
                    Version = Datas[3];
                    OnlinePlayer = Datas[4];
                    MaxPlayer = Datas[5];
                    LevelName = Datas[7];
                    GameMode = Datas[8];
                    Success = true;
                }
            }
        }
    }
}
