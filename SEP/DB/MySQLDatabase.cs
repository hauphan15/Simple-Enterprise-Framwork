using DB.DatabaseConnection;
using DB.DatabaseDecorator;
using DB.SQLCommand;
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
    public class MySQLDatabase : DBDecorator
    {
        public string database { get; set; }
        public string host { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string port { get; set; }


        public MySQLDatabase(string database, string host, string username, string password, string port)
        {
            this.database = database;
            this.host = host;
            this.username = username;
            this.password = password;
            this.port = port;
            connection = new MySQLConnection(this);
            coreDatabase = new DatabaseConcreteComponent(ref connection, ref tables);
        }

        public override void ReadPrimaryKey()
        {
            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();

            foreach (var table in tables)
            {
                string query = "SHOW KEYS FROM " + table.tableName + " WHERE Key_name = 'PRIMARY'";

                SQLCommandInterface sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.AddQuery(query);

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

        public override void ReadColumnAutoIncrement()
        {
            if (connection.GetState() == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();

            foreach (var table in tables)
            {
                string query = "show columns from " + table.tableName + " where extra like '%auto_increment%'";

                SQLCommandInterface sqlCommand;
                sqlCommand = connection.CreateCommand();
                sqlCommand.AddQuery(query);

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
