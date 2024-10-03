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
        public LoginForm LoginForm;

        public RegistrationForm RegistrationForm;

        public ForgotPasswordForm ForgotPasswordForm;

        public ProfilePictureForm ProfilePictureForm;

        public DefaultProfilePicturesForm DefaultProfilePictureForm;

        private static DiscordFormsHolder instance = null;




        [MethodImpl(MethodImplOptions.Synchronized)]
        public static DiscordFormsHolder getInstance()
        {
            if (DiscordFormsHolder.instance == null)
            {
                DiscordFormsHolder.instance = new DiscordFormsHolder();
            }
            return DiscordFormsHolder.instance;
        }

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
