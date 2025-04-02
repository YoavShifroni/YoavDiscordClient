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
    /// Provides color settings for dark-themed context menus
    /// </summary>
    public class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(64, 68, 75);
        public override Color MenuItemBorder => Color.FromArgb(32, 34, 37);
        public override Color MenuBorder => Color.FromArgb(32, 34, 37);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(64, 68, 75);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(64, 68, 75);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(64, 68, 75);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(64, 68, 75);
        public override Color ToolStripDropDownBackground => Color.FromArgb(47, 49, 54);
        public override Color ImageMarginGradientBegin => Color.FromArgb(47, 49, 54);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(47, 49, 54);
        public override Color ImageMarginGradientEnd => Color.FromArgb(47, 49, 54);
    }
}
