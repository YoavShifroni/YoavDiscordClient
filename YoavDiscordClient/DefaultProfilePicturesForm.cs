using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public partial class DefaultProfilePicturesForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultProfilePicturesForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The function is called when the user loads this form and add specific images to the picture boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefaultProfilePicturesForm_Load(object sender, EventArgs e)
        {
            this.firstPictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("defaultDiscordImage");
            this.secondPictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("dogPicture");
            this.thirdPictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("catImage");
            this.fourthPictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("cowImage");
            this.fifthPictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("horseImage");
            this.sixPictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("monkeyImage");

        }

        /// <summary>
        ///  The function is called when the "go back" button is clicked and will move the user back to the profile picture window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goBackToProfilePictureFormButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DiscordFormsHolder.getInstance().ProfilePictureForm.Visible = true;
        }

        /// <summary>
        /// The function check if the user selected some photo, if he did it will return "true" else "flase"
        /// </summary>
        /// <returns></returns>
        private bool IsTheUserSelectedSomePhoto()
        {
            if(this.selectPicture1.Checked || this.selectPicture2.Checked || this.selectPicture3.Checked || this.selectPicture4.Checked ||
                this.selectPicture5.Checked || this.selectPicture6.Checked)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// The function is called when the "select picture 1" check is changed and will change all the other checks to be false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectPicture1_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture2.Checked = false; 
            this.selectPicture3.Checked = false;
            this.selectPicture4.Checked = false;
            this.selectPicture5.Checked = false;
            this.selectPicture6.Checked = false;
        }

        /// <summary>
        /// The function is called when the "select picture 2" check is changed and will change all the other checks to be false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectPicture2_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture1.Checked = false;
            this.selectPicture3.Checked = false;
            this.selectPicture4.Checked = false;
            this.selectPicture5.Checked = false;
            this.selectPicture6.Checked = false;
        }

        /// <summary>
        /// The function is called when the "select picture 3" check is changed and will change all the other checks to be false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectPicture3_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture1.Checked = false;
            this.selectPicture2.Checked = false;
            this.selectPicture4.Checked = false;
            this.selectPicture5.Checked = false;
            this.selectPicture6.Checked = false;
        }

        /// <summary>
        /// The function is called when the "select picture 4" check is changed and will change all the other checks to be false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectPicture4_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture1.Checked = false;
            this.selectPicture2.Checked = false;
            this.selectPicture3.Checked = false;
            this.selectPicture5.Checked = false;
            this.selectPicture6.Checked = false;
        }

        /// <summary>
        /// The function is called when the "select picture 5" check is changed and will change all the other checks to be false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectPicture5_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture1.Checked = false;
            this.selectPicture2.Checked = false;
            this.selectPicture3.Checked = false;
            this.selectPicture4.Checked = false;
            this.selectPicture6.Checked = false;
        }

        /// <summary>
        /// The function is called when the "select picture 6" check is changed and will change all the other checks to be false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectPicture6_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture1.Checked = false;
            this.selectPicture2.Checked = false;
            this.selectPicture3.Checked = false;
            this.selectPicture4.Checked = false;
            this.selectPicture5.Checked = false;
        }

        /// <summary>
        /// The function is called when the "choose this photo" button is clicked and will firstly check if the user really selected some photo,
        /// if he did the function will check which photo he selected and will call other function that will display this photo in the picture box
        /// on the profile picture window.
        /// After that the function will move the user to the profile picture window and show him a messgae that tell him to choose where he want
        /// to center the circle that will represent his profile picture 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chooseThisPhotoButton_Click(object sender, EventArgs e)
        {
            if (!this.IsTheUserSelectedSomePhoto())
            {
                MessageBox.Show("you have to choose some photo in order to click this button");
                return;
            }
            if(this.selectPicture1.Checked)
            {
                DiscordFormsHolder.getInstance().ProfilePictureForm.DisplayPhotoFromDefaultOptions(this.firstPictureBox.Image);
            }
            if (this.selectPicture2.Checked)
            {
                DiscordFormsHolder.getInstance().ProfilePictureForm.DisplayPhotoFromDefaultOptions(this.secondPictureBox.Image);
            }
            if (this.selectPicture3.Checked)
            {
                DiscordFormsHolder.getInstance().ProfilePictureForm.DisplayPhotoFromDefaultOptions(this.thirdPictureBox.Image);
            }
            if (this.selectPicture4.Checked)
            {
                DiscordFormsHolder.getInstance().ProfilePictureForm.DisplayPhotoFromDefaultOptions(this.fourthPictureBox.Image);
            }
            if (this.selectPicture5.Checked)
            {
                DiscordFormsHolder.getInstance().ProfilePictureForm.DisplayPhotoFromDefaultOptions(this.fifthPictureBox.Image);
            }
            if (this.selectPicture6.Checked)
            {
                DiscordFormsHolder.getInstance().ProfilePictureForm.DisplayPhotoFromDefaultOptions(this.sixPictureBox.Image);
            }
            this.Visible = false;
            DiscordFormsHolder.getInstance().ProfilePictureForm.Visible = true;
            MessageBox.Show("click where you want that the center of the circle that will represent your profile picture will be");
        }

        
    }
}
