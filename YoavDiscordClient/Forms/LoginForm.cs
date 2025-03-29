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

        private bool _isCooldownSizeChanged = false;



        /// <summary>
        /// Constructor
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

            DiscordFormsHolder.ResizeFormBasedOnResolution(this, 305f, 1134f);
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
            if (this.usernameTextBox.Text.Length < 3 || !this.IsValidIpAddress(this.serverIpTextBox.Text))
            {
                MessageBox.Show("please fill your username and server ip first");
                return;
            }
            ConnectionManager.getInstance(this.serverIpTextBox.Text).ProcessForgotPassword(this.usernameTextBox.Text);
        }

        public void ForgotPasswordNextStage()
        {
            MessageBox.Show("email with code was sent to you right now, check your email and enter the code in the right place");
            DiscordFormsHolder.getInstance().SetActiveForm(FormNames.ForgotPassword);
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
        /// Creates and displays an improved captcha with distorted text, random colors, noise, and ambiguity-free characters
        /// </summary>
        private void DisplayCaptcha()
        {
            this.enterTheCodeShownAboveTextBox.Text = "";
            string captchaCode = this.GetRandomCodeForCaptcha();

            // Create bitmap with slightly larger dimensions to accommodate character transformations
            Bitmap bitmap = new Bitmap(this.captchaPictureBox.Width, this.captchaPictureBox.Height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Set high quality rendering for smoother transformations
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                // Fill background with random light color
                Random random = new Random();
                Color bgColor = Color.FromArgb(random.Next(220, 255), random.Next(220, 255), random.Next(220, 255));
                graphics.Clear(bgColor);

                // Add background noise (random dots)
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(0, bitmap.Width);
                    int y = random.Next(0, bitmap.Height);
                    Color noiseColor = Color.FromArgb(random.Next(150, 200), random.Next(150, 200), random.Next(150, 200));
                    bitmap.SetPixel(x, y, noiseColor);
                }

                // Add random lines for additional noise
                for (int i = 0; i < 5; i++)
                {
                    int x1 = random.Next(0, bitmap.Width);
                    int y1 = random.Next(0, bitmap.Height);
                    int x2 = random.Next(0, bitmap.Width);
                    int y2 = random.Next(0, bitmap.Height);
                    Color lineColor = Color.FromArgb(random.Next(100, 180), random.Next(100, 180), random.Next(100, 180));
                    Pen pen = new Pen(lineColor, 1);
                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }

                // Calculate positions for each character
                int charWidth = bitmap.Width / captchaCode.Length;

                // Draw each character with random transformations
                for (int i = 0; i < captchaCode.Length; i++)
                {
                    // Random font size variation
                    int fontSize = random.Next(14, 18);

                    // Random font style from available options
                    FontStyle[] styles = { FontStyle.Bold, FontStyle.Italic, FontStyle.Regular, FontStyle.Bold | FontStyle.Italic };
                    FontStyle style = styles[random.Next(styles.Length)];

                    // Create font with random style
                    Font font = new Font("Arial", fontSize, style);

                    // Random foreground color (dark shades for better visibility)
                    Color textColor = Color.FromArgb(random.Next(10, 80), random.Next(10, 80), random.Next(10, 80));
                    Brush brush = new SolidBrush(textColor);

                    // Base position for the character with some randomization
                    float x = i * charWidth + random.Next(-5, 5) + charWidth / 4;
                    float y = random.Next(5, bitmap.Height / 4);

                    // Create rotation transformation
                    graphics.TranslateTransform(x, y);
                    float angle = random.Next(-15, 15); // Random rotation angle
                    graphics.RotateTransform(angle);

                    // Draw the character
                    graphics.DrawString(captchaCode[i].ToString(), font, brush, 0, 0);

                    // Reset transformation for the next character
                    graphics.ResetTransform();
                }
            }

            this.captchaPictureBox.Image = bitmap;
            this.captchaPictureBox.Tag = captchaCode;
        }

        /// <summary>
        /// Creates a random code with a length of 6 characters, only excluding the easily confused characters 'I' and 'l'
        /// </summary>
        /// <returns>A 6-character captcha code without 'I' and 'l'</returns>
        public string GetRandomCodeForCaptcha()
        {
            // Only exclude capital I and lowercase l as specifically requested
            // All other characters are included
            var charsALL = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789abcdefghijkmnopqrstuvwxyz";
            var random = new Random();

            var rndChars = Enumerable.Range(0, 6)
                            .Select(_ => charsALL[random.Next(charsALL.Length)])
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
            ConnectionManager.getInstance(null).ProcessGetProfilePictureAndUsername();
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
            if (!this._isCooldownSizeChanged)
            {
                int centerX = (this.ClientSize.Width - this.cooldownTimeLabel.Width) / 2;

                // Set the control's Left property to center it
                this.cooldownTimeLabel.Left = centerX;

                this._isCooldownSizeChanged = true;
            }
        }


        
    }
}
