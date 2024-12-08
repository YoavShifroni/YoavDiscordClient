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
            this.serverIpLabel = new System.Windows.Forms.Label();
            this.dontHaveAnAccountLabel = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.showPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.goToRegisterLabel = new System.Windows.Forms.Label();
            this.clearButton = new System.Windows.Forms.Button();
            this.changePasswordLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.serverIpTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startLabel = new System.Windows.Forms.Label();
            this.codeRecivedInMailLabel = new System.Windows.Forms.Label();
            this.codeRecivedInMailTextBox = new System.Windows.Forms.TextBox();
            this.submitCodeButton = new System.Windows.Forms.Button();
            this.captchaPictureBox = new System.Windows.Forms.PictureBox();
            this.checkThatTheCodesAreTheSameButton = new System.Windows.Forms.Button();
            this.enterTheCodeShownAboveTextBox = new System.Windows.Forms.TextBox();
            this.enterTheCodeShownAboveLabel = new System.Windows.Forms.Label();
            this.cooldownTimeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.captchaPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // serverIpLabel
            // 
            this.serverIpLabel.AutoSize = true;
            this.serverIpLabel.Location = new System.Drawing.Point(22, 136);
            this.serverIpLabel.Name = "serverIpLabel";
            this.serverIpLabel.Size = new System.Drawing.Size(63, 17);
            this.serverIpLabel.TabIndex = 39;
            this.serverIpLabel.Text = "Server Ip";
            // 
            // dontHaveAnAccountLabel
            // 
            this.dontHaveAnAccountLabel.AutoSize = true;
            this.dontHaveAnAccountLabel.Location = new System.Drawing.Point(58, 523);
            this.dontHaveAnAccountLabel.Name = "dontHaveAnAccountLabel";
            this.dontHaveAnAccountLabel.Size = new System.Drawing.Size(147, 17);
            this.dontHaveAnAccountLabel.TabIndex = 37;
            this.dontHaveAnAccountLabel.Text = "Dont Have an Account";
            // 
            // loginButton
            // 
            this.loginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.loginButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.ForeColor = System.Drawing.Color.White;
            this.loginButton.Location = new System.Drawing.Point(25, 408);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(216, 35);
            this.loginButton.TabIndex = 35;
            this.loginButton.Text = "LOGIN";
            this.loginButton.UseVisualStyleBackColor = false;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(22, 205);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(69, 17);
            this.usernameLabel.TabIndex = 30;
            this.usernameLabel.Text = "Username";
            // 
            // showPasswordCheckBox
            // 
            this.showPasswordCheckBox.AutoSize = true;
            this.showPasswordCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.showPasswordCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showPasswordCheckBox.Location = new System.Drawing.Point(122, 330);
            this.showPasswordCheckBox.Name = "showPasswordCheckBox";
            this.showPasswordCheckBox.Size = new System.Drawing.Size(119, 21);
            this.showPasswordCheckBox.TabIndex = 34;
            this.showPasswordCheckBox.Text = "Show Password";
            this.showPasswordCheckBox.UseVisualStyleBackColor = true;
            this.showPasswordCheckBox.CheckedChanged += new System.EventHandler(this.showPasswordCheckBox_CheckedChanged);
            // 
            // goToRegisterLabel
            // 
            this.goToRegisterLabel.AutoSize = true;
            this.goToRegisterLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.goToRegisterLabel.Location = new System.Drawing.Point(87, 551);
            this.goToRegisterLabel.Name = "goToRegisterLabel";
            this.goToRegisterLabel.Size = new System.Drawing.Size(76, 17);
            this.goToRegisterLabel.TabIndex = 38;
            this.goToRegisterLabel.Text = "Create One";
            this.goToRegisterLabel.Click += new System.EventHandler(this.goToRegisterLabel_Click);
            // 
            // clearButton
            // 
            this.clearButton.BackColor = System.Drawing.Color.White;
            this.clearButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.clearButton.Location = new System.Drawing.Point(25, 462);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(216, 35);
            this.clearButton.TabIndex = 36;
            this.clearButton.Text = "CLEAR";
            this.clearButton.UseVisualStyleBackColor = false;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // changePasswordLabel
            // 
            this.changePasswordLabel.AutoSize = true;
            this.changePasswordLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.changePasswordLabel.Location = new System.Drawing.Point(169, 366);
            this.changePasswordLabel.Name = "changePasswordLabel";
            this.changePasswordLabel.Size = new System.Drawing.Size(68, 17);
            this.changePasswordLabel.TabIndex = 42;
            this.changePasswordLabel.Text = "Click here";
            this.changePasswordLabel.Click += new System.EventHandler(this.changePasswordLabel_Click);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(22, 279);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(66, 17);
            this.passwordLabel.TabIndex = 32;
            this.passwordLabel.Text = "Password";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.passwordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.Location = new System.Drawing.Point(25, 299);
            this.passwordTextBox.Multiline = true;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '•';
            this.passwordTextBox.Size = new System.Drawing.Size(216, 25);
            this.passwordTextBox.TabIndex = 33;
            this.passwordTextBox.Text = "Yh123!";
            this.passwordTextBox.WordWrap = false;
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.usernameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.usernameTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameTextBox.Location = new System.Drawing.Point(25, 224);
            this.usernameTextBox.Multiline = true;
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(216, 25);
            this.usernameTextBox.TabIndex = 31;
            this.usernameTextBox.Text = "yoav";
            this.usernameTextBox.WordWrap = false;
            // 
            // serverIpTextBox
            // 
            this.serverIpTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.serverIpTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.serverIpTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverIpTextBox.Location = new System.Drawing.Point(25, 156);
            this.serverIpTextBox.Multiline = true;
            this.serverIpTextBox.Name = "serverIpTextBox";
            this.serverIpTextBox.Size = new System.Drawing.Size(216, 25);
            this.serverIpTextBox.TabIndex = 40;
            this.serverIpTextBox.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 366);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 17);
            this.label1.TabIndex = 41;
            this.label1.Text = "Forgot your password -";
            // 
            // startLabel
            // 
            this.startLabel.AutoSize = true;
            this.startLabel.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.startLabel.Location = new System.Drawing.Point(56, 62);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(155, 27);
            this.startLabel.TabIndex = 29;
            this.startLabel.Text = "Get Started";
            // 
            // codeRecivedInMailLabel
            // 
            this.codeRecivedInMailLabel.AutoSize = true;
            this.codeRecivedInMailLabel.Location = new System.Drawing.Point(25, 632);
            this.codeRecivedInMailLabel.Name = "codeRecivedInMailLabel";
            this.codeRecivedInMailLabel.Size = new System.Drawing.Size(222, 17);
            this.codeRecivedInMailLabel.TabIndex = 43;
            this.codeRecivedInMailLabel.Text = "Code that you recived in your mail";
            this.codeRecivedInMailLabel.Visible = false;
            // 
            // codeRecivedInMailTextBox
            // 
            this.codeRecivedInMailTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.codeRecivedInMailTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.codeRecivedInMailTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.codeRecivedInMailTextBox.Location = new System.Drawing.Point(28, 662);
            this.codeRecivedInMailTextBox.Multiline = true;
            this.codeRecivedInMailTextBox.Name = "codeRecivedInMailTextBox";
            this.codeRecivedInMailTextBox.Size = new System.Drawing.Size(216, 25);
            this.codeRecivedInMailTextBox.TabIndex = 44;
            this.codeRecivedInMailTextBox.Visible = false;
            this.codeRecivedInMailTextBox.WordWrap = false;
            // 
            // submitCodeButton
            // 
            this.submitCodeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.submitCodeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.submitCodeButton.FlatAppearance.BorderSize = 0;
            this.submitCodeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.submitCodeButton.ForeColor = System.Drawing.Color.White;
            this.submitCodeButton.Location = new System.Drawing.Point(28, 706);
            this.submitCodeButton.Name = "submitCodeButton";
            this.submitCodeButton.Size = new System.Drawing.Size(216, 35);
            this.submitCodeButton.TabIndex = 45;
            this.submitCodeButton.Text = "SUBMIT";
            this.submitCodeButton.UseVisualStyleBackColor = false;
            this.submitCodeButton.Visible = false;
            this.submitCodeButton.Click += new System.EventHandler(this.submitCodeButton_Click);
            // 
            // captchaPictureBox
            // 
            this.captchaPictureBox.Location = new System.Drawing.Point(28, 795);
            this.captchaPictureBox.Name = "captchaPictureBox";
            this.captchaPictureBox.Size = new System.Drawing.Size(211, 113);
            this.captchaPictureBox.TabIndex = 46;
            this.captchaPictureBox.TabStop = false;
            this.captchaPictureBox.Visible = false;
            // 
            // checkThatTheCodesAreTheSameButton
            // 
            this.checkThatTheCodesAreTheSameButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.checkThatTheCodesAreTheSameButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkThatTheCodesAreTheSameButton.FlatAppearance.BorderSize = 0;
            this.checkThatTheCodesAreTheSameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkThatTheCodesAreTheSameButton.ForeColor = System.Drawing.Color.White;
            this.checkThatTheCodesAreTheSameButton.Location = new System.Drawing.Point(23, 1012);
            this.checkThatTheCodesAreTheSameButton.Name = "checkThatTheCodesAreTheSameButton";
            this.checkThatTheCodesAreTheSameButton.Size = new System.Drawing.Size(216, 35);
            this.checkThatTheCodesAreTheSameButton.TabIndex = 49;
            this.checkThatTheCodesAreTheSameButton.Text = "CHECK";
            this.checkThatTheCodesAreTheSameButton.UseVisualStyleBackColor = false;
            this.checkThatTheCodesAreTheSameButton.Visible = false;
            this.checkThatTheCodesAreTheSameButton.Click += new System.EventHandler(this.checkThatTheCodesAreTheSameButton_Click);
            // 
            // enterTheCodeShownAboveTextBox
            // 
            this.enterTheCodeShownAboveTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.enterTheCodeShownAboveTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.enterTheCodeShownAboveTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enterTheCodeShownAboveTextBox.Location = new System.Drawing.Point(23, 968);
            this.enterTheCodeShownAboveTextBox.Multiline = true;
            this.enterTheCodeShownAboveTextBox.Name = "enterTheCodeShownAboveTextBox";
            this.enterTheCodeShownAboveTextBox.Size = new System.Drawing.Size(216, 25);
            this.enterTheCodeShownAboveTextBox.TabIndex = 48;
            this.enterTheCodeShownAboveTextBox.Visible = false;
            this.enterTheCodeShownAboveTextBox.WordWrap = false;
            // 
            // enterTheCodeShownAboveLabel
            // 
            this.enterTheCodeShownAboveLabel.AutoSize = true;
            this.enterTheCodeShownAboveLabel.Location = new System.Drawing.Point(20, 938);
            this.enterTheCodeShownAboveLabel.Name = "enterTheCodeShownAboveLabel";
            this.enterTheCodeShownAboveLabel.Size = new System.Drawing.Size(154, 17);
            this.enterTheCodeShownAboveLabel.TabIndex = 47;
            this.enterTheCodeShownAboveLabel.Text = "The Code Shown Above";
            this.enterTheCodeShownAboveLabel.Visible = false;
            // 
            // cooldownTimeLabel
            // 
            this.cooldownTimeLabel.BackColor = System.Drawing.Color.Silver;
            this.cooldownTimeLabel.Font = new System.Drawing.Font("Nirmala UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cooldownTimeLabel.ForeColor = System.Drawing.Color.Black;
            this.cooldownTimeLabel.Location = new System.Drawing.Point(28, 583);
            this.cooldownTimeLabel.Name = "cooldownTimeLabel";
            this.cooldownTimeLabel.Size = new System.Drawing.Size(213, 49);
            this.cooldownTimeLabel.TabIndex = 50;
            this.cooldownTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cooldownTimeLabel.Visible = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(289, 1076);
            this.Controls.Add(this.cooldownTimeLabel);
            this.Controls.Add(this.checkThatTheCodesAreTheSameButton);
            this.Controls.Add(this.enterTheCodeShownAboveTextBox);
            this.Controls.Add(this.enterTheCodeShownAboveLabel);
            this.Controls.Add(this.captchaPictureBox);
            this.Controls.Add(this.submitCodeButton);
            this.Controls.Add(this.codeRecivedInMailTextBox);
            this.Controls.Add(this.codeRecivedInMailLabel);
            this.Controls.Add(this.changePasswordLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serverIpTextBox);
            this.Controls.Add(this.serverIpLabel);
            this.Controls.Add(this.goToRegisterLabel);
            this.Controls.Add(this.dontHaveAnAccountLabel);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.showPasswordCheckBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.startLabel);
            this.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.captchaPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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

