using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_LTW
{
    internal class ConnectDB
    {
        private string connectionString = "server=localhost;user=root;password=;database=do_an_ltwd;";

        public MySqlConnection getConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public Dictionary<int, string> loadFoodList()
        {
            var foodList = new Dictionary<int, string>();

            using (var conn = getConnection())
            {
                conn.Open();
                string sql = "SELECT ID, name FROM list_food";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("ID");
                        string name = reader.GetString("name");
                        foodList[id] = name;
                    }
                }
            }

            return foodList;
        }
    }
}
