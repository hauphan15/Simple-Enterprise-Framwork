using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.SQLCommand
{
    public interface SQLCommandInterface
    {

        DbDataReader ExecuteReader();
        void AddQuery(string query);
        void AddParameter(string parameterName, object value);
        int ExecuteNonQuery();

        string GetCommandText();


    }
}
