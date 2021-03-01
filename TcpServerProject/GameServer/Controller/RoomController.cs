using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.Model;

namespace GameServer.Controller
{
    class RoomController : BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }


        public string CreateRoom(string data, Client client, Server server)
        {
            server.AddRoom(client, data);
            //roomList.Add(newRoom);
            return ((int)ReturnCode.Success).ToString();
        }
        public string CloseRoom(string data, Client client, Server server)
        {
            Room room = client.GetClientRoom();
            if (client.IsRoomOWner())
            {
                //room.BroadcastMessage(client, ActionCode.CloseRoom, ((int)ReturnCode.Success).ToString());
                room.Close();
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                client.GetClientRoom().RoomRemoveClient(client);
                room.BroadcastMessage(null, ActionCode.UpdateRoom, room.GetRoomPlayerInfo());
                return ((int)ReturnCode.Success).ToString();
            }
            //Room room = server.GetRoomByID(int.Parse(data));
            //room.QuitRoom(client);
        }
        public string RefreshRoom(string data, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var room in server.GetRoomList())
            {
                if(room.roomState == RoomState.WaitingJoin)
                {
                    sb.Append(room.GetHouseOwnerData() +","+ room.roomName+ "`");
                }
            }
            if (sb.Length == 0) return "0";
            
            string dataStr = sb.ToString();

            return dataStr.Remove(dataStr.Length - 1, 1);
        }

        public string JoinRoom(string data, Client client, Server server)
        {
            Console.WriteLine("???进来了吗?");
            Room room = server.GetRoomByID(int.Parse(data));
            if (room == null)
            {
                Console.WriteLine("房间为空,请求结束");
                return ((int)ReturnCode.NotFound).ToString();
            }
            else if (room.IsWaitingJoin())
            {
                Console.WriteLine("房间已满,请求结束");
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                room.RoomAddClient(client);
                string playerData = room.GetRoomPlayerInfo();
                room.BroadcastMessage(client, ActionCode.UpdateRoom, playerData);
                return ((int)ReturnCode.Success).ToString() + "," + room.roomName + "!" + playerData;
            }
        }

    }
}
