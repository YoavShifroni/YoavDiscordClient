using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public partial class LoginForm : Form
    {

        private string _code;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Enter function login click: " + DateTime.Now.Millisecond);
            if (!this.IsValidIpAddress(this.serverIpTextBox.Text))
            {
                MessageBox.Show("please check the server ip");
                return;
            }
            if(this.usernameTextBox.Text.Length < 3 || this.passwordTextBox.Text.Length < 5)
            {
                MessageBox.Show("your username/ password is to short, please fix them :)");
                return;
            }
            ConnectionManager.getInstance(this.serverIpTextBox.Text).ProcessLogin(this.usernameTextBox.Text, this.passwordTextBox.Text);
            Console.WriteLine("Leave function login click: " + DateTime.Now.Millisecond);

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

        private void showPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (showPasswordCheckBox.Checked)
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
            this.serverIpTextBox.Text = "";
            this.serverIpTextBox.Focus();
        }

        private void changePasswordLabel_Click(object sender, EventArgs e)
        {
            if(this.usernameTextBox.Text.Length < 3 || !this.IsValidIpAddress(this.serverIpTextBox.Text))
            {
                MessageBox.Show("please fill your username and server ip first");
                return;
            }
            ConnectionManager.getInstance(this.serverIpTextBox.Text).ProcessForgotPassword(this.usernameTextBox.Text);
            MessageBox.Show("email with code was sent to you right now, check your email and enter the code in the right place");
            DiscordFormsHolder.getInstance().ForgotPasswordForm.Visible = true;
            this.Visible = false;
        }

        private void goToRegisterLabel_Click(object sender, EventArgs e)
        {
            DiscordFormsHolder.getInstance().RegistrationForm.Visible = true;
            this.Visible = false;
        }

        public void ShowNext(string code)
        {
            this._code = code;
            this.serverIpTextBox.Enabled = false;
            this.usernameTextBox.Enabled = false;
            this.passwordTextBox.Enabled = false;
            this.changePasswordLabel.Enabled = false;
            this.loginButton.Enabled = false;
            this.clearButton.Enabled = false;
            this.codeRecivedInMailLabel.Visible = true;
            this.codeRecivedInMailTextBox.Visible = true;
            this.submitCodeButton.Visible = true;
        }

        private void submitCodeButton_Click(object sender, EventArgs e)
        {
            if (this.codeRecivedInMailTextBox.Text != this._code)
            {
                MessageBox.Show("The code is incorrect, please check again");
                return;
            }
            MessageBox.Show("very well the code is correct, Finally, please confirm that you are not a robot");
            DisplayCaptcha();
            NextStage();
        }

        private void NextStage()
        {
            this.codeRecivedInMailLabel.Enabled = false;
            this.codeRecivedInMailTextBox.Enabled = false;
            this.submitCodeButton.Enabled = false;
            this.captchaPictureBox.Visible = true;
            this.enterTheCodeShownAboveLabel.Visible = true;
            this.enterTheCodeShownAboveTextBox.Visible = true;
            this.checkThatTheCodesAreTheSameButton.Visible = true;
        }

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

        private void checkThatTheCodesAreTheSameButton_Click(object sender, EventArgs e)
        {
            if(this.captchaPictureBox.Tag.ToString() != this.enterTheCodeShownAboveTextBox.Text)
            {
                MessageBox.Show("Captcha is incorrect");
                this.DisplayCaptcha();
                return;
            }
            MessageBox.Show("Captcha is correct");
            ConnectionManager.getInstance(null).ProcessSuccessesLogin();
        }
    }
}
