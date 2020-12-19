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

        public string CheckConnection()
        {
            try
            {
                if(connection == null)
                {
                    return "Err1";
                }
                if(connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                connection.Close();
            }
            catch
            {
                return "Err1";
            }
            return null;
        }

        public void GetTableName()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
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
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
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

        public void ReadNotNullColumnName()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            string query = "SELECT TABLE_CATALOG AS TABLE_NAME, COLUMN_NAME, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND IS_NULLABLE = 'NO'";
            foreach (var table in tables)
            {
                SqlCommand sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@tableName", table.tableName);

                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            table.AddNotNullCoumnName(reader.GetString(1));
                        }
                    }
                }
            }
            connection.Close();
        }
        public void ReadColumnType()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            string query = "SELECT data_type as 'DATA_TYPE' FROM information_schema.columns WHERE table_name =  @tableName AND column_name = @field";
            foreach (var table in tables)
            {
                foreach (var column in table.lstColumnNames)
                {
                    SqlCommand sqlCommand;
                    sqlCommand = connection.CreateCommand();
                    sqlCommand.CommandText = query;
                    sqlCommand.Parameters.AddWithValue("@tableName", table.tableName);
                    sqlCommand.Parameters.AddWithValue("@field", column);

                    using (DbDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                table.AddTypeOfColumn(column, reader.GetString(0));
                            }
                        }
                    }
                }
            }
            connection.Close();
        }

        public void ReadPrimaryKey()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            string query = "SELECT Col.Column_Name from INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col WHERE Col.Constraint_Name = Tab.Constraint_Name AND Col.Table_Name = Tab.Table_Name AND Constraint_Type = 'PRIMARY KEY' AND Col.Table_Name =  @tableName";
            foreach (var table in tables)
            {
                SqlCommand sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@tableName", table.tableName);

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

        public void ReadData()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            
            foreach (var table in tables)
            {
                string query = "select * from " +table.tableName;
                SqlCommand sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                
                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, string> record = new Dictionary<string, string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                record.Add(table.lstColumnNames[i], reader.GetValue(i).ToString());
                            }
                            table.rows.Add(record);
                        }
                    }
                }
            }
            connection.Close();
        }

        public void ReadColumnAutoIncrement()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            string query = "SELECT name FROM sys.identity_columns " + "WHERE object_id = OBJECT_ID(@tableName)";
            foreach (var table in tables)
            {
                SqlCommand sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@tableName", table.tableName);

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
