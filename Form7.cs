using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//Admin Candidate Selection Form
namespace Onlne_Voting_System
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        String ConnectionString = @"Data Source=Moazzam-Laptop;Initial Catalog=""Voting Database"";Integrated Security=True;Pooling=False";
        private void Form7_Load(object sender, EventArgs e)
        {
            String[] Parties = { "PTI", "PMLN", "PPP" };
            String[] Candidates = { "Imran Khan", "Nawaz Sharif", "Asif Zardari" };
            comboBox2.Items.AddRange(Candidates);
            comboBox3.Items.AddRange(Parties);

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select NA_Name FROM NA", connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
            }
            cmd.Dispose();
            reader.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form5 F5 = new Form5();
            F5.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 F1 = new Form1();
            F1.Show();
            this.Hide();
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(fileDialog.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            
            String NA_Name_Value = comboBox1.Text.ToString();
            String SelectQuery = @"Select NA_ID FROM NA WHERE NA_Name = @NA_Name_Value";
            SqlCommand SelectCommand = new SqlCommand(SelectQuery, connection);
            
            SelectCommand.Parameters.AddWithValue("@NA_Name_Value", NA_Name_Value);
            SqlDataReader reader = SelectCommand.ExecuteReader();
            int NA_ID_Value = -1;
            while (reader.Read())
            {
                NA_ID_Value = reader.GetInt32(0);
            }
            connection.Close();
            SelectCommand.Dispose();
            reader.Close();

            SqlCommand cmd = new SqlCommand("Insert into Candidates Values (@NA_ID, @Candidate_Name, @Party, @Picture, @NA_Name)", connection);
            connection.Open();
            cmd.Parameters.AddWithValue("@NA_ID", NA_ID_Value);
            cmd.Parameters.AddWithValue("@Candidate_Name", comboBox2.Text);
            cmd.Parameters.AddWithValue("@Picture", ImageToByteArray(pictureBox1.Image));
            cmd.Parameters.AddWithValue("@Party", comboBox3.Text);
            cmd.Parameters.AddWithValue("@NA_Name", comboBox1.Text);
            cmd.ExecuteNonQuery();

            connection.Close();
            MessageBox.Show("Candidate Added Successfully!!!");

            pictureBox1.Image = null;
            comboBox1.Text = string.Empty; comboBox2.Text = string.Empty; comboBox3.Text = string.Empty;
        }
    }
}
