using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient.Style
{
    /// <summary>
    /// Manages theme settings and provides theme-consistent colors throughout the Discord clone application.
    /// </summary>
    /// <remarks>
    /// The ThemeManager provides a centralized system for theme management, supporting both dark and light themes.
    /// It maintains a dictionary of theme colors and offers methods to retrieve appropriate colors for various UI elements.
    /// This ensures visual consistency across the application and simplifies theme switching functionality.
    /// 
    /// Common usage:
    /// - Get a theme color: ThemeManager.GetColor("Background")
    /// - Get a role-specific color: ThemeManager.GetRoleColor(roleId, isOnline)
    /// - Switch themes: ThemeManager.IsDarkTheme = false;
    /// </remarks>
    public static class ThemeManager
    {
        #region Private fields
        /// <summary>
        /// Flag indicating whether the dark theme is currently active.
        /// Default is true (dark theme).
        /// </summary>
        private static bool isDarkTheme = true;

        /// <summary>
        /// Dictionary storing the current set of theme colors by name.
        /// Updated whenever the theme changes.
        /// </summary>
        private static Dictionary<string, Color> currentThemeColors = new Dictionary<string, Color>();
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets whether the dark theme is enabled.
        /// Setting this property triggers an update of the current theme colors.
        /// </summary>
        /// <remarks>
        /// When this property is changed, the UpdateCurrentThemeColors method is called
        /// to refresh all theme colors in the application.
        /// </remarks>
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
        /// Static constructor to initialize theme settings when the class is first accessed.
        /// </summary>
        /// <remarks>
        /// This populates the currentThemeColors dictionary with the default theme colors (dark theme).
        /// </remarks>
        static ThemeManager()
        {
            UpdateCurrentThemeColors();
        }

        /// <summary>
        /// Gets a color from the current theme by its name.
        /// </summary>
        /// <param name="colorName">The name of the color to retrieve (e.g., "Background", "TextColor").</param>
        /// <returns>The color from the current theme, or a default color if the requested color name is not found.</returns>
        /// <remarks>
        /// If the requested color is not found in the current theme, a warning is logged and a default color is returned.
        /// The default color is dark gray (#363945) for dark theme or white (#FFFFFF) for light theme.
        /// </remarks>
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
        /// Gets the appropriate color for a user based on their role ID and online status.
        /// </summary>
        /// <param name="roleId">The role ID of the user (0 for Admin, 1 for Moderator, other values for regular Member).</param>
        /// <param name="isOnline">Whether the user is currently online. Defaults to true.</param>
        /// <returns>A color appropriate for the user's role and status in the current theme.</returns>
        /// <remarks>
        /// This method handles:
        /// - Different role colors based on the current theme (dark or light)
        /// - Adjustment of colors for offline users (reduced brightness)
        /// - Special case handling for member colors when offline
        /// 
        /// Role IDs:
        /// - 0: Administrator (typically red)
        /// - 1: Moderator (typically blue/green)
        /// - Other values: Regular member (typically white/black depending on theme)
        /// </remarks>
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

        /// <summary>
        /// Updates the current theme colors dictionary based on the active theme.
        /// </summary>
        /// <remarks>
        /// This method is called:
        /// - During static initialization
        /// - Whenever the IsDarkTheme property changes
        /// 
        /// It refreshes all color values in the currentThemeColors dictionary to match
        /// either the dark or light theme. The dictionary is organized into categories:
        /// - Main colors (backgrounds, text)
        /// - Button colors
        /// - Status colors (online, offline, muted)
        /// - Role colors (admin, moderator, member)
        /// </remarks>
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