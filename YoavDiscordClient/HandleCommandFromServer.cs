using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public class HandleCommandFromServer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HandleCommandFromServer() 
        {

        }

        /// <summary>
        /// The function get the message that the server sent to the client and will call the right function in order to handle this message
        /// </summary>
        /// <param name="command"></param>
        public void HandleCommand(string command)
        {
            DiscordFormsHolder.getInstance().GetActiveForm().Invoke(new Action(() => DiscordFormsHolder.getInstance().ChangeCursorSignAndActiveFormStatus(true)));
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol(command);
            switch(clientServerProtocol.TypeOfCommand)
            {
                case TypeOfCommand.Error_Command:
                    ConnectionManager.getInstance(null).ProcessError(clientServerProtocol.ErrorMessage);
                    break;

                case TypeOfCommand.Code_Sent_To_Email_Command:
                    ConnectionManager.getInstance(null).ProcessCodeSentToEmail(clientServerProtocol.Code);
                    break;

                case TypeOfCommand.Successes_Username_Not_In_The_System_Command:
                    ConnectionManager.getInstance(null).ProcessSuccessesUsernameNotInTheSystem();
                    break;

                case TypeOfCommand.Successes_Connected_To_The_Application_Command:
                    ConnectionManager.getInstance(null).ProcessSuccessConnctedToTheApplication(clientServerProtocol.ProfilePicture, 
                        clientServerProtocol.Username);
                    break;

                case TypeOfCommand.Successes_Forgot_Password_Command:
                    ConnectionManager.getInstance(null).ProcessSuccessesForgotPassword();
                    break;

                case TypeOfCommand.Login_Cooldown_Command:
                    ConnectionManager.getInstance(null).ProcessLoginCooldown(clientServerProtocol.ErrorMessage, clientServerProtocol.TimeToCooldown);
                    break;

                case TypeOfCommand.Message_From_Other_User_Command:
                    ConnectionManager.getInstance(null).ProcessMessageFromOtherUserCommand(clientServerProtocol.UserId, clientServerProtocol.Username,
                        clientServerProtocol.MessageThatTheUserSent, clientServerProtocol.TimeThatTheMessageWasSent);
                    break;

                case TypeOfCommand.Return_Image_Of_User_Command:
                    ConnectionManager.getInstance(null).ProcessReturnImageOfUser(clientServerProtocol.UserId, clientServerProtocol.ProfilePicture,
                        clientServerProtocol.Username, clientServerProtocol.MessageThatTheUserSent, clientServerProtocol.TimeThatTheMessageWasSent);
                    break;


            }
        }
    }
}
