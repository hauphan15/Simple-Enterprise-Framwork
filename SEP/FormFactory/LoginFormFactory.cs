using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Forms;

namespace FormFactory
{
    public class LoginFormFactory : IForm
    {
        public Form createForm(string serverName, string databaseName, string username, string password)
        {
            var loginForm = new LoginForm();
            loginForm.Show();
            return loginForm;
        }
    }
}
