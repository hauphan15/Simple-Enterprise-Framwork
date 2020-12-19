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
            databaseConnection.ReadData();
            databaseConnection.ReadPrimaryKey();
            databaseConnection.ReadNotNullColumnName();
            databaseConnection.ReadColumnAutoIncrement();

            //thêm các bảng vào combobox
            foreach (var table in databaseConnection.tables)
            {
                cbxTable.Items.Add(table.tableName);
            }

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
            LoadTable(cbxTable.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var add = new AddForm(databaseConnection.tables[0]);
            add.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var add = new UpdateForm(databaseConnection.tables[0]);
            add.ShowDialog();
        }
    }
}
