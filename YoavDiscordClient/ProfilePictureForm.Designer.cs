namespace YoavDiscordClient
{
    partial class ProfilePictureForm
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
            this.startLabel = new System.Windows.Forms.Label();
            this.seeDefaultOptionsButton = new System.Windows.Forms.Button();
            this.uploadPhotoButton = new System.Windows.Forms.Button();
            this.chooseThisPhotoButton = new System.Windows.Forms.Button();
            this.userProfilePictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.userProfilePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // startLabel
            // 
            this.startLabel.AutoSize = true;
            this.startLabel.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.startLabel.Location = new System.Drawing.Point(151, 64);
            this.startLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(359, 27);
            this.startLabel.TabIndex = 36;
            this.startLabel.Text = "Choose Your Profile Picture";
            // 
            // seeDefaultOptionsButton
            // 
            this.seeDefaultOptionsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.seeDefaultOptionsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.seeDefaultOptionsButton.FlatAppearance.BorderSize = 0;
            this.seeDefaultOptionsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.seeDefaultOptionsButton.ForeColor = System.Drawing.Color.White;
            this.seeDefaultOptionsButton.Location = new System.Drawing.Point(386, 192);
            this.seeDefaultOptionsButton.Margin = new System.Windows.Forms.Padding(4);
            this.seeDefaultOptionsButton.Name = "seeDefaultOptionsButton";
            this.seeDefaultOptionsButton.Size = new System.Drawing.Size(281, 46);
            this.seeDefaultOptionsButton.TabIndex = 39;
            this.seeDefaultOptionsButton.Text = "See Default Options";
            this.seeDefaultOptionsButton.UseVisualStyleBackColor = false;
            this.seeDefaultOptionsButton.Click += new System.EventHandler(this.seeDefaultOptionsButton_Click);
            // 
            // uploadPhotoButton
            // 
            this.uploadPhotoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.uploadPhotoButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uploadPhotoButton.FlatAppearance.BorderSize = 0;
            this.uploadPhotoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uploadPhotoButton.ForeColor = System.Drawing.Color.White;
            this.uploadPhotoButton.Location = new System.Drawing.Point(386, 290);
            this.uploadPhotoButton.Margin = new System.Windows.Forms.Padding(4);
            this.uploadPhotoButton.Name = "uploadPhotoButton";
            this.uploadPhotoButton.Size = new System.Drawing.Size(281, 46);
            this.uploadPhotoButton.TabIndex = 40;
            this.uploadPhotoButton.Text = "Upload Photo";
            this.uploadPhotoButton.UseVisualStyleBackColor = false;
            this.uploadPhotoButton.Click += new System.EventHandler(this.uploadPhotoButton_Click);
            // 
            // chooseThisPhotoButton
            // 
            this.chooseThisPhotoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(86)))), ((int)(((byte)(174)))));
            this.chooseThisPhotoButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chooseThisPhotoButton.FlatAppearance.BorderSize = 0;
            this.chooseThisPhotoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chooseThisPhotoButton.ForeColor = System.Drawing.Color.White;
            this.chooseThisPhotoButton.Location = new System.Drawing.Point(173, 462);
            this.chooseThisPhotoButton.Margin = new System.Windows.Forms.Padding(4);
            this.chooseThisPhotoButton.Name = "chooseThisPhotoButton";
            this.chooseThisPhotoButton.Size = new System.Drawing.Size(281, 46);
            this.chooseThisPhotoButton.TabIndex = 41;
            this.chooseThisPhotoButton.Text = "Choose This Photo";
            this.chooseThisPhotoButton.UseVisualStyleBackColor = false;
            this.chooseThisPhotoButton.Click += new System.EventHandler(this.chooseThisPhotoButton_Click);
            // 
            // userProfilePictureBox
            // 
            this.userProfilePictureBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.userProfilePictureBox.Location = new System.Drawing.Point(50, 161);
            this.userProfilePictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.userProfilePictureBox.Name = "userProfilePictureBox";
            this.userProfilePictureBox.Size = new System.Drawing.Size(243, 205);
            this.userProfilePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.userProfilePictureBox.TabIndex = 42;
            this.userProfilePictureBox.TabStop = false;
            this.userProfilePictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.userProfilePictureBox_MouseClick);
            // 
            // ProfilePictureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(700, 618);
            this.Controls.Add(this.userProfilePictureBox);
            this.Controls.Add(this.chooseThisPhotoButton);
            this.Controls.Add(this.uploadPhotoButton);
            this.Controls.Add(this.seeDefaultOptionsButton);
            this.Controls.Add(this.startLabel);
            this.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProfilePictureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProfilePictureForm";
            ((System.ComponentModel.ISupportInitialize)(this.userProfilePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Button seeDefaultOptionsButton;
        private System.Windows.Forms.Button uploadPhotoButton;
        private System.Windows.Forms.Button chooseThisPhotoButton;
        private System.Windows.Forms.PictureBox userProfilePictureBox;
    }
}