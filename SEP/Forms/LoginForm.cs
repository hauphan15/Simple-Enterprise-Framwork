using DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            txtServerName.Text = "";
            txtDbName.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        private SqlServer sqlServer = null;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            sqlServer = new SqlServer(txtDbName.Text, txtServerName.Text, txtUsername.Text, txtPassword.Text);
            string check = sqlServer.CheckConnection();
            if (string.IsNullOrEmpty(check))
            {
                this.Hide();
                var mainForm = new MainForm(txtServerName.Text, txtDbName.Text, txtUsername.Text, txtPassword.Text);
                mainForm.Show();
            }
            else
            {
                var message = new Message(check);
                message.Show();
            }
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
