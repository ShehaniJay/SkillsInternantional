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

namespace SkillsInternantional
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
  
        
        }
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@gmail\.com$"; 
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }
        private bool IsValidDOB(DateTime dob)
        {
            return dob <= DateTime.Now; 
        }
        private void comboBoxRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-FVJPNHV\\SQLEXPRESS;Initial Catalog=Student;Integrated Security=True";
            string query = "SELECT * FROM Registration WHERE regNo = @regNo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@regNo", comboBoxRegNo.Text);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    textBoxFirstName.Text = reader["firstName"].ToString();
                    textBoxLastName.Text = reader["lastName"].ToString();
                    dateTimePickerDOB.Value = Convert.ToDateTime(reader["dateOfBirth"]);
                    if (reader["gender"].ToString() == "Male")
                        radioButtonMale.Checked = true;
                    else
                        radioButtonFemale.Checked = true;
                    textBoxAddress.Text = reader["address"].ToString();
                    textBoxEmail.Text = reader["email"].ToString();
                    textBoxMobilePhone.Text = reader["mobilePhone"].ToString();
                    textBoxHomePhone.Text = reader["homePhone"].ToString();
                    textBoxParentName.Text = reader["parentName"].ToString();
                    textBoxNIC.Text = reader["nic"].ToString();
                    textBoxContactNo.Text = reader["contactNo"].ToString();
                }
            }

        }

        private void labelTitle_Click(object sender, EventArgs e)
        {

        }

        private void labelMobilePhone_Click(object sender, EventArgs e)
        {

        }

        private void labelHomePhone_Click(object sender, EventArgs e)
        {

        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFirstName.Text) || string.IsNullOrEmpty(textBoxLastName.Text) ||
      string.IsNullOrEmpty(textBoxAddress.Text) || string.IsNullOrEmpty(textBoxEmail.Text) ||
      string.IsNullOrEmpty(textBoxMobilePhone.Text) || string.IsNullOrEmpty(textBoxParentName.Text) ||
      string.IsNullOrEmpty(textBoxNIC.Text) || string.IsNullOrEmpty(textBoxContactNo.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           
            if (!IsValidEmail(textBoxEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxEmail.Focus();
                return; 
            }

            if (!IsValidDOB(dateTimePickerDOB.Value))
            {
                MessageBox.Show("Date of Birth cannot be in the future.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string connectionString = "Data Source=DESKTOP-FVJPNHV\\SQLEXPRESS;Initial Catalog=Student;Integrated Security=True";

            string query = "INSERT INTO Registration (regNo, firstName, lastName, dateOfBirth, gender, address, email, mobilePhone, homePhone, parentName, nic, contactNo) " +
                           "VALUES (@regNo, @firstName, @lastName, @dateOfBirth, @gender, @address, @email, @mobilePhone, @homePhone, @parentName, @nic, @contactNo)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                
                cmd.Parameters.AddWithValue("@regNo", comboBoxRegNo.Text);
                cmd.Parameters.AddWithValue("@firstName", textBoxFirstName.Text);
                cmd.Parameters.AddWithValue("@lastName", textBoxLastName.Text);
                cmd.Parameters.AddWithValue("@dateOfBirth", dateTimePickerDOB.Value);
                cmd.Parameters.AddWithValue("@gender", radioButtonMale.Checked ? "Male" : "Female");
                cmd.Parameters.AddWithValue("@address", textBoxAddress.Text);
                cmd.Parameters.AddWithValue("@email", textBoxEmail.Text);
                cmd.Parameters.AddWithValue("@mobilePhone", textBoxMobilePhone.Text);
                cmd.Parameters.AddWithValue("@homePhone", textBoxHomePhone.Text);
                cmd.Parameters.AddWithValue("@parentName", textBoxParentName.Text);
                cmd.Parameters.AddWithValue("@nic", textBoxNIC.Text);
                cmd.Parameters.AddWithValue("@contactNo", textBoxContactNo.Text);

                conn.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Student registered successfully!", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                ClearFields();
            }
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-FVJPNHV\\SQLEXPRESS;Initial Catalog=Student;Integrated Security=True";
            string query = "SELECT regNo FROM Registration";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBoxRegNo.Items.Add(reader["regNo"].ToString());
                }
            }
        }

        private void linkLabelLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }

        private void linkLabelExit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {
            comboBoxRegNo.SelectedIndex = -1;
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            dateTimePickerDOB.Value = DateTime.Now;
            radioButtonMale.Checked = false;
            radioButtonFemale.Checked = false;
            textBoxAddress.Clear();
            textBoxEmail.Clear();
            textBoxMobilePhone.Clear();
            textBoxHomePhone.Clear();
            textBoxParentName.Clear();
            textBoxNIC.Clear();
            textBoxContactNo.Clear();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFirstName.Text) || string.IsNullOrEmpty(textBoxLastName.Text) ||
       string.IsNullOrEmpty(textBoxAddress.Text) || string.IsNullOrEmpty(textBoxEmail.Text) ||
       string.IsNullOrEmpty(textBoxMobilePhone.Text) || string.IsNullOrEmpty(textBoxParentName.Text) ||
       string.IsNullOrEmpty(textBoxNIC.Text) || string.IsNullOrEmpty(textBoxContactNo.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(comboBoxRegNo.Text))
            {
                MessageBox.Show("Please select a valid Registration Number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=DESKTOP-FVJPNHV\\SQLEXPRESS;Initial Catalog=Student;Integrated Security=True";
            string query = "SELECT * FROM Registration WHERE regNo = @regNo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@regNo", comboBoxRegNo.Text);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string currentFirstName = reader["firstName"].ToString();
                        string currentLastName = reader["lastName"].ToString();
                        string currentAddress = reader["address"].ToString();
                        string currentEmail = reader["email"].ToString();
                        string currentMobilePhone = reader["mobilePhone"].ToString();
                        string currentParentName = reader["parentName"].ToString();
                        string currentNIC = reader["nic"].ToString();
                        string currentContactNo = reader["contactNo"].ToString();
                        DateTime currentDOB = Convert.ToDateTime(reader["dateOfBirth"]);
                        string currentGender = reader["gender"].ToString();

                        reader.Close();

                        bool hasChanges = false;

                        if (textBoxFirstName.Text != currentFirstName) hasChanges = true;
                        if (textBoxLastName.Text != currentLastName) hasChanges = true;
                        if (textBoxAddress.Text != currentAddress) hasChanges = true;
                        if (textBoxEmail.Text != currentEmail) hasChanges = true;
                        if (textBoxMobilePhone.Text != currentMobilePhone) hasChanges = true;
                        if (textBoxParentName.Text != currentParentName) hasChanges = true;
                        if (textBoxNIC.Text != currentNIC) hasChanges = true;
                        if (textBoxContactNo.Text != currentContactNo) hasChanges = true;
                        if (dateTimePickerDOB.Value != currentDOB) hasChanges = true;
                        if ((radioButtonMale.Checked && currentGender != "Male") || (radioButtonFemale.Checked && currentGender != "Female"))
                        {
                            hasChanges = true;
                        }

                        if (hasChanges)
                        {
                            
                            string updateQuery = "UPDATE Registration SET firstName = @firstName, lastName = @lastName, dateOfBirth = @dateOfBirth, " +
                                                 "gender = @gender, address = @address, email = @email, mobilePhone = @mobilePhone, homePhone = @homePhone, " +
                                                 "parentName = @parentName, nic = @nic, contactNo = @contactNo WHERE regNo = @regNo";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@regNo", comboBoxRegNo.Text);
                                updateCmd.Parameters.AddWithValue("@firstName", textBoxFirstName.Text);
                                updateCmd.Parameters.AddWithValue("@lastName", textBoxLastName.Text);
                                updateCmd.Parameters.AddWithValue("@dateOfBirth", dateTimePickerDOB.Value);
                                updateCmd.Parameters.AddWithValue("@gender", radioButtonMale.Checked ? "Male" : "Female");
                                updateCmd.Parameters.AddWithValue("@address", textBoxAddress.Text);
                                updateCmd.Parameters.AddWithValue("@email", textBoxEmail.Text);
                                updateCmd.Parameters.AddWithValue("@mobilePhone", textBoxMobilePhone.Text);
                                updateCmd.Parameters.AddWithValue("@homePhone", textBoxHomePhone.Text);
                                updateCmd.Parameters.AddWithValue("@parentName", textBoxParentName.Text);
                                updateCmd.Parameters.AddWithValue("@nic", textBoxNIC.Text);
                                updateCmd.Parameters.AddWithValue("@contactNo", textBoxContactNo.Text);

                                int rowsAffected = updateCmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Student information updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No changes detected. The student information is the same as before.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Student not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxRegNo.Text))
            {
                MessageBox.Show("Please select a valid Registration Number to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this student's record?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string connectionString = "Data Source=DESKTOP-FVJPNHV\\SQLEXPRESS;Initial Catalog=Student;Integrated Security=True";
                string query = "DELETE FROM Registration WHERE regNo = @regNo";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@regNo", comboBoxRegNo.Text);

                    try
                    {
                        conn.Open();

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("No student found with the provided Registration Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
