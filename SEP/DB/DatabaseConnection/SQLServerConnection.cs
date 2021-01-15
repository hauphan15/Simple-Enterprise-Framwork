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
    class SQLServerConnection : DBConnectionInterface
    {
        private SqlConnection connection;
        public SQLServerConnection(SQLServerDatabase sql)
        {
            connection = SQLServerConnector.GetConnection(sql);
        }

        public void Close()
        {
            connection.Close();
        }

        public SQLCommandInterface CreateCommand()
        {
            SqlCommand sqlCommand = connection.CreateCommand();
            return new SQLServerCommand(sqlCommand);
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
