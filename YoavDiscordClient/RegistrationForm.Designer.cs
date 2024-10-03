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
            this.showPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.serverIpTextBox = new System.Windows.Forms.TextBox();
            this.serverIpLabel = new System.Windows.Forms.Label();
            this.genderComboBox = new System.Windows.Forms.ComboBox();
            this.cityComboBox = new System.Windows.Forms.ComboBox();
            this.genderLabel = new System.Windows.Forms.Label();
            this.cityLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.backToLoginLabel = new System.Windows.Forms.Label();
            this.HaveAnAccountLabel = new System.Windows.Forms.Label();
            this.clearButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.startLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // showPasswordCheckBox
            // 
            this.showPasswordCheckBox.AutoSize = true;
            this.showPasswordCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.showPasswordCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showPasswordCheckBox.Location = new System.Drawing.Point(127, 270);
            this.showPasswordCheckBox.Name = "showPasswordCheckBox";
            this.showPasswordCheckBox.Size = new System.Drawing.Size(119, 21);
            this.showPasswordCheckBox.TabIndex = 52;
            this.showPasswordCheckBox.Text = "Show Password";
            this.showPasswordCheckBox.UseVisualStyleBackColor = true;
            this.showPasswordCheckBox.CheckedChanged += new System.EventHandler(this.showPasswordCheckBox_CheckedChanged);
            // 
            // serverIpTextBox
            // 
            this.serverIpTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.serverIpTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.serverIpTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverIpTextBox.Location = new System.Drawing.Point(322, 162);
            this.serverIpTextBox.Multiline = true;
            this.serverIpTextBox.Name = "serverIpTextBox";
            this.serverIpTextBox.Size = new System.Drawing.Size(216, 25);
            this.serverIpTextBox.TabIndex = 51;
            // 
            // serverIpLabel
            // 
            this.serverIpLabel.AutoSize = true;
            this.serverIpLabel.Location = new System.Drawing.Point(319, 142);
            this.serverIpLabel.Name = "serverIpLabel";
            this.serverIpLabel.Size = new System.Drawing.Size(63, 17);
            this.serverIpLabel.TabIndex = 50;
            this.serverIpLabel.Text = "Server Ip";
            // 
            // genderComboBox
            // 
            this.genderComboBox.Font = new System.Drawing.Font("David", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genderComboBox.FormattingEnabled = true;
            this.genderComboBox.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.genderComboBox.Location = new System.Drawing.Point(322, 381);
            this.genderComboBox.Name = "genderComboBox";
            this.genderComboBox.Size = new System.Drawing.Size(216, 23);
            this.genderComboBox.TabIndex = 49;
            // 
            // cityComboBox
            // 
            this.cityComboBox.Font = new System.Drawing.Font("David", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cityComboBox.FormattingEnabled = true;
            this.cityComboBox.Items.AddRange(new object[] {
            "Tel Aviv-Yafo",
            "Jerusalem",
            "Haifa",
            "Rishon LeẔiyyon",
            "Petaẖ Tiqwa",
            "Ashdod",
            "Netanya",
            "Beersheba",
            "Holon",
            "Bené Beraq",
            "Ramat Gan",
            "Ashqelon",
            "Reẖovot",
            "Bat Yam",
            "Bet Shemesh",
            "Kefar Sava",
            "Hadera",
            "Herẕliyya",
            "Modi‘in Makkabbim Re‘ut",
            "Nazareth",
            "Lod",
            "Ramla",
            "Ra‘ananna",
            "Rahat",
            "Qiryat Gat",
            "Nahariyya",
            "Afula",
            "Givatayim",
            "Hod HaSharon",
            "Rosh Ha‘Ayin",
            "Qiryat Ata",
            "Umm el Faḥm",
            "Eilat",
            "Nes Ẕiyyona",
            "‘Akko",
            "El‘ad",
            "Ramat HaSharon",
            "Karmiel",
            "Tiberias",
            "Eṭ Ṭaiyiba",
            "Ben Zakkay",
            "Pardés H̱anna Karkur",
            "Qiryat Moẕqin",
            "Qiryat Ono",
            "Shefar‘am",
            "Qiryat Bialik",
            "Qiryat Yam",
            "Or Yehuda",
            "Ma‘alot Tarshīḥā",
            "Ẕefat",
            "Dimona",
            "Tamra",
            "Netivot",
            "Sakhnīn",
            "Yehud",
            "Al Buţayḩah",
            "Al Khushnīyah",
            "Fīq"});
            this.cityComboBox.Location = new System.Drawing.Point(322, 309);
            this.cityComboBox.Name = "cityComboBox";
            this.cityComboBox.Size = new System.Drawing.Size(216, 23);
            this.cityComboBox.TabIndex = 48;
            // 
            // genderLabel
            // 
            this.genderLabel.AutoSize = true;
            this.genderLabel.Location = new System.Drawing.Point(319, 361);
            this.genderLabel.Name = "genderLabel";
            this.genderLabel.Size = new System.Drawing.Size(52, 17);
            this.genderLabel.TabIndex = 47;
            this.genderLabel.Text = "Gender";
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.Location = new System.Drawing.Point(319, 289);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(32, 17);
            this.cityLabel.TabIndex = 46;
            this.cityLabel.Text = "City";
            // 
            // emailTextBox
            // 
            this.emailTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.emailTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.emailTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailTextBox.Location = new System.Drawing.Point(322, 236);
            this.emailTextBox.Multiline = true;
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(216, 25);
            this.emailTextBox.TabIndex = 45;
            this.emailTextBox.WordWrap = false;
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(319, 216);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(42, 17);
            this.emailLabel.TabIndex = 44;
            this.emailLabel.Text = "Email";
            // 
            // backToLoginLabel
            // 
            this.backToLoginLabel.AutoSize = true;
            this.backToLoginLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.backToLoginLabel.Location = new System.Drawing.Point(225, 598);
            this.backToLoginLabel.Name = "backToLoginLabel";
            this.backToLoginLabel.Size = new System.Drawing.Size(97, 17);
            this.backToLoginLabel.TabIndex = 43;
            this.backToLoginLabel.Text = "Back to LOGIN";
            this.backToLoginLabel.Click += new System.EventHandler(this.backToLoginLabel_Click);
            // 
            // HaveAnAccountLabel
            // 
            this.HaveAnAccountLabel.AutoSize = true;
            this.HaveAnAccountLabel.Location = new System.Drawing.Point(198, 568);
            this.HaveAnAccountLabel.Name = "HaveAnAccountLabel";
            this.HaveAnAccountLabel.Size = new System.Drawing.Size(163, 17);
            this.HaveAnAccountLabel.TabIndex = 42;
            this.HaveAnAccountLabel.Text = "Already Have an Account";
            // 
            // clearButton
            // 
            this.clearButton.BackColor = System.Drawing.Color.White;
            this.clearButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.clearButton.Location = new System.Drawing.Point(172, 506);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(216, 35);
            this.clearButton.TabIndex = 41;
            this.clearButton.Text = "CLEAR";
            this.clearButton.UseVisualStyleBackColor = false;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // registerButton
            // 
            this.registerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.registerButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.registerButton.FlatAppearance.BorderSize = 0;
            this.registerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.registerButton.ForeColor = System.Drawing.Color.White;
            this.registerButton.Location = new System.Drawing.Point(172, 447);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(216, 35);
            this.registerButton.TabIndex = 40;
            this.registerButton.Text = "REGISTER";
            this.registerButton.UseVisualStyleBackColor = false;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // lastNameTextBox
            // 
            this.lastNameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.lastNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lastNameTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastNameTextBox.Location = new System.Drawing.Point(30, 380);
            this.lastNameTextBox.Multiline = true;
            this.lastNameTextBox.Name = "lastNameTextBox";
            this.lastNameTextBox.Size = new System.Drawing.Size(216, 25);
            this.lastNameTextBox.TabIndex = 39;
            this.lastNameTextBox.WordWrap = false;
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(27, 360);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(73, 17);
            this.lastNameLabel.TabIndex = 38;
            this.lastNameLabel.Text = "Last Name";
            // 
            // firstNameTextBox
            // 
            this.firstNameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.firstNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.firstNameTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstNameTextBox.Location = new System.Drawing.Point(30, 307);
            this.firstNameTextBox.Multiline = true;
            this.firstNameTextBox.Name = "firstNameTextBox";
            this.firstNameTextBox.Size = new System.Drawing.Size(216, 25);
            this.firstNameTextBox.TabIndex = 37;
            this.firstNameTextBox.WordWrap = false;
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(27, 287);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(75, 17);
            this.firstNameLabel.TabIndex = 36;
            this.firstNameLabel.Text = "First Name";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.passwordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.passwordTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.Location = new System.Drawing.Point(30, 236);
            this.passwordTextBox.Multiline = true;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '•';
            this.passwordTextBox.Size = new System.Drawing.Size(216, 25);
            this.passwordTextBox.TabIndex = 35;
            this.passwordTextBox.WordWrap = false;
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(27, 216);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(66, 17);
            this.PasswordLabel.TabIndex = 34;
            this.PasswordLabel.Text = "Password";
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            this.usernameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.usernameTextBox.Font = new System.Drawing.Font("David", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameTextBox.Location = new System.Drawing.Point(30, 162);
            this.usernameTextBox.Multiline = true;
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(216, 25);
            this.usernameTextBox.TabIndex = 33;
            this.usernameTextBox.WordWrap = false;
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(27, 142);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(69, 17);
            this.usernameLabel.TabIndex = 32;
            this.usernameLabel.Text = "Username";
            // 
            // startLabel
            // 
            this.startLabel.AutoSize = true;
            this.startLabel.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.startLabel.Location = new System.Drawing.Point(206, 60);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(155, 27);
            this.startLabel.TabIndex = 31;
            this.startLabel.Text = "Get Started";
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(595, 645);
            this.Controls.Add(this.showPasswordCheckBox);
            this.Controls.Add(this.serverIpTextBox);
            this.Controls.Add(this.serverIpLabel);
            this.Controls.Add(this.genderComboBox);
            this.Controls.Add(this.cityComboBox);
            this.Controls.Add(this.genderLabel);
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.backToLoginLabel);
            this.Controls.Add(this.HaveAnAccountLabel);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.lastNameTextBox);
            this.Controls.Add(this.lastNameLabel);
            this.Controls.Add(this.firstNameTextBox);
            this.Controls.Add(this.firstNameLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.startLabel);
            this.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "RegistrationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RegistrationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}