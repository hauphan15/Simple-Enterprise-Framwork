using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class SqlServer
    {
        public string dbName { get; set; }
        public string dtSource { get; set; }
        public string userName { get; set; }
        public string password { get; set; }

        private SqlConnection connection { get; set; }

        public List<Table> tables = new List<Table>();

        public SqlServer(string dbName, string dtSource, string userName, string password)
        {
            this.dbName = dbName;
            this.dtSource = dtSource;
            this.userName = userName;
            this.password = password;
            connection = Connector.GetConnection(this);
        }

        public void GetTableName()
        {
            connection.Open();
            DataTable ltb = connection.GetSchema("Tables");

            foreach(DataRow row in ltb.Rows)
            {
                var tb = new Table
                {
                    tableName = row[2].ToString()
                };
                tables.Add(tb);
            }
            connection.Close();
        }

        public void ReadColumnName()
        {
            connection.Open();
            string query = "SELECT column_name as 'Column Name' FROM information_schema.columns WHERE table_name =  @tableName";
            foreach (var table in tables)
            {
                SqlCommand sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@tableName", table.tableName);

                using(DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            table.AddColumnName(reader.GetString(0));
                        }
                    }
                }
            }
            connection.Close();
        }
    }
}
