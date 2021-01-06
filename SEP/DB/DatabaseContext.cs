using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class DatabaseContext
    {
        public MyDatabase database;

        public DatabaseContext(MyDatabase myDatabase)
        {
            database = myDatabase;//set Strategy
        }

        public List<Table> GetTableList()
        {
            return database.GetTableList();
        }

        public string CheckConnection()
        {
            return database.CheckConnection();
        }
        public void GetTableName()
        {
            database.GetTableName();
        }
        public void ReadColumnName()
        {
            database.ReadColumnName();
        }
        public void ReadNotNullColumnName()
        {
            database.ReadNotNullColumnName();
        }
        public void ReadColumnType()
        {
            database.ReadColumnType();
        }
        public void ReadPrimaryKey()
        {
            database.ReadPrimaryKey();
        }
        public void ReadData()
        {
            database.ReadData();
        }
        public void ReadDataTable(string name)
        {
            database.ReadDataTable(name);
        }
        public void ReadColumnAutoIncrement()
        {
            database.ReadColumnAutoIncrement();
        }
        public Table GetTable(string name)
        {
            return database.GetTable(name);
        }
        public bool InsertData(Dictionary<string, string> values, Table table)
        {
            return database.InsertData(values, table);
        }
        public bool UpdateData(Dictionary<string, object> values, Table table, Dictionary<string, object> oldValues)
        {
            return database.UpdateData(values, table, oldValues);
        }
        public bool DeleteData(Dictionary<string, string> selectedRow, Table table)
        {
            return database.DeleteData(selectedRow, table);
        }
    }
}
