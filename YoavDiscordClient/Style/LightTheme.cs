using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient.Style
{
    public static class LightTheme
    {
        // Main colors
        public static Color Background = Color.FromArgb(255, 255, 255);
        public static Color SecondaryBackground = Color.FromArgb(240, 240, 240);
        public static Color ChannelBackground = Color.FromArgb(230, 230, 230);
        public static Color TextColor = Color.FromArgb(23, 23, 23);
        public static Color SecondaryTextColor = Color.FromArgb(100, 100, 100);
        public static Color AccentColor = Color.FromArgb(114, 137, 218);

        // Button colors
        public static Color ButtonBackground = Color.FromArgb(220, 221, 222);
        public static Color ButtonHoverBackground = Color.FromArgb(200, 200, 200);
        public static Color ButtonTextColor = Color.FromArgb(23, 23, 23);

        // Status colors
        public static Color OnlineColor = Color.FromArgb(67, 181, 129);
        public static Color OfflineColor = Color.FromArgb(128, 132, 142);
        public static Color MutedColor = Color.FromArgb(240, 71, 71);
        public static Color DeafenedColor = Color.FromArgb(240, 71, 71);

        // Role colors
        public static Color AdminColor = Color.FromArgb(220, 50, 50);
        public static Color ModeratorColor = Color.FromArgb(50, 120, 200);
        public static Color MemberColor = Color.FromArgb(23, 23, 23);
    }
}
