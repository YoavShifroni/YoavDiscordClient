using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        private void DiscordApp_Load(object sender, EventArgs e)
        {
            DiscordFormsHolder.ResizeFormBasedOnResolution(this, 2175f, 1248f);

            this.AddPicturesToTheWindow();

            ConnectionManager.getInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);

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


        private int WhichChatMessagesPanelIsVisible()
        {
            if (this.ChatMessagesPanel1.Visible)
            {
                return 1;
            }
            if (this.ChatMessagesPanel2.Visible)
            {
                return 2;
            }
            if (this.ChatMessagesPanel3.Visible)
            {
                return 3;
            }
            return -1;
        }

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            if(this.messageInputTextBox.Text.Length == 0)
            {
                MessageBox.Show("you can't send an empty message!");
                return;
            }
            int chatRoomId = this.WhichChatMessagesPanelIsVisible();
            this.AddMessageToChat(this._username, this.messageInputTextBox.Text, this._userProfilePicture, DateTime.Now, chatRoomId);
            ConnectionManager.getInstance(null).ProcessSendMessage(this.messageInputTextBox.Text, chatRoomId);
            this.messageInputTextBox.Text = "";
        }

        public void AddMessageToChat(string username, string message, Image profileImage, DateTime time, int chatRoomId)
        {
            // Create a new ChatMessagePanel for the new message
            ChatMessagePanel newMessagePanel = new ChatMessagePanel(username, message, profileImage, time);

            string nameOfActivePanel = $"ChatMessagesPanel{chatRoomId}";
            Control[] control = chatAreaPanel.Controls.Find(nameOfActivePanel, true);
            // Calculate the Y-position where the new message will go (at the bottom)
            Panel messagesPanel = ((Panel)control[0]);
            int newYPosition = messagesPanel.Controls.Count > 1
                ? messagesPanel.Controls[messagesPanel.Controls.Count - 1].Bottom + 10  // Add some space between messages
                : 50;  // If no controls are in the panel yet, start from the top

            // Set the location of the new message panel
            newMessagePanel.Location = new Point(10, newYPosition);

            // Add the new message panel to the chatAreaPanel
            messagesPanel.Controls.Add(newMessagePanel);

            //// Optionally, scroll the panel to the latest message (if you are using a scrollable panel)
            messagesPanel.ScrollControlIntoView(newMessagePanel);

            //string nameOfActiveScrollBar = $"customScrollBar{chatRoomId}";
            //Control[] controls = messagesPanel.Controls.Find(nameOfActiveScrollBar, true);
            //CustomScrollBar customScrollBar = ((CustomScrollBar)controls[0]);
            //customScrollBar.Maximum = messagesPanel.VerticalScroll.Maximum;
            //customScrollBar.ScrollValue = messagesPanel.VerticalScroll.Maximum;
            ////this.UpdateScrollBarVisibility(messagesPanel, customScrollBar);

            //this.UpdateScrollBarThumbSize(customScrollBar, messagesPanel);
        }


        public void AddMessageToChatFromOtherUser(string username, int userId, string message, DateTime time, int chatRoomId)
        {
            if (!this.usersImages.ContainsKey(userId))
            {
                ConnectionManager.getInstance(null).ProcessFetchImageOfUser(userId, username, message, time, chatRoomId);
                return;
            }
            this.AddMessageToChat(username, message, this.usersImages[userId], time, chatRoomId);
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

        public void AddNewUserImageAndShowItsMessage(int userId, byte[] profilePicture, string username, string messageThatTheUserSent, 
            DateTime timeThatTheMessageWasSent, int chatRoomId)
        {

            this.usersImages[userId] = this.ByteArrayToImage(profilePicture);
            this.AddMessageToChatFromOtherUser(username, userId, messageThatTheUserSent, timeThatTheMessageWasSent, chatRoomId);
        }

        private void textChanel1Button_Click(object sender, EventArgs e)
        {
            this.ChatMessagesPanel1.Visible = true;
            this.ChatMessagesPanel2.Visible = false;
            this.ChatMessagesPanel3.Visible = false;
            //this.customScrollBar1.Visible = true;
            //this.customScrollBar2.Visible = false;
            //this.customScrollBar3.Visible = false;
            if (((string)this.ChatMessagesPanel1.Tag) == "0")
            {
                ConnectionManager.getInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);
            }

        }

        private void textChanel2Button_Click(object sender, EventArgs e)
        {
            this.ChatMessagesPanel1.Visible = false;
            this.ChatMessagesPanel2.Visible = true;
            this.ChatMessagesPanel3.Visible = false;
            //this.customScrollBar1.Visible = false;
            //this.customScrollBar2.Visible = true;
            //this.customScrollBar3.Visible = false;
            if (((string)this.ChatMessagesPanel2.Tag) == "0")
            {
                ConnectionManager.getInstance(null).ProcessGetMessagesHistoryOfChatRoom(2);
            }
        }

        private void textChanel3Button_Click(object sender, EventArgs e)
        {
            this.ChatMessagesPanel1.Visible = false;
            this.ChatMessagesPanel2.Visible = false;
            this.ChatMessagesPanel3.Visible = true;
            //this.customScrollBar1.Visible = false;
            //this.customScrollBar2.Visible = false;
            //this.customScrollBar3.Visible = true;
            if (((string)this.ChatMessagesPanel3.Tag) == "0")
            {
                ConnectionManager.getInstance(null).ProcessGetMessagesHistoryOfChatRoom(3);
            }
        }

        public void SetMessagesHistoryOfAChatRoom(List<UserMessage> messages)
        {
            foreach(UserMessage message in messages)
            {
                this.AddMessageToChatFromOtherUser(message.Username, message.userId, message.Message, message.Time, message.ChatRoomId);
            }
            if(messages != null && messages.Count > 0)
            {
                string nameOfActivePanel = $"ChatMessagesPanel{messages[0].ChatRoomId}";
                Control[] control = chatAreaPanel.Controls.Find(nameOfActivePanel, true);
                control[0].Tag = "1";
            }
            
        }

        //private void CustomScrollBar1_ScrollValueChanged(object sender, EventArgs e)
        //{
        //    // Set the panel's vertical scroll value
        //    this.ChatMessagesPanel1.VerticalScroll.Value = this.customScrollBar1.ScrollValue;
        //    this.ChatMessagesPanel1.PerformLayout(); // Redraw the panel
        //}

        //private void CustomScrollBar2_ScrollValueChanged(object sender, EventArgs e)
        //{
        //    // Set the panel's vertical scroll value
        //    if (this.ChatMessagesPanel2.VerticalScroll.Value != this.customScrollBar2.ScrollValue)
        //    {
        //        this.ChatMessagesPanel2.VerticalScroll.Value = this.customScrollBar2.ScrollValue;
        //        this.ChatMessagesPanel2.PerformLayout(); // Redraw the panel

        //    }

        //}

        //private void CustomScrollBar3_ScrollValueChanged(object sender, EventArgs e)
        //{
        //    // Set the panel's vertical scroll value
        //    this.ChatMessagesPanel3.VerticalScroll.Value = this.customScrollBar3.ScrollValue;
        //    this.ChatMessagesPanel3.PerformLayout(); // Redraw the panel
        //}

        //private void ChatMessagesPanel1_Scroll(object sender, ScrollEventArgs e)
        //{
        //    this.customScrollBar1.ScrollValue = this.ChatMessagesPanel1.VerticalScroll.Value;
        //}

        //private void ChatMessagesPanel2_Scroll(object sender, ScrollEventArgs e)
        //{
        //    this.customScrollBar2.ScrollValue = this.ChatMessagesPanel2.VerticalScroll.Value;
        //}

        //private void ChatMessagesPanel3_Scroll(object sender, ScrollEventArgs e)
        //{
        //    this.customScrollBar3.ScrollValue = this.ChatMessagesPanel3.VerticalScroll.Value;
        //}

        //private void UpdateScrollBarVisibility(Panel chatPanel, CustomScrollBar customScrollBar)
        //{
        //    int totalContentHeight = chatPanel.DisplayRectangle.Height; // Total height of content
        //    int visibleHeight = chatPanel.ClientSize.Height;            // Visible height of the panel

        //    if (totalContentHeight > visibleHeight)
        //    {
        //        customScrollBar.Visible = true;
        //        customScrollBar.MaximumValue = totalContentHeight - visibleHeight;
        //    }
        //    else
        //    {
        //        customScrollBar.Visible = false;
        //    }
        //}


        //private void UpdateScrollBarThumbSize(CustomScrollBar customScrollBar, Panel chatPanel)
        //{
        //    int totalContentHeight = chatPanel.DisplayRectangle.Height; // Total height of content
        //    int visibleHeight = chatPanel.ClientSize.Height;            // Visible height of the panel

        //    if (totalContentHeight > 0 && visibleHeight > 0)
        //    {
        //        // Calculate the thumb size proportionally
        //        float thumbSizeRatio = (float)visibleHeight / totalContentHeight;

        //        // Set the scrollbar's thumb size
        //        customScrollBar.LargeChange = (int)(customScrollBar.Maximum * thumbSizeRatio);
        //    }
        //    else
        //    {
        //        // Default thumb size if no content
        //        customScrollBar.LargeChange = customScrollBar.Maximum;
        //    }
        //}

        private void voiceChannel1Button_Click(object sender, EventArgs e)
        {
            ConnectionManager.getInstance(null).ProcessConnectToMediaRoom(1);
        }

        private void voiceChannel2Button_Click(object sender, EventArgs e)
        {
            ConnectionManager.getInstance(null).ProcessConnectToMediaRoom(2);
        }

        private void voiceChannel3Button_Click(object sender, EventArgs e)
        {
            ConnectionManager.getInstance(null).ProcessConnectToMediaRoom(3);
        }
    }
}
