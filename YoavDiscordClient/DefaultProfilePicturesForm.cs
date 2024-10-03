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
        public DefaultProfilePicturesForm()
        {
            InitializeComponent();
        }

        private void DefaultProfilePicturesForm_Load(object sender, EventArgs e)
        {
            // enter the pictures to there picture boxes
        }

        private void goBackToProfilePictureFormButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DiscordFormsHolder.getInstance().ProfilePictureForm.Visible = true;
        }

        private bool IsTheUserSelectedSomePhoto()
        {
            if(this.selectPicture1.Checked || this.selectPicture2.Checked || this.selectPicture3.Checked || this.selectPicture4.Checked ||
                this.selectPicture5.Checked || this.selectPicture6.Checked)
            {
                return true;
            }
            return false;
        }

        private void selectPicture1_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture2.Checked = false; 
            this.selectPicture3.Checked = false;
            this.selectPicture4.Checked = false;
            this.selectPicture5.Checked = false;
            this.selectPicture6.Checked = false;
        }

        private void selectPicture2_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture1.Checked = false;
            this.selectPicture3.Checked = false;
            this.selectPicture4.Checked = false;
            this.selectPicture5.Checked = false;
            this.selectPicture6.Checked = false;
        }

        private void selectPicture3_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture1.Checked = false;
            this.selectPicture2.Checked = false;
            this.selectPicture4.Checked = false;
            this.selectPicture5.Checked = false;
            this.selectPicture6.Checked = false;
        }

        private void selectPicture4_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture1.Checked = false;
            this.selectPicture2.Checked = false;
            this.selectPicture3.Checked = false;
            this.selectPicture5.Checked = false;
            this.selectPicture6.Checked = false;
        }

        private void selectPicture5_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture1.Checked = false;
            this.selectPicture2.Checked = false;
            this.selectPicture3.Checked = false;
            this.selectPicture4.Checked = false;
            this.selectPicture6.Checked = false;
        }

        private void selectPicture6_CheckedChanged(object sender, EventArgs e)
        {
            this.selectPicture1.Checked = false;
            this.selectPicture2.Checked = false;
            this.selectPicture3.Checked = false;
            this.selectPicture4.Checked = false;
            this.selectPicture5.Checked = false;
        }

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
