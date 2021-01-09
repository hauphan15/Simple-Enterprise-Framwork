using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Row
    {
        private Dictionary<string, string> attributes = new Dictionary<string, string>();
        private static bool bautoAddNew = true;
        public string this[string columnName]
        {
            get
            {
                return getAttribute(columnName);
            }
            set
            {
                setAttribute(columnName, value);
            }
        }

        private void setAttribute(string columnName, string value)
        {
            if (attributes.ContainsKey(columnName))
            {
                attributes[columnName] = value;
            }
            else if (bautoAddNew)
            {
                attributes.Add(columnName, value);
            }
        }

        private string getAttribute(string columnName)
        {
            if (attributes.ContainsKey(columnName))
            {
                return attributes[columnName];
            }
            return null;
        }
    }
}
