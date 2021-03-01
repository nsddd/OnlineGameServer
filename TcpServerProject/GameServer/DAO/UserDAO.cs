using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;

namespace GameServer.DAO
{
    class UserDAO
    {
        public User VerifyUser(MySqlConnection conn, string username, string password)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from users where username=@username and password=@password", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int userID = reader.GetInt32("id");
                    User user = new User(userID, username, password);
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在VerifyUser时出错:"+e);
            }
            finally
            {
                if(reader!=null)
                reader.Close();
            }
            return null;
        }
        public bool GetIDByUsername(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from users where username=@username;", conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    return true;//查询到重复的名称
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return false;
        }
        public bool InsertUser(MySqlConnection conn, string username, string password)
        {
            try
            {
                if (GetIDByUsername(conn, username))
                {
                    MySqlCommand cmd = new MySqlCommand("insert into users set username=@username,password=@password;", conn);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);
                    int amount = cmd.ExecuteNonQuery();
                    if (amount > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }
    }
}
