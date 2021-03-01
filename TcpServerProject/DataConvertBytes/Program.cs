using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConvertBytes
{
    class Program
    {
        static void Main(string[] args)
        {

            byte[] data = BitConverter.GetBytes(123);
            foreach (var b in data)
            {
                Console.Write(b+":");
            }
            Console.ReadKey();
        }
    }
}
