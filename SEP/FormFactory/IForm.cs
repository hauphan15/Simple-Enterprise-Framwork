using DB;
using Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormFactory
{
    public interface IForm
    {
        Form GetForm(Table table, SqlServer server, Dictionary<string, object> row);
    }

    public class FLogin : IForm
    {
        public Form GetForm(Table table, SqlServer server, Dictionary<string, object> row)
        {
            return new LoginForm();
        }
    }

    public class FMain : IForm
    {

        public Form GetForm(Table table, SqlServer server, Dictionary<string, object> row)
        {
            return new MainForm(server);
        }
    }

    public class FAdd : IForm
    {

        public Form GetForm(Table table, SqlServer server, Dictionary<string, object> row)
        {
            return new AddForm(table, server);
        }
    }

    public class FUpdate : IForm
    {
        public Form GetForm(Table table, SqlServer server, Dictionary<string, object> row)
        {
            return new UpdateForm(table, server, row);
        }
    }
}
