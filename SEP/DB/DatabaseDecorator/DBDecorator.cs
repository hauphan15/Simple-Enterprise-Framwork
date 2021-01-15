using DB.DatabaseConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.DatabaseDecorator
{
    public class DBDecorator : MyDatabase
    {
        public DatabaseConcreteComponent coreDatabase;

        public DBConnectionInterface connection;

        public List<Table> tables = new List<Table>();


        public string CheckConnection()
        {
            return coreDatabase.CheckConnection();
        }

        public bool DeleteData(Dictionary<string, string> selectedRow, Table table)
        {
            return coreDatabase.DeleteData(selectedRow, table);
        }

        public Table GetTable(string name)
        {
            return coreDatabase.GetTable(name);
        }

        public List<Table> GetTableList()
        {
            return coreDatabase.GetTableList();
        }

        public void GetTableName()
        {
            coreDatabase.GetTableName();
        }

        public bool InsertData(Dictionary<string, string> values, Table table)
        {
            return coreDatabase.InsertData(values, table);
        }

        public virtual void ReadColumnAutoIncrement()
        {
            throw new NotImplementedException();
        }

        public void ReadColumnName()
        {
            coreDatabase.ReadColumnName();
        }

        public void ReadColumnType()
        {
            coreDatabase.ReadColumnType();
        }

        public void ReadData()
        {
            coreDatabase.ReadData();
        }

        public void ReadDataTable(string tableName)
        {
            coreDatabase.ReadDataTable(tableName);
        }

        public void ReadNotNullColumnName()
        {
            coreDatabase.ReadNotNullColumnName();
        }

        public virtual void ReadPrimaryKey()
        {
            throw new NotImplementedException();
        }

        public bool UpdateData(Dictionary<string, object> values, Table table, Dictionary<string, object> oldValues)
        {
            return coreDatabase.UpdateData(values, table, oldValues);
        }
    }
}
