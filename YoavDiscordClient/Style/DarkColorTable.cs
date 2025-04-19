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
    /// Provides color settings for dark-themed context menus in the Discord clone application.
    /// </summary>
    /// <remarks>
    /// This class extends the <see cref="ProfessionalColorTable"/> to customize the appearance of 
    /// context menus and toolstrips with a dark theme that matches Discord's visual style.
    /// All colors are based on Discord's dark theme color palette for consistent UI appearance.
    /// </remarks>
    public class DarkColorTable : ProfessionalColorTable
    {
        /// <summary>
        /// Gets the color used for selected menu items.
        /// </summary>
        /// <returns>A mid-dark gray color (RGB: 64, 68, 75) that highlights selected menu items.</returns>
        public override Color MenuItemSelected => Color.FromArgb(64, 68, 75);

        /// <summary>
        /// Gets the color used for menu item borders.
        /// </summary>
        /// <returns>A dark gray color (RGB: 32, 34, 37) for subtle menu item borders.</returns>
        public override Color MenuItemBorder => Color.FromArgb(32, 34, 37);

        /// <summary>
        /// Gets the color used for the outer border of the entire menu.
        /// </summary>
        /// <returns>A dark gray color (RGB: 32, 34, 37) for the menu's outer border.</returns>
        public override Color MenuBorder => Color.FromArgb(32, 34, 37);

        /// <summary>
        /// Gets the starting gradient color for pressed menu items.
        /// </summary>
        /// <returns>A mid-dark gray color (RGB: 64, 68, 75) for the beginning of pressed item gradients.</returns>
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(64, 68, 75);

        /// <summary>
        /// Gets the ending gradient color for pressed menu items.
        /// </summary>
        /// <returns>A mid-dark gray color (RGB: 64, 68, 75) for the end of pressed item gradients.</returns>
        /// <remarks>
        /// This is the same as the beginning gradient color, creating a solid color rather than a true gradient.
        /// This matches Discord's flat UI design aesthetic.
        /// </remarks>
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(64, 68, 75);

        /// <summary>
        /// Gets the starting gradient color for selected menu items.
        /// </summary>
        /// <returns>A mid-dark gray color (RGB: 64, 68, 75) for the beginning of selected item gradients.</returns>
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(64, 68, 75);

        /// <summary>
        /// Gets the ending gradient color for selected menu items.
        /// </summary>
        /// <returns>A mid-dark gray color (RGB: 64, 68, 75) for the end of selected item gradients.</returns>
        /// <remarks>
        /// This is the same as the beginning gradient color, creating a solid color rather than a true gradient.
        /// This matches Discord's flat UI design aesthetic.
        /// </remarks>
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(64, 68, 75);

        /// <summary>
        /// Gets the background color for dropdown menus.
        /// </summary>
        /// <returns>A dark gray color (RGB: 47, 49, 54) for the menu background.</returns>
        /// <remarks>
        /// This color serves as the main background for all dropdown menus and context menus.
        /// </remarks>
        public override Color ToolStripDropDownBackground => Color.FromArgb(47, 49, 54);

        /// <summary>
        /// Gets the starting gradient color for the margin where menu icons are displayed.
        /// </summary>
        /// <returns>A dark gray color (RGB: 47, 49, 54) for the beginning of the image margin gradient.</returns>
        public override Color ImageMarginGradientBegin => Color.FromArgb(47, 49, 54);

        /// <summary>
        /// Gets the middle gradient color for the margin where menu icons are displayed.
        /// </summary>
        /// <returns>A dark gray color (RGB: 47, 49, 54) for the middle of the image margin gradient.</returns>
        public override Color ImageMarginGradientMiddle => Color.FromArgb(47, 49, 54);

        /// <summary>
        /// Gets the ending gradient color for the margin where menu icons are displayed.
        /// </summary>
        /// <returns>A dark gray color (RGB: 47, 49, 54) for the end of the image margin gradient.</returns>
        /// <remarks>
        /// All three image margin colors are identical to create a solid color background for the icon area,
        /// matching Discord's flat design approach.
        /// </remarks>
        public override Color ImageMarginGradientEnd => Color.FromArgb(47, 49, 54);
    }
}