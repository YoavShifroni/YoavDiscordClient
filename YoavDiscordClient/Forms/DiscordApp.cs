using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoavDiscordClient.Style;
using YoavDiscordClient.Managers;

namespace YoavDiscordClient
{
#pragma warning disable CA1416

    /// <summary>
    /// Main application form for the Discord clone client.
    /// Manages UI, chat functionality, media connections, and user interactions.
    /// </summary>
    public partial class DiscordApp : Form
    {
        // Core managers
        private readonly ChatManager _chatManager;
        private readonly MediaChannelManager _mediaChannelManager;
        private readonly UserManager _userManager;
        private readonly EmojiManager _emojiManager;
        private readonly ContextMenuManager _contextMenuManager;

        /// <summary>
        /// Constructor for the DiscordApp form.
        /// </summary>
        public DiscordApp()
        {
            InitializeComponent();

            // Initialize managers
            _userManager = new UserManager(this, rightSidePanel);
            _mediaChannelManager = new MediaChannelManager(this, leftSidePanel, chatAreaPanel, mediaControlsPanel);
            _chatManager = new ChatManager(this, chatAreaPanel);
            _emojiManager = new EmojiManager(this, emojiSelectionPanel, emojiPanel, messageInputTextBox, emojiButton);
            _contextMenuManager = new ContextMenuManager(this);

            // Subscribe to mouse events for emoji panel
            this.MouseDown += DiscordApp_MouseDown;
        }

        /// <summary>
        /// Event handler for form load. Initializes the application UI and loads initial data.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DiscordApp_Load(object sender, EventArgs e)
        {
            DiscordFormsHolder.ResizeFormBasedOnResolution(this, 2175f, 1248f);

            _mediaChannelManager.InitializeMediaChannelPanels();
            _contextMenuManager.InitializeUserContextMenu();
            _userManager.AddPicturesToTheWindow();
            _emojiManager.AddEmojisToPanel();

            ConnectionManager.GetInstance(null).ProcessFetchAllUsers();
            ConnectionManager.GetInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);
        }

        /// <summary>
        /// Event handler for mouse down events on the form.
        /// Used to close the emoji selection panel when clicking outside of it.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Mouse event arguments.</param>
        private void DiscordApp_MouseDown(object sender, MouseEventArgs e)
        {
            _emojiManager.HandleFormMouseDown(e);
        }

        #region Event Handlers

        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            _chatManager.SendMessage(messageInputTextBox.Text);
        }

        private void textChanel1Button_Click(object sender, EventArgs e)
        {
            _chatManager.SwitchToTextChannel(1);
        }

        private void textChanel2Button_Click(object sender, EventArgs e)
        {
            _chatManager.SwitchToTextChannel(2);
        }

        private void textChanel3Button_Click(object sender, EventArgs e)
        {
            _chatManager.SwitchToTextChannel(3);
        }

        private async void voiceChannel1Button_Click(object sender, EventArgs e)
        {
            await _mediaChannelManager.ConnectToMediaRoom(1);
        }

        private async void voiceChannel2Button_Click(object sender, EventArgs e)
        {
            await _mediaChannelManager.ConnectToMediaRoom(2);
        }

        private async void voiceChannel3Button_Click(object sender, EventArgs e)
        {
            await _mediaChannelManager.ConnectToMediaRoom(3);
        }

        private void mediaChannelMuteButton_Click(object sender, EventArgs e)
        {
            _mediaChannelManager.ToggleAudioMute();
        }

        private void mediaChannelVideoMuteButton_Click(object sender, EventArgs e)
        {
            _mediaChannelManager.ToggleVideoMute();
        }

        private async void mediaChannelDisconnectButton_Click(object sender, EventArgs e)
        {
            await _mediaChannelManager.DisconnectFromMediaRoom();
            _chatManager.SwitchToTextChannel(1);
        }

        private void globalMuteButton_Click(object sender, EventArgs e)
        {
            _mediaChannelManager.ToggleGlobalMute(globalMuteButton);
        }

        private void deafenButton_Click(object sender, EventArgs e)
        {
            _mediaChannelManager.ToggleGlobalDeafen(deafenButton);
        }

        private void messageInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            _chatManager.HandleMessageInputKeyDown(e);
        }

        private void emojiButton_Click(object sender, EventArgs e)
        {
            _emojiManager.ToggleEmojiSelectionPanel();
        }

        #endregion
    }
}