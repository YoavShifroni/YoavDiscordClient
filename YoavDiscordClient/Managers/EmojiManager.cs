using System;
using System.Drawing;
using System.Windows.Forms;
using YoavDiscordClient.Style;

namespace YoavDiscordClient.Managers
{
    /// <summary>
    /// Manages emoji functionality including emoji selection and insertion.
    /// </summary>
    public class EmojiManager
    {
        /// <summary>
        /// Reference to the main Discord application form that owns this manager.
        /// Used to access application-wide functionality and other managers.
        /// </summary>
        private readonly DiscordApp _form;

        /// <summary>
        /// The panel that contains the emoji selection interface.
        /// This panel is shown or hidden when the emoji button is clicked.
        /// </summary>
        private readonly Panel _emojiSelectionPanel;

        /// <summary>
        /// The panel that contains the emoji icon grid.
        /// This is where individual emoji icons are added for user selection.
        /// </summary>
        private readonly Panel _emojiPanel;

        /// <summary>
        /// The text box where messages are typed and where selected emojis are inserted.
        /// </summary>
        private readonly TextBox _messageInputTextBox;

        /// <summary>
        /// The button that toggles the visibility of the emoji selection panel.
        /// </summary>
        private readonly Button _emojiButton;

        /// <summary>
        /// Flag indicating whether the emoji selection panel is currently visible.
        /// </summary>
        private bool _isEmojiSelectionVisible = false;

