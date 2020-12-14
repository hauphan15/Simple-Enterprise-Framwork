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
         Form createForm(string serverName, string databaseName, string username, string password);
    }
}
