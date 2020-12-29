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
    public partial class UpdateForm : BaseForm
    {
        public UpdateForm()
        {
            InitializeComponent();
        }

        public UpdateForm(Table table, SqlServer server) : base(table, server)
        {
            InitializeComponent();
            base.Name = "UpdateForm";
            label1.Text = "Cập Nhật Data Cho Bảng: " + table.tableName;
        }

        protected override int AddorUpdate()
        {
            MessageBox.Show("abc");
            return 1;
        }
    }
}
