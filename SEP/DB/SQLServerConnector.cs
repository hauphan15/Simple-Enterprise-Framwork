using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class SQLServerConnector
    {
        private static SqlConnection connection = null;
        private SQLServerConnector() { }
        public static SqlConnection GetConnection(SQLServerDatabase sql)
        {
            connection = MakeConnection(sql);
            return connection;
        }

        private static SqlConnection MakeConnection(SQLServerDatabase sql)
        {
            string connectionString;
            if (string.IsNullOrEmpty(sql.dtSource))
            {
                return null;
            }
            if (string.IsNullOrEmpty(sql.dbName))
            {
                connectionString = @"Data Source=" + sql.dtSource + "; Integrated Security = True";
            }
            else if (string.IsNullOrEmpty(sql.userName) && string.IsNullOrEmpty(sql.password))
            {
                connectionString = @"Data Source=" + sql.dtSource + ";Initial Catalog=" + sql.dbName + "; Integrated Security = True";
            }
            else
            {
                connectionString = @"Data Source=" + sql.dtSource + ";Initial Catalog=" + sql.dbName + ";User ID=" + sql.userName
                    + ";Password=" + sql.password + "; Integrated Security = True";
            }

            return new SqlConnection(connectionString);
        }
    }
}
