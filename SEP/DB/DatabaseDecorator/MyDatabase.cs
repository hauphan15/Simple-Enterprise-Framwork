using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public interface MyDatabase
    {
        List<Table> GetTableList();
        string CheckConnection();
        void GetTableName();
        void ReadColumnName();
        void ReadNotNullColumnName();
        void ReadColumnType();
        void ReadPrimaryKey();
        void ReadData();
        void ReadDataTable(string tableName);
        void ReadColumnAutoIncrement();
        Table GetTable(string name);
        bool InsertData(Dictionary<string, string> values, Table table);
        bool UpdateData(Dictionary<string, object> values, Table table, Dictionary<string, object> oldValues);
        bool DeleteData(Dictionary<string, string> selectedRow, Table table);
    }
}
