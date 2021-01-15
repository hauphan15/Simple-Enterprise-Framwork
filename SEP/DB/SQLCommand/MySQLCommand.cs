using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.SQLCommand
{
    class MySQLCommand : SQLCommandInterface
    {
        MySqlCommand sqlCommand;

        public MySQLCommand(MySqlCommand sqlCommand)
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

        public void ExecuteNonQuery()
        {
            sqlCommand.ExecuteNonQuery();
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
