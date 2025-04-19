using System.Windows.Forms;

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
            leftSidePanel = new Panel();
            userInfoPanel = new Panel();
            globalMuteButton = new Button();
            deafenButton = new Button();
            userProfilePicturePictureBox = new CirclePictureBox();
            usernameLabel = new Label();
            voiceChannel3Button = new Button();
            voiceChannel2Button = new Button();
            voiceChannel1Button = new Button();
            voiceChannelsLabel = new Label();
            textChannel3Button = new Button();
            textChannel2Button = new Button();
            textChannel1Button = new Button();
            textChannelsLabel = new Label();
            rightSidePanel = new Panel();
            offlineUsersLabel = new Label();
            onlineUsersLabel = new Label();
            chatAreaPanel = new Panel();
            ChatMessagesPanel1 = new Panel();
            ChatMessagesPanel2 = new Panel();
            ChatMessagesPanel3 = new Panel();
            VideoPanel1 = new Panel();
            VideoPanel2 = new Panel();
            VideoPanel3 = new Panel();
            sendMessageButton = new Button();
            messageInputTextBox = new TextBox();
            nameOfTheProjectLabel = new Label();
            mediaControlsPanel = new Panel();
            mediaChannelMuteButton = new Button();
            mediaChannelVideoMuteButton = new Button();
            mediaChannelDisconnectButton = new Button();
            emojiPanel = new Panel();
            emojiButton = new Button();
            emojiSelectionPanel = new Panel();
            leftSidePanel.SuspendLayout();
            userInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)userProfilePicturePictureBox).BeginInit();
            rightSidePanel.SuspendLayout();
            chatAreaPanel.SuspendLayout();
            mediaControlsPanel.SuspendLayout();
            emojiPanel.SuspendLayout();
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
            leftSidePanel.Dock = DockStyle.Left;
            leftSidePanel.Location = new System.Drawing.Point(0, 0);
            leftSidePanel.Name = "leftSidePanel";
            leftSidePanel.Size = new System.Drawing.Size(471, 1209);
            leftSidePanel.TabIndex = 0;
            leftSidePanel.Enabled = false;
            leftSidePanel.MouseDown += DiscordApp_MouseDown;
            // 
            // userInfoPanel
            // 
            userInfoPanel.BackColor = System.Drawing.Color.FromArgb(44, 47, 51);
            userInfoPanel.Controls.Add(globalMuteButton);
            userInfoPanel.Controls.Add(deafenButton);
            userInfoPanel.Controls.Add(userProfilePicturePictureBox);
            userInfoPanel.Controls.Add(usernameLabel);
            userInfoPanel.Dock = DockStyle.Bottom;
            userInfoPanel.Location = new System.Drawing.Point(0, 1068);
            userInfoPanel.Name = "userInfoPanel";
            userInfoPanel.Size = new System.Drawing.Size(471, 141);
            userInfoPanel.TabIndex = 4;
            // 
            // globalMuteButton
            // 
            globalMuteButton.BackgroundImageLayout = ImageLayout.Stretch;
            globalMuteButton.FlatStyle = FlatStyle.Flat;
            globalMuteButton.Location = new System.Drawing.Point(279, 76);
            globalMuteButton.Name = "globalMuteButton";
            globalMuteButton.Size = new System.Drawing.Size(47, 47);
            globalMuteButton.TabIndex = 14;
            globalMuteButton.UseVisualStyleBackColor = true;
            globalMuteButton.Click += globalMuteButton_Click;
            // 
            // deafenButton
            // 
            deafenButton.BackgroundImageLayout = ImageLayout.Stretch;
            deafenButton.FlatStyle = FlatStyle.Flat;
            deafenButton.Location = new System.Drawing.Point(354, 76);
            deafenButton.Name = "deafenButton";
            deafenButton.Size = new System.Drawing.Size(47, 47);
            deafenButton.TabIndex = 13;
            deafenButton.UseVisualStyleBackColor = true;
            deafenButton.Click += deafenButton_Click;
            // 
            // userProfilePicturePictureBox
            // 
            userProfilePicturePictureBox.BackColor = System.Drawing.Color.Gray;
            userProfilePicturePictureBox.Location = new System.Drawing.Point(12, 37);
            userProfilePicturePictureBox.Name = "userProfilePicturePictureBox";
            userProfilePicturePictureBox.Size = new System.Drawing.Size(105, 86);
            userProfilePicturePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
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
            // voiceChannel3Button
            // 
            voiceChannel3Button.AutoSize = true;
            voiceChannel3Button.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel3Button.FlatAppearance.BorderSize = 0;
            voiceChannel3Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel3Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            voiceChannel3Button.FlatStyle = FlatStyle.Flat;
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
            voiceChannel2Button.FlatStyle = FlatStyle.Flat;
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
            voiceChannel1Button.FlatStyle = FlatStyle.Flat;
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
            voiceChannelsLabel.Text = "Voice Channels";
            voiceChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textChannel3Button
            // 
            textChannel3Button.AutoSize = true;
            textChannel3Button.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel3Button.FlatAppearance.BorderSize = 0;
            textChannel3Button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel3Button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            textChannel3Button.FlatStyle = FlatStyle.Flat;
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
            textChannel2Button.FlatStyle = FlatStyle.Flat;
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
            textChannel1Button.FlatStyle = FlatStyle.Flat;
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
            textChannelsLabel.Text = "Text Channels";
            textChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rightSidePanel
            // 
            rightSidePanel.BackColor = System.Drawing.Color.FromArgb(54, 57, 63);
            rightSidePanel.Controls.Add(offlineUsersLabel);
            rightSidePanel.Controls.Add(onlineUsersLabel);
            rightSidePanel.Dock = DockStyle.Right;
            rightSidePanel.Location = new System.Drawing.Point(1771, 0);
            rightSidePanel.Name = "rightSidePanel";
            rightSidePanel.Size = new System.Drawing.Size(388, 1209);
            rightSidePanel.TabIndex = 2;
            rightSidePanel.MouseDown += DiscordApp_MouseDown;
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
            chatAreaPanel.Controls.Add(mediaControlsPanel);
            chatAreaPanel.Controls.Add(emojiPanel);
            chatAreaPanel.Controls.Add(emojiSelectionPanel);
            chatAreaPanel.Dock = DockStyle.Fill;
            chatAreaPanel.Location = new System.Drawing.Point(471, 0);
            chatAreaPanel.Name = "chatAreaPanel";
            chatAreaPanel.Size = new System.Drawing.Size(1300, 1209);
            chatAreaPanel.TabIndex = 4;
            chatAreaPanel.MouseDown += DiscordApp_MouseDown;
            // 
            // ChatMessagesPanel1
            // 
            ChatMessagesPanel1.AutoScroll = true;
            ChatMessagesPanel1.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            ChatMessagesPanel1.Location = new System.Drawing.Point(0, 86);
            ChatMessagesPanel1.Name = "ChatMessagesPanel1";
            ChatMessagesPanel1.Size = new System.Drawing.Size(1274, 1043);
            ChatMessagesPanel1.TabIndex = 15;
            ChatMessagesPanel1.Tag = "0";
            ChatMessagesPanel1.MouseDown += DiscordApp_MouseDown;
            // 
            // ChatMessagesPanel2
            // 
            ChatMessagesPanel2.AutoScroll = true;
            ChatMessagesPanel2.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            ChatMessagesPanel2.Location = new System.Drawing.Point(0, 86);
            ChatMessagesPanel2.Name = "ChatMessagesPanel2";
            ChatMessagesPanel2.Size = new System.Drawing.Size(1274, 1043);
            ChatMessagesPanel2.TabIndex = 15;
            ChatMessagesPanel2.Tag = "0";
            ChatMessagesPanel2.Visible = false;
            ChatMessagesPanel2.MouseDown += DiscordApp_MouseDown;
            // 
            // ChatMessagesPanel3
            // 
            ChatMessagesPanel3.AutoScroll = true;
            ChatMessagesPanel3.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            ChatMessagesPanel3.Location = new System.Drawing.Point(0, 86);
            ChatMessagesPanel3.Name = "ChatMessagesPanel3";
            ChatMessagesPanel3.Size = new System.Drawing.Size(1274, 1043);
            ChatMessagesPanel3.TabIndex = 15;
            ChatMessagesPanel3.Tag = "0";
            ChatMessagesPanel3.Visible = false;
            ChatMessagesPanel3.MouseDown += DiscordApp_MouseDown;
            // 
            // VideoPanel1
            // 
            VideoPanel1.AutoScroll = true;
            VideoPanel1.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            VideoPanel1.Location = new System.Drawing.Point(0, 86);
            VideoPanel1.Name = "VideoPanel1";
            VideoPanel1.Size = new System.Drawing.Size(1274, 1043);
            VideoPanel1.TabIndex = 15;
            VideoPanel1.Tag = "0";
            VideoPanel1.Visible = false;
            VideoPanel1.MouseDown += DiscordApp_MouseDown;
            // 
            // VideoPanel2
            // 
            VideoPanel2.AutoScroll = true;
            VideoPanel2.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            VideoPanel2.Location = new System.Drawing.Point(0, 86);
            VideoPanel2.Name = "VideoPanel2";
            VideoPanel2.Size = new System.Drawing.Size(1274, 1043);
            VideoPanel2.TabIndex = 15;
            VideoPanel2.Tag = "0";
            VideoPanel2.Visible = false;
            VideoPanel2.MouseDown += DiscordApp_MouseDown;
            // 
            // VideoPanel3
            // 
            VideoPanel3.AutoScroll = true;
            VideoPanel3.BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            VideoPanel3.Location = new System.Drawing.Point(0, 86);
            VideoPanel3.Name = "VideoPanel3";
            VideoPanel3.Size = new System.Drawing.Size(1274, 1043);
            VideoPanel3.TabIndex = 15;
            VideoPanel3.Tag = "0";
            VideoPanel3.Visible = false;
            VideoPanel3.MouseDown += DiscordApp_MouseDown;
            // 
            // sendMessageButton
            // 
            sendMessageButton.BackColor = System.Drawing.Color.FromArgb(54, 57, 63);
            sendMessageButton.FlatAppearance.BorderSize = 0;
            sendMessageButton.FlatStyle = FlatStyle.Flat;
            sendMessageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            sendMessageButton.ForeColor = System.Drawing.Color.White;
            sendMessageButton.Location = new System.Drawing.Point(1161, 1135);
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
            messageInputTextBox.BorderStyle = BorderStyle.FixedSingle;
            messageInputTextBox.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            messageInputTextBox.ForeColor = System.Drawing.Color.White;
            messageInputTextBox.Location = new System.Drawing.Point(6, 1144);
            messageInputTextBox.Multiline = true;
            messageInputTextBox.Name = "messageInputTextBox";
            messageInputTextBox.Size = new System.Drawing.Size(1138, 34);
            messageInputTextBox.TabIndex = 13;
            messageInputTextBox.KeyDown += messageInputTextBox_KeyDown;
            // 
            // nameOfTheProjectLabel
            // 
            nameOfTheProjectLabel.Dock = DockStyle.Top;
            nameOfTheProjectLabel.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            nameOfTheProjectLabel.ForeColor = System.Drawing.Color.White;
            nameOfTheProjectLabel.Location = new System.Drawing.Point(0, 0);
            nameOfTheProjectLabel.Name = "nameOfTheProjectLabel";
            nameOfTheProjectLabel.Size = new System.Drawing.Size(1300, 48);
            nameOfTheProjectLabel.TabIndex = 12;
            nameOfTheProjectLabel.Text = "Yodiscord";
            nameOfTheProjectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mediaControlsPanel
            // 
            mediaControlsPanel.BackColor = System.Drawing.Color.FromArgb(200, 47, 49, 54);
            mediaControlsPanel.Controls.Add(mediaChannelMuteButton);
            mediaControlsPanel.Controls.Add(mediaChannelVideoMuteButton);
            mediaControlsPanel.Controls.Add(mediaChannelDisconnectButton);
            mediaControlsPanel.Location = new System.Drawing.Point(0, 0);
            mediaControlsPanel.Name = "mediaControlsPanel";
            mediaControlsPanel.Size = new System.Drawing.Size(230, 80);
            mediaControlsPanel.TabIndex = 16;
            mediaControlsPanel.Visible = false;
            // 
            // mediaChannelMuteButton
            // 
            mediaChannelMuteButton.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            mediaChannelMuteButton.FlatAppearance.BorderSize = 0;
            mediaChannelMuteButton.FlatStyle = FlatStyle.Flat;
            mediaChannelMuteButton.Location = new System.Drawing.Point(20, 15);
            mediaChannelMuteButton.Margin = new Padding(10);
            mediaChannelMuteButton.Name = "mediaChannelMuteButton";
            mediaChannelMuteButton.Size = new System.Drawing.Size(50, 50);
            mediaChannelMuteButton.TabIndex = 0;
            mediaChannelMuteButton.UseVisualStyleBackColor = false;
            mediaChannelMuteButton.Click += mediaChannelMuteButton_Click;
            // 
            // mediaChannelVideoMuteButton
            // 
            mediaChannelVideoMuteButton.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            mediaChannelVideoMuteButton.FlatAppearance.BorderSize = 0;
            mediaChannelVideoMuteButton.FlatStyle = FlatStyle.Flat;
            mediaChannelVideoMuteButton.Location = new System.Drawing.Point(90, 15);
            mediaChannelVideoMuteButton.Margin = new Padding(10);
            mediaChannelVideoMuteButton.Name = "mediaChannelVideoMuteButton";
            mediaChannelVideoMuteButton.Size = new System.Drawing.Size(50, 50);
            mediaChannelVideoMuteButton.TabIndex = 1;
            mediaChannelVideoMuteButton.UseVisualStyleBackColor = false;
            mediaChannelVideoMuteButton.Click += mediaChannelVideoMuteButton_Click;
            // 
            // mediaChannelDisconnectButton
            // 
            mediaChannelDisconnectButton.BackColor = System.Drawing.Color.FromArgb(240, 71, 71);
            mediaChannelDisconnectButton.FlatAppearance.BorderSize = 0;
            mediaChannelDisconnectButton.FlatStyle = FlatStyle.Flat;
            mediaChannelDisconnectButton.Location = new System.Drawing.Point(160, 15);
            mediaChannelDisconnectButton.Margin = new Padding(10);
            mediaChannelDisconnectButton.Name = "mediaChannelDisconnectButton";
            mediaChannelDisconnectButton.Size = new System.Drawing.Size(50, 50);
            mediaChannelDisconnectButton.TabIndex = 2;
            mediaChannelDisconnectButton.UseVisualStyleBackColor = false;
            mediaChannelDisconnectButton.Click += mediaChannelDisconnectButton_Click;
            // 
            // emojiPanel
            // 
            emojiPanel.BackColor = System.Drawing.Color.Transparent;
            emojiPanel.Controls.Add(emojiButton);
            emojiPanel.Location = new System.Drawing.Point(1144, 1144);
            emojiPanel.Name = "emojiPanel";
            emojiPanel.Size = new System.Drawing.Size(40, 34);
            emojiPanel.TabIndex = 17;
            emojiPanel.Visible = false;
            // 
            // emojiButton
            // 
            emojiButton.BackColor = System.Drawing.Color.FromArgb(64, 68, 75);
            emojiButton.FlatAppearance.BorderSize = 0;
            emojiButton.FlatStyle = FlatStyle.Flat;
            emojiButton.Font = new System.Drawing.Font("Segoe UI Emoji", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            emojiButton.ForeColor = System.Drawing.Color.White;
            emojiButton.Location = new System.Drawing.Point(0, 0);
            emojiButton.Name = "emojiButton";
            emojiButton.Size = new System.Drawing.Size(34, 34);
            emojiButton.TabIndex = 0;
            emojiButton.Text = "😀";
            emojiButton.UseVisualStyleBackColor = false;
            emojiButton.Click += emojiButton_Click;
            // 
            // emojiSelectionPanel
            // 
            emojiSelectionPanel.BackColor = System.Drawing.Color.FromArgb(54, 57, 63);
            emojiSelectionPanel.BorderStyle = BorderStyle.FixedSingle;
            emojiSelectionPanel.Location = new System.Drawing.Point(0, -205);
            emojiSelectionPanel.Name = "emojiSelectionPanel";
            emojiSelectionPanel.Size = new System.Drawing.Size(200, 200);
            emojiSelectionPanel.TabIndex = 1;
            emojiSelectionPanel.Visible = false;
            // 
            // DiscordApp
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            BackColor = System.Drawing.Color.FromArgb(47, 49, 54);
            ClientSize = new System.Drawing.Size(2159, 1209);
            Controls.Add(chatAreaPanel);
            Controls.Add(rightSidePanel);
            Controls.Add(leftSidePanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "DiscordApp";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DiscordApp";
            Load += DiscordApp_Load;
            MouseDown += DiscordApp_MouseDown;
            leftSidePanel.ResumeLayout(false);
            leftSidePanel.PerformLayout();
            userInfoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)userProfilePicturePictureBox).EndInit();
            rightSidePanel.ResumeLayout(false);
            chatAreaPanel.ResumeLayout(false);
            chatAreaPanel.PerformLayout();
            mediaControlsPanel.ResumeLayout(false);
            emojiPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        internal System.Windows.Forms.Panel leftSidePanel;
        internal System.Windows.Forms.Panel rightSidePanel;
        internal System.Windows.Forms.Label textChannelsLabel;
        internal System.Windows.Forms.Button textChannel1Button;
        internal System.Windows.Forms.Button textChannel3Button;
        internal System.Windows.Forms.Button textChannel2Button;
        internal System.Windows.Forms.Button voiceChannel3Button;
        internal System.Windows.Forms.Button voiceChannel2Button;
        internal System.Windows.Forms.Button voiceChannel1Button;
        internal System.Windows.Forms.Label voiceChannelsLabel;
        internal System.Windows.Forms.Panel userInfoPanel;
        internal System.Windows.Forms.Label usernameLabel;
        internal System.Windows.Forms.Panel chatAreaPanel;
        internal System.Windows.Forms.Label nameOfTheProjectLabel;
        internal System.Windows.Forms.Label onlineUsersLabel;
        internal System.Windows.Forms.Label offlineUsersLabel;
        internal System.Windows.Forms.TextBox messageInputTextBox;
        internal System.Windows.Forms.Button sendMessageButton;
        internal CirclePictureBox userProfilePicturePictureBox;
        internal System.Windows.Forms.Button globalMuteButton;
        internal System.Windows.Forms.Button deafenButton;
        internal System.Windows.Forms.Panel ChatMessagesPanel1;
        internal System.Windows.Forms.Panel ChatMessagesPanel2;
        internal System.Windows.Forms.Panel ChatMessagesPanel3;
        internal System.Windows.Forms.Panel VideoPanel1;
        internal System.Windows.Forms.Panel VideoPanel2;
        internal System.Windows.Forms.Panel VideoPanel3;
        internal System.Windows.Forms.Button mediaChannelMuteButton;
        internal System.Windows.Forms.Button mediaChannelVideoMuteButton;
        internal System.Windows.Forms.Button mediaChannelDisconnectButton;
        internal System.Windows.Forms.Panel mediaControlsPanel;
        internal System.Windows.Forms.Button emojiButton;
        internal System.Windows.Forms.Panel emojiPanel;
        internal System.Windows.Forms.Panel emojiSelectionPanel;
        //private CustomScrollBar customScrollBar1;
        //private CustomScrollBar customScrollBar2;
        //private CustomScrollBar customScrollBar3;

    }
}
