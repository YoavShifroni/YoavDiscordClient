using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public class BotMessagePanel : Panel
    {
        public BotMessagePanel(int botId, string botName, string message, DateTime time)
        {
            this.Size = new Size(580, 0); // Height will be calculated based on content

            // Create bot avatar
            CirclePictureBox botAvatar = new CirclePictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(5, 5),
                BackColor = Color.FromArgb(64, 68, 75)
                // You would set the image based on botId
            };

            // Create bot name label with distinguishing color
            Label botNameLabel = new Label
            {
                Text = botName,
                Location = new Point(55, 5),
                AutoSize = true,
                ForeColor = GetBotColor(botId),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            // Create message label
            Label messageLabel = new Label
            {
                Text = message,
                Location = new Point(55, 25),
                Size = new Size(510, 0),
                AutoSize = false,
                ForeColor = Color.White
            };

            // Calculate the height needed for the text
            Size textSize = TextRenderer.MeasureText(message, messageLabel.Font,
                new Size(messageLabel.Width, 0), TextFormatFlags.WordBreak);
            messageLabel.Height = textSize.Height;

            // Add controls to the panel
            this.Controls.Add(botAvatar);
            this.Controls.Add(botNameLabel);
            this.Controls.Add(messageLabel);

            // Adjust panel height to fit all contents
            this.Height = Math.Max(50, messageLabel.Bottom + 10);
        }

        private Color GetBotColor(int botId)
        {
            // Return different colors for different bots
            // For example, ModBot could be red, TranslateBot could be blue
            switch (botId)
            {
                case 1: return Color.FromArgb(192, 80, 80); // ModBot
                case 2: return Color.FromArgb(80, 192, 192); // TranslateBot
                default: return Color.White;
            }
        }
    }
}