        /// <summary>
        /// Initializes a new instance of the EmojiManager class.
        /// </summary>
        /// <param name="form">The main Discord application form that owns this manager.</param>
        /// <param name="emojiSelectionPanel">The panel containing the emoji selection interface.</param>
        /// <param name="emojiPanel">The panel where emoji icons are displayed in a grid.</param>
        /// <param name="messageInputTextBox">The text box where messages are typed and emojis are inserted.</param>
        /// <param name="emojiButton">The button that toggles the emoji selection panel visibility.</param>
        /// <remarks>
        /// The EmojiManager is responsible for:
        /// - Loading and displaying available emojis in a selectable grid
        /// - Handling emoji selection events
        /// - Inserting selected emojis into the message input text box
        /// - Managing the visibility of the emoji selection panel
        /// - Handling clicks outside the emoji panel to dismiss it
        /// 
        /// It requires references to UI components to manage the emoji selection
        /// workflow and coordinate with the chat input system.
        /// </remarks>
        public EmojiManager(DiscordApp form, Panel emojiSelectionPanel, Panel emojiPanel, TextBox messageInputTextBox, Button emojiButton)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));
            _emojiSelectionPanel = emojiSelectionPanel ?? throw new ArgumentNullException(nameof(emojiSelectionPanel));
            _emojiPanel = emojiPanel ?? throw new ArgumentNullException(nameof(emojiPanel));
            _messageInputTextBox = messageInputTextBox ?? throw new ArgumentNullException(nameof(messageInputTextBox));
            _emojiButton = emojiButton ?? throw new ArgumentNullException(nameof(emojiButton));
        }

        /// <summary>
        /// Adds emoji buttons to the emoji selection panel.
        /// Also initializes the position and visibility of emoji-related UI elements.
        /// </summary>
        public void AddEmojisToPanel()
        {
            _messageInputTextBox.Width -= 40;

            // Set initial position of emoji panel
            _emojiPanel.Location = new Point(
                _messageInputTextBox.Right,  // Place it right at the edge
                _messageInputTextBox.Top);

            _emojiPanel.Visible = true;

            // Initialize emoji selection panel position
            _emojiSelectionPanel.Location = new Point(
                _emojiButton.Left - _emojiSelectionPanel.Width + _emojiButton.Width,
                _emojiButton.Top - _emojiSelectionPanel.Height - 5);

            // Common emojis array
            string[] emojis = new string[]
            {
                "😀", "😃", "😄", "😁", "😆", "😅", "🤣", "😂", "🙂", "🙃",
                "😉", "😊", "😇", "😍", "🥰", "😘", "😗", "😚", "😙", "😋",
                "😛", "😜", "😝", "🤑", "🤗", "🤔", "🤐", "😐", "😑", "😶",
                "😏", "😒", "🙄", "😬", "🤥", "😌", "😔", "😪", "🤤", "😴",
                "😷", "🤒", "🤕", "🤢", "🤮", "🤧", "🥵", "🥶", "🥴", "😵",
                "👋", "🤚", "✋", "🖖", "👌", "✌️", "🤞", "🤟", "🤘", "🤙"
            };

            int buttonSize = 30;
            int columns = 6;
            int margin = 3;
            int xPos = margin;
            int yPos = margin;
            int column = 0;

            foreach (string emoji in emojis)
            {
                Button emojiBtn = new Button
                {
                    Text = emoji,
                    Size = new Size(buttonSize, buttonSize),
                    Location = new Point(xPos, yPos),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = ThemeManager.GetColor("ButtonBackground"),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI Emoji", 12)
                };

                emojiBtn.FlatAppearance.BorderSize = 0;
                emojiBtn.Click += (sender, e) => EmojiSelected(emoji);
                _emojiSelectionPanel.Controls.Add(emojiBtn);

                column++;
                if (column >= columns)
                {
                    column = 0;
                    xPos = margin;
                    yPos += buttonSize + margin;
                }
                else
                {
                    xPos += buttonSize + margin;
                }
            }
        }

        /// <summary>
        /// Handles the form mouse down event to close emoji panel when clicking outside it.
        /// </summary>
        /// <param name="e">Mouse event arguments.</param>
        public void HandleFormMouseDown(MouseEventArgs e)
        {
            if (_isEmojiSelectionVisible)
            {
                // Convert the mouse position to client coordinates relative to the emoji selection panel
                Point mousePos = _emojiSelectionPanel.PointToClient(Cursor.Position);

                // Check if the click is outside the selection panel
                if (!_emojiSelectionPanel.ClientRectangle.Contains(mousePos))
                {
                    CloseEmojiPanel();
                }
            }
        }

        /// <summary>
        /// Toggles the visibility of the emoji selection panel.
        /// </summary>
        public void ToggleEmojiSelectionPanel()
        {
            // Update emoji selection panel position relative to the emoji button
            // Position it above the emoji button
            _emojiSelectionPanel.Location = new Point(
                _emojiButton.Parent.Left + _emojiButton.Left - _emojiSelectionPanel.Width + _emojiButton.Width,
                _emojiButton.Parent.Top + _emojiButton.Top - _emojiSelectionPanel.Height - 5);

            // Toggle emoji selection panel visibility
            _isEmojiSelectionVisible = !_isEmojiSelectionVisible;
            _emojiSelectionPanel.Visible = _isEmojiSelectionVisible;

            // Bring selection panel to front if visible
            if (_isEmojiSelectionVisible)
            {
                _emojiSelectionPanel.BringToFront();
            }
        }

        /// <summary>
        /// Handles the selection of an emoji from the emoji panel.
        /// Inserts the selected emoji at the current cursor position in the message input text box.
        /// </summary>
        /// <param name="emoji">The emoji character to insert.</param>
        private void EmojiSelected(string emoji)
        {
            // Insert emoji at current cursor position
            int cursorPosition = _messageInputTextBox.SelectionStart;
            _messageInputTextBox.Text = _messageInputTextBox.Text.Insert(cursorPosition, emoji);

            // Update cursor position after insertion
            _messageInputTextBox.SelectionStart = cursorPosition + emoji.Length;

            // Hide emoji selection panel after selection
            this.CloseEmojiPanel();

            // Focus back on the text input box
            _messageInputTextBox.Focus();
        }

        /// <summary>
        /// Updates the visibility and position of the emoji panel based on the visibility of the message input text box.
        /// </summary>
        public void UpdateEmojiPanelVisibility()
        {
            // IMPORTANT: The emoji panel should be visible only when the text input is visible
            _emojiPanel.Visible = _messageInputTextBox.Visible;

            // Ensure the emoji panel is in the correct position
            if (_messageInputTextBox.Visible)
            {
                _emojiPanel.Location = new Point(
                    _messageInputTextBox.Right,  // Always at the right edge
                    _messageInputTextBox.Top);
                _emojiPanel.BringToFront();  // Ensure it's on top of other controls
            }

            // Hide emoji selection panel if we're switching channels
            if (_isEmojiSelectionVisible)
            {
                CloseEmojiPanel();
            }
        }

        /// <summary>
        /// Closes the emoji selection panel.
        /// </summary>
        public void CloseEmojiPanel()
        {
            _isEmojiSelectionVisible = false;
            _emojiSelectionPanel.Visible = false;
        }

        /// <summary>
        /// Hides the emoji panel.
        /// </summary>
        public void HideEmojiPanel()
        {
            _emojiPanel.Visible = false;
            CloseEmojiPanel();
        }

        /// <summary>
        /// Checks if the emoji selection panel is currently visible.
        /// </summary>
        /// <returns>True if the emoji selection panel is visible, false otherwise.</returns>
        public bool IsEmojiSelectionVisible()
        {
            return _isEmojiSelectionVisible;
        }
    }
}