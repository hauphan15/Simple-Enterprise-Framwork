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
    public partial class AddForm : BaseForm
    {
        public AddForm()
        {
            InitializeComponent();
        }

        public AddForm(Table table) : base(table)
        {
            InitializeComponent();
            base.Name = "Add Form";
            label1.Text = "Thêm Data Vào Bảng: " + table.tableName;
        }
    }
}
