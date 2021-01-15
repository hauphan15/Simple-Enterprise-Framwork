using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.SQLCommand
{
    class MySQLCommandAdaptercs : SQLCommandInterface
    {
        MySqlCommand sqlCommand;

        public MySQLCommandAdaptercs(MySqlCommand sqlCommand)
        {
            this.sqlCommand = sqlCommand;
        }
        public void AddParameter(string parameterName, object value)
        {
            sqlCommand.Parameters.AddWithValue(parameterName, value);
        }

        public void AddQuery(string query)
        {
            sqlCommand.CommandText = query;
        }

        public int ExecuteNonQuery()
        {
            return sqlCommand.ExecuteNonQuery();
        }

        public DbDataReader ExecuteReader()
        {
            return sqlCommand.ExecuteReader();
        }

        public string GetCommandText()
        {
            return sqlCommand.CommandText.ToString();
        }
    }
}
