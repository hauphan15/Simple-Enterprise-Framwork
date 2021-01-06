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
            cbxDatabaseType.Items.Add("SQLServer");
            cbxDatabaseType.Items.Add("MySQL");
            cbxDatabaseType.Text = "SQLServer";
        }

        private DatabaseContext databaseContext = null;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cbxDatabaseType.Text == "SQLServer")
            {
                databaseContext = new DatabaseContext(new SQLServerDatabase(txtServerName.Text, txtDbName.Text, txtUsername.Text, txtPassword.Text));
            }
            else if (cbxDatabaseType.Text == "MySQL")
            {
                databaseContext = new DatabaseContext(new MySQLDatabase(txtDbName.Text, txtServerName.Text, txtUsername.Text, txtPassword.Text));
            }

            string check = databaseContext.CheckConnection();

            if (string.IsNullOrEmpty(check))
            {
                this.Hide();
                var mainForm = new MainForm(databaseContext);
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

        private void cbxDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDatabaseType.Text == "SQLServer")
            {
                lbServerName.Visible = true;
                lbHost.Visible = false;
            }
            if (cbxDatabaseType.Text == "MySQL")
            {
                lbServerName.Visible = false;
                lbHost.Visible = true;
            }
        }
    }
}
