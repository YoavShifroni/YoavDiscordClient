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
            leftSidePanel = new System.Windows.Forms.Panel();
            userInfoPanel = new System.Windows.Forms.Panel();
            muteButton = new System.Windows.Forms.Button();
            deafenButton = new System.Windows.Forms.Button();
            userProfilePicturePictureBox = new CirclePictureBox();
            usernameLabel = new System.Windows.Forms.Label();
            settingsButton = new System.Windows.Forms.Button();
            voiceChannel3Button = new System.Windows.Forms.Button();
            voiceChannel2Button = new System.Windows.Forms.Button();
            voiceChannel1Button = new System.Windows.Forms.Button();
            voiceChannelsLabel = new System.Windows.Forms.Label();
            textChannel3Button = new System.Windows.Forms.Button();
            textChannel2Button = new System.Windows.Forms.Button();
            textChannel1Button = new System.Windows.Forms.Button();
            textChannelsLabel = new System.Windows.Forms.Label();
            rightSidePanel = new System.Windows.Forms.Panel();
            offlineUsersLabel = new System.Windows.Forms.Label();
            onlineUsersLabel = new System.Windows.Forms.Label();
            chatAreaPanel = new System.Windows.Forms.Panel();
            ChatMessagesPanel1 = new System.Windows.Forms.Panel();
            ChatMessagesPanel2 = new System.Windows.Forms.Panel();
            ChatMessagesPanel3 = new System.Windows.Forms.Panel();
            VideoPanel1 = new System.Windows.Forms.Panel();
            VideoPanel2 = new System.Windows.Forms.Panel();
            VideoPanel3 = new System.Windows.Forms.Panel();
            sendMessageButton = new System.Windows.Forms.Button();
            messageInputTextBox = new System.Windows.Forms.TextBox();
            nameOfTheProjectLabel = new System.Windows.Forms.Label();
            leftSidePanel.SuspendLayout();
            userInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)userProfilePicturePictureBox).BeginInit();
            rightSidePanel.SuspendLayout();
            chatAreaPanel.SuspendLayout();
            SuspendLayout();
            // 
            // leftSidePanel
            // 
            leftSidePanel.BackColor = System.Drawing.Color.FromArgb(54, 57, 63);
            leftSidePanel.Controls.Add(userInfoPanel);
            leftSidePanel.Controls.Add(voiceChannel3Button);
            leftSidePanel.Controls.Add(voiceChannel2Button);
            leftSidePanel.Controls.Add(voiceChannel1Button);
            leftSidePanel.Controls.Add(voiceChannelsLabel);
            leftSidePanel.Controls.Add(textChannel3Button);
            leftSidePanel.Controls.Add(textChannel2Button);
            leftSidePanel.Controls.Add(textChannel1Button);
            leftSidePanel.Controls.Add(textChannelsLabel);
            leftSidePanel.Dock = System.Windows.Forms.DockStyle.Left;
            leftSidePanel.Location = new System.Drawing.Point(0, 0);
            leftSidePanel.Name = "leftSidePanel";
            leftSidePanel.Size = new System.Drawing.Size(471, 1209);
            leftSidePanel.TabIndex = 0;
            // 
            // userInfoPanel
            // 
            userInfoPanel.BackColor = System.Drawing.Color.FromArgb(44, 47, 51);
            userInfoPanel.Controls.Add(muteButton);
            userInfoPanel.Controls.Add(deafenButton);
            userInfoPanel.Controls.Add(userProfilePicturePictureBox);
            userInfoPanel.Controls.Add(usernameLabel);
            userInfoPanel.Controls.Add(settingsButton);
            userInfoPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userInfoPanel.Location = new System.Drawing.Point(0, 1068);
            userInfoPanel.Name = "userInfoPanel";
            userInfoPanel.Size = new System.Drawing.Size(471, 141);
            userInfoPanel.TabIndex = 4;
            // 
            // muteButton
            // 
            muteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            muteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            muteButton.Location = new System.Drawing.Point(279, 76);
            muteButton.Name = "muteButton";
            muteButton.Size = new System.Drawing.Size(47, 47);
            muteButton.TabIndex = 14;
            muteButton.UseVisualStyleBackColor = true;
            // 
            // deafenButton
            // 
            deafenButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            deafenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            deafenButton.Location = new System.Drawing.Point(354, 76);
            deafenButton.Name = "deafenButton";
            deafenButton.Size = new System.Drawing.Size(47, 47);
            deafenButton.TabIndex = 13;
            deafenButton.UseVisualStyleBackColor = true;
            // 
            // userProfilePicturePictureBox
            // 
            userProfilePicturePictureBox.BackColor = System.Drawing.Color.Gray;
            userProfilePicturePictureBox.Location = new System.Drawing.Point(12, 37);
            userProfilePicturePictureBox.Name = "userProfilePicturePictureBox";
            userProfilePicturePictureBox.Size = new System.Drawing.Size(105, 86);
            userProfilePicturePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            userProfilePicturePictureBox.TabIndex = 12;
            userProfilePicturePictureBox.TabStop = false;
            // 
            // usernameLabel
            // 
            usernameLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            usernameLabel.ForeColor = System.Drawing.Color.White;
            usernameLabel.Location = new System.Drawing.Point(114, 67);
            usernameLabel.Name = "usernameLabel";
            usernameLabel.Size = new System.Drawing.Size(193, 23);
            usernameLabel.TabIndex = 11;
            usernameLabel.Text = "Username";
            usernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // settingsButton
            // 
            settingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            settingsButton.Location = new System.Drawing.Point(418, 76);
            settingsButton.Name = "settingsButton";
            settingsButton.Size = new System.Drawing.Size(47, 47);
            settingsButton.TabIndex = 4;
            settingsButton.UseVisualStyleBackColor = true;
            // 
            // voiceChannel3Button
            // 
            voiceChannel3Button.AutoSize = true;
            voiceChannel3Button.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel3Button.FlatAppearance.BorderSize = 0;
            voiceChannel3Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel3Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel3Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            voiceChannel3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            voiceChannel3Button.ForeColor = System.Drawing.Color.White;
            voiceChannel3Button.Location = new System.Drawing.Point(73, 596);
            voiceChannel3Button.Name = "voiceChannel3Button";
            voiceChannel3Button.Size = new System.Drawing.Size(328, 35);
            voiceChannel3Button.TabIndex = 10;
            voiceChannel3Button.TabStop = false;
            voiceChannel3Button.Text = "Voice Channel 3";
            voiceChannel3Button.UseVisualStyleBackColor = false;
            voiceChannel3Button.Click += voiceChannel3Button_Click;
            // 
            // voiceChannel2Button
            // 
            voiceChannel2Button.AutoSize = true;
            voiceChannel2Button.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel2Button.FlatAppearance.BorderSize = 0;
            voiceChannel2Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel2Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel2Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            voiceChannel2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            voiceChannel2Button.ForeColor = System.Drawing.Color.White;
            voiceChannel2Button.Location = new System.Drawing.Point(73, 512);
            voiceChannel2Button.Name = "voiceChannel2Button";
            voiceChannel2Button.Size = new System.Drawing.Size(328, 35);
            voiceChannel2Button.TabIndex = 9;
            voiceChannel2Button.TabStop = false;
            voiceChannel2Button.Text = "Voice Channel 2";
            voiceChannel2Button.UseVisualStyleBackColor = false;
            voiceChannel2Button.Click += voiceChannel2Button_Click;
            // 
            // voiceChannel1Button
            // 
            voiceChannel1Button.AutoSize = true;
            voiceChannel1Button.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel1Button.FlatAppearance.BorderSize = 0;
            voiceChannel1Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel1Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel1Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            voiceChannel1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            voiceChannel1Button.ForeColor = System.Drawing.Color.White;
            voiceChannel1Button.Location = new System.Drawing.Point(73, 439);
            voiceChannel1Button.Name = "voiceChannel1Button";
            voiceChannel1Button.Size = new System.Drawing.Size(328, 35);
            voiceChannel1Button.TabIndex = 7;
            voiceChannel1Button.TabStop = false;
            voiceChannel1Button.Text = "Voice Channel 1";
            voiceChannel1Button.UseVisualStyleBackColor = false;
            voiceChannel1Button.Click += voiceChannel1Button_Click;
            // 
            // voiceChannelsLabel
            // 
            voiceChannelsLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            voiceChannelsLabel.ForeColor = System.Drawing.Color.White;
            voiceChannelsLabel.Location = new System.Drawing.Point(3, 387);
            voiceChannelsLabel.Name = "voiceChannelsLabel";
            voiceChannelsLabel.Size = new System.Drawing.Size(207, 23);
            voiceChannelsLabel.TabIndex = 8;
            voiceChannelsLabel.Text = "Voice Channels ▼";
            voiceChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textChannel3Button
            // 
            textChannel3Button.AutoSize = true;
            textChannel3Button.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel3Button.FlatAppearance.BorderSize = 0;
            textChannel3Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel3Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel3Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            textChannel3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            textChannel3Button.ForeColor = System.Drawing.Color.White;
            textChannel3Button.Location = new System.Drawing.Point(73, 295);
            textChannel3Button.Name = "textChannel3Button";
            textChannel3Button.Size = new System.Drawing.Size(328, 35);
            textChannel3Button.TabIndex = 6;
            textChannel3Button.TabStop = false;
            textChannel3Button.Tag = "3";
            textChannel3Button.Text = "Text Channel 3";
            textChannel3Button.UseVisualStyleBackColor = false;
            textChannel3Button.Click += textChanel3Button_Click;
            // 
            // textChannel2Button
            // 
            textChannel2Button.AutoSize = true;
            textChannel2Button.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel2Button.FlatAppearance.BorderSize = 0;
            textChannel2Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel2Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel2Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            textChannel2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            textChannel2Button.ForeColor = System.Drawing.Color.White;
            textChannel2Button.Location = new System.Drawing.Point(73, 211);
            textChannel2Button.Name = "textChannel2Button";
            textChannel2Button.Size = new System.Drawing.Size(328, 35);
            textChannel2Button.TabIndex = 5;
            textChannel2Button.TabStop = false;
            textChannel2Button.Tag = "2";
            textChannel2Button.Text = "Text Channel 2";
            textChannel2Button.UseVisualStyleBackColor = false;
            textChannel2Button.Click += textChanel2Button_Click;
            // 
            // textChannel1Button
            // 
            textChannel1Button.AutoSize = true;
            textChannel1Button.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel1Button.FlatAppearance.BorderSize = 0;
            textChannel1Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel1Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel1Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            textChannel1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            textChannel1Button.ForeColor = System.Drawing.Color.White;
            textChannel1Button.Location = new System.Drawing.Point(73, 138);
            textChannel1Button.Name = "textChannel1Button";
            textChannel1Button.Size = new System.Drawing.Size(328, 37);
            textChannel1Button.TabIndex = 4;
            textChannel1Button.TabStop = false;
            textChannel1Button.Tag = "1";
            textChannel1Button.Text = "Text Channel 1";
            textChannel1Button.UseVisualStyleBackColor = false;
            textChannel1Button.Click += textChanel1Button_Click;
            // 
            // textChannelsLabel
            // 
            textChannelsLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            textChannelsLabel.ForeColor = System.Drawing.Color.White;
            textChannelsLabel.Location = new System.Drawing.Point(3, 86);
            textChannelsLabel.Name = "textChannelsLabel";
            textChannelsLabel.Size = new System.Drawing.Size(207, 23);
            textChannelsLabel.TabIndex = 4;
            textChannelsLabel.Text = "Text Channels ▼";
            textChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rightSidePanel
            // 
            rightSidePanel.BackColor = System.Drawing.Color.FromArgb(54, 57, 63);
            rightSidePanel.Controls.Add(offlineUsersLabel);
            rightSidePanel.Controls.Add(onlineUsersLabel);
            rightSidePanel.Dock = System.Windows.Forms.DockStyle.Right;
            rightSidePanel.Location = new System.Drawing.Point(1771, 0);
            rightSidePanel.Name = "rightSidePanel";
            rightSidePanel.Size = new System.Drawing.Size(388, 1209);
            rightSidePanel.TabIndex = 2;
            // 
            // offlineUsersLabel
            // 
            offlineUsersLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            offlineUsersLabel.ForeColor = System.Drawing.Color.White;
            offlineUsersLabel.Location = new System.Drawing.Point(6, 557);
            offlineUsersLabel.Name = "offlineUsersLabel";
            offlineUsersLabel.Size = new System.Drawing.Size(207, 23);
            offlineUsersLabel.TabIndex = 12;
            offlineUsersLabel.Text = "Offline - (number)";
            offlineUsersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // onlineUsersLabel
            // 
            onlineUsersLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            onlineUsersLabel.ForeColor = System.Drawing.Color.White;
            onlineUsersLabel.Location = new System.Drawing.Point(6, 56);
            onlineUsersLabel.Name = "onlineUsersLabel";
            onlineUsersLabel.Size = new System.Drawing.Size(207, 23);
            onlineUsersLabel.TabIndex = 11;
            onlineUsersLabel.Text = "Online - (number)";
            onlineUsersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chatAreaPanel
            // 
            chatAreaPanel.AutoScroll = true;
            chatAreaPanel.Controls.Add(ChatMessagesPanel1);
            chatAreaPanel.Controls.Add(ChatMessagesPanel2);
            chatAreaPanel.Controls.Add(ChatMessagesPanel3);
            chatAreaPanel.Controls.Add(VideoPanel1);
            chatAreaPanel.Controls.Add(VideoPanel2);
            chatAreaPanel.Controls.Add(VideoPanel3);
            chatAreaPanel.Controls.Add(sendMessageButton);
            chatAreaPanel.Controls.Add(messageInputTextBox);
            chatAreaPanel.Controls.Add(nameOfTheProjectLabel);
            chatAreaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            chatAreaPanel.Location = new System.Drawing.Point(471, 0);
            chatAreaPanel.Name = "chatAreaPanel";
            chatAreaPanel.Size = new System.Drawing.Size(1300, 1209);
            chatAreaPanel.TabIndex = 4;
            // 
            // ChatMessagesPanel1
            // 
            ChatMessagesPanel1.AutoScroll = true;
            ChatMessagesPanel1.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            ChatMessagesPanel1.Location = new System.Drawing.Point(0, 86);
            ChatMessagesPanel1.Name = "ChatMessagesPanel1";
            ChatMessagesPanel1.Size = new System.Drawing.Size(1300, 1043);
            ChatMessagesPanel1.TabIndex = 15;
            ChatMessagesPanel1.Tag = "0";
            // 
            // ChatMessagesPanel2
            // 
            ChatMessagesPanel2.AutoScroll = true;
            ChatMessagesPanel2.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            ChatMessagesPanel2.Location = new System.Drawing.Point(0, 86);
            ChatMessagesPanel2.Name = "ChatMessagesPanel2";
            ChatMessagesPanel2.Size = new System.Drawing.Size(1300, 1043);
            ChatMessagesPanel2.TabIndex = 15;
            ChatMessagesPanel2.Tag = "0";
            ChatMessagesPanel2.Visible = false;
            // 
            // ChatMessagesPanel3
            // 
            ChatMessagesPanel3.AutoScroll = true;
            ChatMessagesPanel3.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            ChatMessagesPanel3.Location = new System.Drawing.Point(0, 86);
            ChatMessagesPanel3.Name = "ChatMessagesPanel3";
            ChatMessagesPanel3.Size = new System.Drawing.Size(1300, 1043);
            ChatMessagesPanel3.TabIndex = 15;
            ChatMessagesPanel3.Tag = "0";
            ChatMessagesPanel3.Visible = false;
            // 
            // VideoPanel1
            // 
            VideoPanel1.AutoScroll = true;
            VideoPanel1.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            VideoPanel1.Location = new System.Drawing.Point(0, 86);
            VideoPanel1.Name = "VideoPanel1";
            VideoPanel1.Size = new System.Drawing.Size(1300, 1043);
            VideoPanel1.TabIndex = 15;
            VideoPanel1.Tag = "0";
            VideoPanel1.Visible = false;
            // 
            // VideoPanel2
            // 
            VideoPanel2.AutoScroll = true;
            VideoPanel2.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            VideoPanel2.Location = new System.Drawing.Point(0, 86);
            VideoPanel2.Name = "VideoPanel2";
            VideoPanel2.Size = new System.Drawing.Size(1300, 1043);
            VideoPanel2.TabIndex = 15;
            VideoPanel2.Tag = "0";
            VideoPanel2.Visible = false;
            // 
            // VideoPanel3
            // 
            VideoPanel3.AutoScroll = true;
            VideoPanel3.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            VideoPanel3.Location = new System.Drawing.Point(0, 86);
            VideoPanel3.Name = "VideoPanel3";
            VideoPanel3.Size = new System.Drawing.Size(1300, 1043);
            VideoPanel3.TabIndex = 15;
            VideoPanel3.Tag = "0";
            VideoPanel3.Visible = false;
            // 
            // sendMessageButton
            // 
            sendMessageButton.BackColor = System.Drawing.Color.FromArgb(54, 57, 63);
            sendMessageButton.FlatAppearance.BorderSize = 0;
            sendMessageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            sendMessageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            sendMessageButton.ForeColor = System.Drawing.Color.White;
            sendMessageButton.Location = new System.Drawing.Point(1182, 1135);
            sendMessageButton.Name = "sendMessageButton";
            sendMessageButton.Size = new System.Drawing.Size(102, 49);
            sendMessageButton.TabIndex = 14;
            sendMessageButton.Text = "Send";
            sendMessageButton.UseVisualStyleBackColor = false;
            sendMessageButton.Click += sendMessageButton_Click;
            // 
            // messageInputTextBox
            // 
            messageInputTextBox.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            messageInputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            messageInputTextBox.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            messageInputTextBox.ForeColor = System.Drawing.Color.White;
            messageInputTextBox.Location = new System.Drawing.Point(6, 1144);
            messageInputTextBox.Multiline = true;
            messageInputTextBox.Name = "messageInputTextBox";
            messageInputTextBox.Size = new System.Drawing.Size(1155, 40);
            messageInputTextBox.TabIndex = 13;
            // 
            // nameOfTheProjectLabel
            // 
            nameOfTheProjectLabel.Dock = System.Windows.Forms.DockStyle.Top;
            nameOfTheProjectLabel.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            nameOfTheProjectLabel.ForeColor = System.Drawing.Color.White;
            nameOfTheProjectLabel.Location = new System.Drawing.Point(0, 0);
            nameOfTheProjectLabel.Name = "nameOfTheProjectLabel";
            nameOfTheProjectLabel.Size = new System.Drawing.Size(1300, 48);
            nameOfTheProjectLabel.TabIndex = 12;
            nameOfTheProjectLabel.Text = "Yoav Discord";
            nameOfTheProjectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DiscordApp
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoSize = true;
            BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            ClientSize = new System.Drawing.Size(2159, 1209);
            Controls.Add(chatAreaPanel);
            Controls.Add(rightSidePanel);
            Controls.Add(leftSidePanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Name = "DiscordApp";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "DiscordApp";
            Load += DiscordApp_Load;
            leftSidePanel.ResumeLayout(false);
            leftSidePanel.PerformLayout();
            userInfoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)userProfilePicturePictureBox).EndInit();
            rightSidePanel.ResumeLayout(false);
            chatAreaPanel.ResumeLayout(false);
            chatAreaPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel leftSidePanel;
        private System.Windows.Forms.Panel rightSidePanel;
        private System.Windows.Forms.Label textChannelsLabel;
        private System.Windows.Forms.Button textChannel1Button;
        private System.Windows.Forms.Button textChannel3Button;
        private System.Windows.Forms.Button textChannel2Button;
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
        private System.Windows.Forms.Panel VideoPanel1;
        private System.Windows.Forms.Panel VideoPanel2;
        private System.Windows.Forms.Panel VideoPanel3;
        //private CustomScrollBar customScrollBar1;
        //private CustomScrollBar customScrollBar2;
        //private CustomScrollBar customScrollBar3;

    }
}