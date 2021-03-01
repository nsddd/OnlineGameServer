using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Tools;
using System.Reflection;
using GameServer.Servers;

namespace GameServer.Controller
{
    class ControllerManager
    {
        Dictionary<RequestCode, BaseController> requestDict = new Dictionary<RequestCode, BaseController>();
        Server server;
        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }
        void InitController()
        {
            DefaultController defaultController = new DefaultController();
            requestDict.Add(defaultController.RequestCode, defaultController);
            requestDict.Add(RequestCode.User, new UserController());
            requestDict.Add(RequestCode.Room, new RoomController());
            requestDict.Add(RequestCode.Game, new GameController());
        }
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            BaseController controller = DictTool.GetValue(requestDict, requestCode);
            string methodName = Enum.GetName(typeof(ActionCode),actionCode);
            Console.WriteLine("开始处理"+ methodName + "请求");
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if(mi == null)
            {
                Console.WriteLine("[警告]在controller:["+controller.GetType()+"]中没有对应的处理方法["+methodName+"]");return;
            }
            object[] parameters = new object[] { data,client,server };
            object o = mi.Invoke(controller, parameters);
            if (o == null || string.IsNullOrEmpty(o as string))
            {
                return;
            }
            server.SendResponse(o as string, actionCode, client);
        }
    }
}
