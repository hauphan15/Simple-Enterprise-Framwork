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
    public partial class Message : Form
    {
        public Message(string ErrorCode)
        {
            InitializeComponent();
            this.richTextBox1.Text = getErrorMessage(ErrorCode);
        }

        private string getErrorMessage(string code)
        {
            switch (code)
            {
                case "Err1": return "Cant make connection!";
                default: return "Something error!";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
