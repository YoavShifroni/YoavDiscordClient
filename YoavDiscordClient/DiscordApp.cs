using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public partial class DiscordApp : Form
    {

        private Image _userProfilePicture;

        private string _username;

        private bool _isWindowAlreadyChanged = false;

        private double _ratioWidth;

        private double _ratioHeight;

        public DiscordApp()
        {
            InitializeComponent();
        }

        public void SetUsernameAndProfilePicture(byte[] profilePicture, string username)
        {
            this._userProfilePicture = this.ByteArrayToImage(profilePicture);
            this._username = username;
            this.userProfilePicturePictureBox.Image = this._userProfilePicture;
            this.usernameLabel.Text = username;
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            if(this.messageInputTextBox.Text.Length == 0)
            {
                return;
            }
            this.AddMessageToChat(_username, this.messageInputTextBox.Text, this._userProfilePicture);
            this.messageInputTextBox.Text = "";
        }

        private void AddMessageToChat(string username, string message, Image profileImage)
        {
            // Create a new ChatMessagePanel for the new message
            ChatMessagePanel newMessagePanel = new ChatMessagePanel(username, message, profileImage, DateTime.Now);

            // Calculate the Y-position where the new message will go (at the bottom)
            int newYPosition = chatAreaPanel.Controls.Count > 3
                ? chatAreaPanel.Controls[chatAreaPanel.Controls.Count - 1].Bottom + 10  // Add some space between messages
                : 50;  // If no controls are in the panel yet, start from the top
                // TODO: change this so the panel will start under the label

            // Set the location of the new message panel
            newMessagePanel.Location = new Point(10, newYPosition);

            // Add the new message panel to the chatAreaPanel
            chatAreaPanel.Controls.Add(newMessagePanel);

            // Optionally, scroll the panel to the latest message (if you are using a scrollable panel)
            chatAreaPanel.ScrollControlIntoView(newMessagePanel);
        }


        private Image ResizeImage(Image imgToResize, int width, int height)
        {
            return new Bitmap(imgToResize, new Size(width, height));
        }

        private void AddPicturesToTheWindow()
        {
            Image originalSettingsLogoImage = Properties.Resources.settingsLogo;
            Image originalDeafenLogoImage = Properties.Resources.deafenLogo;
            Image originalMuteLogoImage = Properties.Resources.muteLogo;
            this.settingsButton.Image = this.ResizeImage(originalSettingsLogoImage, this.settingsButton.Width, this.settingsButton.Height);
            this.deafenButton.Image = this.ResizeImage(originalDeafenLogoImage, this.deafenButton.Width, this.deafenButton.Height);
            this.muteButton.Image = this.ResizeImage(originalMuteLogoImage, this.muteButton.Width, this.muteButton.Height);
        }


        private void DiscordApp_Load(object sender, EventArgs e)
        {
            this.AddPicturesToTheWindow();
            Rectangle resolutionRect = System.Windows.Forms.Screen.FromControl(this).Bounds;
            if (this.Width >= resolutionRect.Width || this.Height >= resolutionRect.Height || this._isWindowAlreadyChanged)
            {
                if (this._isWindowAlreadyChanged == false)
                {
                    this._isWindowAlreadyChanged = true;
                    double ratio = this.Width / this.Height;
                    int newWidth = (int)(resolutionRect.Width * 0.7);
                    int newHeight = (int)(resolutionRect.Height * 0.7 * ratio);
                    this._ratioWidth = (double)newWidth / (double)this.Width;
                    this._ratioHeight = (double)newHeight / (double)this.Height;
                    this.Width = newWidth;
                    this.Height = newHeight;
                }

                foreach (Control control in this.Controls)
                {
                    control.Width = (int)(control.Width * this._ratioWidth);
                    control.Height = (int)(control.Height * this._ratioHeight);
                    control.Left = (int)(control.Left * this._ratioWidth);
                    control.Top = (int)(control.Top * this._ratioHeight);
                    float fontSize = (float)(control.Font.Size * 0.7);
                    control.Font = new Font(control.Font.Name, fontSize);
                }
            }
        }

        private void textChanel1Button_Click(object sender, EventArgs e)
        {

        }
    }
}
