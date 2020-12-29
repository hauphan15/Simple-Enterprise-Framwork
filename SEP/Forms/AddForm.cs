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

        public AddForm(Table table, SqlServer server) : base(table, server)
        {
            InitializeComponent();
            base.Name = "Add Form";
            label1.Text = "Thêm Data Vào Bảng: " + table.tableName;
        }

        protected override int AddorUpdate()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            var lst = base.Mytable.lstColumnNames;
            var fieldAuto = base.Mytable.AutoIncrementColumnNames;
            var i = 0;
            foreach(var column in lst)
            {
                if(column != fieldAuto)
                {
                    values.Add(column, getDataTextBox("txt" + i));
                }
                i++;
            }
            if(base.Myserver.InsertData(values, base.Mytable))
            {
                base.ClearTextBox();
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Thêm thất bại, vui lòng kiểm tra lại dữ liệu!", "Thông báo", MessageBoxButtons.OK);
            }
            return 1;
        }

    }
}
