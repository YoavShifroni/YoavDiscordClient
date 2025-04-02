using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace YoavDiscordClient
{
    public class CirclePictureBox : PictureBox
    {
        public CirclePictureBox()
        {
            // Set the size mode to ensure the image fits the control
            this.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Create a circular region based on the size of the PictureBox
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, this.Width, this.Height);
                this.Region = new Region(path); // Clip the PictureBox to a circular region
            }
        }
    }
}

