using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Voter Login Form
namespace Onlne_Voting_System
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        String ConnectionString = @"Data Source=Moazzam-Laptop;Initial Catalog=""Voting Database"";Integrated Security=True;Pooling=False";
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            Int64 CNIC_Value = Convert.ToInt64(textBox1.Text);
            string query = "Select * from Voters where CNIC = @CNIC_Value";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CNIC_Value", CNIC_Value);
            SqlDataReader dataReader = command.ExecuteReader();

            String Name = string.Empty;
            String CNIC = string.Empty;
            String Password = string.Empty;
            String Constituency = string.Empty;

            int chk = 0;
            while (dataReader.Read())
            {
                Name = dataReader.GetString(1) + " " +  dataReader.GetString(2);
                CNIC = dataReader.GetInt64(3).ToString();
                Password = dataReader.GetString(5);
                Constituency = dataReader.GetString(11);

                if (CNIC == textBox1.Text.ToString() && Password == textBox2.Text.ToString())
                {
                    MessageBox.Show("Login Successful!!!" + "\n\n" + "Welcome " + Name);
                    chk = 1;
                }
            }
            if (chk == 0)
            {
                MessageBox.Show("Invalid CNIC or Password!!!");
            }
            else
            {
                dataReader.Close();
                command.Dispose();
               // MessageBox.Show(Name + "\n" + CNIC + "\n" + Constituency);
                Form6 F6 = new Form6();
                F6.Username = Name;
                F6.CNIC = CNIC;
                F6.Constituency = Constituency;
                textBox1.Text = "";
                textBox2.Text = "";
                F6.Show();
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 F2 = new Form2();
            F2.Show();
            this.Hide();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 F1 = new Form1();
            F1.Show();
            this.Hide();
        }
    }
}
