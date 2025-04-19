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
    /// This is the primary user interface, coordinating all user interactions through
    /// specialized manager classes for different functional areas.
    /// </summary>
    /// <remarks>
    /// The application structure follows a manager-based architecture where different
    /// responsibilities are delegated to specialized manager classes:
    /// - ChatManager: Handles text messaging and chat history
    /// - MediaChannelManager: Manages voice/video connections
    /// - UserManager: Handles user profiles and status
    /// - EmojiManager: Manages emoji insertion and selection
    /// - ContextMenuManager: Handles right-click context menus
    /// 
    /// Each manager works with specific panels in the UI to maintain separation of concerns.
    /// </remarks>
    public partial class DiscordApp : Form
    {
        // Core managers

        /// <summary>
        /// Manages all chat-related functionality including sending messages,
        /// displaying chat history, and switching between text channels.
        /// </summary>
        private readonly ChatManager _chatManager;

        /// <summary>
        /// Handles voice and video communications, including connecting to
        /// media rooms, muting, and managing media controls.
        /// </summary>
        private readonly MediaChannelManager _mediaChannelManager;

        /// <summary>
        /// Manages user profiles, status, and display within the application.
        /// Handles user-related data and UI elements.
        /// </summary>
        private readonly UserManager _userManager;

        /// <summary>
        /// Manages emoji selection, display, and insertion into chat messages.
        /// Controls the emoji panel and its interactions.
        /// </summary>
        private readonly EmojiManager _emojiManager;

        /// <summary>
        /// Handles context menus throughout the application, particularly
        /// for user interaction options.
        /// </summary>
        private readonly ContextMenuManager _contextMenuManager;

        /// <summary>
        /// Constructor for the DiscordApp form.
        /// Initializes the form components and sets up all manager classes
        /// with their respective UI panels.
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
        /// Sets up channel panels, context menus, user profiles, and connects to the server
        /// to retrieve initial user and message data.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DiscordApp_Load(object sender, EventArgs e)
        {
            // Resize the form based on screen resolution for optimal display
            DiscordFormsHolder.ResizeFormBasedOnResolution(this, 2175f, 1248f);

            // Initialize UI components through manager classes
            _mediaChannelManager.InitializeMediaChannelPanels();
            _contextMenuManager.InitializeUserContextMenu();
            _userManager.AddPicturesToTheWindow();
            _emojiManager.AddEmojisToPanel();

            // Connect to server and fetch initial data
            ConnectionManager.GetInstance(null).ProcessFetchAllUsers();
            ConnectionManager.GetInstance(null).ProcessGetMessagesHistoryOfChatRoom(1);
        }

        /// <summary>
        /// Event handler for mouse down events on the form.
        /// Used primarily to close the emoji selection panel when clicking outside of it,
        /// providing a natural user interaction experience.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Mouse event arguments containing cursor position and button state.</param>
        private void DiscordApp_MouseDown(object sender, MouseEventArgs e)
        {
            _emojiManager.HandleFormMouseDown(e);
        }

        #region Button Event Handlers

        /// <summary>
        /// Handles the send message button click event.
        /// Delegates to ChatManager to process and send the message.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            _chatManager.SendMessage(messageInputTextBox.Text);
        }

        /// <summary>
        /// Handles the text channel 1 button click.
        /// Switches the active chat to text channel 1.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void textChanel1Button_Click(object sender, EventArgs e)
        {
            _chatManager.SwitchToTextChannel(1);
        }

        /// <summary>
        /// Handles the text channel 2 button click.
        /// Switches the active chat to text channel 2.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void textChanel2Button_Click(object sender, EventArgs e)
        {
            _chatManager.SwitchToTextChannel(2);
        }

        /// <summary>
        /// Handles the text channel 3 button click.
        /// Switches the active chat to text channel 3.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void textChanel3Button_Click(object sender, EventArgs e)
        {
            _chatManager.SwitchToTextChannel(3);
        }

        /// <summary>
        /// Handles the voice channel 1 button click.
        /// Connects the user to voice/video media room 1.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private async void voiceChannel1Button_Click(object sender, EventArgs e)
        {
            await _mediaChannelManager.ConnectToMediaRoom(1);
        }

        /// <summary>
        /// Handles the voice channel 2 button click.
        /// Connects the user to voice/video media room 2.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private async void voiceChannel2Button_Click(object sender, EventArgs e)
        {
            await _mediaChannelManager.ConnectToMediaRoom(2);
        }

        /// <summary>
        /// Handles the voice channel 3 button click.
        /// Connects the user to voice/video media room 3.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private async void voiceChannel3Button_Click(object sender, EventArgs e)
        {
            await _mediaChannelManager.ConnectToMediaRoom(3);
        }

        /// <summary>
        /// Handles the media channel mute button click.
        /// Toggles the user's microphone on/off for the current media session.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void mediaChannelMuteButton_Click(object sender, EventArgs e)
        {
            _mediaChannelManager.ToggleAudioMute();
        }

        /// <summary>
        /// Handles the media channel video mute button click.
        /// Toggles the user's camera on/off for the current media session.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void mediaChannelVideoMuteButton_Click(object sender, EventArgs e)
        {
            _mediaChannelManager.ToggleVideoMute();
        }

        /// <summary>
        /// Handles the media channel disconnect button click.
        /// Disconnects the user from the current media room and returns to text channel 1.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private async void mediaChannelDisconnectButton_Click(object sender, EventArgs e)
        {
            await _mediaChannelManager.DisconnectFromMediaRoom();
            _chatManager.SwitchToTextChannel(1);
        }

        /// <summary>
        /// Handles the global mute button click.
        /// Toggles muting of all incoming audio across all media sessions.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void globalMuteButton_Click(object sender, EventArgs e)
        {
            _mediaChannelManager.ToggleGlobalMute(globalMuteButton);
        }

        /// <summary>
        /// Handles the deafen button click.
        /// Toggles deafening (muting all audio input and output) for the user.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void deafenButton_Click(object sender, EventArgs e)
        {
            _mediaChannelManager.ToggleGlobalDeafen(deafenButton);
        }

        /// <summary>
        /// Handles key down events in the message input text box.
        /// Allows for keyboard shortcuts like Enter to send messages.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Key event arguments containing the key code and modifiers.</param>
        private void messageInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            _chatManager.HandleMessageInputKeyDown(e);
        }

        /// <summary>
        /// Handles the emoji button click.
        /// Shows or hides the emoji selection panel.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void emojiButton_Click(object sender, EventArgs e)
        {
            _emojiManager.ToggleEmojiSelectionPanel();
        }

        #endregion
    }
}