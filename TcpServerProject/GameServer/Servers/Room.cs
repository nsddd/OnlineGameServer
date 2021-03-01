using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using Common;
using System.Threading;

namespace GameServer.Servers
{
    public enum RoomState
    {
        WaitingJoin,
        WaitingStart,
        Battle
    }
    class Room
    {

        public Room(string roomName, Server server)
        {
            this.roomName = roomName;
            this.server = server;
        }
        private List<Client> clientList = new List<Client>(2);
        public RoomState roomState = RoomState.WaitingJoin;
        private Server server;
        public string roomName;

        public bool IsWaitingJoin()
        {
            return roomState != RoomState.WaitingJoin;
        }
        public bool IsRoomFull()
        {
            if (clientList.Count >= 2)
            {
                return true;
            }
            return false;
        }
        void Timer()
        {
            Thread.Sleep(500);
            for (int i = 3; i > 0; i--)
            {
                BroadcastMessage(null, ActionCode.Timer, i.ToString());
                Thread.Sleep(1000);
            }
        }
        public void RunTimer()
        {
            new Thread(Timer).Start();
        }
        public void RoomAddClient(Client client)
        {
            clientList.Add(client);
            client.Room = this;
            if (clientList.Count >= 2)
            {
                roomState = RoomState.WaitingStart;
                //return GetRoomPlayerInfo(clientList[0]);
            }
        }
        public void RoomRemoveClient(Client client)
        {
            clientList.Remove(client);
            client.Room = null;
            if (clientList.Count >= 2)
            {
                roomState = RoomState.WaitingStart;
            }
            else
            {
                roomState = RoomState.WaitingJoin;
            }
        }
        public string GetRoomPlayerInfo()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in clientList)
            {
                sb.Append(c.GetUserData() + "`");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
        public int GetID()
        {
            if(clientList.Count>0)
                return clientList[0].GetUserID();
            return -1;
        }
        public void Close()
        {
            BroadcastMessage(clientList[0], ActionCode.CloseRoom, ((int)ReturnCode.Success).ToString());
            foreach (var item in clientList)
            {
                item.Room = null;
            }
            server.RemoveRoom(this);
        }
        public string GetHouseOwnerData()
        {
            return clientList[0].GetUserData();
        }
        public void BroadcastMessage(Client excludeClient, ActionCode actionCode, string data)
        {
            foreach (var client in clientList)
            {
                if (excludeClient != client)
                    server.SendResponse(data, actionCode, client);
            }
        }

        public bool IsRoomOWner(Client client)
        {
            return client == clientList[0];
        }
        //public int GetRoomListCount()
        //{
        //    return clientList.Count;
        ////}
        public void QuitRoom(Client client)
        {
            if (client == clientList[0])
            {
                //BroadcastMessage(client, ActionCode.CloseRoom, ((int)ReturnCode.Success).ToString());
                Close();return;
            }
            RoomRemoveClient(client);
        }
    }
}
