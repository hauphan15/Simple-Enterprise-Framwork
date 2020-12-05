using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    class Program
    {
        static void Main(string[] args)
        {
            //var sr = new SqlServer("", @".\SQLEXPRESS", "", "");
            var sr = new SqlServer("StudentList", @"DESKTOP-TPBIN8U", "", "");
            sr.GetTableName();
            sr.ReadColumnName();
            sr.ReadColumnType();
            sr.ReadNotNullColumnName();
            sr.ReadPrimaryKey();
        }
    }
}
