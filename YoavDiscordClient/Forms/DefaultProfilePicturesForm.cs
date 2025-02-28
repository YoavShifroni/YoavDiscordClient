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
        /// Enum to track which category is currently active
        /// </summary>
        private enum PictureCategory
        {
            Male,
            Female,
            AnimalsOthers
        }

        /// <summary>
        /// The currently active picture category
        /// </summary>
        private PictureCategory currentCategory = PictureCategory.Male;

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultProfilePicturesForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The function is called when the user loads this form and adds 
        /// profile images to each category of picture boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefaultProfilePicturesForm_Load(object sender, EventArgs e)
        {
            LoadMalePictures();
            LoadFemalePictures();
            LoadAnimalPictures();

            // Show the male panel by default
            ShowCategory(PictureCategory.Male);
        }

        /// <summary>
        /// Loads the male profile pictures from resources
        /// </summary>
        private void LoadMalePictures()
        {
            this.male1PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("defaultDiscordImage");
            // The rest would be actual male avatar pictures from resources
            // For now using placeholder images from the original form
            this.male2PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("dogPicture");
            this.male3PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("catImage");
            this.male4PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("cowImage");
            this.male5PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("horseImage");
            this.male6PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("monkeyImage");
        }

        /// <summary>
        /// Loads the female profile pictures from resources
        /// </summary>
        private void LoadFemalePictures()
        {
            // These would be actual female avatar pictures from resources
            // For now using placeholder images from the original form
            this.female1PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("defaultDiscordImage");
            this.female2PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("dogPicture");
            this.female3PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("catImage");
            this.female4PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("cowImage");
            this.female5PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("horseImage");
            this.female6PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("monkeyImage");
        }

        /// <summary>
        /// Loads the animal and other profile pictures from resources
        /// </summary>
        private void LoadAnimalPictures()
        {
            this.animal1PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("dogPicture");
            this.animal2PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("catImage");
            this.animal3PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("cowImage");
            this.animal4PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("horseImage");
            this.animal5PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("monkeyImage");
            this.animal6PictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("defaultDiscordImage");
        }

        /// <summary>
        /// Shows the selected category panel and hides others
        /// </summary>
        /// <param name="category">The category to show</param>
        private void ShowCategory(PictureCategory category)
        {
            // First, reset button colors
            maleButton.BackColor = femaleButton.BackColor = animalsOthersButton.BackColor =
                Color.FromArgb(164, 165, 169); // Default gray color

            // Hide all panels first
            malePanel.Visible = femalePanel.Visible = animalsOthersPanel.Visible = false;

            // Show selected panel and set active button color
            switch (category)
            {
                case PictureCategory.Male:
                    malePanel.Visible = true;
                    maleButton.BackColor = Color.FromArgb(116, 86, 174); // Purple highlight color
                    break;
                case PictureCategory.Female:
                    femalePanel.Visible = true;
                    femaleButton.BackColor = Color.FromArgb(116, 86, 174);
                    break;
                case PictureCategory.AnimalsOthers:
                    animalsOthersPanel.Visible = true;
                    animalsOthersButton.BackColor = Color.FromArgb(116, 86, 174);
                    break;
            }

            currentCategory = category;

            // Uncheck all checkboxes when switching categories
            UncheckAllCheckboxes();
        }

        /// <summary>
        /// Unchecks all checkboxes across all panels
        /// </summary>
        private void UncheckAllCheckboxes()
        {
            // Uncheck male checkboxes
            selectMale1.Checked = selectMale2.Checked = selectMale3.Checked =
            selectMale4.Checked = selectMale5.Checked = selectMale6.Checked = false;

            // Uncheck female checkboxes
            selectFemale1.Checked = selectFemale2.Checked = selectFemale3.Checked =
            selectFemale4.Checked = selectFemale5.Checked = selectFemale6.Checked = false;

            // Uncheck animal checkboxes
            selectAnimal1.Checked = selectAnimal2.Checked = selectAnimal3.Checked =
            selectAnimal4.Checked = selectAnimal5.Checked = selectAnimal6.Checked = false;
        }

        /// <summary>
        /// Handles the click event for the male button
        /// </summary>
        private void maleButton_Click(object sender, EventArgs e)
        {
            ShowCategory(PictureCategory.Male);
        }

        /// <summary>
        /// Handles the click event for the female button
        /// </summary>
        private void femaleButton_Click(object sender, EventArgs e)
        {
            ShowCategory(PictureCategory.Female);
        }

        /// <summary>
        /// Handles the click event for the animals & others button
        /// </summary>
        private void animalsOthersButton_Click(object sender, EventArgs e)
        {
            ShowCategory(PictureCategory.AnimalsOthers);
        }

        /// <summary>
        /// The function is called when the "go back" button is clicked and will move the user 
        /// back to the profile picture window
        /// </summary>
        private void goBackToProfilePictureFormButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DiscordFormsHolder.getInstance().ProfilePictureForm.Visible = true;
            DiscordFormsHolder.getInstance().SetActiveForm(FormNames.ProfilePicture);
        }

        /// <summary>
        /// Check if the user selected any photo across all panels
        /// </summary>
        /// <returns>True if a photo is selected, false otherwise</returns>
        private bool IsTheUserSelectedSomePhoto()
        {
            // Check male selections
            if (selectMale1.Checked || selectMale2.Checked || selectMale3.Checked ||
                selectMale4.Checked || selectMale5.Checked || selectMale6.Checked)
                return true;

            // Check female selections
            if (selectFemale1.Checked || selectFemale2.Checked || selectFemale3.Checked ||
                selectFemale4.Checked || selectFemale5.Checked || selectFemale6.Checked)
                return true;

            // Check animal selections
            if (selectAnimal1.Checked || selectAnimal2.Checked || selectAnimal3.Checked ||
                selectAnimal4.Checked || selectAnimal5.Checked || selectAnimal6.Checked)
                return true;

            return false;
        }

        /// <summary>
        /// The function is called when the "choose this photo" button is clicked. It checks if a photo
        /// is selected, then finds which photo the user selected and displays it in the ProfilePictureForm.
        /// After that, it shows the profile picture form and instructs the user to select the circle center.
        /// </summary>
        private void chooseThisPhotoButton_Click(object sender, EventArgs e)
        {
            if (!IsTheUserSelectedSomePhoto())
            {
                MessageBox.Show("You must choose a photo before clicking this button");
                return;
            }

            Image selectedImage = null;

            // Check which image is selected from the male panel
            if (malePanel.Visible)
            {
                if (selectMale1.Checked) selectedImage = male1PictureBox.Image;
                else if (selectMale2.Checked) selectedImage = male2PictureBox.Image;
                else if (selectMale3.Checked) selectedImage = male3PictureBox.Image;
                else if (selectMale4.Checked) selectedImage = male4PictureBox.Image;
                else if (selectMale5.Checked) selectedImage = male5PictureBox.Image;
                else if (selectMale6.Checked) selectedImage = male6PictureBox.Image;
            }
            // Check which image is selected from the female panel
            else if (femalePanel.Visible)
            {
                if (selectFemale1.Checked) selectedImage = female1PictureBox.Image;
                else if (selectFemale2.Checked) selectedImage = female2PictureBox.Image;
                else if (selectFemale3.Checked) selectedImage = female3PictureBox.Image;
                else if (selectFemale4.Checked) selectedImage = female4PictureBox.Image;
                else if (selectFemale5.Checked) selectedImage = female5PictureBox.Image;
                else if (selectFemale6.Checked) selectedImage = female6PictureBox.Image;
            }
            // Check which image is selected from the animals panel
            else if (animalsOthersPanel.Visible)
            {
                if (selectAnimal1.Checked) selectedImage = animal1PictureBox.Image;
                else if (selectAnimal2.Checked) selectedImage = animal2PictureBox.Image;
                else if (selectAnimal3.Checked) selectedImage = animal3PictureBox.Image;
                else if (selectAnimal4.Checked) selectedImage = animal4PictureBox.Image;
                else if (selectAnimal5.Checked) selectedImage = animal5PictureBox.Image;
                else if (selectAnimal6.Checked) selectedImage = animal6PictureBox.Image;
            }

            if (selectedImage != null)
            {
                DiscordFormsHolder.getInstance().ProfilePictureForm.DisplayPhotoFromDefaultOptions(selectedImage);
                this.Visible = false;
                DiscordFormsHolder.getInstance().ProfilePictureForm.Visible = true;
                DiscordFormsHolder.getInstance().SetActiveForm(FormNames.ProfilePicture);
                MessageBox.Show("Click where you want the center of the circle that will represent your profile picture to be");
            }
        }

        #region Male Checkbox Event Handlers

        /// <summary>
        /// Event handler for the first male picture checkbox
        /// </summary>
        private void selectMale1_CheckedChanged(object sender, EventArgs e)
        {
            if (selectMale1.Checked)
            {
                selectMale2.Checked = selectMale3.Checked = selectMale4.Checked =
                selectMale5.Checked = selectMale6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the second male picture checkbox
        /// </summary>
        private void selectMale2_CheckedChanged(object sender, EventArgs e)
        {
            if (selectMale2.Checked)
            {
                selectMale1.Checked = selectMale3.Checked = selectMale4.Checked =
                selectMale5.Checked = selectMale6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the third male picture checkbox
        /// </summary>
        private void selectMale3_CheckedChanged(object sender, EventArgs e)
        {
            if (selectMale3.Checked)
            {
                selectMale1.Checked = selectMale2.Checked = selectMale4.Checked =
                selectMale5.Checked = selectMale6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the fourth male picture checkbox
        /// </summary>
        private void selectMale4_CheckedChanged(object sender, EventArgs e)
        {
            if (selectMale4.Checked)
            {
                selectMale1.Checked = selectMale2.Checked = selectMale3.Checked =
                selectMale5.Checked = selectMale6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the fifth male picture checkbox
        /// </summary>
        private void selectMale5_CheckedChanged(object sender, EventArgs e)
        {
            if (selectMale5.Checked)
            {
                selectMale1.Checked = selectMale2.Checked = selectMale3.Checked =
                selectMale4.Checked = selectMale6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the sixth male picture checkbox
        /// </summary>
        private void selectMale6_CheckedChanged(object sender, EventArgs e)
        {
            if (selectMale6.Checked)
            {
                selectMale1.Checked = selectMale2.Checked = selectMale3.Checked =
                selectMale4.Checked = selectMale5.Checked = false;
            }
        }

        #endregion

        #region Female Checkbox Event Handlers

        /// <summary>
        /// Event handler for the first female picture checkbox
        /// </summary>
        private void selectFemale1_CheckedChanged(object sender, EventArgs e)
        {
            if (selectFemale1.Checked)
            {
                selectFemale2.Checked = selectFemale3.Checked = selectFemale4.Checked =
                selectFemale5.Checked = selectFemale6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the second female picture checkbox
        /// </summary>
        private void selectFemale2_CheckedChanged(object sender, EventArgs e)
        {
            if (selectFemale2.Checked)
            {
                selectFemale1.Checked = selectFemale3.Checked = selectFemale4.Checked =
                selectFemale5.Checked = selectFemale6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the third female picture checkbox
        /// </summary>
        private void selectFemale3_CheckedChanged(object sender, EventArgs e)
        {
            if (selectFemale3.Checked)
            {
                selectFemale1.Checked = selectFemale2.Checked = selectFemale4.Checked =
                selectFemale5.Checked = selectFemale6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the fourth female picture checkbox
        /// </summary>
        private void selectFemale4_CheckedChanged(object sender, EventArgs e)
        {
            if (selectFemale4.Checked)
            {
                selectFemale1.Checked = selectFemale2.Checked = selectFemale3.Checked =
                selectFemale5.Checked = selectFemale6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the fifth female picture checkbox
        /// </summary>
        private void selectFemale5_CheckedChanged(object sender, EventArgs e)
        {
            if (selectFemale5.Checked)
            {
                selectFemale1.Checked = selectFemale2.Checked = selectFemale3.Checked =
                selectFemale4.Checked = selectFemale6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the sixth female picture checkbox
        /// </summary>
        private void selectFemale6_CheckedChanged(object sender, EventArgs e)
        {
            if (selectFemale6.Checked)
            {
                selectFemale1.Checked = selectFemale2.Checked = selectFemale3.Checked =
                selectFemale4.Checked = selectFemale5.Checked = false;
            }
        }

        #endregion

        #region Animal Checkbox Event Handlers

        /// <summary>
        /// Event handler for the first animal picture checkbox
        /// </summary>
        private void selectAnimal1_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAnimal1.Checked)
            {
                selectAnimal2.Checked = selectAnimal3.Checked = selectAnimal4.Checked =
                selectAnimal5.Checked = selectAnimal6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the second animal picture checkbox
        /// </summary>
        private void selectAnimal2_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAnimal2.Checked)
            {
                selectAnimal1.Checked = selectAnimal3.Checked = selectAnimal4.Checked =
                selectAnimal5.Checked = selectAnimal6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the third animal picture checkbox
        /// </summary>
        private void selectAnimal3_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAnimal3.Checked)
            {
                selectAnimal1.Checked = selectAnimal2.Checked = selectAnimal4.Checked =
                selectAnimal5.Checked = selectAnimal6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the fourth animal picture checkbox
        /// </summary>
        private void selectAnimal4_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAnimal4.Checked)
            {
                selectAnimal1.Checked = selectAnimal2.Checked = selectAnimal3.Checked =
                selectAnimal5.Checked = selectAnimal6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the fifth animal picture checkbox
        /// </summary>
        private void selectAnimal5_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAnimal5.Checked)
            {
                selectAnimal1.Checked = selectAnimal2.Checked = selectAnimal3.Checked =
                selectAnimal4.Checked = selectAnimal6.Checked = false;
            }
        }

        /// <summary>
        /// Event handler for the sixth animal picture checkbox
        /// </summary>
        private void selectAnimal6_CheckedChanged(object sender, EventArgs e)
        {
            if (selectAnimal6.Checked)
            {
                selectAnimal1.Checked = selectAnimal2.Checked = selectAnimal3.Checked =
                selectAnimal4.Checked = selectAnimal5.Checked = false;
            }
        }

        #endregion
    }
}