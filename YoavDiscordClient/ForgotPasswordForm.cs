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
        public ForgotPasswordForm()
        {
            InitializeComponent();
        }

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

        private void clearButton_Click(object sender, EventArgs e)
        {
            this.passwordTextBox.Text = "";
            this.confirmPasswordTextBox.Text = "";
        }

        private void ForgotPasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            DiscordFormsHolder.getInstance().LoginForm.Visible = true;
        }
    }
}
