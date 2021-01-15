using DB.SQLCommand;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DatabaseConnection
{
    class MySQLConnection : DBConnectionInterface
    {
        MySqlConnection connection;
        public MySQLConnection(MySQLDatabase sql)
        {
            connection = MySQLConnector.GetConnection(sql);
        }

        public void Close()
        {
            connection.Close();
        }

        public SQLCommandInterface CreateCommand()
        {
            MySqlCommand sqlCommand = connection.CreateCommand();
            return new MySQLCommand(sqlCommand);
        }


        public DataTable GetSchema(string name)
        {
            return connection.GetSchema(name);
        }

        public ConnectionState GetState()
        {
            return connection.State;
        }

        public void Open()
        {
            connection.Open();
        }
    }
}
