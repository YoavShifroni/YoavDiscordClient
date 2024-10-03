using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            if (!this.IsValidIpAddress(this.serverIpTextBox.Text))
            {
                MessageBox.Show("please check the server ip");
                return;
            }
            if (this.isFilled())
            {
                ConnectionManager.getInstance(this.serverIpTextBox.Text).ProcessRegistration(this.usernameTextBox.Text, this.passwordTextBox.Text,
                    this.firstNameTextBox.Text, this.lastNameTextBox.Text, this.emailTextBox.Text, this.cityComboBox.Text, this.genderComboBox.Text);
            }

        }

        /// <summary>
        /// the function check if the string ip is a valid ip address, ip it is it will return true otherwise false
        /// I took this function from the website StackOverFlow in this link: 
        /// https://stackoverflow.com/questions/799060/how-to-determine-if-a-string-is-a-valid-ipv4-or-ipv6-address-in-c
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private bool IsValidIpAddress(string address)
        {
            IPAddress ip;
            if (IPAddress.TryParse(address, out ip))
            {
                if (address.Length >= 8 && address.Contains("."))
                {
                    string[] s = address.Split('.');
                    if (s.Length == 4 && s[0].Length > 0 && s[1].Length > 0 && s[2].Length > 0 && s[3].Length > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// this function checks if all the fields for the register are filled corectly
        /// </summary>
        /// <returns>True if all fields are correct</returns>
        private bool isFilled()
        {
            if (this.usernameTextBox.Text.Length < 3 || this.usernameTextBox.Text.Length > 20)
            {
                MessageBox.Show("your username should be between 3-20 letters, please fix it, Regestration Failed");
                return false;
            }
            if (!RegistrationForm.IsValidPassword(this.passwordTextBox.Text))
            {
                MessageBox.Show("your password should contain at least: one capital letter, one small letter, one digit, one sign " +
                    "and at least five letters, please fix it, Regestration Failed");
                return false;

            }
            if (this.firstNameTextBox.Text.Length < 3 || this.firstNameTextBox.Text.Length > 40)
            {
                MessageBox.Show("your first name should be between 3-40 letters, please fix it, Regestration Failed");
                return false;
            }
            if (this.lastNameTextBox.Text.Length < 3 || this.lastNameTextBox.Text.Length > 40)
            {
                MessageBox.Show("your last name should be between 3-40 letters, please fix it, Regestration Failed");
                return false;
            }
            if (!this.IsValidEmail(this.emailTextBox.Text))
            {
                MessageBox.Show("fix email, Registration Failed");
                return false;
            }
            if (this.cityComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("choose city, Registration Failed");
                return false;
            }
            if (this.genderComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("choose gender, Registration Failed");
                return false;
            }
            return true;
        }


        /// <summary>
        /// this function check if the string password that it get is a correct password using Regex, if it is the function will return true 
        /// otherwise the function will return false
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            string s = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{5,}$";
            Regex regex = new Regex(s);
            if (regex.IsMatch(password))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// this function checks if the string email that it get is a correct email, if it is the function will return true 
        /// otherwise the function will return false
        /// </summary>
        /// <param name="emailaddress">The email address</param>
        /// <returns></returns>
        private bool IsValidEmail(string emailaddress)
        {
            if(emailaddress.Length > 60)
            {
                MessageBox.Show("your email must be under 61 letters!");
                return false;
            }
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void showPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.showPasswordCheckBox.Checked)
            {
                this.passwordTextBox.PasswordChar = '\0';
            }
            else
            {
                this.passwordTextBox.PasswordChar = '•';
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            this.usernameTextBox.Text = "";
            this.passwordTextBox.Text = "";
            this.firstNameTextBox.Text = "";
            this.lastNameTextBox.Text = "";
            this.emailTextBox.Text = "";
            this.serverIpTextBox.Text = "";
            this.cityComboBox.SelectedIndex = -1;
            this.genderComboBox.SelectedIndex = -1;
        }

        private void backToLoginLabel_Click(object sender, EventArgs e)
        {
            DiscordFormsHolder.getInstance().LoginForm.Visible = true;
            this.Visible = false;
        }

        public void MoveToProfilePictureForm()
        {
            this.Visible = false;
            DiscordFormsHolder.getInstance().ProfilePictureForm.Visible = true;
        }
    }
}
