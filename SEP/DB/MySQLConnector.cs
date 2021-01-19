using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    class MySQLConnector
    {
        private static MySqlConnection connection = null;
        private MySQLConnector() { }
        public static MySqlConnection GetConnection(MySQLDatabase mySql)
        {
            connection = MakeConnection(mySql);
            return connection;//nếu đã khởi tạo rồi thì trả về connection hiện tại
        }

        private static MySqlConnection MakeConnection(MySQLDatabase mySql)
        {
            string connectionString = "Server=" + mySql.host + ";Database=" + mySql.database
               + ";port=" + mySql.port + ";User Id=" + mySql.username + ";password=" + mySql.password;

            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }
    }
}
