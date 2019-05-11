using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;  // Access bağlantısı kurabilmek için.
using System.Collections; //ArrayList kullanabilmek için.

namespace ClockInOut
{
    public partial class Form2 : Form
    {
        OleDbConnection Con;
        OleDbDataAdapter Data_Adapter;
        OleDbCommand Cmd;
        DataSet Data_Set;
        public Form2()
        {
            InitializeComponent();
            FillGrid();
            FillGridTimes();
        }

        void FillGrid()
        {
            Con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + Program.appForm.dbfilename); ;
            Data_Adapter = new OleDbDataAdapter("Select * From " + Program.appForm.dbtablename, Con);
            Data_Set = new DataSet();
            Con.Open();
            Data_Adapter.Fill(Data_Set, Program.appForm.dbtablename);
            dataGridView1.DataSource = Data_Set.Tables[Program.appForm.dbtablename];
            Con.Close();
        }

        void FillGridTimes()
        {
            Con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + Program.appForm.dbfilename); ;
            Data_Adapter = new OleDbDataAdapter("Select SchTimes.WorkerId, Name, StartTime, StopTime, WorkTHours From SchTimes Left JOIN " + Program.appForm.dbtablename + " ON SchTimes.WorkerId = " + Program.appForm.dbtablename + ".WorkerID;", Con);
            Data_Set = new DataSet();
            Con.Open();
            Data_Adapter.Fill(Data_Set, "SchTimes");
            dataGridView2.DataSource = Data_Set.Tables["SchTimes"];
            Con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunSQLQuery("INSERT INTO " + Program.appForm.dbtablename + " (WorkerID, Name) Values (" + maskedTextBox1.Text + ",\"" + textBox1.Text + "\")");
            FillGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RunSQLQuery("Update " + Program.appForm.dbtablename + " Set Name ='" + textBox1.Text + "' Where WorkerId = " + maskedTextBox1.Text + "");
            FillGrid();
        }
        public void RunSQLQuery(string query)
        {
            Cmd = new OleDbCommand();
            Con.Open();
            Cmd.Connection = Con;
            Cmd.CommandText = query;
            Cmd.ExecuteNonQuery();
            Con.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            maskedTextBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RunSQLQuery("Delete From " + Program.appForm.dbtablename + " Where WorkerId = " + maskedTextBox1.Text + "");
            FillGrid();
        }
    }
}
