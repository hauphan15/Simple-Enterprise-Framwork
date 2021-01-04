using DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormFactory
{
    public class MyFactory
    {
        public Form ReturnForm(Table table, MyDatabase myDatabase, Dictionary<string, object> row, string formName)
        {
            switch (formName)
            {
                case "login": var form1 = new FLogin(); return form1.GetForm(table, myDatabase, row);
                case "main": var form2 = new FMain(); return form2.GetForm(table, myDatabase, row);
                case "add": var form3 = new FAdd(); return form3.GetForm(table, myDatabase, row);
                case "update": var form4 = new FUpdate(); return form4.GetForm(table, myDatabase, row);
                default: var form5 = new FLogin(); return form5.GetForm(table, myDatabase, row);
            }
        }
    }
}
