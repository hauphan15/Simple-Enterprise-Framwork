using DB.DatabaseConnection;
using DB.DatabaseDecorator;
using DB.SQLCommand;
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
    public class SQLServerDatabase : DBDecorator
    {
        public string dbName { get; set; }
        public string dtSource { get; set; }
        public string userName { get; set; }
        public string password { get; set; }

        public SQLServerDatabase(string dbName, string dtSource, string userName, string password)
        {
            this.dbName = dbName;
            this.dtSource = dtSource;
            this.userName = userName;
            this.password = password;
            connection = new SQLServerConnection(this);
            coreDatabase = new DatabaseConcreteComponent(ref connection, ref tables);
        }


        public override void ReadPrimaryKey()
        {
            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            string query = "SELECT Col.Column_Name from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col WHERE Col.Constraint_Name = Tab.Constraint_Name AND Col.Table_Name = Tab.Table_Name AND Constraint_Type = 'PRIMARY KEY' AND Col.Table_Name =  @tableName";
            foreach (var table in tables)
            {
                SQLCommandInterface sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.AddQuery(query);
                sqlCommand.AddParameter("@tableName", table.tableName);
                //sqlCommand.Parameters.AddWithValue("@tableName", table.tableName);

                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            table.primaryKey = reader.GetString(0);
                        }
                    }
                }
            }
            connection.Close();
        }

        public override void ReadColumnAutoIncrement()
        {
            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            string query = "SELECT name FROM sys.identity_columns " + "WHERE object_id = OBJECT_ID(@tableName)";
            foreach (var table in tables)
            {
                SQLCommandInterface sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.AddQuery(query);
                sqlCommand.AddParameter("@tableName", table.tableName);
                //sqlCommand.Parameters.AddWithValue("@tableName", table.tableName);

                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            table.AutoIncrementColumnNames = reader.GetString(0);
                        }
                    }
                }
            }
            connection.Close();
        }
    }
}
