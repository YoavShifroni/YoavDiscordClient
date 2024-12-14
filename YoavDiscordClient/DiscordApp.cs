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

        private Dictionary<int, Image> usersImages = new Dictionary<int, Image>();

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
                MessageBox.Show("you can't send an empty message!");
                return;
            }
            this.AddMessageToChat(this._username, this.messageInputTextBox.Text, this._userProfilePicture, DateTime.Now);
            ConnectionManager.getInstance(null).ProcessSendMessage(this.messageInputTextBox.Text, DateTime.Now);
            this.messageInputTextBox.Text = "";
        }

        public void AddMessageToChat(string username, string message, Image profileImage, DateTime time)
        {
            // Create a new ChatMessagePanel for the new message
            ChatMessagePanel newMessagePanel = new ChatMessagePanel(username, message, profileImage, time);

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

        public void AddMessageToChatFromOtherUser(string username, int userId, string message, DateTime time)
        {
            if (!this.usersImages.ContainsKey(userId))
            {
                ConnectionManager.getInstance(null).ProcessFetchImageOfUser(userId, username, message, time);
                return;
            }
            this.AddMessageToChat(username, message, this.usersImages[userId], time);
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
            DiscordFormsHolder.ResizeFormBasedOnResolution(this, 2175f, 1248f);

            this.AddPicturesToTheWindow();

        }

        private void textChanel1Button_Click(object sender, EventArgs e)
        {

        }

        public void AddNewUserImageAndShowItsMessage(int userId, byte[] profilePicture, string username, string messageThatTheUserSent, DateTime timeThatTheMessageWasSent)
        {

            this.usersImages[userId] = this.ByteArrayToImage(profilePicture);
            this.AddMessageToChatFromOtherUser(username, userId, messageThatTheUserSent, timeThatTheMessageWasSent);
        }
    }
}
