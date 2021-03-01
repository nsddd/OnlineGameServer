using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServerAsync();
            Console.ReadKey();
        }

        static byte[] dataBuffer = new byte[1024];
        public static Message msg = new Message();
        static void StartServerAsync()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.151"), 88);
            socket.Bind(iPEndPoint);
            socket.Listen(0);
            //Socket clientSocket = socket.Accept();

            socket.BeginAccept(AcceptCallBack, socket);


            Console.ReadKey();
            socket.Close();

        }
        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;
            Socket clientSocket = socket.EndAccept(ar);
            string msgStr = "你好,我带你们打";
            clientSocket.Send(Encoding.UTF8.GetBytes(msgStr));

            //开始异步加载
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiverCallBack, clientSocket);
            socket.BeginAccept(AcceptCallBack, socket);
        }
        static void ReceiverCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    clientSocket.Close(); return;
                }
                msg.AddCount(count);
                //Console.WriteLine("从客户端收到数据:"+Encoding.UTF8.GetString(dataBuffer, 0, count));
                //clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiverCallBack, clientSocket);
                msg.ReadMessage();
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiverCallBack, clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }
            }
        }

        static void StartServerSync()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.151"), 88);
            socket.Bind(iPEndPoint);
            socket.Listen(0);
            Socket clientSocket = socket.Accept();

            string msg = "你好,我带你们打";
            clientSocket.Send(Encoding.UTF8.GetBytes(msg));

            //接收消息
            byte[] data = new byte[1024];
            int count = clientSocket.Receive(data);
            Console.WriteLine(Encoding.UTF8.GetString(data, 0, count));

            Console.ReadKey();
            clientSocket.Close();
            socket.Close();

        }
    }
}
