using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public class DiscordFormsHolder
    {
        /// <summary>
        /// Login form instance
        /// </summary>
        public LoginForm LoginForm;

        /// <summary>
        /// Registration form instance
        /// </summary>
        public RegistrationForm RegistrationForm;

        /// <summary>
        /// Forgot password form instance
        /// </summary>
        public ForgotPasswordForm ForgotPasswordForm;

        /// <summary>
        /// Profile picture form instance
        /// </summary>
        public ProfilePictureForm ProfilePictureForm;

        /// <summary>
        /// Default profile pictures form instance
        /// </summary>
        public DefaultProfilePicturesForm DefaultProfilePictureForm;


        public DiscordApp DiscordApp;

        /// <summary>
        /// Form instance that represent the current active form
        /// </summary>
        private Form _activeForm;

        /// <summary>
        /// The instance of this class per singleton design pattern
        /// </summary>
        private static DiscordFormsHolder instance = null;


        public const float optimizedScreenWidth = 2560f;

        public const float optimizedScreenHeight = 1440f;



        /// <summary>
        /// Static getInstance method, as in Singleton patterns.
        /// Protected with mutex
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static DiscordFormsHolder getInstance()
        {
            if (DiscordFormsHolder.instance == null)
            {
                DiscordFormsHolder.instance = new DiscordFormsHolder();
            }
            return DiscordFormsHolder.instance;
        }

        /// <summary>
        /// Private constructor, create instance for each form
        /// </summary>
        private DiscordFormsHolder()
        {
            this.LoginForm = new LoginForm();
            this.RegistrationForm = new RegistrationForm();
            this.ForgotPasswordForm = new ForgotPasswordForm();
            this.ProfilePictureForm = new ProfilePictureForm();
            this.DefaultProfilePictureForm = new DefaultProfilePicturesForm();
            this.DiscordApp = new DiscordApp();
        }

        /// <summary>
        /// The function return the active form
        /// </summary>
        /// <returns></returns>
        public Form GetActiveForm()
        {
            return this._activeForm;
        }


        /// <summary>
        /// The function set the active form according to the enum value
        /// </summary>
        /// <param name="formName"></param>
        public void SetActiveForm(FormNames formName)
        {
            switch (formName)
            {
                case FormNames.Login:
                    this._activeForm = LoginForm;
                    break;

                case FormNames.Registration:
                    this._activeForm = RegistrationForm;
                    break;

                case FormNames.ForgotPassword:
                    this._activeForm = ForgotPasswordForm;
                    break;

                case FormNames.ProfilePicture:
                    this._activeForm = ProfilePictureForm;
                    break;

                case FormNames.DefaultProfilePicture:
                    this._activeForm = DefaultProfilePictureForm;
                    break;
            }
        }

        /// <summary>
        /// The function change the active form currsor and enabled status according to the bool
        /// </summary>
        /// <param name="active"></param>
        public void ChangeCursorSignAndActiveFormStatus(bool active)
        {
            this._activeForm.Cursor = active ? Cursors.Arrow : Cursors.WaitCursor;
            this._activeForm.Enabled = active;
        }

        public void MoveToTheDiscordAppWindow(byte[] profilePicture, string username, int userId)
        {
            this.DiscordApp.SetUsernameAndProfilePicture(profilePicture, username, userId);
            this._activeForm.Visible = false;
            this.DiscordApp.Visible = true;
            this.SetActiveForm(FormNames.DiscordApp);
        }

        public static void ResizeFormBasedOnResolution(Form form, float optimizedFormWidth, float optimizedFormHeight)
        {
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;


            // Check if the form's size exceeds the screen's resolution
            if (DiscordFormsHolder.optimizedScreenWidth > screenWidth || DiscordFormsHolder.optimizedScreenHeight > screenHeight)
            {


                // Preserve aspect ratio while resizing

                // Set new dimensions based on screen resolution
                int maxWidth = (int)(screenWidth * (optimizedFormWidth / DiscordFormsHolder.optimizedScreenWidth));
                int maxHeight = (int)(screenHeight * (optimizedFormHeight / DiscordFormsHolder.optimizedScreenHeight));

                double ratioWidth = (double)screenWidth / DiscordFormsHolder.optimizedScreenWidth;
                double ratioHeight = (double)screenHeight / DiscordFormsHolder.optimizedScreenHeight;

                // Apply new size to the form
                form.Width = maxWidth;
                form.Height = maxHeight;
                ScaleControls(form.Controls, ratioWidth, ratioHeight);

            }
        }

        // Helper method to scale controls
        private static void ScaleControls(Control.ControlCollection controls, double ratioWidth, double ratioHeight)
        {

            // Scale all controls on the form
            foreach (Control control in controls)
            {
                // Scale dimensions
                control.Width = (int)(control.Width * ratioWidth);
                control.Height = (int)(control.Height * ratioHeight);

                // Scale position
                control.Left = (int)(control.Left * ratioWidth);
                control.Top = (int)(control.Top * ratioHeight);

                // Scale font size
                float newFontSize = control.Font.Size * (float)Math.Min(ratioWidth, ratioHeight);
                control.Font = new Font(control.Font.FontFamily, newFontSize);

                // Recursively scale child controls
                if (control.HasChildren)
                {
                    ScaleControls(control.Controls, ratioWidth, ratioHeight);
                }
            }
        }


    }
}