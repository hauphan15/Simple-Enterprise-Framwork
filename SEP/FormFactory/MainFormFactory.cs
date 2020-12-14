using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Forms;

namespace FormFactory
{
    public class MainFormFactory : IForm
    {
        public Form createForm(string serverName, string databaseName, string username, string password)
        {
            var mainForm = new MainForm(serverName, databaseName, username, password);
            mainForm.Show();
            return mainForm;
        }
    }
}
