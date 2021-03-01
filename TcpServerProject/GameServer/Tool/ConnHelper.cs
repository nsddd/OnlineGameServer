using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GameServer.Tool
{
    class ConnHelper
    {
        private const string CONNSTR = "server=127.0.0.1;port=3306;database=onlinegame;user=root;password=admin";
        public static MySqlConnection Connect()
        {
            MySqlConnection mySqlConnection = new MySqlConnection(CONNSTR);
            try
            {
                mySqlConnection.Open();
                return mySqlConnection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public static void Close(MySqlConnection mySqlConnection)
        {
            if (mySqlConnection != null)
                mySqlConnection.Close();
            else
            {
                Console.WriteLine("MySql无链接");
            }
        }
    }
}
