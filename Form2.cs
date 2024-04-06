using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;

//Voter Registration Form
namespace Onlne_Voting_System
{
    public partial class Form2 : Form
    {
        String ConnectionString = @"Data Source=Moazzam-Laptop;Initial Catalog=""Voting Database"";Integrated Security=True;Pooling=False";
        public Form2()
        {
            InitializeComponent();

            String[] Months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            comboBox2.Items.AddRange(Months);

            for (int i = 1; i <= 31; i++)
            {
                comboBox1.Items.Add(i);
            }

            for (int i = 1900; i <= 2005; i++)
            {
                comboBox3.Items.Add(i);
            }

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select Province_Name FROM Provinces", connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox4.Items.Add(reader.GetString(0));
            }
            cmd.Dispose();
            reader.Close();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Enabled = true;
            
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            comboBox7.Items.Clear();
            
            comboBox5.Text = string.Empty;
            comboBox6.Text = string.Empty;
            comboBox7.Text = string.Empty;

            SqlConnection connection = new SqlConnection(ConnectionString);
            int Province_ID_Value = comboBox4.SelectedIndex + 1;
            string query = $"Select Division_Name FROM Divisions WHERE Province_ID = @Province_ID_Value";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Province_ID_Value", Province_ID_Value);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox5.Items.Add(reader.GetString(0));
            }
            connection.Close();
            cmd.Dispose();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox6.Enabled = true;
            comboBox6.Items.Clear();
            comboBox6.Text = string.Empty;
            SqlConnection connection = new SqlConnection(ConnectionString);
            int Divsion_ID_Value = 0;
            if (comboBox4.SelectedIndex == 0)
            {
                Divsion_ID_Value = comboBox5.SelectedIndex + 1;
            }
            else if (comboBox4.SelectedIndex == 1)
            {
                Divsion_ID_Value = comboBox5.SelectedIndex + 2;
            }
            else if (comboBox4.SelectedIndex == 2)
            {
                Divsion_ID_Value = comboBox5.SelectedIndex + 13;
            }
            else if(comboBox4.SelectedIndex == 3)
            {
                Divsion_ID_Value = comboBox5.SelectedIndex + 20;
            }
            else if( comboBox4.SelectedIndex == 4)
            {
                Divsion_ID_Value = comboBox5.SelectedIndex + 27;
            }
            
            string query = $"Select District_Name FROM Districts WHERE Division_ID = @Divsion_ID_Value";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Divsion_ID_Value", Divsion_ID_Value);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox6.Items.Add(reader.GetString(0));
            }
            connection.Close();
            cmd.Dispose();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox7.Enabled = true;
            comboBox7.Items.Clear();
            comboBox7.Text = string.Empty;

            string District_Name_Value = comboBox6.Text.ToString();
            
            SqlConnection connection = new SqlConnection(ConnectionString);
            string query = $"Select District_ID FROM Districts WHERE District_Name = @District_Name_Value";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@District_Name_Value", District_Name_Value);
            connection.Open();
            
            SqlDataReader reader = cmd.ExecuteReader();
            int District_ID_Value = -1;
            while (reader.Read())
            {
                District_ID_Value = reader.GetInt32(0);
            }
            connection.Close();
            cmd.Dispose();
            reader.Close();

            if (District_ID_Value != -1)
            {
                query = $"Select NA_Name FROM NA WHERE District_ID = @District_ID_Value";
                cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@District_ID_Value", District_ID_Value);
                connection.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBox7.Items.Add(reader.GetString(0));
                }
                connection.Close();
                cmd.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
           
            string query = @"Insert into Pending (First_Name, Last_Name, CNIC, Email, Password, DOB, Gender, Province, Division, District, Constituency) Values (@First_Name, @Last_Name, @CNIC, @Email, @Password, @DOB, @Gender, @Province, @Division, @District, @Constituency)";
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@First_Name", textBox1.Text);
            command.Parameters.AddWithValue("@Last_Name", textBox2.Text);
            command.Parameters.AddWithValue("@CNIC", textBox3.Text);
            command.Parameters.AddWithValue("@Email", textBox4.Text);
            command.Parameters.AddWithValue("@Password", textBox5.Text);

            string selectedDay = comboBox1.Text; 
            string selectedMonth = comboBox2.Text; 
            string selectedYear = comboBox3.Text;
            string formattedDate = $"{selectedDay.PadLeft(2, '0')}-{selectedMonth.PadLeft(2, '0')}-{selectedYear}";
            string format = "dd-MMM-yyyy";
            DateTime date = DateTime.ParseExact(formattedDate, format, CultureInfo.InvariantCulture);

            command.Parameters.AddWithValue("@DOB", date.ToString("dd-MMM-yyyy"));

            string gender = string.Empty;

            if (radioButton1.Checked)
            {
                gender = radioButton1.Text;
            }
            else if (radioButton2.Checked)
            {
                gender = radioButton2.Text;
            }

            command.Parameters.AddWithValue("@Gender", gender);
            command.Parameters.AddWithValue("@Province", comboBox4.Text);
            command.Parameters.AddWithValue("@Division", comboBox5.Text);
            command.Parameters.AddWithValue("@District", comboBox6.Text);
            command.Parameters.AddWithValue("@Constituency", comboBox7.Text);
            command.ExecuteNonQuery();

            command.Dispose();
            connection.Close();

            MessageBox.Show("Request sent for Verification");

            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            comboBox1.Text = string.Empty;
            comboBox2.Text = string.Empty;
            comboBox3.Text = string.Empty;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            comboBox4.Text = string.Empty;
            comboBox5.Text = string.Empty;
            comboBox6.Text = string.Empty;
            comboBox7.Text = string.Empty;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 F1 = new Form1();
            F1.Show();
            this.Hide();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form4 F4 = new Form4();
            F4.Show();
            this.Hide();
        }
    }
}