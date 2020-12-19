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
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
        }
        public BaseForm(Table table)
        {
            InitializeComponent();
            Mytable = table;
            CreateForm();
        }
        protected Table Mytable = null;
        
        private void CreateForm()
        {
            int space = 0;
            int idx = 1;
            foreach(var column in Mytable.lstColumnNames)
            {
                Label label = new Label();
                label.Text = column;
                label.AutoSize = false;
                label.Location = new Point(100, 102 + space);
                label.Size = new Size(200, 13);
                label.TextAlign = ContentAlignment.MiddleRight;

                TextBox textBox = new TextBox();
                textBox.Name = "txt" + idx;
                textBox.Location = new Point(320, 100 + space);
                textBox.Size = new Size(200, 20);

                if(column == Mytable.primaryKey)
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Image = new Bitmap(Application.StartupPath + "\\ICON\\primary-key.png");
                    pictureBox.Location = new Point(525, 100 + space);
                    pictureBox.Size = new Size(20, 20);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.Controls.Add(pictureBox);
                }

                if(column == Mytable.AutoIncrementColumnNames)
                {
                    textBox.Enabled = false;
                }

                var temp = Mytable.listNotNullColumnNames.FirstOrDefault(x => x == column);

                if (temp != null)
                {
                    Label label1 = new Label();
                    label1.Text = "*";
                    label1.AutoSize = false;
                    label1.Location = new Point(301, 102 + space);
                    label1.Size = new Size(10, 13);
                    label1.TextAlign = ContentAlignment.MiddleLeft;
                    label1.ForeColor = Color.Red;
                    this.Controls.Add(label1);
                }

                space += 40;
                idx++;
                this.Controls.Add(label);
                this.Controls.Add(textBox);
            }

            Button btn1 = new Button()
            {
                Name = "btnSubmit",
                Location = new Point(250, 120 + space),
                Size = new Size(100, 30),
                Text = "Submit"
            };
            Button btn2 = new Button()
            {
                Name = "btnCancel",
                Location = new Point(450, 120 + space),
                Size = new Size(100, 30),
                Text = "Cancel"
            };

            Label hidelabel = new Label();
            hidelabel.Location = new Point(0, 150 + space);
            this.Controls.Add(hidelabel);
            this.Controls.Add(btn1);
            this.Controls.Add(btn2);
        }
    }
}
