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
    public partial class Form1 : Form
    {
        // Objects
        OleDbConnection Con;
        OleDbDataAdapter Data_Adapter;
        OleDbCommand Cmd;
        OleDbDataReader Data_Reader;
        DataSet Data_Set;

        //Settings
        public string dbfilename = "clock.accdb";
        public string dbtablename = "Workers";
        public Form1()
        {
            InitializeComponent();
        }

        private void enterNum(int Num)
        {
            textBox1.Text += Num.ToString();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
        private void button12_Click(object sender, EventArgs e)
        {
            int WorkerId;
            string WorkerName;
            bool IsWorking;
            DateTime ClockTime,ClockTimeNow = DateTime.Now;
            Con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + dbfilename);
            Data_Adapter = new OleDbDataAdapter("Select * From " + dbtablename + " Where WorkerID = " + textBox1.Text + "", Con);
            textBox1.Text = "";
            Data_Set = new DataSet();
            Con.Open();
            Data_Adapter.Fill(Data_Set);
            if (Data_Set.Tables[0].Rows.Count > 0)
            {
                WorkerName = Data_Set.Tables[0].Rows[0][2].ToString();
                WorkerId = Convert.ToInt16(Data_Set.Tables[0].Rows[0][1]);
                IsWorking = Convert.ToBoolean(Data_Set.Tables[0].Rows[0][3]);
                Con.Close();
            }
            else
            {
                label1.Text = "Wrong ID Number!";
                return;
                Con.Close();
            }
            if (IsWorking)
            {
                RunSQLQuery("Update " + dbtablename + " Set IsWorking = False Where WorkerID = " + WorkerId.ToString() + "");
                Data_Adapter = new OleDbDataAdapter("Select StartTime From SchTimes Where WorkerId=" + WorkerId + " And (StopTime Is NULL)", Con);
                Data_Set = new DataSet();
                Con.Open();
                Data_Adapter.Fill(Data_Set);
                ClockTime = Convert.ToDateTime((Data_Set.Tables[0].Rows[0][0]));
                Con.Close();
                TimeSpan TimeDifference = ClockTimeNow.Subtract(ClockTime);
                label1.Text = "" + WorkerName + " is stopped at " + ClockTimeNow.ToString("HH:mm") + "\n" + TimeDifference.Hours + " hours, " + TimeDifference.Minutes + " minutes";
                RunSQLQuery("Update SchTimes Set WorkTHours = " + (TimeDifference.Hours * 60 + TimeDifference.Minutes) + ", StopTime='" + ClockTimeNow.ToString("dd-MM-yyyy HH:mm:ss") + "' Where WorkerId=" + WorkerId + " And (StopTime Is NULL)");
            }
            else
            {
                RunSQLQuery("Update " + dbtablename + " Set IsWorking = True Where WorkerID = " + WorkerId.ToString() + "");
                RunSQLQuery("INSERT INTO SchTimes (StartTime, WorkerId) Values (\"" + ClockTimeNow.ToString("dd-MM-yyyy HH:mm:ss") + "\"," + WorkerId + ")");
                label1.Text = "" + WorkerName + " is started at " + ClockTimeNow.ToString("HH:mm") + "";
            }
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
        private void button1_Click(object sender, EventArgs e)
        {
            enterNum(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            enterNum(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            enterNum(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            enterNum(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            enterNum(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            enterNum(6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            enterNum(7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            enterNum(8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            enterNum(9);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            enterNum(0);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Form2 settingsForm = new Form2();
            settingsForm.ShowDialog();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
