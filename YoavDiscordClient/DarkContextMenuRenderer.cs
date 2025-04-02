using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoavDiscordClient.Style;

namespace YoavDiscordClient
{
    public class DarkContextMenuRenderer : ToolStripProfessionalRenderer
    {
        /// <summary>
        /// Creates a new instance of DarkContextMenuRenderer
        /// </summary>
        public DarkContextMenuRenderer() : base(new DarkColorTable())
        {
        }

        /// <summary>
        /// Renders the text of a menu item
        /// </summary>
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            try
            {
                // Use theme color for text
                e.TextColor = ThemeManager.GetColor("TextColor");
                base.OnRenderItemText(e);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error rendering menu item text: {ex.Message}");
                e.TextColor = Color.White; // Fallback
                base.OnRenderItemText(e);
            }
        }

        /// <summary>
        /// Renders the check mark of a menu item
        /// </summary>
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            try
            {
                // Get the item and check if it's a MenuItem and if it's checked
                if (e.Item is ToolStripMenuItem menuItem && menuItem.Checked)
                {
                    // Custom checkbox rendering
                    Rectangle rect = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
                    rect.Inflate(-2, -2);

                    // Fill background with accent color from theme
                    Color accentColor = ThemeManager.GetColor("AccentColor");
                    e.Graphics.FillRectangle(new SolidBrush(accentColor), rect);

                    // Draw checkmark
                    using (Pen pen = new Pen(ThemeManager.GetColor("TextColor"), 2))
                    {
                        Point[] points = new Point[]
                        {
                            new Point(rect.Left + 3, rect.Top + rect.Height / 2),
                            new Point(rect.Left + rect.Width / 3, rect.Bottom - 3),
                            new Point(rect.Right - 3, rect.Top + 3)
                        };
                        e.Graphics.DrawLines(pen, points);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error rendering menu item check: {ex.Message}");
                base.OnRenderItemCheck(e);
            }
        }

        /// <summary>
        /// Renders a separator in the menu
        /// </summary>
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            try
            {
                // Draw a custom separator
                Rectangle rect = e.Item.ContentRectangle;
                int y = rect.Height / 2;
                using (Pen pen = new Pen(ThemeManager.GetColor("SecondaryTextColor")))
                {
                    e.Graphics.DrawLine(pen, new Point(rect.Left + 2, y), new Point(rect.Right - 2, y));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error rendering menu separator: {ex.Message}");
                base.OnRenderSeparator(e);
            }
        }

        /// <summary>
        /// Renders the background of a menu item
        /// </summary>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            try
            {
                if (e.Item.Selected)
                {
                    // Draw selected background with hover color from theme
                    Rectangle rect = new Rectangle(2, 0, e.Item.Width - 4, e.Item.Height);
                    using (SolidBrush brush = new SolidBrush(ThemeManager.GetColor("ButtonHoverBackground")))
                    {
                        e.Graphics.FillRectangle(brush, rect);
                    }
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error rendering menu item background: {ex.Message}");
                base.OnRenderMenuItemBackground(e);
            }
        }
    }
}
