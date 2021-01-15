using DB.DatabaseConnection;
using DB.SQLCommand;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DatabaseDecorator
{
    public class DatabaseConcreteComponent : MyDatabase
    {

        public DBConnectionInterface connection { get; set; }

        public List<Table> tables = new List<Table>();

        public DatabaseConcreteComponent(ref DBConnectionInterface connection, ref List<Table> tables)
        {
            this.connection = connection;
            this.tables = tables;
        }

        public string CheckConnection()
        {
            try
            {
                if (connection == null)
                {
                    return "Err1";
                }
                if (connection.GetState() == ConnectionState.Open)
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

        public bool DeleteData(Dictionary<string, string> selectedRow, Table table)
        {
            StringBuilder whereFields = new StringBuilder();

            var lst = table.lstColumnNames.ToList();
            if (string.IsNullOrEmpty(table.primaryKey))
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    whereFields.Append(lst[i] + "=@param" + i + " ");
                    if (i != lst.Count - 1)
                    {
                        whereFields.Append(" AND ");
                    }
                }
            }
            else
            {
                whereFields.Append(table.primaryKey + "=@param0");
            }
            string query = "DELETE FROM " + table.tableName + " WHERE " + whereFields;

            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            var s = "";
            try
            {
                SQLCommandInterface sqlCommand = connection.CreateCommand();
                sqlCommand.AddQuery(query);
                for (int i = 0; i < lst.Count; i++)
                {
                    sqlCommand.AddParameter("@param" + i, selectedRow[lst[i]].ToString());
                }
                s = sqlCommand.GetCommandText();
                var result = sqlCommand.ExecuteNonQuery();
                connection.Close();
                if (result != 0)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                connection.Close();
                return false;
            }

            return true;
        }

        public Table GetTable(string name)
        {
            return tables.FirstOrDefault(a => a.tableName == name);
        }

        public List<Table> GetTableList()
        {
            return tables;
        }

        public void GetTableName()
        {
            if (connection.GetState() == ConnectionState.Open)
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
            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            try
            {
                SQLCommandInterface sqlCommand = connection.CreateCommand();
                sqlCommand.AddQuery(query);
                for (int i = 0; i < lstColumn.Count; i++)
                {
                    if (table.typeOfColumns[lstColumn[i]] == "datetime" || table.typeOfColumns[lstColumn[i]] == "date")
                    {
                        var date = values[lstColumn[i]].ToString().Split('/');
                        var date1 = date[2].Split(' ');
                        var datetemp = date1[0] + "/" + date[0] + "/" + date[1];
                        values[lstColumn[i]] = datetemp;
                    }
                    //sqlCommand.Parameters.AddWithValue("@param" + i, values[lstColumn[i]]);
                    sqlCommand.AddParameter("@param" + i, values[lstColumn[i]]);
                }
                var result = sqlCommand.ExecuteNonQuery();
                connection.Close();
                if (result != 0)
                    return true;
                return false;
            }
            catch
            {
                connection.Close();
                return false;
            }

            return true;
        }

        public virtual void ReadColumnAutoIncrement()
        {
            throw new NotImplementedException();
        }

        public void ReadColumnName()
        {
            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            string query = "SELECT column_name as 'Column Name' FROM information_schema.columns WHERE table_name =  @tableName";
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
                            table.AddColumnName(reader.GetString(0));
                        }
                    }
                }
            }
            connection.Close();
        }

        public void ReadColumnType()
        {
            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            string query = "SELECT data_type as 'DATA_TYPE' FROM information_schema.columns WHERE table_name =  @tableName AND column_name = @field";
            foreach (var table in tables)
            {
                foreach (var column in table.lstColumnNames)
                {
                    SQLCommandInterface sqlCommand;
                    sqlCommand = connection.CreateCommand();
                    sqlCommand.AddQuery(query);
                    sqlCommand.AddParameter("@tableName", table.tableName);
                    sqlCommand.AddParameter("@field", column);
                    //sqlCommand.Parameters.AddWithValue("@tableName", table.tableName);
                    //sqlCommand.Parameters.AddWithValue("@field", column);

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

        public void ReadData()
        {
            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();

            foreach (var table in tables)
            {
                string query = "select * from " + table.tableName;
                SQLCommandInterface sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.AddQuery(query);

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
            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();

            var table = tables.FirstOrDefault(x => x.tableName == tableName);

            string query = "select * from " + table.tableName;
            SQLCommandInterface sqlCommand;
            sqlCommand = connection.CreateCommand();
            sqlCommand.AddQuery(query);
            using (DbDataReader reader = sqlCommand.ExecuteReader())
            {
                table.rows = new List<Row>();
                if (reader.HasRows)
                {
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

        public void ReadNotNullColumnName()
        {
            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            string query = "SELECT TABLE_CATALOG AS TABLE_NAME, COLUMN_NAME, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND IS_NULLABLE = 'NO'";
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
                            table.AddNotNullCoumnName(reader.GetString(1));
                        }
                    }
                }
            }
            connection.Close();
        }

        public virtual void ReadPrimaryKey()
        {
            throw new NotImplementedException();
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

            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            var s = "";
            try
            {
                SQLCommandInterface sqlCommand = connection.CreateCommand();
                sqlCommand.AddQuery(query);
                var count = 0;
                for (int i = 0; i < lst.Count; i++)
                {
                    if (table.typeOfColumns[lst[i]] == "datetime" || table.typeOfColumns[lst[i]] == "date")
                    {
                        var date = values[lst[i]].ToString().Split('/');
                        var date1 = date[2].Split(' ');
                        var datetemp = date1[0] + "/" + date[0] + "/" + date[1];
                        values[lst[i]] = datetemp;
                    }
                    sqlCommand.AddParameter("@parama" + i, values[lst[i]].ToString());
                    //sqlCommand.Parameters.AddWithValue("@parama" + i, values[lst[i]].ToString());
                    if (!havePrimaryKey && count == 0)
                    {
                        count++;
                        sqlCommand.AddParameter("@paramb", oldValues[table.primaryKey].ToString());
                        //sqlCommand.Parameters.AddWithValue("@paramb", oldValues[table.primaryKey].ToString());
                    }
                    else
                    {
                        sqlCommand.AddParameter("@paramb" + i, oldValues[lst[i]].ToString());
                        //sqlCommand.Parameters.AddWithValue("@paramb" + i, oldValues[lst[i]].ToString());
                    }
                }
                s = sqlCommand.GetCommandText();
                var result = sqlCommand.ExecuteNonQuery();
                connection.Close();
                if (result != 0)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                connection.Close();
                return false;
            }

            return true;
        }
    }
}
