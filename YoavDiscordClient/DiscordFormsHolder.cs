using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// The instance of this class per singleton design pattern
        /// </summary>
        private static DiscordFormsHolder instance = null;



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
        }
    }
}
