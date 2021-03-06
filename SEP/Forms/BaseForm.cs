﻿using DB;
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
    public abstract partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
        }

        protected Table Mytable = null;
        protected DatabaseContext MyDatabase = null;
        protected Dictionary<string, object> Myrow = new Dictionary<string, object>();

        public BaseForm(Table table, DatabaseContext myDatabase)
        {
            InitializeComponent();
            Mytable = table;
            MyDatabase = myDatabase;
            CreateForm();
        }
        public BaseForm(Table table, DatabaseContext myDatabase, Dictionary<string, object> row)
        {
            InitializeComponent();
            Mytable = table;
            MyDatabase = myDatabase;
            Myrow = row;
            CreateForm();
        }

        private void CreateForm()
        {
            int space = 0;
            int idx = 0;
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
                    try
                    {
                        pictureBox.Image = new Bitmap(Application.StartupPath + "\\ICON\\primary-key.png");
                    }
                    catch
                    {
                        pictureBox.Image = DrawText("PK", new Font("Arial", 12), Color.Yellow, Color.Gray);
                    }
                    pictureBox.Location = new Point(525, 100 + space);
                    pictureBox.Size = new Size(20, 20);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.Controls.Add(pictureBox);
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

                if (column == Mytable.AutoIncrementColumnNames)
                {
                    textBox.Enabled = false;
                }

                if (Myrow.ContainsKey(column))
                {
                    textBox.Text = Myrow[column].ToString();
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
            btn1.Click += new EventHandler(btn1_click);
            Button btn2 = new Button()
            {
                Name = "btnCancel",
                Location = new Point(450, 120 + space),
                Size = new Size(100, 30),
                Text = "Cancel"
            };
            btn2.Click += new EventHandler(btn2_click);

            Label hidelabel = new Label();
            hidelabel.Location = new Point(0, 150 + space);
            this.Controls.Add(hidelabel);
            this.Controls.Add(btn1);
            this.Controls.Add(btn2);
        }

        private Image DrawText(String text, Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }
        private void btn1_click(object sender, EventArgs e)
        {
           if( CheckisNull())
            {
                AddorUpdate();
            }
        }

        private void btn2_click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool CheckisNull()
        {
            var lst1 = Mytable.AutoIncrementColumnNames;
            var lst2 = Mytable.listNotNullColumnNames;
            var lst3 = Mytable.lstColumnNames;
            for(int i = 0; i<lst3.Count; i++)
            {
                var j = lst2.FirstOrDefault(x => x == lst3[i]);
                if (!string.IsNullOrEmpty(j))
                {
                    var t = getDataTextBox("txt" + i);
                    if (string.IsNullOrEmpty(t))
                    {
                        if(checkTextboxEnable("txt" + i))
                        {
                                                    MessageBox.Show("fields required cant not null!");
                        return false;
                        }
                    }
                }
            }
            return true;
        }

        protected string getDataTextBox(string textboxName)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                    if ((control as TextBox).Name == textboxName)
                    {
                        return control.Text;
                    }
            }
            return "";
        }

        protected bool checkTextboxEnable(string textboxName)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    if ((control as TextBox).Name == textboxName)
                    {
                        return (control as TextBox).Enabled;
                    }
                }

            }
            return true;
        }

        protected void ClearTextBox()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    (control as TextBox).Text = "";
                }

            }
        }
        protected abstract int AddorUpdate();
    }
}
