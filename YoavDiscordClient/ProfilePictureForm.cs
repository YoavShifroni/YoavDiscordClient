using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public partial class ProfilePictureForm : Form
    {
        /// <summary>
        /// ???
        /// </summary>
        private Bitmap originalImage;

        /// <summary>
        /// Pointer that represent where the user clicked
        /// </summary>
        private Point circleCenter;

        /// <summary>
        /// Radius for the circle
        /// </summary>
        private int circleRadius;

        /// <summary>
        /// Boolean that tell is image is loaded yet or no
        /// </summary>
        private bool isImageLoaded = false;

        /// <summary>
        /// Boolean that tell is there is already a circle on the image or no
        /// </summary>
        private bool isThereACircleOnTheImage = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProfilePictureForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The function is called when the "see deafult option" button is clicked and will move the user to the deafult profile pictures window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seeDefaultOptionsButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DiscordFormsHolder.getInstance().DefaultProfilePictureForm.Visible = true;
        }


        /// <summary>
        /// The function let the user chose an image from his file explorer
        /// I took this function from the website StackOverFlow in this link:
        /// https://stackoverflow.com/questions/13775006/how-to-browse-and-save-the-image-in-folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploadPhotoButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.originalImage = new Bitmap(openFileDialog.FileName);
                PictureBox pictureBox = (PictureBox)this.Controls["userProfilePictureBox"];
                pictureBox.Image = originalImage;
                this.isImageLoaded = true;
            }
        }

        private void userProfilePictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (isImageLoaded)
            {
                PictureBox pictureBox = (PictureBox)sender;

                // Get the click position relative to the picture box image
                float xScale = (float)originalImage.Width / pictureBox.ClientSize.Width;
                float yScale = (float)originalImage.Height / pictureBox.ClientSize.Height;
                circleCenter = new Point((int)(e.X * xScale), (int)(e.Y * yScale));

                // Default radius for the circle
                circleRadius = (int)(Math.Min(originalImage.Width, originalImage.Height)*0.45);
                if(circleRadius > 400)
                {
                    circleRadius = 400;
                }
                this.DisplayCircularMaskPreview();
            }
        }

        private void DisplayCircularMaskPreview()
        {
            // Create a new bitmap that will display the circular mask over the image
            Bitmap maskedImage = new Bitmap(originalImage.Width, originalImage.Height);

            using (Graphics g = Graphics.FromImage(maskedImage))
            {
                // Draw the original image
                g.DrawImage(originalImage, new Point(0, 0));

                // Create a circular path for the mask
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(circleCenter.X - circleRadius, circleCenter.Y - circleRadius, circleRadius * 2, circleRadius * 2);

                // Fill everything outside the circle with a semi-transparent mask
                Region regionOutsideCircle = new Region(new Rectangle(0, 0, maskedImage.Width, maskedImage.Height));
                regionOutsideCircle.Exclude(path);
                using (SolidBrush maskBrush = new SolidBrush(Color.FromArgb(150, Color.Black)))
                {
                    g.FillRegion(maskBrush, regionOutsideCircle);
                }

                // Optionally, draw a border around the circle for better visibility
                using (Pen pen = new Pen(Color.Red, 3))
                {
                    g.DrawEllipse(pen, circleCenter.X - circleRadius, circleCenter.Y - circleRadius, circleRadius * 2, circleRadius * 2);
                }
            }

            // Display the masked image in the PictureBox
            PictureBox pictureBox = (PictureBox)this.Controls["userProfilePictureBox"];
            pictureBox.Image = maskedImage;
            this.isThereACircleOnTheImage = true;
        }

        private Bitmap CropToCircle(Bitmap originalImage, Point center, int radius)
        {
            Bitmap croppedImage = new Bitmap(radius * 2, radius * 2);

            using (Graphics g = Graphics.FromImage(croppedImage))
            {
                g.Clear(Color.Transparent);

                // Create a circular clipping region
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, radius * 2, radius * 2);
                g.SetClip(path);

                // Draw the cropped region from the original image
                Rectangle srcRect = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
                Rectangle destRect = new Rectangle(0, 0, radius * 2, radius * 2);

                g.DrawImage(originalImage, destRect, srcRect, GraphicsUnit.Pixel);
            }

            return croppedImage;
        }

        private void chooseThisPhotoButton_Click(object sender, EventArgs e)
        {
            if(!this.isThereACircleOnTheImage)
            {
                MessageBox.Show("you need to select a circle before choosing an image");
                return;
            }
            Bitmap croppedImage = CropToCircle(originalImage, circleCenter, circleRadius);
            byte[] imageToByteArray = this.ImageToByteArray(croppedImage);
            RegistrationInfo registrationInfo = DiscordFormsHolder.getInstance().RegistrationForm.RegistrationInfo;
            ConnectionManager.getInstance(null).ProcessRegistration(registrationInfo, imageToByteArray);
        }

        /// <summary>
        /// The functino convert the object image to byte array
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Save as PNG or any format you prefer
                return ms.ToArray();
            }
        }

        /// <summary>
        /// The function get as a parameter image and display it in the picture box
        /// </summary>
        /// <param name="photo"></param>
        public void DisplayPhotoFromDefaultOptions(Image photo)
        {
            this.userProfilePictureBox.Image = photo;
            this.originalImage = new Bitmap(photo);
            this.isImageLoaded = true;
        }

        
    }
}
