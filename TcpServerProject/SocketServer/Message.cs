using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    class Message
    {
        byte[] data = new byte[1024];
        int startIndex = 0;
        public byte[] Data
        {
            get { return data; }
        }
        public int StartIndex
        {
            get { return startIndex; }
        }
        public int RemainSize
        {
            get { return data.Length - startIndex; }
        }
        public void AddCount(int count)
        {
            startIndex += count;
        }
        public void ReadMessage()
        {
            while (true)
            {
                //Console.WriteLine(StartIndex);
                if (startIndex <= 4) return;
                int count = BitConverter.ToInt32(data, 0);
                //Console.WriteLine("count:"+count);
                if ((startIndex-4) >= count)
                {
                    string dataString = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析出来一条数据:" + dataString);
                    Array.Copy(data, count + 4, data, 0, startIndex - count - 4);
                    startIndex -= (count + 4);
                    
                }
                else
                {
                    break;
                }
            }
        }
    }
}
