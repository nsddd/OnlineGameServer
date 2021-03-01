using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySQLExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = "port=3306;server=127.0.0.1;database=test007;user=root;password=admin";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            //查
            //MySqlCommand cmd = new MySqlCommand("select * from users", conn);
            //string username = "wdnmd";
            //string password = "wdnmd';delete from users;";
            //增
            //MySqlCommand cmd = new MySqlCommand("insert into users set username='" + username + "',password='" + password + "';", conn);
            //MySqlCommand cmd = new MySqlCommand("insert into users(username,password)values('qqq','qqq')", conn);
            //MySqlCommand cmd = new MySqlCommand("insert into users set username=@un,password=@pwd;", conn);

            //改
            //MySqlCommand cmd = new MySqlCommand("update users set password=@pwd where id=11;", conn);
            
            //删除
            MySqlCommand cmd = new MySqlCommand("delete from users where id=11;", conn);


            cmd.Parameters.AddWithValue("pwd", "nicooooo");
           // cmd.Parameters.AddWithValue("un", username);
            //cmd.Parameters.AddWithValue("pwd", password);
            
            cmd.ExecuteNonQuery();
            //MySqlDataReader reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    Console.WriteLine(reader.GetString("username")+":::"+reader.GetString("password"));
            //}

            //reader.Close();
            conn.Close();
            Console.ReadKey();
        }
    }
}
