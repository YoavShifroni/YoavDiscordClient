using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public partial class LoginForm : Form
    {
        /// <summary>
        /// The code that sent to the user mail
        /// </summary>
        private string _code;



        /// <summary>
        /// Constructor
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The function check if the server ip id valid if it is it will check if the username
        /// and password length are ok and if they are it will start the process to login the user into the project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginButton_Click(object sender, EventArgs e)
        {
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
            DiscordFormsHolder.getInstance().ChangeCursorSignAndActiveFormStatus(false);
            ConnectionManager.getInstance(this.serverIpTextBox.Text).ProcessLogin(this.usernameTextBox.Text, this.passwordTextBox.Text);

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
        /// The function change the PasswordChar in the password text box so when it's checked it will show to password and when it's not check
        /// it will show circle for each leater in the password so you wouldn't be able to read it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// The function will remove all the text that exist in all the text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            this.usernameTextBox.Text = "";
            this.passwordTextBox.Text = "";
            this.serverIpTextBox.Text = "";
            this.serverIpTextBox.Focus();
        }

        /// <summary>
        /// The function check if the username is long enough and if the server ip is valid, if they are it will start the process to let 
        /// the user create a new password for himself 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changePasswordLabel_Click(object sender, EventArgs e)
        {
            if(this.usernameTextBox.Text.Length < 3 || !this.IsValidIpAddress(this.serverIpTextBox.Text))
            {
                MessageBox.Show("please fill your username and server ip first");
                return;
            }
            DiscordFormsHolder.getInstance().SetActiveForm(FormNames.ForgotPassword);
            ConnectionManager.getInstance(this.serverIpTextBox.Text).ProcessForgotPassword(this.usernameTextBox.Text);
            MessageBox.Show("email with code was sent to you right now, check your email and enter the code in the right place");
            DiscordFormsHolder.getInstance().ForgotPasswordForm.Visible = true;
            this.Visible = false;
        }

        /// <summary>
        /// The function move the user to the register window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToRegisterLabel_Click(object sender, EventArgs e)
        {
            DiscordFormsHolder.getInstance().RegistrationForm.Visible = true;
            this.Visible = false;
            DiscordFormsHolder.getInstance().SetActiveForm(FormNames.Registration);
        }

        /// <summary>
        /// The function save the code in his property and change some visual stuff in this window
        /// </summary>
        /// <param name="code"></param>
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

        /// <summary>
        /// The function check if the code that the user enter is the same code that sent to his mail, if it is it will show him the captch
        /// that he need to enter to confirm that he isn't a bot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// The function change some visual stuff in this window
        /// </summary>
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


        /// <summary>
        /// The function display the captcha - the code that the user need to enter to confirm that he isn't a bot
        /// </summary>
        private void DisplayCaptcha()
        {
            this.enterTheCodeShownAboveTextBox.Text = "";
            string captchaCode = this.GetRandomCodeForCaptcha();
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
        /// The function create a random code with a length of 6 characters and return it
        /// </summary>
        /// <returns></returns>
        public string GetRandomCodeForCaptcha()
        {
            var charsALL = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var randomIns = new Random();
            var rndChars = Enumerable.Range(0, 6)
                            .Select(_ => charsALL[randomIns.Next(charsALL.Length)])
                            .ToArray();
            return new string(rndChars);
        }


        /// <summary>
        /// The function check if the code that the user enter is the same code that appering on the picture box, if it is it will start
        /// the process to enter the user into the discord, if it isn't a message will apper saying that the codes aren't the same and a new captcha
        /// code will apper
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkThatTheCodesAreTheSameButton_Click(object sender, EventArgs e)
        {
            if(this.captchaPictureBox.Tag.ToString() != this.enterTheCodeShownAboveTextBox.Text)
            {
                MessageBox.Show("Captcha is incorrect");
                this.DisplayCaptcha();
                return;
            }
            MessageBox.Show("Captcha is correct");
            ConnectionManager.getInstance(null).ProcessSuccessesLoginOrRegistration();
        }


        /// <summary>
        /// The function create a CountDownTimer instance that represent the time that left until the user can try to login again
        /// </summary>
        /// <param name="time"></param>
        public void ShowCooldownTimer(int time)
        {
            CountDownTimer timer = new CountDownTimer(time, 0);

            timer.Start();

            this.ToggleLoginStatus(false);

        }


        /// <summary>
        /// The function change the Enable status of some Controls according to the bool value
        /// </summary>
        /// <param name="status"></param>
        public void ToggleLoginStatus(bool status)
        {
            this.loginButton.Enabled = status;
            this.goToRegisterLabel.Enabled = status;
            this.changePasswordLabel.Enabled = status;
            this.cooldownTimeLabel.Visible = !status;

        }


        /// <summary>
        /// The function show to user the time that left until the user can try to login again
        /// </summary>
        /// <param name="text"></param>
        public void ShowCooldownTimerOnLabel(string text)
        {
            this.cooldownTimeLabel.Text = text;
        }

        
    }
}
