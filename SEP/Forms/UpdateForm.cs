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
        private UpdateForm()
        {
            InitializeComponent();
        }

        public UpdateForm(Table table, DatabaseContext myDatabase, Dictionary<string, object> row) : base(table, myDatabase, row)
        {
            InitializeComponent();
            base.Name = "UpdateForm";
            label1.Text = "Cập Nhật Data Cho Bảng: " + table.tableName;
        }

        protected override int AddorUpdate()
        {
            Dictionary<string, object> values = new Dictionary<string, object>();
            var lst = base.Mytable.lstColumnNames;
            var lstype = base.Mytable.typeOfColumns;
            var fieldAuto = base.Mytable.AutoIncrementColumnNames;
            var i = 0;
            foreach (var column in lst)
            {
                //var type = lstype[column];
                //try
                //{
                //    switch (type.ToLower())
                //    {
                //        case "int32": values.Add(column, Int32.Parse(getDataTextBox("txt" + i))); break;
                //        case "String": values.Add(column, getDataTextBox("txt" + i)); break;
                //        case "datetime": values.Add(column, DateTime.Parse(getDataTextBox("txt" + i))); break;
                //        case "date": values.Add(column, DateTime.Parse(getDataTextBox("txt" + i))); break;
                //        case "boolean": values.Add(column, Boolean.Parse(getDataTextBox("txt" + i))); break;
                //        case "double": values.Add(column, Boolean.Parse(getDataTextBox("txt" + i))); break;
                //        case "byte[]": values.Add(column, Encoding.ASCII.GetBytes(getDataTextBox("txt" + i))); break;
                //        default: values.Add(column, getDataTextBox("txt" + i)); break;
                //    }
                //}
                //catch
                //{
                //    MessageBox.Show("Nhập sai!");
                //    return 0;
                //}
                if (column != fieldAuto)
                {
                    values.Add(column, getDataTextBox("txt" + i));
                }
                i++;
            }
            if (base.MyDatabase.UpdateData(values, base.Mytable, base.Myrow))
            {
                base.ClearTextBox();
                MessageBox.Show("Cập nhập thành công!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Cập nhập thất bại, vui lòng kiểm tra lại dữ liệu!", "Thông báo", MessageBoxButtons.OK);
            }
            return 1;
        }
    }
}
