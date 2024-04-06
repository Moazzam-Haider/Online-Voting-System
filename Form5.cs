using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.OleDb;
using System.Data.SqlTypes;

//Admin Voter Approval Form
namespace Onlne_Voting_System
{
    public partial class Form5 : Form
    {
        String ConnectionString = @"Data Source=Moazzam-Laptop;Initial Catalog=""Voting Database"";Integrated Security=True;Pooling=False";

        DataGridViewButtonColumn button1 = new DataGridViewButtonColumn();
        DataGridViewButtonColumn button2 = new DataGridViewButtonColumn();

        public Form5()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string query = "SELECT * FROM Pending";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;

            button1.HeaderText = "Actions";
            button1.Name = "btn1";
            button1.Text = "Add";
            button1.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(button1);

            button2.HeaderText = "Actions";
            button2.Name = "btn2";
            button2.Text = "Remove";
            button2.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(button2);

        }
        private void Form5_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void AddData(int index)
        {

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            string selectQuery = @"Select * FROM Pending WHERE S_No = @index";
            SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
            selectCommand.Parameters.AddWithValue("@index", index);
            SqlDataReader reader = selectCommand.ExecuteReader();

            string First_Name = string.Empty;
            string Last_Name = string.Empty;
            Int64 CNIC = 0;
            string Email = string.Empty;
            string Password = string.Empty;
            DateTime DOB = DateTime.Now;
            string Gender = string.Empty;
            string Province = string.Empty;
            string Division = string.Empty;
            string District = string.Empty;
            string Constituency = string.Empty;

            while (reader.Read())
            {
                First_Name = reader.GetString(1);
                Last_Name= reader.GetString(2);
                CNIC = reader.GetInt64(3);
                Email = reader.GetString(4);
                Password = reader.GetString(5);
                DOB= reader.GetDateTime(6);
                Gender = reader.GetString(7);
                Province = reader.GetString(8);
                Division = reader.GetString(9);
                District = reader.GetString(10);
                Constituency = reader.GetString(11);
            }

            reader.Close();
            
            string insertQuery = @"Insert into Voters (First_Name, Last_Name, CNIC, Email, Password, DOB, Gender, Province, Division, District, Constituency) Values (@First_Name, @Last_Name, @CNIC, @Email, @Password, @DOB, @Gender, @Province, @Division, @District, @Constituency)";
            SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

            insertCommand.Parameters.AddWithValue("@First_Name", First_Name);
            insertCommand.Parameters.AddWithValue("@Last_Name", Last_Name);
            insertCommand.Parameters.AddWithValue("@CNIC", CNIC);
            insertCommand.Parameters.AddWithValue("@Email", Email);
            insertCommand.Parameters.AddWithValue("@Password", Password);
            insertCommand.Parameters.AddWithValue("@DOB", DOB.Date);
            insertCommand.Parameters.AddWithValue("@Gender", Gender);
            insertCommand.Parameters.AddWithValue("@Province", Province);
            insertCommand.Parameters.AddWithValue("@Division", Division);
            insertCommand.Parameters.AddWithValue("@District", District);
            insertCommand.Parameters.AddWithValue("@Constituency", Constituency);
            insertCommand.ExecuteNonQuery();

            selectCommand.Dispose();
            insertCommand.Dispose();
            connection.Close();
            MessageBox.Show("Voter Approved!!!");
            DeleteData(index);
        }

        private void DeleteData(int index)
        {

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            string deleteQuery = @"Delete FROM Pending WHERE S_No = @index";
            SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
            deleteCommand.Parameters.AddWithValue("@index", index);
            deleteCommand.ExecuteNonQuery();

            deleteCommand.Dispose();
            connection.Close();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string col = "S_No";
                DataGridViewCell cell = dataGridView1.CurrentRow.Cells[col];
                object cellValue = cell.Value;
                int index = Convert.ToInt32(cellValue);

                if (e.ColumnIndex == dataGridView1.Columns[button1.Index].Index)
                {
                    AddData(index);
                    MessageBox.Show("Voter Approved");
                }
                else if (e.ColumnIndex == dataGridView1.Columns[button2.Index].Index)
                {
                    DeleteData(index);
                    MessageBox.Show("Voter Rejected");
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 F1 = new Form1();
            F1.Show();
            this.Hide();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form7 F7 = new Form7();
            F7.Show();
            this.Hide();
        }
    }
}
