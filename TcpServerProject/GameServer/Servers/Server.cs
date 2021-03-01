using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GameServer.Controller;
using Common;

namespace GameServer.Servers
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket socket;
        private List<Client> clientList = new List<Client>();
        private List<Room> roomList = new List<Room>();
        private ControllerManager controllerManager;


        public Server(){ }
        public Server(string ipAddress, int port)
        {
            controllerManager = new ControllerManager(this);
            SetIPAndPort(ipAddress, port);
        }
        public void SetIPAndPort(string ipAddress, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        }

        public void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipEndPoint);
            socket.Listen(3);
            socket.BeginAccept(AcceptCallBack,null);
        }
        void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = socket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            Console.WriteLine("一台客户端已连接");
            clientList.Add(client);
            socket.BeginAccept(AcceptCallBack, socket);
        }
        public void RemoveClient(Client client)
        {
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }
        public void SendResponse(string data, ActionCode actionCode, Client client)
        {
            client.Send(actionCode, data);
        }
        public void HandleRequest(Client client, RequestCode requestCode, ActionCode actionCode, string data)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }
        public void AddRoom(Client client,string data)
        {
            Room room = new Room(data, this);
            roomList.Add(room);
            room.RoomAddClient(client);
        }
        public void RemoveRoom(Room room)
        {
            if(roomList!=null&&room!=null)
            {
                roomList.Remove(room);
            }
            ////if (roomList.Count <= 0) return;
            //for (int i = 0; i < roomList.Count; i++)
            //{
            //    Room room = roomList[i];
            //    if (room.roomBluePlayerID == client.GetResultInfo().UserID)
            //    {
            //        room.RemovePlayerInRoom(client);
            //        if (room.GetRoomListCount() > 0)
            //        {
            //            return;
            //        }
            //        lock (roomList)
            //        {
            //            roomList.Remove(room);
            //        }
            //    }
            //}

        }
        public List<Room> GetRoomList()
        {
            return roomList;
        }
        public Room GetRoomByID(int id)
        {
            foreach (var room in roomList)
            {
                if(room.GetID() == id)
                {
                    return room;
                }
            }
            return null;
        }
    }
}
