using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine(command);
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol(command);
            switch(clientServerProtocol.TypeOfCommand)
            {
                case TypeOfCommand.Error_Command:
                    ConnectionManager.getInstance(null).ProcessError(clientServerProtocol.Message);
                    break;

                case TypeOfCommand.Code_Sent_To_Email_Command:
                    ConnectionManager.getInstance(null).ProcessCodeSentToEmail(clientServerProtocol.Code);
                    break;

                case TypeOfCommand.Successes_Username_Not_In_The_System_Command:
                    ConnectionManager.getInstance(null).ProcessSuccessesUsernameNotInTheSystem();
                    break;

                case TypeOfCommand.Successes_Registration_Command:
                    ConnectionManager.getInstance(null).ProcessSuccessesLoginOrRegistration();
                    break;

                case TypeOfCommand.Login_Cooldown_Command:
                    ConnectionManager.getInstance(null).ProcessLoginCooldown(clientServerProtocol.Message, clientServerProtocol.TimeToCooldown);
                    break;


            }
        }
    }
}
