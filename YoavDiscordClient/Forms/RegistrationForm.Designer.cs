namespace YoavDiscordClient
{
    partial class RegistrationForm
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
            showPasswordCheckBox = new System.Windows.Forms.CheckBox();
            serverIpTextBox = new System.Windows.Forms.TextBox();
            serverIpLabel = new System.Windows.Forms.Label();
            genderComboBox = new System.Windows.Forms.ComboBox();
            cityComboBox = new System.Windows.Forms.ComboBox();
            genderLabel = new System.Windows.Forms.Label();
            cityLabel = new System.Windows.Forms.Label();
            emailTextBox = new System.Windows.Forms.TextBox();
            emailLabel = new System.Windows.Forms.Label();
            backToLoginLabel = new System.Windows.Forms.Label();
            HaveAnAccountLabel = new System.Windows.Forms.Label();
            clearButton = new System.Windows.Forms.Button();
            registerButton = new System.Windows.Forms.Button();
            lastNameTextBox = new System.Windows.Forms.TextBox();
            lastNameLabel = new System.Windows.Forms.Label();
            firstNameTextBox = new System.Windows.Forms.TextBox();
            firstNameLabel = new System.Windows.Forms.Label();
            passwordTextBox = new System.Windows.Forms.TextBox();
            PasswordLabel = new System.Windows.Forms.Label();
            usernameTextBox = new System.Windows.Forms.TextBox();
            usernameLabel = new System.Windows.Forms.Label();
            startLabel = new System.Windows.Forms.Label();
            checkThatTheCodesAreTheSameButton = new System.Windows.Forms.Button();
            enterTheCodeShownAboveTextBox = new System.Windows.Forms.TextBox();
            enterTheCodeShownAboveLabel = new System.Windows.Forms.Label();
            captchaPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)captchaPictureBox).BeginInit();
            SuspendLayout();
            // 
            // showPasswordCheckBox
            // 
            showPasswordCheckBox.AutoSize = true;
            showPasswordCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            showPasswordCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            showPasswordCheckBox.Location = new System.Drawing.Point(127, 270);
            showPasswordCheckBox.Name = "showPasswordCheckBox";
            showPasswordCheckBox.Size = new System.Drawing.Size(119, 21);
            showPasswordCheckBox.TabIndex = 52;
            showPasswordCheckBox.Text = "Show Password";
            showPasswordCheckBox.UseVisualStyleBackColor = true;
            showPasswordCheckBox.CheckedChanged += showPasswordCheckBox_CheckedChanged;
            // 
            // serverIpTextBox
            // 
            serverIpTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            serverIpTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            serverIpTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            serverIpTextBox.Location = new System.Drawing.Point(322, 162);
            serverIpTextBox.Multiline = true;
            serverIpTextBox.Name = "serverIpTextBox";
            serverIpTextBox.Size = new System.Drawing.Size(216, 25);
            serverIpTextBox.TabIndex = 51;
            // 
            // serverIpLabel
            // 
            serverIpLabel.AutoSize = true;
            serverIpLabel.Location = new System.Drawing.Point(319, 142);
            serverIpLabel.Name = "serverIpLabel";
            serverIpLabel.Size = new System.Drawing.Size(63, 17);
            serverIpLabel.TabIndex = 50;
            serverIpLabel.Text = "Server Ip";
            // 
            // genderComboBox
            // 
            genderComboBox.Font = new System.Drawing.Font("David", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            genderComboBox.FormattingEnabled = true;
            genderComboBox.Items.AddRange(new object[] { "Male", "Female" });
            genderComboBox.Location = new System.Drawing.Point(322, 381);
            genderComboBox.Name = "genderComboBox";
            genderComboBox.Size = new System.Drawing.Size(216, 23);
            genderComboBox.TabIndex = 49;
            // 
            // cityComboBox
            // 
            cityComboBox.Font = new System.Drawing.Font("David", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cityComboBox.FormattingEnabled = true;
            cityComboBox.Items.AddRange(new object[] { "Tel Aviv-Yafo", "Jerusalem", "Haifa", "Rishon LeẔiyyon", "Petaẖ Tiqwa", "Ashdod", "Netanya", "Beersheba", "Holon", "Bené Beraq", "Ramat Gan", "Ashqelon", "Reẖovot", "Bat Yam", "Bet Shemesh", "Kefar Sava", "Hadera", "Herẕliyya", "Modi‘in Makkabbim Re‘ut", "Nazareth", "Lod", "Ramla", "Ra‘ananna", "Rahat", "Qiryat Gat", "Nahariyya", "Afula", "Givatayim", "Hod HaSharon", "Rosh Ha‘Ayin", "Qiryat Ata", "Umm el Faḥm", "Eilat", "Nes Ẕiyyona", "‘Akko", "El‘ad", "Ramat HaSharon", "Karmiel", "Tiberias", "Eṭ Ṭaiyiba", "Ben Zakkay", "Pardés H̱anna Karkur", "Qiryat Moẕqin", "Qiryat Ono", "Shefar‘am", "Qiryat Bialik", "Qiryat Yam", "Or Yehuda", "Ma‘alot Tarshīḥā", "Ẕefat", "Dimona", "Tamra", "Netivot", "Sakhnīn", "Yehud", "Al Buţayḩah", "Al Khushnīyah", "Fīq" });
            cityComboBox.Location = new System.Drawing.Point(322, 309);
            cityComboBox.Name = "cityComboBox";
            cityComboBox.Size = new System.Drawing.Size(216, 23);
            cityComboBox.TabIndex = 48;
            // 
            // genderLabel
            // 
            genderLabel.AutoSize = true;
            genderLabel.Location = new System.Drawing.Point(319, 361);
            genderLabel.Name = "genderLabel";
            genderLabel.Size = new System.Drawing.Size(52, 17);
            genderLabel.TabIndex = 47;
            genderLabel.Text = "Gender";
            // 
            // cityLabel
            // 
            cityLabel.AutoSize = true;
            cityLabel.Location = new System.Drawing.Point(319, 289);
            cityLabel.Name = "cityLabel";
            cityLabel.Size = new System.Drawing.Size(32, 17);
            cityLabel.TabIndex = 46;
            cityLabel.Text = "City";
            // 
            // emailTextBox
            // 
            emailTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            emailTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            emailTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            emailTextBox.Location = new System.Drawing.Point(322, 236);
            emailTextBox.Multiline = true;
            emailTextBox.Name = "emailTextBox";
            emailTextBox.Size = new System.Drawing.Size(216, 25);
            emailTextBox.TabIndex = 45;
            emailTextBox.WordWrap = false;
            // 
            // emailLabel
            // 
            emailLabel.AutoSize = true;
            emailLabel.Location = new System.Drawing.Point(319, 216);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new System.Drawing.Size(42, 17);
            emailLabel.TabIndex = 44;
            emailLabel.Text = "Email";
            // 
            // backToLoginLabel
            // 
            backToLoginLabel.AutoSize = true;
            backToLoginLabel.ForeColor = System.Drawing.Color.FromArgb(116, 86, 174);
            backToLoginLabel.Location = new System.Drawing.Point(225, 598);
            backToLoginLabel.Name = "backToLoginLabel";
            backToLoginLabel.Size = new System.Drawing.Size(97, 17);
            backToLoginLabel.TabIndex = 43;
            backToLoginLabel.Text = "Back to LOGIN";
            backToLoginLabel.Click += backToLoginLabel_Click;
            // 
            // HaveAnAccountLabel
            // 
            HaveAnAccountLabel.AutoSize = true;
            HaveAnAccountLabel.Location = new System.Drawing.Point(198, 568);
            HaveAnAccountLabel.Name = "HaveAnAccountLabel";
            HaveAnAccountLabel.Size = new System.Drawing.Size(163, 17);
            HaveAnAccountLabel.TabIndex = 42;
            HaveAnAccountLabel.Text = "Already Have an Account";
            // 
            // clearButton
            // 
            clearButton.BackColor = System.Drawing.Color.White;
            clearButton.Cursor = System.Windows.Forms.Cursors.Hand;
            clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            clearButton.ForeColor = System.Drawing.Color.FromArgb(116, 86, 174);
            clearButton.Location = new System.Drawing.Point(172, 506);
            clearButton.Name = "clearButton";
            clearButton.Size = new System.Drawing.Size(216, 35);
            clearButton.TabIndex = 41;
            clearButton.Text = "CLEAR";
            clearButton.UseVisualStyleBackColor = false;
            clearButton.Click += clearButton_Click;
            // 
            // registerButton
            // 
            registerButton.BackColor = System.Drawing.Color.FromArgb(116, 86, 174);
            registerButton.Cursor = System.Windows.Forms.Cursors.Hand;
            registerButton.FlatAppearance.BorderSize = 0;
            registerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            registerButton.ForeColor = System.Drawing.Color.White;
            registerButton.Location = new System.Drawing.Point(172, 447);
            registerButton.Name = "registerButton";
            registerButton.Size = new System.Drawing.Size(216, 35);
            registerButton.TabIndex = 40;
            registerButton.Text = "REGISTER";
            registerButton.UseVisualStyleBackColor = false;
            registerButton.Click += registerButton_Click;
            // 
            // lastNameTextBox
            // 
            lastNameTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            lastNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            lastNameTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lastNameTextBox.Location = new System.Drawing.Point(30, 380);
            lastNameTextBox.Multiline = true;
            lastNameTextBox.Name = "lastNameTextBox";
            lastNameTextBox.Size = new System.Drawing.Size(216, 25);
            lastNameTextBox.TabIndex = 39;
            lastNameTextBox.WordWrap = false;
            // 
            // lastNameLabel
            // 
            lastNameLabel.AutoSize = true;
            lastNameLabel.Location = new System.Drawing.Point(27, 360);
            lastNameLabel.Name = "lastNameLabel";
            lastNameLabel.Size = new System.Drawing.Size(73, 17);
            lastNameLabel.TabIndex = 38;
            lastNameLabel.Text = "Last Name";
            // 
            // firstNameTextBox
            // 
            firstNameTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            firstNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            firstNameTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            firstNameTextBox.Location = new System.Drawing.Point(30, 307);
            firstNameTextBox.Multiline = true;
            firstNameTextBox.Name = "firstNameTextBox";
            firstNameTextBox.Size = new System.Drawing.Size(216, 25);
            firstNameTextBox.TabIndex = 37;
            firstNameTextBox.WordWrap = false;
            // 
            // firstNameLabel
            // 
            firstNameLabel.AutoSize = true;
            firstNameLabel.Location = new System.Drawing.Point(27, 287);
            firstNameLabel.Name = "firstNameLabel";
            firstNameLabel.Size = new System.Drawing.Size(75, 17);
            firstNameLabel.TabIndex = 36;
            firstNameLabel.Text = "First Name";
            // 
            // passwordTextBox
            // 
            passwordTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            passwordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            passwordTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            passwordTextBox.Location = new System.Drawing.Point(30, 236);
            passwordTextBox.Multiline = true;
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '•';
            passwordTextBox.Size = new System.Drawing.Size(216, 25);
            passwordTextBox.TabIndex = 35;
            passwordTextBox.WordWrap = false;
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.Location = new System.Drawing.Point(27, 216);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new System.Drawing.Size(66, 17);
            PasswordLabel.TabIndex = 34;
            PasswordLabel.Text = "Password";
            // 
            // usernameTextBox
            // 
            usernameTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            usernameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            usernameTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            usernameTextBox.Location = new System.Drawing.Point(30, 162);
            usernameTextBox.Multiline = true;
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new System.Drawing.Size(216, 25);
            usernameTextBox.TabIndex = 33;
            usernameTextBox.WordWrap = false;
            // 
            // usernameLabel
            // 
            usernameLabel.AutoSize = true;
            usernameLabel.Location = new System.Drawing.Point(27, 142);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new System.Drawing.Size(69, 17);
            usernameLabel.TabIndex = 32;
            usernameLabel.Text = "Username";
            // 
            // startLabel
            // 
            startLabel.AutoSize = true;
            startLabel.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            startLabel.ForeColor = System.Drawing.Color.FromArgb(116, 86, 174);
            startLabel.Location = new System.Drawing.Point(206, 60);
            startLabel.Name = "startLabel";
            startLabel.Size = new System.Drawing.Size(155, 27);
            startLabel.TabIndex = 31;
            startLabel.Text = "Get Started";
            // 
            // checkThatTheCodesAreTheSameButton
            // 
            checkThatTheCodesAreTheSameButton.BackColor = System.Drawing.Color.FromArgb(116, 86, 174);
            checkThatTheCodesAreTheSameButton.Cursor = System.Windows.Forms.Cursors.Hand;
            checkThatTheCodesAreTheSameButton.FlatAppearance.BorderSize = 0;
            checkThatTheCodesAreTheSameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkThatTheCodesAreTheSameButton.ForeColor = System.Drawing.Color.White;
            checkThatTheCodesAreTheSameButton.Location = new System.Drawing.Point(167, 852);
            checkThatTheCodesAreTheSameButton.Name = "checkThatTheCodesAreTheSameButton";
            checkThatTheCodesAreTheSameButton.Size = new System.Drawing.Size(216, 35);
            checkThatTheCodesAreTheSameButton.TabIndex = 56;
            checkThatTheCodesAreTheSameButton.Text = "CHECK";
            checkThatTheCodesAreTheSameButton.UseVisualStyleBackColor = false;
            checkThatTheCodesAreTheSameButton.Visible = false;
            checkThatTheCodesAreTheSameButton.Click += checkThatTheCodesAreTheSameButton_Click;
            // 
            // enterTheCodeShownAboveTextBox
            // 
            enterTheCodeShownAboveTextBox.BackColor = System.Drawing.Color.FromArgb(230, 231, 233);
            enterTheCodeShownAboveTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            enterTheCodeShownAboveTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            enterTheCodeShownAboveTextBox.Location = new System.Drawing.Point(167, 808);
            enterTheCodeShownAboveTextBox.Multiline = true;
            enterTheCodeShownAboveTextBox.Name = "enterTheCodeShownAboveTextBox";
            enterTheCodeShownAboveTextBox.Size = new System.Drawing.Size(216, 25);
            enterTheCodeShownAboveTextBox.TabIndex = 55;
            enterTheCodeShownAboveTextBox.Visible = false;
            enterTheCodeShownAboveTextBox.WordWrap = false;
            // 
            // enterTheCodeShownAboveLabel
            // 
            enterTheCodeShownAboveLabel.AutoSize = true;
            enterTheCodeShownAboveLabel.Location = new System.Drawing.Point(164, 778);
            enterTheCodeShownAboveLabel.Name = "enterTheCodeShownAboveLabel";
            enterTheCodeShownAboveLabel.Size = new System.Drawing.Size(154, 17);
            enterTheCodeShownAboveLabel.TabIndex = 54;
            enterTheCodeShownAboveLabel.Text = "The Code Shown Above";
            enterTheCodeShownAboveLabel.Visible = false;
            // 
            // captchaPictureBox
            // 
            captchaPictureBox.Location = new System.Drawing.Point(172, 635);
            captchaPictureBox.Name = "captchaPictureBox";
            captchaPictureBox.Size = new System.Drawing.Size(211, 113);
            captchaPictureBox.TabIndex = 53;
            captchaPictureBox.TabStop = false;
            captchaPictureBox.Visible = false;
            // 
            // RegistrationForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(595, 930);
            Controls.Add(checkThatTheCodesAreTheSameButton);
            Controls.Add(enterTheCodeShownAboveTextBox);
            Controls.Add(enterTheCodeShownAboveLabel);
            Controls.Add(captchaPictureBox);
            Controls.Add(showPasswordCheckBox);
            Controls.Add(serverIpTextBox);
            Controls.Add(serverIpLabel);
            Controls.Add(genderComboBox);
            Controls.Add(cityComboBox);
            Controls.Add(genderLabel);
            Controls.Add(cityLabel);
            Controls.Add(emailTextBox);
            Controls.Add(emailLabel);
            Controls.Add(backToLoginLabel);
            Controls.Add(HaveAnAccountLabel);
            Controls.Add(clearButton);
            Controls.Add(registerButton);
            Controls.Add(lastNameTextBox);
            Controls.Add(lastNameLabel);
            Controls.Add(firstNameTextBox);
            Controls.Add(firstNameLabel);
            Controls.Add(passwordTextBox);
            Controls.Add(PasswordLabel);
            Controls.Add(usernameTextBox);
            Controls.Add(usernameLabel);
            Controls.Add(startLabel);
            Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            ForeColor = System.Drawing.Color.FromArgb(164, 165, 169);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "RegistrationForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "RegistrationForm";
            ((System.ComponentModel.ISupportInitialize)captchaPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.CheckBox showPasswordCheckBox;
        private System.Windows.Forms.TextBox serverIpTextBox;
        private System.Windows.Forms.Label serverIpLabel;
        private System.Windows.Forms.ComboBox genderComboBox;
        private System.Windows.Forms.ComboBox cityComboBox;
        private System.Windows.Forms.Label genderLabel;
        private System.Windows.Forms.Label cityLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.Label backToLoginLabel;
        private System.Windows.Forms.Label HaveAnAccountLabel;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.TextBox lastNameTextBox;
        private System.Windows.Forms.Label lastNameLabel;
        private System.Windows.Forms.TextBox firstNameTextBox;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Button checkThatTheCodesAreTheSameButton;
        private System.Windows.Forms.TextBox enterTheCodeShownAboveTextBox;
        private System.Windows.Forms.Label enterTheCodeShownAboveLabel;
        private System.Windows.Forms.PictureBox captchaPictureBox;
    }
}