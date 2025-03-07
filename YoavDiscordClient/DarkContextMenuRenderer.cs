using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public class DarkContextMenuRenderer : ToolStripProfessionalRenderer
    {
        public DarkContextMenuRenderer() : base(new DarkColorTable())
        {
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            // Set text color
            e.TextColor = Color.White;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            // Get the item and check if it's a MenuItem and if it's checked
            if (e.Item is ToolStripMenuItem menuItem && menuItem.Checked)
            {
                // Custom checkbox rendering
                Rectangle rect = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
                rect.Inflate(-2, -2);

                // Fill background
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(114, 137, 218)), rect);

                // Draw checkmark
                using (Pen pen = new Pen(Color.White, 2))
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

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            // Draw a custom separator
            Rectangle rect = e.Item.ContentRectangle;
            int y = rect.Height / 2;
            using (Pen pen = new Pen(Color.FromArgb(80, 80, 80)))
            {
                e.Graphics.DrawLine(pen, new Point(rect.Left + 2, y), new Point(rect.Right - 2, y));
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                // Draw selected background
                Rectangle rect = new Rectangle(2, 0, e.Item.Width - 4, e.Item.Height);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(64, 68, 75)))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }
    }
}
