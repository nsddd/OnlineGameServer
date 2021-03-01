using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    class GameController:BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }
        public string StartGame(string data, Client client, Server server)
        {
            string str = ",";
            Room room = client.GetClientRoom();
            if (client.IsRoomOWner())
            {
                if (!room.IsRoomFull())
                {
                    str += "房间未满无法开始游戏!";
                }
                room.BroadcastMessage(client, ActionCode.StartGame, ((int)ReturnCode.Success).ToString());
                room.RunTimer();
                return ((int)ReturnCode.Success).ToString() + str;
            }
            else
            {
                str += "请等待房主开始游戏!";
                return ((int)ReturnCode.Fail).ToString() + str;
            }
        }

    }
}
