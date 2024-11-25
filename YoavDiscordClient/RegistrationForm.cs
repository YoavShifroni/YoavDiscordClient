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
        /// <summary>
        /// Save the detailt that the user enter in order to register
        /// </summary>
        public RegistrationInfo RegistrationInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The function check if the server ip address that the user enter is valid, if it is the function will check if the user filled
        /// all the stuff corectly, if he did the function will start the process that check if there is no other user with this username
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void registerButton_Click(object sender, EventArgs e)
        {
            if (!this.IsValidIpAddress(this.serverIpTextBox.Text))
            {
                MessageBox.Show("please check the server ip");
                return;
            }
            if (this.isFilled())
            {
                ConnectionManager.getInstance(this.serverIpTextBox.Text).ProcessCheckIfUsernameAlreadyExist(this.usernameTextBox.Text);
            }

        }

        /// <summary>
        /// The function check if the string ip is a valid ip address, ip it is it will return true otherwise false
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
        /// This function checks if all the fields for the register are filled corectly
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
        /// This function check if the string password that it get is a correct password using Regex, if it is the function will return true 
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
        /// This function checks if the string email that it get is a correct email, if it is the function will return true 
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

        /// <summary>
        /// The function change the PasswordChar in the password text box so when it's checked it will show to password and when it's not check
        /// it will show circle for each leater in the password so you wouldn't be able to read it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// The function will remove all the text that exist in all the text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// The function move the user to the login window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backToLoginLabel_Click(object sender, EventArgs e)
        {
            DiscordFormsHolder.getInstance().LoginForm.Visible = true;
            this.Visible = false;
        }


        /// <summary>
        /// The function change some visual stuff in this window and call the function that displat the captcha
        /// </summary>
        public void NextStage()
        {
            this.usernameTextBox.Enabled = false;
            this.passwordTextBox.Enabled = false;
            this.firstNameTextBox.Enabled = false;
            this.lastNameTextBox.Enabled = false;
            this.serverIpTextBox.Enabled = false;
            this.emailTextBox.Enabled = false;
            this.cityComboBox.Enabled = false;
            this.genderComboBox.Enabled = false;
            this.registerButton.Enabled = false;
            this.clearButton.Enabled = false;
            this.backToLoginLabel.Enabled = false;
            this.captchaPictureBox.Visible = true;
            this.enterTheCodeShownAboveLabel.Visible = true;
            this.enterTheCodeShownAboveTextBox.Visible = true;
            this.checkThatTheCodesAreTheSameButton.Visible = true;
            this.DisplayCaptcha();
        }

        /// <summary>
        /// The function display the captcha - the code that the user need to enter to confirm that he isn't a bot
        /// </summary>
        private void DisplayCaptcha()
        {
            this.enterTheCodeShownAboveTextBox.Text = "";
            string captchaCode = ConnectionManager.getInstance(null).GetRandomCode();
            Font font = new Font("Arial", 12, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Black);
            Bitmap bitmap = new Bitmap(this.captchaPictureBox.Width, this.captchaPictureBox.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);
                float x = (this.captchaPictureBox.Width - graphics.MeasureString(captchaCode, font).Width) / 2;
                float y = (this.captchaPictureBox.Height - graphics.MeasureString(captchaCode, font).Height) / 2;
                graphics.DrawString(captchaCode, font, brush, x, y);
            }
            this.captchaPictureBox.Image = bitmap;
            this.captchaPictureBox.Tag = captchaCode;
        }

        /// <summary>
        /// The function check if the code that the user enter is the same code that appering on the picture box, if it is it will call
        /// the function that will move the user to other window there he will choose his profile picture, if it isn't a message will apper saying
        /// that the codes aren't the same and a new captcha code will apper
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkThatTheCodesAreTheSameButton_Click(object sender, EventArgs e)
        {
            if (this.captchaPictureBox.Tag.ToString() != this.enterTheCodeShownAboveTextBox.Text)
            {
                MessageBox.Show("Captcha is incorrect");
                this.DisplayCaptcha();
                return;
            }
            MessageBox.Show("Captcha is correct, now choose your profile picture");
            this.RegistrationInfo = new RegistrationInfo(this.usernameTextBox.Text, this.passwordTextBox.Text, this.firstNameTextBox.Text,
                this.lastNameTextBox.Text, this.serverIpTextBox.Text, this.emailTextBox.Text, this.cityComboBox.Text, this.genderComboBox.Text);
            this.MoveToProfilePictureForm();
        }

        /// <summary>
        /// The function will move the user to the profile picture window
        /// </summary>
        private void MoveToProfilePictureForm()
        {
            this.Visible = false;
            DiscordFormsHolder.getInstance().ProfilePictureForm.Visible = true;
        }
    }
}
