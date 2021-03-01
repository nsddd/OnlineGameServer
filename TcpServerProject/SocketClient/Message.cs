using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class Message
    {
        public static byte[] GetBytes(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataLength = dataBytes.Length;
            byte[] langthBytes = BitConverter.GetBytes(dataLength);
            byte[] newBytes = langthBytes.Concat(dataBytes).ToArray();
            return newBytes;
        }
    }
}
