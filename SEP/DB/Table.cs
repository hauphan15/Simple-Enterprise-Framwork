using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Table
    {
        public string tableName { get; set; }
        public string primaryKey { get; set; }

        public List<string> listNotNullColumnNames = new List<string>();
        public string AutoIncrementColumnNames { get; set; }
        public List<string> lstColumnNames = new List<string>();
        public Dictionary<string, string> typeOfColumns = new Dictionary<string, string>();
        //public List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
        public List<Row> rows = new List<Row>();
        public void AddColumnName(string name)
        {
            lstColumnNames.Add(name);
        }

        public void AddNotNullCoumnName(string columnName)
        {
            listNotNullColumnNames.Add(columnName);
        }

        public void AddTypeOfColumn(string columnName, string type)
        {
            typeOfColumns.Add(columnName, type);
        }

    }
}
