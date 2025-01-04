using System;
using System.Drawing;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public class ChatMessagePanel : Panel
    {
        private CirclePictureBox _profilePictureBox;
        private Label _usernameLabel;
        private Label _dateTimeLabel; // New label for date and time
        private Label _messageLabel;

        public ChatMessagePanel(string username, string message, Image profileImage, DateTime dateTime)
        {
            // Initialize the controls
            this._profilePictureBox = new CirclePictureBox();
            this._usernameLabel = new Label();
            this._dateTimeLabel = new Label();
            this._messageLabel = new Label();

            // Set the properties for the profile picture
            this._profilePictureBox.Width = 40;  // Set a fixed size for the circle
            this._profilePictureBox.Height = 40;
            this._profilePictureBox.Image = profileImage;
            this._profilePictureBox.Location = new Point(10, 10);  // Adjust position

            // Set the properties for the username label
            this._usernameLabel.Text = username;
            this._usernameLabel.Location = new Point(60, 10);  // Position next to the picture
            this._usernameLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            this._usernameLabel.ForeColor = Color.White;
            this._usernameLabel.AutoSize = true;

            // Set the properties for the date and time label
            this._dateTimeLabel.Text = dateTime.ToString("g"); // Format: "MM/dd/yyyy HH:mm"
            this._dateTimeLabel.Location = new Point(this._usernameLabel.Right + 10, 12); // Next to the username
            this._dateTimeLabel.Font = new Font("Arial", 8); // Smaller font
            this._dateTimeLabel.ForeColor = Color.LightGray;
            this._dateTimeLabel.AutoSize = true;

            // Set the properties for the message label
            this._messageLabel.Text = message;
            this._messageLabel.Location = new Point(60, 30);  // New line below the username
            this._messageLabel.Font = new Font("Arial", 10);
            this._messageLabel.ForeColor = Color.White;
            this._messageLabel.AutoSize = true;

            // Set the size of the panel based on the message
            this.Width = 400;  // Adjust width to fit the content
            this.Height = 60 + _messageLabel.Height;  // Adjust height based on the message text

            // Add controls to the panel
            this.Controls.Add(_profilePictureBox);
            this.Controls.Add(_usernameLabel);
            this.Controls.Add(_dateTimeLabel);
            this.Controls.Add(_messageLabel);
        }
    }
}
