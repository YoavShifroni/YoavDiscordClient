namespace YoavDiscordClient
{
    partial class DiscordApp
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
            this.leftSidePanel = new System.Windows.Forms.Panel();
            this.userInfoPanel = new System.Windows.Forms.Panel();
            this.muteButton = new System.Windows.Forms.Button();
            this.deafenButton = new System.Windows.Forms.Button();
            this.userProfilePicturePictureBox = new YoavDiscordClient.CirclePictureBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.settingsButton = new System.Windows.Forms.Button();
            this.voiceChannel3Button = new System.Windows.Forms.Button();
            this.voiceChannel2Button = new System.Windows.Forms.Button();
            this.voiceChannel1Button = new System.Windows.Forms.Button();
            this.voiceChannelsLabel = new System.Windows.Forms.Label();
            this.textChanel3Button = new System.Windows.Forms.Button();
            this.textChanel2Button = new System.Windows.Forms.Button();
            this.textChanel1Button = new System.Windows.Forms.Button();
            this.textChannelsLabel = new System.Windows.Forms.Label();
            this.rightSidePanel = new System.Windows.Forms.Panel();
            this.offlineUsersLabel = new System.Windows.Forms.Label();
            this.onlineUsersLabel = new System.Windows.Forms.Label();
            this.chatAreaPanel = new System.Windows.Forms.Panel();
            this.ChatMessagesPanel1 = new System.Windows.Forms.Panel();
            this.ChatMessagesPanel2 = new System.Windows.Forms.Panel();
            this.ChatMessagesPanel3 = new System.Windows.Forms.Panel();
            this.sendMessageButton = new System.Windows.Forms.Button();
            this.messageInputTextBox = new System.Windows.Forms.TextBox();
            this.nameOfTheProjectLabel = new System.Windows.Forms.Label();
            this.leftSidePanel.SuspendLayout();
            this.userInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userProfilePicturePictureBox)).BeginInit();
            this.rightSidePanel.SuspendLayout();
            this.chatAreaPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftSidePanel
            // 
            this.leftSidePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(63)))));
            this.leftSidePanel.Controls.Add(this.userInfoPanel);
            this.leftSidePanel.Controls.Add(this.voiceChannel3Button);
            this.leftSidePanel.Controls.Add(this.voiceChannel2Button);
            this.leftSidePanel.Controls.Add(this.voiceChannel1Button);
            this.leftSidePanel.Controls.Add(this.voiceChannelsLabel);
            this.leftSidePanel.Controls.Add(this.textChanel3Button);
            this.leftSidePanel.Controls.Add(this.textChanel2Button);
            this.leftSidePanel.Controls.Add(this.textChanel1Button);
            this.leftSidePanel.Controls.Add(this.textChannelsLabel);
            this.leftSidePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftSidePanel.Location = new System.Drawing.Point(0, 0);
            this.leftSidePanel.Name = "leftSidePanel";
            this.leftSidePanel.Size = new System.Drawing.Size(471, 1209);
            this.leftSidePanel.TabIndex = 0;
            // 
            // userInfoPanel
            // 
            this.userInfoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.userInfoPanel.Controls.Add(this.muteButton);
            this.userInfoPanel.Controls.Add(this.deafenButton);
            this.userInfoPanel.Controls.Add(this.userProfilePicturePictureBox);
            this.userInfoPanel.Controls.Add(this.usernameLabel);
            this.userInfoPanel.Controls.Add(this.settingsButton);
            this.userInfoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userInfoPanel.Location = new System.Drawing.Point(0, 1068);
            this.userInfoPanel.Name = "userInfoPanel";
            this.userInfoPanel.Size = new System.Drawing.Size(471, 141);
            this.userInfoPanel.TabIndex = 4;
            // 
            // muteButton
            // 
            this.muteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.muteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.muteButton.Location = new System.Drawing.Point(279, 76);
            this.muteButton.Name = "muteButton";
            this.muteButton.Size = new System.Drawing.Size(47, 47);
            this.muteButton.TabIndex = 14;
            this.muteButton.UseVisualStyleBackColor = true;
            // 
            // deafenButton
            // 
            this.deafenButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.deafenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deafenButton.Location = new System.Drawing.Point(354, 76);
            this.deafenButton.Name = "deafenButton";
            this.deafenButton.Size = new System.Drawing.Size(47, 47);
            this.deafenButton.TabIndex = 13;
            this.deafenButton.UseVisualStyleBackColor = true;
            // 
            // userProfilePicturePictureBox
            // 
            this.userProfilePicturePictureBox.BackColor = System.Drawing.Color.Gray;
            this.userProfilePicturePictureBox.Location = new System.Drawing.Point(12, 37);
            this.userProfilePicturePictureBox.Name = "userProfilePicturePictureBox";
            this.userProfilePicturePictureBox.Size = new System.Drawing.Size(105, 86);
            this.userProfilePicturePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.userProfilePicturePictureBox.TabIndex = 12;
            this.userProfilePicturePictureBox.TabStop = false;
            // 
            // usernameLabel
            // 
            this.usernameLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameLabel.ForeColor = System.Drawing.Color.White;
            this.usernameLabel.Location = new System.Drawing.Point(114, 67);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(193, 23);
            this.usernameLabel.TabIndex = 11;
            this.usernameLabel.Text = "Username";
            this.usernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // settingsButton
            // 
            this.settingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.Location = new System.Drawing.Point(418, 76);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(47, 47);
            this.settingsButton.TabIndex = 4;
            this.settingsButton.UseVisualStyleBackColor = true;
            // 
            // voiceChannel3Button
            // 
            this.voiceChannel3Button.AutoSize = true;
            this.voiceChannel3Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.voiceChannel3Button.FlatAppearance.BorderSize = 0;
            this.voiceChannel3Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.voiceChannel3Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.voiceChannel3Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.voiceChannel3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.voiceChannel3Button.ForeColor = System.Drawing.Color.White;
            this.voiceChannel3Button.Location = new System.Drawing.Point(73, 596);
            this.voiceChannel3Button.Name = "voiceChannel3Button";
            this.voiceChannel3Button.Size = new System.Drawing.Size(328, 35);
            this.voiceChannel3Button.TabIndex = 10;
            this.voiceChannel3Button.TabStop = false;
            this.voiceChannel3Button.Text = "Voice Chanel 3";
            this.voiceChannel3Button.UseVisualStyleBackColor = false;
            // 
            // voiceChannel2Button
            // 
            this.voiceChannel2Button.AutoSize = true;
            this.voiceChannel2Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.voiceChannel2Button.FlatAppearance.BorderSize = 0;
            this.voiceChannel2Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.voiceChannel2Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.voiceChannel2Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.voiceChannel2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.voiceChannel2Button.ForeColor = System.Drawing.Color.White;
            this.voiceChannel2Button.Location = new System.Drawing.Point(73, 512);
            this.voiceChannel2Button.Name = "voiceChannel2Button";
            this.voiceChannel2Button.Size = new System.Drawing.Size(328, 35);
            this.voiceChannel2Button.TabIndex = 9;
            this.voiceChannel2Button.TabStop = false;
            this.voiceChannel2Button.Text = "Voice Chanel 2";
            this.voiceChannel2Button.UseVisualStyleBackColor = false;
            // 
            // voiceChannel1Button
            // 
            this.voiceChannel1Button.AutoSize = true;
            this.voiceChannel1Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.voiceChannel1Button.FlatAppearance.BorderSize = 0;
            this.voiceChannel1Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.voiceChannel1Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.voiceChannel1Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.voiceChannel1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.voiceChannel1Button.ForeColor = System.Drawing.Color.White;
            this.voiceChannel1Button.Location = new System.Drawing.Point(73, 439);
            this.voiceChannel1Button.Name = "voiceChannel1Button";
            this.voiceChannel1Button.Size = new System.Drawing.Size(328, 35);
            this.voiceChannel1Button.TabIndex = 7;
            this.voiceChannel1Button.TabStop = false;
            this.voiceChannel1Button.Text = "Voice Chanel 1";
            this.voiceChannel1Button.UseVisualStyleBackColor = false;
            // 
            // voiceChannelsLabel
            // 
            this.voiceChannelsLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.voiceChannelsLabel.ForeColor = System.Drawing.Color.White;
            this.voiceChannelsLabel.Location = new System.Drawing.Point(3, 387);
            this.voiceChannelsLabel.Name = "voiceChannelsLabel";
            this.voiceChannelsLabel.Size = new System.Drawing.Size(207, 23);
            this.voiceChannelsLabel.TabIndex = 8;
            this.voiceChannelsLabel.Text = "Voice Channels ▼";
            this.voiceChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textChanel3Button
            // 
            this.textChanel3Button.AutoSize = true;
            this.textChanel3Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.textChanel3Button.FlatAppearance.BorderSize = 0;
            this.textChanel3Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.textChanel3Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.textChanel3Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.textChanel3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textChanel3Button.ForeColor = System.Drawing.Color.White;
            this.textChanel3Button.Location = new System.Drawing.Point(73, 295);
            this.textChanel3Button.Name = "textChanel3Button";
            this.textChanel3Button.Size = new System.Drawing.Size(328, 35);
            this.textChanel3Button.TabIndex = 6;
            this.textChanel3Button.TabStop = false;
            this.textChanel3Button.Tag = "3";
            this.textChanel3Button.Text = "Text Chanel 3";
            this.textChanel3Button.UseVisualStyleBackColor = false;
            this.textChanel3Button.Click += new System.EventHandler(this.textChanel3Button_Click);
            // 
            // textChanel2Button
            // 
            this.textChanel2Button.AutoSize = true;
            this.textChanel2Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.textChanel2Button.FlatAppearance.BorderSize = 0;
            this.textChanel2Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.textChanel2Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.textChanel2Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.textChanel2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textChanel2Button.ForeColor = System.Drawing.Color.White;
            this.textChanel2Button.Location = new System.Drawing.Point(73, 211);
            this.textChanel2Button.Name = "textChanel2Button";
            this.textChanel2Button.Size = new System.Drawing.Size(328, 35);
            this.textChanel2Button.TabIndex = 5;
            this.textChanel2Button.TabStop = false;
            this.textChanel2Button.Tag = "2";
            this.textChanel2Button.Text = "Text Chanel 2";
            this.textChanel2Button.UseVisualStyleBackColor = false;
            this.textChanel2Button.Click += new System.EventHandler(this.textChanel2Button_Click);
            // 
            // textChanel1Button
            // 
            this.textChanel1Button.AutoSize = true;
            this.textChanel1Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.textChanel1Button.FlatAppearance.BorderSize = 0;
            this.textChanel1Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.textChanel1Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.textChanel1Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.textChanel1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textChanel1Button.ForeColor = System.Drawing.Color.White;
            this.textChanel1Button.Location = new System.Drawing.Point(73, 138);
            this.textChanel1Button.Name = "textChanel1Button";
            this.textChanel1Button.Size = new System.Drawing.Size(328, 37);
            this.textChanel1Button.TabIndex = 4;
            this.textChanel1Button.TabStop = false;
            this.textChanel1Button.Tag = "1";
            this.textChanel1Button.Text = "Text Chanel 1";
            this.textChanel1Button.UseVisualStyleBackColor = false;
            this.textChanel1Button.Click += new System.EventHandler(this.textChanel1Button_Click);
            // 
            // textChannelsLabel
            // 
            this.textChannelsLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textChannelsLabel.ForeColor = System.Drawing.Color.White;
            this.textChannelsLabel.Location = new System.Drawing.Point(3, 86);
            this.textChannelsLabel.Name = "textChannelsLabel";
            this.textChannelsLabel.Size = new System.Drawing.Size(207, 23);
            this.textChannelsLabel.TabIndex = 4;
            this.textChannelsLabel.Text = "Text Channels ▼";
            this.textChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rightSidePanel
            // 
            this.rightSidePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(63)))));
            this.rightSidePanel.Controls.Add(this.offlineUsersLabel);
            this.rightSidePanel.Controls.Add(this.onlineUsersLabel);
            this.rightSidePanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightSidePanel.Location = new System.Drawing.Point(1771, 0);
            this.rightSidePanel.Name = "rightSidePanel";
            this.rightSidePanel.Size = new System.Drawing.Size(388, 1209);
            this.rightSidePanel.TabIndex = 2;
            // 
            // offlineUsersLabel
            // 
            this.offlineUsersLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offlineUsersLabel.ForeColor = System.Drawing.Color.White;
            this.offlineUsersLabel.Location = new System.Drawing.Point(6, 557);
            this.offlineUsersLabel.Name = "offlineUsersLabel";
            this.offlineUsersLabel.Size = new System.Drawing.Size(207, 23);
            this.offlineUsersLabel.TabIndex = 12;
            this.offlineUsersLabel.Text = "Offline - (number)";
            this.offlineUsersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // onlineUsersLabel
            // 
            this.onlineUsersLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onlineUsersLabel.ForeColor = System.Drawing.Color.White;
            this.onlineUsersLabel.Location = new System.Drawing.Point(6, 56);
            this.onlineUsersLabel.Name = "onlineUsersLabel";
            this.onlineUsersLabel.Size = new System.Drawing.Size(207, 23);
            this.onlineUsersLabel.TabIndex = 11;
            this.onlineUsersLabel.Text = "Online - (number)";
            this.onlineUsersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chatAreaPanel
            // 
            this.chatAreaPanel.AutoScroll = true;
            this.chatAreaPanel.Controls.Add(this.ChatMessagesPanel1);
            this.chatAreaPanel.Controls.Add(this.ChatMessagesPanel2);
            this.chatAreaPanel.Controls.Add(this.ChatMessagesPanel3);
            this.chatAreaPanel.Controls.Add(this.sendMessageButton);
            this.chatAreaPanel.Controls.Add(this.messageInputTextBox);
            this.chatAreaPanel.Controls.Add(this.nameOfTheProjectLabel);
            this.chatAreaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatAreaPanel.Location = new System.Drawing.Point(471, 0);
            this.chatAreaPanel.Name = "chatAreaPanel";
            this.chatAreaPanel.Size = new System.Drawing.Size(1300, 1209);
            this.chatAreaPanel.TabIndex = 4;
            // 
            // ChatMessagesPanel1
            // 
            this.ChatMessagesPanel1.Location = new System.Drawing.Point(0, 86);
            this.ChatMessagesPanel1.Name = "ChatMessagesPanel1";
            this.ChatMessagesPanel1.Size = new System.Drawing.Size(1300, 1043);
            this.ChatMessagesPanel1.TabIndex = 15;
            this.ChatMessagesPanel1.Tag = "0";
            // 
            // ChatMessagesPanel2
            // 
            this.ChatMessagesPanel2.Location = new System.Drawing.Point(0, 86);
            this.ChatMessagesPanel2.Name = "ChatMessagesPanel2";
            this.ChatMessagesPanel2.Size = new System.Drawing.Size(1300, 1043);
            this.ChatMessagesPanel2.TabIndex = 15;
            this.ChatMessagesPanel2.Tag = "0";
            this.ChatMessagesPanel2.Visible = false;
            // 
            // ChatMessagesPanel3
            // 
            this.ChatMessagesPanel3.Location = new System.Drawing.Point(0, 86);
            this.ChatMessagesPanel3.Name = "ChatMessagesPanel3";
            this.ChatMessagesPanel3.Size = new System.Drawing.Size(1300, 1043);
            this.ChatMessagesPanel3.TabIndex = 15;
            this.ChatMessagesPanel3.Tag = "0";
            this.ChatMessagesPanel3.Visible = false;
            // 
            // sendMessageButton
            // 
            this.sendMessageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(57)))), ((int)(((byte)(63)))));
            this.sendMessageButton.FlatAppearance.BorderSize = 0;
            this.sendMessageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendMessageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendMessageButton.ForeColor = System.Drawing.Color.White;
            this.sendMessageButton.Location = new System.Drawing.Point(1182, 1135);
            this.sendMessageButton.Name = "sendMessageButton";
            this.sendMessageButton.Size = new System.Drawing.Size(102, 49);
            this.sendMessageButton.TabIndex = 14;
            this.sendMessageButton.Text = "Send";
            this.sendMessageButton.UseVisualStyleBackColor = false;
            this.sendMessageButton.Click += new System.EventHandler(this.sendMessageButton_Click);
            // 
            // messageInputTextBox
            // 
            this.messageInputTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(68)))), ((int)(((byte)(75)))));
            this.messageInputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.messageInputTextBox.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageInputTextBox.ForeColor = System.Drawing.Color.White;
            this.messageInputTextBox.Location = new System.Drawing.Point(6, 1144);
            this.messageInputTextBox.Multiline = true;
            this.messageInputTextBox.Name = "messageInputTextBox";
            this.messageInputTextBox.Size = new System.Drawing.Size(1155, 40);
            this.messageInputTextBox.TabIndex = 13;
            // 
            // nameOfTheProjectLabel
            // 
            this.nameOfTheProjectLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.nameOfTheProjectLabel.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameOfTheProjectLabel.ForeColor = System.Drawing.Color.White;
            this.nameOfTheProjectLabel.Location = new System.Drawing.Point(0, 0);
            this.nameOfTheProjectLabel.Name = "nameOfTheProjectLabel";
            this.nameOfTheProjectLabel.Size = new System.Drawing.Size(1300, 48);
            this.nameOfTheProjectLabel.TabIndex = 12;
            this.nameOfTheProjectLabel.Text = "Yoav Discord";
            this.nameOfTheProjectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DiscordApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(49)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(2159, 1209);
            this.Controls.Add(this.chatAreaPanel);
            this.Controls.Add(this.rightSidePanel);
            this.Controls.Add(this.leftSidePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DiscordApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DiscordApp";
            this.Load += new System.EventHandler(this.DiscordApp_Load);
            this.leftSidePanel.ResumeLayout(false);
            this.leftSidePanel.PerformLayout();
            this.userInfoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userProfilePicturePictureBox)).EndInit();
            this.rightSidePanel.ResumeLayout(false);
            this.chatAreaPanel.ResumeLayout(false);
            this.chatAreaPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel leftSidePanel;
        private System.Windows.Forms.Panel rightSidePanel;
        private System.Windows.Forms.Label textChannelsLabel;
        private System.Windows.Forms.Button textChanel1Button;
        private System.Windows.Forms.Button textChanel3Button;
        private System.Windows.Forms.Button textChanel2Button;
        private System.Windows.Forms.Button voiceChannel3Button;
        private System.Windows.Forms.Button voiceChannel2Button;
        private System.Windows.Forms.Button voiceChannel1Button;
        private System.Windows.Forms.Label voiceChannelsLabel;
        private System.Windows.Forms.Panel userInfoPanel;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Panel chatAreaPanel;
        private System.Windows.Forms.Label nameOfTheProjectLabel;
        private System.Windows.Forms.Label onlineUsersLabel;
        private System.Windows.Forms.Label offlineUsersLabel;
        private System.Windows.Forms.TextBox messageInputTextBox;
        private System.Windows.Forms.Button sendMessageButton;
        private CirclePictureBox userProfilePicturePictureBox;
        private System.Windows.Forms.Button muteButton;
        private System.Windows.Forms.Button deafenButton;
        private System.Windows.Forms.Panel ChatMessagesPanel1;
        private System.Windows.Forms.Panel ChatMessagesPanel2;
        private System.Windows.Forms.Panel ChatMessagesPanel3;

    }
}