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
            DatabaseContext database = new DatabaseContext(new MySQLDatabase("internetbanking", "localhost", "root", ""));
            database.GetTableName();
            database.ReadColumnType();
            database.ReadColumnName();
            database.ReadData();
        }
    }
}
