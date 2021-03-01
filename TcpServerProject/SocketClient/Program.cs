using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.151"), 88));

            byte[] data = new byte[1024];
            int count = socket.Receive(data);
            Console.WriteLine(Encoding.UTF8.GetString(data, 0, count));
            Console.WriteLine();
            //发送消息
            for (int i = 0; i < 100; i++)
            {
                socket.Send(Message.GetBytes(i.ToString()));
            }
            //while (true)
            //{
            //    string s = Console.ReadLine();
            //    if (s == "quit")
            //    {
            //        socket.Close();return;
            //    }
            //    socket.Send(Encoding.UTF8.GetBytes(s));
            //}


            Console.ReadKey();
            socket.Close();
        }
    }
}
