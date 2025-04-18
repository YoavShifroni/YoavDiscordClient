using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient.Style
{
    public static class ThemeManager
    {

        #region Private fields
        private static bool isDarkTheme = true;

        private static Dictionary<string, Color> currentThemeColors = new Dictionary<string, Color>();

        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets whether the dark theme is enabled
        /// </summary>
        public static bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                if (isDarkTheme != value)
                {
                    isDarkTheme = value;
                    UpdateCurrentThemeColors();
                }
            }
        }
        #endregion

        /// <summary>
        /// Static constructor to initialize theme
        /// </summary>
        static ThemeManager()
        {
            UpdateCurrentThemeColors();
        }

        /// <summary>
        /// Gets a color from the current theme
        /// </summary>
        /// <param name="colorName">The name of the color to retrieve</param>
        /// <returns>The color from the current theme</returns>
        public static Color GetColor(string colorName)
        {
            if (currentThemeColors.ContainsKey(colorName))
            {
                return currentThemeColors[colorName];
            }

            // If color not found, return a default color and log warning
            System.Diagnostics.Debug.WriteLine($"Theme color '{colorName}' not found in current theme.");
            return isDarkTheme ? Color.FromArgb(54, 57, 63) : Color.FromArgb(255, 255, 255);
        }

        /// <summary>
        /// Gets the role color for the specified role ID
        /// </summary>
        /// <param name="roleId">The role ID</param>
        /// <param name="isOnline">Whether the user is online</param>
        /// <returns>The color for the specified role</returns>
        public static Color GetRoleColor(int roleId, bool isOnline = true)
        {
            Color baseColor;

            if (isDarkTheme)
            {
                // Get color from dark theme
                switch (roleId)
                {
                    case 0: // Admin
                        baseColor = DarkTheme.AdminColor;
                        break;
                    case 1: // Moderator
                        baseColor = DarkTheme.ModeratorColor;
                        break;
                    default: // Member or unknown
                        baseColor = DarkTheme.MemberColor;
                        break;
                }
            }
            else
            {
                // Get color from light theme
                switch (roleId)
                {
                    case 0: // Admin
                        baseColor = LightTheme.AdminColor;
                        break;
                    case 1: // Moderator
                        baseColor = LightTheme.ModeratorColor;
                        break;
                    default: // Member or unknown
                        baseColor = LightTheme.MemberColor;
                        break;
                }
            }

            // If user is offline, reduce brightness of the color
            if (!isOnline)
            {
                // If the color is white/black (for Members), use a standard gray
                if (isDarkTheme && baseColor == DarkTheme.MemberColor ||
                    !isDarkTheme && baseColor == LightTheme.MemberColor)
                {
                    return Color.FromArgb(128, 128, 128);
                }

                // Otherwise, darken the color
                float factor = 0.6f;
                int r = (int)(baseColor.R * factor);
                int g = (int)(baseColor.G * factor);
                int b = (int)(baseColor.B * factor);

                return Color.FromArgb(r, g, b);
            }

            return baseColor;
        }



        private static void UpdateCurrentThemeColors()
        {
            // Clear existing colors
            currentThemeColors.Clear();

            // Add colors from current theme
            if (isDarkTheme)
            {
                // Main colors
                currentThemeColors["Background"] = DarkTheme.Background;
                currentThemeColors["SecondaryBackground"] = DarkTheme.SecondaryBackground;
                currentThemeColors["ChannelBackground"] = DarkTheme.ChannelBackground;
                currentThemeColors["TextColor"] = DarkTheme.TextColor;
                currentThemeColors["SecondaryTextColor"] = DarkTheme.SecondaryTextColor;
                currentThemeColors["AccentColor"] = DarkTheme.AccentColor;

                // Button colors
                currentThemeColors["ButtonBackground"] = DarkTheme.ButtonBackground;
                currentThemeColors["ButtonHoverBackground"] = DarkTheme.ButtonHoverBackground;
                currentThemeColors["ButtonTextColor"] = DarkTheme.ButtonTextColor;

                // Status colors
                currentThemeColors["OnlineColor"] = DarkTheme.OnlineColor;
                currentThemeColors["OfflineColor"] = DarkTheme.OfflineColor;
                currentThemeColors["MutedColor"] = DarkTheme.MutedColor;
                currentThemeColors["DeafenedColor"] = DarkTheme.DeafenedColor;

                // Role colors
                currentThemeColors["AdminColor"] = DarkTheme.AdminColor;
                currentThemeColors["ModeratorColor"] = DarkTheme.ModeratorColor;
                currentThemeColors["MemberColor"] = DarkTheme.MemberColor;
            }
            else
            {
                // Main colors
                currentThemeColors["Background"] = LightTheme.Background;
                currentThemeColors["SecondaryBackground"] = LightTheme.SecondaryBackground;
                currentThemeColors["ChannelBackground"] = LightTheme.ChannelBackground;
                currentThemeColors["TextColor"] = LightTheme.TextColor;
                currentThemeColors["SecondaryTextColor"] = LightTheme.SecondaryTextColor;
                currentThemeColors["AccentColor"] = LightTheme.AccentColor;

                // Button colors
                currentThemeColors["ButtonBackground"] = LightTheme.ButtonBackground;
                currentThemeColors["ButtonHoverBackground"] = LightTheme.ButtonHoverBackground;
                currentThemeColors["ButtonTextColor"] = LightTheme.ButtonTextColor;

                // Status colors
                currentThemeColors["OnlineColor"] = LightTheme.OnlineColor;
                currentThemeColors["OfflineColor"] = LightTheme.OfflineColor;
                currentThemeColors["MutedColor"] = LightTheme.MutedColor;
                currentThemeColors["DeafenedColor"] = LightTheme.DeafenedColor;

                // Role colors
                currentThemeColors["AdminColor"] = LightTheme.AdminColor;
                currentThemeColors["ModeratorColor"] = LightTheme.ModeratorColor;
                currentThemeColors["MemberColor"] = LightTheme.MemberColor;
            }
        }






    }
}
