using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace YoavDiscordClient
{
    public class ConnectionManager
    {

        public string Code { get; set; }

        public string Username { get; set; }

        public ConnectionWithServer ConnectionWithServer { get; set; }

        private static ConnectionManager instance = null;




        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ConnectionManager getInstance(string ipAddress)
        {
            if (ConnectionManager.instance != null)
            {
                return ConnectionManager.instance;
            }
            if (ipAddress == null)
            {
                throw new ArgumentNullException("ip address cannot be null");
            }
            ConnectionManager.instance = new ConnectionManager(ipAddress);
            return ConnectionManager.instance;
        }


        private ConnectionManager(string ipAddress)
        {
            this.ConnectionWithServer = ConnectionWithServer.getInstance(ipAddress);
        }

        public void ProcessLogin(string username, string password)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Login_Command;
            clientServerProtocol.Username = username;
            clientServerProtocol.Password = password;
            this.ConnectionWithServer.SendMessage(clientServerProtocol.Generate());
        }


        public void ProcessRegistration(string username, string password, string firstName, string lastName, string email, string city, string gender) 
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Registration_Command;
            clientServerProtocol.Username = username;
            clientServerProtocol.Password = password;
            clientServerProtocol.FirstName = firstName;
            clientServerProtocol.LastName = lastName;
            clientServerProtocol.Email = email;
            clientServerProtocol.City = city;
            clientServerProtocol.Gender = gender;
            this.ConnectionWithServer.SendMessage(clientServerProtocol.Generate());

        }

        public void ProcessForgotPassword(string username)
        {
            this.Code = this.GetRandomCode();
            this.Username = username;
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Forgot_Password_Command;
            clientServerProtocol.Username = username;
            clientServerProtocol.Code = this.Code;
            this.ConnectionWithServer.SendMessage(clientServerProtocol.Generate());
        }

        public string GetRandomCode()
        {
            var charsALL = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz#?!@$%^&*-";
            var randomIns = new Random();
            var rndChars = Enumerable.Range(0, 6)
                            .Select(_ => charsALL[randomIns.Next(charsALL.Length)])
                            .ToArray();
            return new string(rndChars);
        }

        public void ProcessUpdatePassword(string username, string newPassword)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Update_Password_Command;
            clientServerProtocol.Username = username;
            clientServerProtocol.Password = newPassword;
            this.ConnectionWithServer.SendMessage(clientServerProtocol.Generate());
        }

        public void ProcessError(string message)
        {
            MessageBox.Show(message);
        }

        public void ProcessCodeSentToEmail(string code)
        {
            MessageBox.Show("code sent to your email, check what is the code and enter it in the right place");
            DiscordFormsHolder.getInstance().LoginForm.Invoke(new Action(() => DiscordFormsHolder.getInstance().LoginForm.ShowNext(code)));
        }

        public void ProcessSuccessesLogin()
        {
            MessageBox.Show("Login successesfuly!!"); // for now...
        }

        public void ProcessSuccessesRegistration()
        {
            MessageBox.Show("Registration successesfuly, now chose your profile picture");
            DiscordFormsHolder.getInstance().RegistrationForm.Invoke(new Action(() => DiscordFormsHolder.getInstance().RegistrationForm.MoveToProfilePictureForm()));
        }

        public void ProcessProfilePictureSelected(byte[] imageToByteArray)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Profile_Picture_Selected_Command;
            clientServerProtocol.ProfilePicture = imageToByteArray;
            this.ConnectionWithServer.SendMessage(clientServerProtocol.Generate());
        }
    }
}
