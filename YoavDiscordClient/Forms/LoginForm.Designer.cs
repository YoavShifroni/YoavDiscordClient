using System.Windows.Forms;

namespace YoavDiscordClient
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            serverIpLabel = new Label();
            dontHaveAnAccountLabel = new Label();
            loginButton = new Button();
            usernameLabel = new Label();
            showPasswordCheckBox = new CheckBox();
            goToRegisterLabel = new Label();
            clearButton = new Button();
            changePasswordLabel = new Label();
            passwordLabel = new Label();
            passwordTextBox = new TextBox();
            usernameTextBox = new TextBox();
            serverIpTextBox = new TextBox();
            label1 = new Label();
            startLabel = new Label();
            codeRecivedInMailLabel = new Label();
            codeRecivedInMailTextBox = new TextBox();
            submitCodeButton = new Button();
            captchaPictureBox = new PictureBox();
            checkThatTheCodesAreTheSameButton = new Button();
            enterTheCodeShownAboveTextBox = new TextBox();
            enterTheCodeShownAboveLabel = new Label();
            cooldownTimeLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)captchaPictureBox).BeginInit();
            SuspendLayout();
            // 
            // serverIpLabel
            // 
            serverIpLabel.AutoSize = true;
            serverIpLabel.Location = new System.Drawing.Point(22, 136);
            serverIpLabel.Name = "serverIpLabel";
            serverIpLabel.Size = new System.Drawing.Size(63, 17);
            serverIpLabel.TabIndex = 39;
            serverIpLabel.Text = "Server Ip";
            // 
            // dontHaveAnAccountLabel
            // 
            dontHaveAnAccountLabel.AutoSize = true;
            dontHaveAnAccountLabel.Location = new System.Drawing.Point(58, 523);
            dontHaveAnAccountLabel.Name = "dontHaveAnAccountLabel";
            dontHaveAnAccountLabel.Size = new System.Drawing.Size(147, 17);
            dontHaveAnAccountLabel.TabIndex = 37;
            dontHaveAnAccountLabel.Text = "Dont Have an Account";
            // 
            // loginButton
            // 
            loginButton.AutoSize = true;
            loginButton.BackColor = System.Drawing.Color.FromArgb(116, 86, 174);
            loginButton.Cursor = Cursors.Hand;
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.FlatStyle = FlatStyle.Flat;
            loginButton.ForeColor = System.Drawing.Color.White;
            loginButton.Location = new System.Drawing.Point(25, 408);
            loginButton.Name = "loginButton";
            loginButton.Size = new System.Drawing.Size(216, 35);
            loginButton.TabIndex = 35;
            loginButton.Text = "LOGIN";
            loginButton.UseVisualStyleBackColor = false;
            loginButton.Click += loginButton_Click;
            // 
            // usernameLabel
            // 
            usernameLabel.AutoSize = true;
            usernameLabel.Location = new System.Drawing.Point(22, 205);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new System.Drawing.Size(69, 17);
            usernameLabel.TabIndex = 30;
            usernameLabel.Text = "Username";
            // 
            // showPasswordCheckBox
            // 
            showPasswordCheckBox.AutoSize = true;
            showPasswordCheckBox.Cursor = Cursors.Hand;
            showPasswordCheckBox.FlatStyle = FlatStyle.Flat;
            showPasswordCheckBox.Location = new System.Drawing.Point(122, 330);
            showPasswordCheckBox.Name = "showPasswordCheckBox";
            showPasswordCheckBox.Size = new System.Drawing.Size(119, 21);
            showPasswordCheckBox.TabIndex = 34;
            showPasswordCheckBox.Text = "Show Password";
            showPasswordCheckBox.UseVisualStyleBackColor = true;
            showPasswordCheckBox.CheckedChanged += showPasswordCheckBox_CheckedChanged;
            // 
            // goToRegisterLabel
            // 
            goToRegisterLabel.AutoSize = true;
            goToRegisterLabel.ForeColor = System.Drawing.Color.FromArgb(116, 86, 174);
            goToRegisterLabel.Location = new System.Drawing.Point(87, 551);
            goToRegisterLabel.Name = "goToRegisterLabel";
            goToRegisterLabel.Size = new System.Drawing.Size(76, 17);
            goToRegisterLabel.TabIndex = 38;
            goToRegisterLabel.Text = "Create One";
            goToRegisterLabel.Click += goToRegisterLabel_Click;
            // 
            // clearButton
            // 
            clearButton.AutoSize = true;
            clearButton.BackColor = System.Drawing.Color.White;
            clearButton.Cursor = Cursors.Hand;
            clearButton.FlatStyle = FlatStyle.Flat;
            clearButton.ForeColor = System.Drawing.Color.FromArgb(116, 86, 174);
            clearButton.Location = new System.Drawing.Point(25, 462);
            clearButton.Name = "clearButton";
            clearButton.Size = new System.Drawing.Size(216, 35);
            clearButton.TabIndex = 36;
            clearButton.Text = "CLEAR";
            clearButton.UseVisualStyleBackColor = false;
            clearButton.Click += clearButton_Click;
            // 
            // changePasswordLabel
            // 
            changePasswordLabel.AutoSize = true;
            changePasswordLabel.ForeColor = System.Drawing.Color.FromArgb(116, 86, 174);
            changePasswordLabel.Location = new System.Drawing.Point(169, 366);
            changePasswordLabel.Name = "changePasswordLabel";
            changePasswordLabel.Size = new System.Drawing.Size(68, 17);
            changePasswordLabel.TabIndex = 42;
            changePasswordLabel.Text = "Click here";
            changePasswordLabel.Click += changePasswordLabel_Click;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new System.Drawing.Point(22, 279);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(66, 17);
            passwordLabel.TabIndex = 32;
            passwordLabel.Text = "Password";
            // 
            // passwordTextBox
            // 
            passwordTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            passwordTextBox.BorderStyle = BorderStyle.None;
            passwordTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            passwordTextBox.Location = new System.Drawing.Point(25, 299);
            passwordTextBox.Multiline = true;
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '•';
            passwordTextBox.Size = new System.Drawing.Size(216, 25);
            passwordTextBox.TabIndex = 33;
            passwordTextBox.WordWrap = false;
            // 
            // usernameTextBox
            // 
            usernameTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            usernameTextBox.BorderStyle = BorderStyle.None;
            usernameTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            usernameTextBox.Location = new System.Drawing.Point(25, 224);
            usernameTextBox.Multiline = true;
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new System.Drawing.Size(216, 25);
            usernameTextBox.TabIndex = 31;
            usernameTextBox.WordWrap = false;
            // 
            // serverIpTextBox
            // 
            serverIpTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            serverIpTextBox.BorderStyle = BorderStyle.None;
            serverIpTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            serverIpTextBox.Location = new System.Drawing.Point(25, 156);
            serverIpTextBox.Multiline = true;
            serverIpTextBox.Name = "serverIpTextBox";
            serverIpTextBox.Size = new System.Drawing.Size(216, 25);
            serverIpTextBox.TabIndex = 40;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 366);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(152, 17);
            label1.TabIndex = 41;
            label1.Text = "Forgot your password -";
            // 
            // startLabel
            // 
            startLabel.AutoSize = true;
            startLabel.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            startLabel.ForeColor = System.Drawing.Color.FromArgb(116, 86, 174);
            startLabel.Location = new System.Drawing.Point(56, 62);
            startLabel.Name = "startLabel";
            startLabel.Size = new System.Drawing.Size(155, 27);
            startLabel.TabIndex = 29;
            startLabel.Text = "Get Started";
            // 
            // codeRecivedInMailLabel
            // 
            codeRecivedInMailLabel.AutoSize = true;
            codeRecivedInMailLabel.Location = new System.Drawing.Point(25, 632);
            codeRecivedInMailLabel.Name = "codeRecivedInMailLabel";
            codeRecivedInMailLabel.Size = new System.Drawing.Size(222, 17);
            codeRecivedInMailLabel.TabIndex = 43;
            codeRecivedInMailLabel.Text = "Code that you recived in your mail";
            codeRecivedInMailLabel.Visible = false;
            // 
            // codeRecivedInMailTextBox
            // 
            codeRecivedInMailTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            codeRecivedInMailTextBox.BorderStyle = BorderStyle.None;
            codeRecivedInMailTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            codeRecivedInMailTextBox.Location = new System.Drawing.Point(28, 662);
            codeRecivedInMailTextBox.Multiline = true;
            codeRecivedInMailTextBox.Name = "codeRecivedInMailTextBox";
            codeRecivedInMailTextBox.Size = new System.Drawing.Size(216, 25);
            codeRecivedInMailTextBox.TabIndex = 44;
            codeRecivedInMailTextBox.Visible = false;
            codeRecivedInMailTextBox.WordWrap = false;
            // 
            // submitCodeButton
            // 
            submitCodeButton.AutoSize = true;
            submitCodeButton.BackColor = System.Drawing.Color.FromArgb(116, 86, 174);
            submitCodeButton.Cursor = Cursors.Hand;
            submitCodeButton.FlatAppearance.BorderSize = 0;
            submitCodeButton.FlatStyle = FlatStyle.Flat;
            submitCodeButton.ForeColor = System.Drawing.Color.White;
            submitCodeButton.Location = new System.Drawing.Point(28, 706);
            submitCodeButton.Name = "submitCodeButton";
            submitCodeButton.Size = new System.Drawing.Size(216, 35);
            submitCodeButton.TabIndex = 45;
            submitCodeButton.Text = "SUBMIT";
            submitCodeButton.UseVisualStyleBackColor = false;
            submitCodeButton.Visible = false;
            submitCodeButton.Click += submitCodeButton_Click;
            // 
            // captchaPictureBox
            // 
            captchaPictureBox.Location = new System.Drawing.Point(28, 795);
            captchaPictureBox.Name = "captchaPictureBox";
            captchaPictureBox.Size = new System.Drawing.Size(211, 113);
            captchaPictureBox.TabIndex = 46;
            captchaPictureBox.TabStop = false;
            captchaPictureBox.Visible = false;
            // 
            // checkThatTheCodesAreTheSameButton
            // 
            checkThatTheCodesAreTheSameButton.AutoSize = true;
            checkThatTheCodesAreTheSameButton.BackColor = System.Drawing.Color.FromArgb(116, 86, 174);
            checkThatTheCodesAreTheSameButton.Cursor = Cursors.Hand;
            checkThatTheCodesAreTheSameButton.FlatAppearance.BorderSize = 0;
            checkThatTheCodesAreTheSameButton.FlatStyle = FlatStyle.Flat;
            checkThatTheCodesAreTheSameButton.ForeColor = System.Drawing.Color.White;
            checkThatTheCodesAreTheSameButton.Location = new System.Drawing.Point(23, 1012);
            checkThatTheCodesAreTheSameButton.Name = "checkThatTheCodesAreTheSameButton";
            checkThatTheCodesAreTheSameButton.Size = new System.Drawing.Size(216, 35);
            checkThatTheCodesAreTheSameButton.TabIndex = 49;
            checkThatTheCodesAreTheSameButton.Text = "CHECK";
            checkThatTheCodesAreTheSameButton.UseVisualStyleBackColor = false;
            checkThatTheCodesAreTheSameButton.Visible = false;
            checkThatTheCodesAreTheSameButton.Click += checkThatTheCodesAreTheSameButton_Click;
            // 
            // enterTheCodeShownAboveTextBox
            // 
            enterTheCodeShownAboveTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            enterTheCodeShownAboveTextBox.BorderStyle = BorderStyle.None;
            enterTheCodeShownAboveTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            enterTheCodeShownAboveTextBox.Location = new System.Drawing.Point(23, 968);
            enterTheCodeShownAboveTextBox.Multiline = true;
            enterTheCodeShownAboveTextBox.Name = "enterTheCodeShownAboveTextBox";
            enterTheCodeShownAboveTextBox.Size = new System.Drawing.Size(216, 25);
            enterTheCodeShownAboveTextBox.TabIndex = 48;
            enterTheCodeShownAboveTextBox.Visible = false;
            enterTheCodeShownAboveTextBox.WordWrap = false;
            // 
            // enterTheCodeShownAboveLabel
            // 
            enterTheCodeShownAboveLabel.AutoSize = true;
            enterTheCodeShownAboveLabel.Location = new System.Drawing.Point(20, 938);
            enterTheCodeShownAboveLabel.Name = "enterTheCodeShownAboveLabel";
            enterTheCodeShownAboveLabel.Size = new System.Drawing.Size(154, 17);
            enterTheCodeShownAboveLabel.TabIndex = 47;
            enterTheCodeShownAboveLabel.Text = "The Code Shown Above";
            enterTheCodeShownAboveLabel.Visible = false;
            // 
            // cooldownTimeLabel
            // 
            cooldownTimeLabel.Anchor = AnchorStyles.Top;
            cooldownTimeLabel.AutoSize = true;
            cooldownTimeLabel.BackColor = System.Drawing.Color.Silver;
            cooldownTimeLabel.Font = new System.Drawing.Font("Nirmala UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            cooldownTimeLabel.ForeColor = System.Drawing.Color.Black;
            cooldownTimeLabel.Location = new System.Drawing.Point(28, 583);
            cooldownTimeLabel.Name = "cooldownTimeLabel";
            cooldownTimeLabel.Size = new System.Drawing.Size(0, 30);
            cooldownTimeLabel.TabIndex = 50;
            cooldownTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            cooldownTimeLabel.Visible = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(289, 1095);
            Controls.Add(cooldownTimeLabel);
            Controls.Add(checkThatTheCodesAreTheSameButton);
            Controls.Add(enterTheCodeShownAboveTextBox);
            Controls.Add(enterTheCodeShownAboveLabel);
            Controls.Add(captchaPictureBox);
            Controls.Add(submitCodeButton);
            Controls.Add(codeRecivedInMailTextBox);
            Controls.Add(codeRecivedInMailLabel);
            Controls.Add(changePasswordLabel);
            Controls.Add(label1);
            Controls.Add(serverIpTextBox);
            Controls.Add(serverIpLabel);
            Controls.Add(goToRegisterLabel);
            Controls.Add(dontHaveAnAccountLabel);
            Controls.Add(clearButton);
            Controls.Add(loginButton);
            Controls.Add(showPasswordCheckBox);
            Controls.Add(passwordTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(usernameTextBox);
            Controls.Add(usernameLabel);
            Controls.Add(startLabel);
            Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            ForeColor = System.Drawing.Color.FromArgb(164, 165, 169);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LoginForm";
            Load += LoginForm_Load;
            ((System.ComponentModel.ISupportInitialize)captchaPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label serverIpLabel;
        private System.Windows.Forms.Label dontHaveAnAccountLabel;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.CheckBox showPasswordCheckBox;
        private System.Windows.Forms.Label goToRegisterLabel;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Label changePasswordLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox serverIpTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Label codeRecivedInMailLabel;
        private System.Windows.Forms.TextBox codeRecivedInMailTextBox;
        private System.Windows.Forms.Button submitCodeButton;
        private System.Windows.Forms.PictureBox captchaPictureBox;
        private System.Windows.Forms.Button checkThatTheCodesAreTheSameButton;
        private System.Windows.Forms.TextBox enterTheCodeShownAboveTextBox;
        private System.Windows.Forms.Label enterTheCodeShownAboveLabel;
        private System.Windows.Forms.Label cooldownTimeLabel;
    }
}

