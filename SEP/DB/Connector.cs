using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Connector
    {
        private static SqlConnection connection = null;
        private Connector() { }
        public static SqlConnection GetConnection(SqlServer sql)
        {
                if (!MakeConnection(sql))
                {
                    return null;
                }
            return connection;
        }

        private static bool MakeConnection(SqlServer sql)
        {
            string connectionString;
            if (string.IsNullOrEmpty(sql.dtSource))
            {
                return false;
            }
            if (string.IsNullOrEmpty(sql.dbName))
            {
                connectionString = @"Data Source=" + sql.dtSource + "; Integrated Security = True";
            }
            else if(string.IsNullOrEmpty(sql.userName) && string.IsNullOrEmpty(sql.password))
            {
                connectionString = @"Data Source=" + sql.dtSource + ";Initial Catalog=" + sql.dbName + "; Integrated Security = True";
            }
            else
            {
                connectionString = @"Data Source=" + sql.dtSource + ";Initial Catalog=" + sql.dbName + ";User ID=" + sql.userName 
                    + ";Password=" + sql.password + "; Integrated Security = True";
            }
            try
            {
                connection = new SqlConnection(connectionString);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
