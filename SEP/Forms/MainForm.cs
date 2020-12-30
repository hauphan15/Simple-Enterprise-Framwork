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


        public MainForm(string server, string databaseName, string username, string password)
        {
            InitializeComponent();

            databaseConnection = new SqlServer(databaseName, server, username, password);

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

        private void button1_Click(object sender, EventArgs e)
        {
            var add = new AddForm(findTable(), databaseConnection);
            add.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var add = new UpdateForm(findTable(), databaseConnection, getCurrentRow());
            add.ShowDialog();
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
