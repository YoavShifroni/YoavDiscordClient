using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public class CircleButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            Graphics g = pevent.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Parent.BackColor);
            using (Brush brush = new SolidBrush(BackColor))
            {
                g.FillEllipse(brush, 0, 0, Width - 1, Height - 1);
            }
            using (Pen pen = new Pen(ForeColor, 2))
            {
                g.DrawEllipse(pen, 0, 0, Width - 1, Height - 1);
            }
            TextRenderer.DrawText(g, Text, Font, new Rectangle(0, 0, Width, Height), ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
