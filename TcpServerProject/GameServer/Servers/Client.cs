using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Common;
using MySql.Data.MySqlClient;
using GameServer.Tool;
using GameServer.Model;

namespace GameServer.Servers
{
    class Client
    {
        Server server;
        Socket clientSocket;
        MySqlConnection conn;
        Message msg = new Message();
        private User user;
        private Result result;
        private Room room;
        public Room Room {
            get {
                return room;
            }
            set {
                room = value;
            }
        }


        public MySqlConnection Conn {
            get {
                return conn;
            }
        }
        public Client(){ }
        public Client( Socket client,Server server)
        {
            this.clientSocket = client;
            this.server = server;
            conn = ConnHelper.Connect();
            Start();
        }
        public Room GetClientRoom()
        {
            return room;
        }
        public void SetClientData(User user, Result result)
        {
            this.user = user;
            this.result = result;
        }
        public int GetUserID()
        {
            return user.ID;
        }
        public string GetUserData()
        {
            return result.UserID + "," + user.Username + "," + result.TotalCount + "," + result.WinCount;
        }
        //public Result GetResultInfo()
        //{
        //    return result;
        //}

        void Start()
        {
            clientSocket.BeginReceive(msg.Data,msg.StartIndex,msg.RemainSize,SocketFlags.None,ReceiveCallBack,null);
        }
        void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket.Connected == false || clientSocket == null) return;
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Console.WriteLine("未收到任何信息");
                    Close();
                    Console.WriteLine("客户端正常关闭");
                    return;
                }
                Console.WriteLine("收到客户端传递过来的信息,已开始处理");
                msg.ReadMessage(count, ProcessDataCallBack);
                Start();
            }
            catch (Exception e)
            {
                Close();
                Console.WriteLine(e);
                Console.WriteLine("客户端未正常关闭");
            }
        }
        void ProcessDataCallBack(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.HandleRequest(this, requestCode, actionCode, data);
        }
        void Close()
        {
            if (clientSocket != null)
                clientSocket.Close();
            if (room != null)
                room.QuitRoom(this);
            server.RemoveClient(this);
            
        }
        public void Send(ActionCode actionCode ,string data)
        {
            byte[] bytes = Message.PackMessage(actionCode, data);

            if (clientSocket == null && clientSocket.Connected == false) return;
            clientSocket.Send(bytes);
            Console.WriteLine("请求处理完成,已发送响应");
        }
        public bool IsRoomOWner()
        {
            if (room == null) return false;
            return room.IsRoomOWner(this);
        }
    }
}
