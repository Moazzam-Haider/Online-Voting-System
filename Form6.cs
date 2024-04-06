using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//Voter Dashboard
namespace Onlne_Voting_System
{
    public partial class Form6 : Form
    {
        String ConnectionString = @"Data Source=Moazzam-Laptop;Initial Catalog=""Voting Database"";Integrated Security=True;Pooling=False";
        public Form6()
        {
            InitializeComponent();
        }

        public String Username = string.Empty;
        public String CNIC = string.Empty;
        public String Constituency = string.Empty;
        public bool HasVoted = false; 

        private void Form6_Load(object sender, EventArgs e)
        {
            textBox1.Text = Username;
            textBox2.Text = CNIC;
            textBox3.Text = Constituency;
            
            string NA_Name_Value = Constituency.ToString();

            SqlConnection connection = new SqlConnection(ConnectionString);
            string query = $"Select * FROM Candidates WHERE NA_Name = @NA_Name_Value";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@NA_Name_Value", NA_Name_Value);
            connection.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (pictureBox1.Image == null)
                {
                    byte[] b1 = (byte[])reader["Picture"];
                    MemoryStream stream = new MemoryStream(b1);
                    Image image = Image.FromStream(stream);
                    pictureBox1.Image = image;

                    label5.Text = reader.GetString(reader.GetOrdinal("Party")).ToString();
                    radioButton1.Text = reader.GetString(reader.GetOrdinal("Candidate_Name")).ToString();
                }
                else if (pictureBox2.Image == null)
                {
                    byte[] b2 = (byte[])reader["Picture"];
                    MemoryStream stream = new MemoryStream(b2);
                    Image image = Image.FromStream(stream);
                    pictureBox2.Image = image;

                    label6.Text = reader.GetString(reader.GetOrdinal("Party")).ToString();
                    radioButton2.Text = reader.GetString(reader.GetOrdinal("Candidate_Name")).ToString();
                }
                else if (pictureBox3.Image == null)
                {
                    byte[] b3 = (byte[])reader["Picture"];
                    MemoryStream stream = new MemoryStream(b3);
                    Image image = Image.FromStream(stream);
                    pictureBox3.Image = image;

                    label7.Text = reader.GetString(reader.GetOrdinal("Party")).ToString();
                    radioButton3.Text = reader.GetString(reader.GetOrdinal("Candidate_Name")).ToString();
                }
                
            }
            connection.Close();
            cmd.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked)
            {
                MessageBox.Show("Vote Registered Successfully!!!");
                HasVoted = true;
            }
            else
            {
                MessageBox.Show("Please Select a Candidate to Vote");
            }
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 F1 = new Form1();
            F1.Show();
            this.Hide();
        }
    }
}
