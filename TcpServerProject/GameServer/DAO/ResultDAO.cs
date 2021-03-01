using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;

namespace GameServer.DAO
{
    class ResultDAO
    {
        public Result GetPlayerDataByUserID(MySqlConnection conn, int userID)
        {
            Result result = null;
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from result where userid=@userid;", conn);
                cmd.Parameters.AddWithValue("userid", userID);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                        int id = reader.GetInt32("id");
                        int totalCount = reader.GetInt32("totalcount");
                        int winCount = reader.GetInt32("wincount");
                        result = new Result(id, userID, totalCount, winCount);
                }
                else
                {
                    reader.Close();
                    bool isInsertSuccess = InsertNewPlayerData(conn, userID);
                    if (isInsertSuccess)
                    {
                        result = GetPlayerDataByUserID(conn, userID);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                reader.Close();
            }
            if (reader != null)
            {
                reader.Close();
            }
            return result ?? new Result(-1, -1, -1, -1);
        }

        public bool InsertNewPlayerData(MySqlConnection conn, int userID)
        {
            MySqlCommand cmd = new MySqlCommand("insert into result set userid=@userid;", conn);
            cmd.Parameters.AddWithValue("userid", userID);
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                return true;
            }
            return false;
        } 
    }
}
