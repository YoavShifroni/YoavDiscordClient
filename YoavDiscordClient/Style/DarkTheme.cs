using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient.Style
{
    public static class DarkTheme
    {
        // Main colors
        public static Color Background = Color.FromArgb(54, 57, 63);
        public static Color SecondaryBackground = Color.FromArgb(47, 49, 54);
        public static Color ChannelBackground = Color.FromArgb(64, 68, 75);
        public static Color TextColor = Color.White;
        public static Color SecondaryTextColor = Color.FromArgb(185, 187, 190);
        public static Color AccentColor = Color.FromArgb(114, 137, 218);

        // Button colors
        public static Color ButtonBackground = Color.FromArgb(64, 68, 75);
        public static Color ButtonHoverBackground = Color.FromArgb(79, 84, 92);
        public static Color ButtonTextColor = Color.White;

        // Status colors
        public static Color OnlineColor = Color.FromArgb(67, 181, 129);
        public static Color OfflineColor = Color.FromArgb(128, 132, 142);
        public static Color MutedColor = Color.FromArgb(240, 71, 71);
        public static Color DeafenedColor = Color.FromArgb(240, 71, 71);

        // Role colors
        public static Color AdminColor = Color.FromArgb(255, 75, 75);
        public static Color ModeratorColor = Color.FromArgb(75, 165, 255);
        public static Color MemberColor = Color.White;
    }
}
