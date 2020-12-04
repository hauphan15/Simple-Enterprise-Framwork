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
            var sr = new SqlServer("SLBH", @".\SQLEXPRESS", "", "");
            sr.GetTableName();
            sr.ReadColumnName();
        }
    }
}
