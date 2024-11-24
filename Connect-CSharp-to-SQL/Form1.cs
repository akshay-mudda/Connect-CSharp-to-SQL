using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Connect_CSharp_to_SQL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve the connection string dynamically from app.config
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString;

                // Validate that input fields are not empty
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("All fields are required. Please fill in all the inputs.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if the data already exists in the table
                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM [Table] WHERE ID = @ID AND Column1 = @Column1 AND Column2 = @Column2", con))
                    {
                        checkCmd.Parameters.AddWithValue("@ID", int.Parse(textBox1.Text));
                        checkCmd.Parameters.AddWithValue("@Column1", textBox2.Text);
                        checkCmd.Parameters.AddWithValue("@Column2", textBox3.Text);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("The input data already exists in the database.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert the data if it doesn't exist
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [Table] VALUES (@ID, @Column1, @Column2)", con))
                    {
                        cmd.Parameters.AddWithValue("@ID", int.Parse(textBox1.Text));
                        cmd.Parameters.AddWithValue("@Column1", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Column2", textBox3.Text);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Data successfully saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid number in the ID field.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString;

                // Validate that input fields are not empty
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("All fields are required. Please fill in all the inputs.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Update [Table] set Column1=@Column1,Column2=@Column2 where ID=@ID", con))
                    {
                        cmd.Parameters.AddWithValue("@ID", int.Parse(textBox1.Text));
                        cmd.Parameters.AddWithValue("@Column1", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Column2", textBox3.Text);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                MessageBox.Show("Successfully Updated");

                // Validate that input fields are not empty
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("All fields are required. Please fill in all the inputs.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid number in the ID field.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Delete [Table] where ID=@ID", con))
                    {
                        cmd.Parameters.AddWithValue("@ID", int.Parse(textBox1.Text));
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                MessageBox.Show("Successfully Deleted");
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid number in the ID field.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from [Table]", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            textBox1.Focus();
        }
    }
}
