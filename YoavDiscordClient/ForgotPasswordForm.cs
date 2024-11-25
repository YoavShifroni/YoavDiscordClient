using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public partial class ForgotPasswordForm : Form
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public ForgotPasswordForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// The function check if the code that the user entered is similar to the code that he recieved in his mail, if it's not it will display message
        /// that says the code is incorrect else it will show him the next stage in order to change his password 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkButton_Click(object sender, EventArgs e)
        {
            if(ConnectionManager.getInstance(null).Code != this.codeTextBox.Text)
            {
                MessageBox.Show("check the code that you entered, it's not similer to the code that you recived in the email");
                return;
            }
            this.checkButton.Enabled = false;
            this.passwordLabel.Visible = true;
            this.passwordTextBox.Visible = true;
            this.confirmPasswordLabel.Visible = true;
            this.confirmPasswordTextBox.Visible = true;
            this.showPasswordCheckBox.Visible = true;
            this.loginButton.Visible = true;
            this.clearButton.Visible = true;
        }


        /// <summary>
        /// The function change the PasswordChar in the password text boxes so when it's checked it will show to password and when it's not check
        /// it will show circle for each leater in the password so you wouldn't be able to read it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.showPasswordCheckBox.Checked)
            {
                this.passwordTextBox.PasswordChar = '\0';
                this.confirmPasswordTextBox.PasswordChar = '\0';
            }
            else
            {
                this.passwordTextBox.PasswordChar = '•';
                this.confirmPasswordTextBox.PasswordChar = '•';
            }
        }


        /// <summary>
        /// The function check if the two passwords that the user entered are similar, if they are it will check if the password is valid
        /// and if it is it will start the process in order to change the user password and login him into the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginButton_Click(object sender, EventArgs e)
        {
            if (!this.passwordTextBox.Text.Equals(this.confirmPasswordTextBox.Text))
            {
                MessageBox.Show("the passwords that you wrote aren't the same, please check again :)");
                return;
            }
            if (!RegistrationForm.IsValidPassword(this.passwordTextBox.Text))
            {
                MessageBox.Show("fix your password that it will contain at least one capital leater, one small letter, one digit and one sign");
                return;
            }
            ConnectionManager.getInstance(null).ProcessUpdatePassword(ConnectionManager.getInstance(null).Username, this.passwordTextBox.Text);
        }


        /// <summary>
        /// The function will remove all the text that exist in all the text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            this.passwordTextBox.Text = "";
            this.confirmPasswordTextBox.Text = "";
        }

        /// <summary>
        /// The function is called if the user close this window and if he did that it will move him back to the login window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForgotPasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            DiscordFormsHolder.getInstance().LoginForm.Visible = true;
            DiscordFormsHolder.getInstance().SetActiveForm(FormNames.Login);
        }
    }
}
