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
        public List<string> lstColumnNames = new List<string>();
        public Dictionary<string, string> typeOfColumns = new Dictionary<string, string>();
        public List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

        public void AddColumnName(string name)
        {
            lstColumnNames.Add(name);
        }

        public void AddTypeOfColumn(string columnName, string type)
        {
            typeOfColumns.Add(columnName, type);
        }

        public void AddValueOfColumn(string columnName, string value)
        {
            Dictionary<string, string> row = new Dictionary<string, string>
            {
                { columnName, value }
            };
            rows.Add(row);
        }
    }
}
