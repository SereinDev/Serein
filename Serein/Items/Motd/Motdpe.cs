using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Serein.Items.Motd
{
    internal class Motdpe : Motd
    {
        public Motdpe(string newip = "127.0.0.1", string newPort = "19132")
        {
            Init(newip, newPort);
            int length = 0;
            string Data = string.Empty;
            DateTime StartTime = DateTime.Now;
            try
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                client.Bind(new IPEndPoint(IPAddress.Any, 0));
                string[] Guids = Guid.NewGuid().ToString().ToUpper().Split('-');
                int TickCount = Environment.TickCount;
                string Text = $"01{TickCount.ToString("X").PadLeft(16, '0')}00FFFF00FEFEFEFEFDFDFDFD12345678{Guids[2]}{Guids[4]}";
                byte[] sendBytes = new byte[Text.Length / 2];
                for (int i = 0; i < Text.Length / 2; i++)
                {
                    sendBytes[i] = Convert.ToByte(Text.Substring(i * 2, 2), 16);
                }
                StartTime = DateTime.Now;
                client.SendTo(sendBytes, new IPEndPoint(IP, Port));
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = new byte[1024];
                length = client.ReceiveFrom(buffer, ref point);
                Delay = DateTime.Now - StartTime;
                Data = length > 35 ?
                    Encoding.UTF8.GetString(buffer, 35, length - 35) :
                    Encoding.UTF8.GetString(buffer, 0, length);
                client.Close();
            }
            catch (Exception e)
            {
                Global.Debug($"[Motdpe] {e.Message}");
                Exception = e.Message;
                return;
            }
            if (length > 35)
            {
                Original = Data;
                string[] Datas = Data.Split(';');
                if (Datas.Length >= 12)
                {
                    Description = Regex.Replace(Datas[1], "§.", string.Empty);
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
