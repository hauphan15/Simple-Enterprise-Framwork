using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DB;

namespace Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private SqlServer databaseConnection = null;


        public MainForm(SqlServer server)
        {
            InitializeComponent();

            databaseConnection = server;

            databaseConnection.GetTableName();
            databaseConnection.ReadColumnName();
            databaseConnection.ReadColumnType();
            databaseConnection.ReadPrimaryKey();
            databaseConnection.ReadNotNullColumnName();
            databaseConnection.ReadColumnAutoIncrement();
            databaseConnection.ReadDataTable(databaseConnection.tables[0].tableName);

            //thêm các bảng vào combobox
            foreach (var table in databaseConnection.tables)
            {
                cbxTable.Items.Add(table.tableName);
            }
            cbxTable.SelectedIndex = 0;
            //mặc định chạy lên là load table[0]
            LoadTable(databaseConnection.tables[0].tableName);
        }

        private void LoadTable(string tableName)
        {
            gridView.Rows.Clear();
            gridView.Columns.Clear();
            gridView.Refresh();

            Table selectedTable = new Table();
            foreach (var table in databaseConnection.tables)
            {
                if (table.tableName == tableName)
                {
                    selectedTable = table;
                }
            }

            //Load cột
            foreach (var column in selectedTable.lstColumnNames)
            {
                gridView.Columns.Add(column, column);
            }

            //Load dòng
            foreach (var dictionary in selectedTable.rows) // mỗi row là 1 dictionary
            {
                int rowIndex = this.gridView.Rows.Add();
                var newRow = this.gridView.Rows[rowIndex];
                int idx = 0;

                foreach (KeyValuePair<string, string> entry in dictionary)
                {
                    newRow.Cells[idx].Value = entry.Value;
                    idx++;
                }
            }

            //Orther properties
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //autosize
            gridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9.75F, FontStyle.Bold); // font
            gridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Black; // column
            gridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridView.EnableHeadersVisualStyles = false;

        }

        private void cbxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            databaseConnection.ReadDataTable(cbxTable.Text);
            LoadTable(cbxTable.Text);
        }

        //add button
        private void button1_Click(object sender, EventArgs e)
        {
            var add = new AddForm(findTable(), databaseConnection);
            add.ShowDialog();
            databaseConnection.ReadDataTable(cbxTable.Text);
            LoadTable(cbxTable.Text);
        }

        //update button
        private void button2_Click(object sender, EventArgs e)
        {
            var update = new UpdateForm(findTable(), databaseConnection, getCurrentRow());
            update.ShowDialog();
            databaseConnection.ReadDataTable(cbxTable.Text);
            LoadTable(cbxTable.Text);
        }

        //delete button
        private void button3_Click(object sender, EventArgs e)
        {

            var res = MessageBox.Show("Bạn có muốn xóa dòng dữ liệu hiện tại?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (res == DialogResult.OK)
            {
                Dictionary<string, string> selectedRow = new Dictionary<string, string>();

                var lst = findTable().lstColumnNames;

                var i = 0;
                foreach (var column in lst)
                {
                    string value = gridView.Rows[gridView.CurrentRow.Index].Cells[i].Value.ToString();//get value of each cell in row
                    selectedRow.Add(column, value); //add to dictionary: columnName - value
                    i++;
                }

                var delete = databaseConnection.DeleteData(selectedRow, findTable());

                if (delete)
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButtons.OK);
                }

                //refresh datagrid vỉew
                databaseConnection.ReadDataTable(cbxTable.Text);
                LoadTable(cbxTable.Text);
            }
        }

        private Table findTable()
        {
            return databaseConnection.tables.FirstOrDefault(x => x.tableName == cbxTable.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            databaseConnection.ReadDataTable(cbxTable.Text);
            LoadTable(cbxTable.Text);
        }

        private Dictionary<string, object> getCurrentRow()
        {
            var selRow = gridView.CurrentCell.RowIndex;
            //MessageBox.Show(selRow.ToString());
            var columns = databaseConnection.GetTable(cbxTable.Text).lstColumnNames;
            var row = gridView.Rows[selRow];
            Dictionary<string, object> obj = new Dictionary<string, object>();
            for (int i = 0; i < columns.Count; i++)
            {
                obj.Add(columns[i], row.Cells[i].Value);
            }
            return obj;
        }

    }
}
