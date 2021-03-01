using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.Model;
using GameServer.DAO;

namespace GameServer.Controller
{
    class UserController:BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private ResultDAO resultDAO = new ResultDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }
        public string Login(string data, Client client, Server server)
        {
            Console.WriteLine("收到来自客户端的登录请求");
            string[] strs = data.Split(',');
            User user = userDAO.VerifyUser(client.Conn, strs[0], strs[1]);
            if (user != null)
            {
                Result resultData = resultDAO.GetPlayerDataByUserID(client.Conn, user.ID);
                client.SetClientData(user, resultData);
                string returnData = string.Format("{0},{1},{2},{3},{4}", ((int)ReturnCode.Success).ToString(), resultData.UserID, strs[0], resultData.TotalCount, resultData.WinCount);
                return returnData;
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
        public string Register(string data, Client client, Server server)
        {
            Console.WriteLine("收到来自客户端的注册请求");
            string[] strs = data.Split(',');
            bool isSuccess = userDAO.InsertUser(client.Conn, strs[0], strs[1]);
            if (isSuccess)
            {
                return ((int)ReturnCode.Success).ToString();
            }
            return ((int)ReturnCode.Fail).ToString();
        }
    }
}
