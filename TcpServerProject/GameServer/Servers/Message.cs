using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Servers
{
    class Message
    {
        byte[] data = new byte[1024];
        int startIndex = 0;
        public byte[] Data {
            get {
                return data;
            }
        }
        public int StartIndex {
            get {
                return startIndex;
            }
        }
        public int RemainSize {
            get {
                return data.Length - startIndex;
            }
        }

        public void ReadMessage(int newDataAmount,Action<RequestCode,ActionCode,string> processDataCallBack)
        {
            startIndex += newDataAmount;
            while (true)
            {
                if (startIndex <= 4) return;
                int count = BitConverter.ToInt32(Data, 0);
                if (startIndex >= count + 4)
                {
                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(Data, 4);//将请求代码解析出来
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(Data, 8);
                    string s = Encoding.UTF8.GetString(data, 12, count - 8);
                    processDataCallBack(requestCode, actionCode, s);
                    //Console.WriteLine(s);
                    Array.Copy(data, count + 4, data, 0, data.Length - count - 4);
                    startIndex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
        }
        public static byte[] PackMessage(ActionCode actionCode, string data)
        {
            byte[] requestBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int amount = requestBytes.Length + dataBytes.Length;
            byte[] amountBytes = BitConverter.GetBytes(amount);
            return (amountBytes.Concat(requestBytes).Concat(dataBytes)).ToArray();
        }

    }
}
