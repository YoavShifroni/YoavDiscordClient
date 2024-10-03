using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    public class HandleCommandFromServer
    {

        public HandleCommandFromServer() 
        {

        }


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

                case TypeOfCommand.Enter_Yoav_Discord_Command:
                    ConnectionManager.getInstance(null).ProcessSuccessesLogin();
                    break;

                case TypeOfCommand.Successes_Registration_Command:
                    ConnectionManager.getInstance(null).ProcessSuccessesRegistration();
                    break;


            }
        }
    }
}
