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
    public interface DBConnectionInterface
    {
        ConnectionState GetState();
        void Open();
        void Close();
        DataTable GetSchema(string name);

        SQLCommandInterface CreateCommand();

    }
}
