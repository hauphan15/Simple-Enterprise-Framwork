using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class MySQLDatabase : MyDatabase
    {
        public string database { get; set; }
        public string host { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string port { get; set; }

        private MySqlConnection connection { get; set; }

        private List<Table> tables = new List<Table>();

        public MySQLDatabase(string database, string host, string username, string password, string port)
        {
            this.database = database;
            this.host = host;
            this.username = username;
            this.password = password;
            this.port = port;
            connection = MySQLConnector.GetConnection(this);
        }

        public List<Table> GetTableList()
        {
            return tables;
        }
        public string CheckConnection()
        {
            try
            {
                if (connection == null)
                {
                    return "Err1";
                }
                if (connection.State == ConnectionState.Open)
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

            foreach (DataRow row in ltb.Rows)
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
                MySqlCommand sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                sqlCommand.Parameters.AddWithValue("@tableName", table.tableName);

                using (DbDataReader reader = sqlCommand.ExecuteReader())
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
                MySqlCommand sqlCommand;
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
                    MySqlCommand sqlCommand;
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
            
            foreach (var table in tables)
            {
                string query = "SHOW KEYS FROM " + table.tableName + " WHERE Key_name = 'PRIMARY'";

                MySqlCommand sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                
                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            table.primaryKey = reader.GetString(4);
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
                string query = "select * from " + table.tableName;
                MySqlCommand sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.rows = new List<Row>();
                        while (reader.Read())
                        {
                            Row row = new Row();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[table.lstColumnNames[i]] = reader.GetValue(i).ToString();
                            }
                            table.rows.Add(row);
                        }
                    }
                }
            }
            connection.Close();
        }

        public void ReadDataTable(string tableName)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();

            var table = tables.FirstOrDefault(x => x.tableName == tableName);

            string query = "select * from " + table.tableName;
            MySqlCommand sqlCommand;
            sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = query;
            using (DbDataReader reader = sqlCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    table.rows = new List<Row>();
                    while (reader.Read())
                    {
                        Row row = new Row();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            try
                            {
                                row[table.lstColumnNames[i]] = reader.GetValue(i).ToString();
                            }
                            catch
                            {
                                row[table.lstColumnNames[i]] = "";
                            }                       

                        }
                        table.rows.Add(row);
                    }
                }
            }
            for (int i = 0; i <= tables.Count; i++)
            {
                if (tables[i].tableName == table.tableName)
                {
                    tables[i] = table;
                    break;
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
           
            foreach (var table in tables)
            {
                string query = "show columns from " + table.tableName + " where extra like '%auto_increment%'";

                MySqlCommand sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;

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

        public Table GetTable(string name)
        {
            return tables.FirstOrDefault(a => a.tableName == name);
        }

        public bool InsertData(Dictionary<string, string> values, Table table)
        {
            StringBuilder columnFields = new StringBuilder();
            StringBuilder valuesFields = new StringBuilder();
            columnFields.Append("(");
            valuesFields.Append("(");
            var lstColumn = table.lstColumnNames.Where(x => x != table.AutoIncrementColumnNames).ToList();
            for (int i = 0; i < lstColumn.Count; i++)
            {
                columnFields.Append(lstColumn[i]);
                valuesFields.Append("@param" + i);
                if (i < lstColumn.Count - 1)
                {
                    columnFields.Append(",");
                    valuesFields.Append(",");
                }
            }
            columnFields.Append(")");
            valuesFields.Append(")");
            string query = "Insert into" + " " + table.tableName + " " + columnFields
                + " " + "values" + " " + valuesFields;
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            try
            {
                MySqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                for (int i = 0; i < lstColumn.Count; i++)
                {
                    sqlCommand.Parameters.AddWithValue("@param" + i, values[lstColumn[i]]);
                }
                sqlCommand.ExecuteNonQuery();
            }
            catch
            {
                connection.Close();
                return false;
            }
            connection.Close();

            return true;
        }

        public bool UpdateData(Dictionary<string, object> values, Table table, Dictionary<string, object> oldValues)
        {
            StringBuilder setFields = new StringBuilder();
            StringBuilder whereFields = new StringBuilder();
            var lst = table.lstColumnNames.Where(x => x != table.AutoIncrementColumnNames).ToList();
            var havePrimaryKey = string.IsNullOrEmpty(table.primaryKey);
            for (int i = 0; i < lst.Count; i++)
            {
                setFields.Append(lst[i] + "=@parama" + i + " ");
                if (havePrimaryKey)
                {
                    whereFields.Append(lst[i] + "=@paramb" + i + " ");
                }
                if (i != lst.Count - 1)
                {
                    setFields.Append(" , ");
                    if (havePrimaryKey)
                    {
                        whereFields.Append(" and ");
                    }

                }
                else
                {
                    if (!havePrimaryKey)
                    {
                        whereFields.Append(table.primaryKey + "=@paramb");
                    }
                }
            }
            string query = "update " + table.tableName + " set " + setFields + " where " + whereFields;

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            var s = "";
            try
            {
                MySqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                var count = 0;
                for (int i = 0; i < lst.Count; i++)
                {
                    if (table.typeOfColumns[lst[i]] == "datetime")
                    {
                        var date = values[lst[i]].ToString().Split('/');
                        var date1 = date[2].Split(' ');
                        var datetemp = date1[0] + "/" + date[0] + "/" + date[1];
                        values[lst[i]] = datetemp;
                    }
                    sqlCommand.Parameters.AddWithValue("@parama" + i, values[lst[i]].ToString());
                    if (!havePrimaryKey && count == 0)
                    {
                        count++;
                        sqlCommand.Parameters.AddWithValue("@paramb", oldValues[table.primaryKey].ToString());
                    }
                    else
                    {
                        sqlCommand.Parameters.AddWithValue("@paramb" + i, oldValues[lst[i]].ToString());
                    }
                }
                s = sqlCommand.CommandText.ToString();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                connection.Close();
                return false;
            }
            connection.Close();

            return true;
        }

        //Delete
        public bool DeleteData(Dictionary<string, string> selectedRow, Table table)
        {
            StringBuilder whereFields = new StringBuilder();

            var lst = table.lstColumnNames.ToList();

            for (int i = 0; i < lst.Count; i++)
            {
                whereFields.Append(lst[i] + "=@param" + i + " ");
                if (i != lst.Count - 1)
                {
                    whereFields.Append(" AND ");
                }
            }

            string query = "DELETE FROM " + table.tableName + " WHERE " + whereFields;

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            var s = "";
            try
            {
                MySqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = query;
                for (int i = 0; i < lst.Count; i++)
                {
                    sqlCommand.Parameters.AddWithValue("@param" + i, selectedRow[lst[i]].ToString());
                }
                s = sqlCommand.CommandText.ToString();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                connection.Close();
                return false;
            }
            connection.Close();

            return true;
        }
    }
}
