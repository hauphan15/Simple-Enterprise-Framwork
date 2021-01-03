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
            if (connection == null)//nếu chưa khởi tạo thì tạo mới
            {
                return MakeConnection(sql);

            }
            return connection;//nếu đã khởi tạo rồi thì trả về connection hiện tại
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

            return connection = new SqlConnection(connectionString);
        }
    }
}
