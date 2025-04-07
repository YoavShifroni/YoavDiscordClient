using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoavDiscordClient.Enums;

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
            ClientServerProtocol clientServerProtocol = ClientServerProtocolParser.Parse(command);
            System.Diagnostics.Debug.WriteLine("Received from server: " + clientServerProtocol.ToString());
            switch (clientServerProtocol.TypeOfCommand)
            {
                case TypeOfCommand.Error_Command:
                    ConnectionManager.GetInstance(null).HandleError(clientServerProtocol.ErrorMessage);
                    break;

                case TypeOfCommand.Code_Sent_To_Email_Command:
                    ConnectionManager.GetInstance(null).HandleCodeSentToEmail(clientServerProtocol.Code);
                    break;

                case TypeOfCommand.Success_Username_Not_In_The_System_Command:
                    ConnectionManager.GetInstance(null).HandleSuccessesUsernameNotInTheSystem();
                    break;

                case TypeOfCommand.Success_Connected_To_The_Application_Command:
                    ConnectionManager.GetInstance(null).HandleSuccessConnctedToTheApplication(clientServerProtocol.ProfilePicture, 
                        clientServerProtocol.Username, clientServerProtocol.UserId, clientServerProtocol.Role);
                    break;

                case TypeOfCommand.Success_Forgot_Password_Command:
                    ConnectionManager.GetInstance(null).HandleSuccessesForgotPassword();
                    break;

                case TypeOfCommand.Login_Cooldown_Command:
                    ConnectionManager.GetInstance(null).HandleLoginCooldown(clientServerProtocol.ErrorMessage, clientServerProtocol.TimeToCooldown);
                    break;

                case TypeOfCommand.Message_From_Other_User_Command:
                    ConnectionManager.GetInstance(null).HandleMessageFromOtherUserCommand(clientServerProtocol.UserId, clientServerProtocol.Username,
                        clientServerProtocol.MessageThatTheUserSent, clientServerProtocol.TimeThatTheMessageWasSent, clientServerProtocol.ChatRoomId);
                    break;

                case TypeOfCommand.Return_Image_Of_User_Command:
                    ConnectionManager.GetInstance(null).HandleReturnImageOfUser(clientServerProtocol.UserId, clientServerProtocol.ProfilePicture);
                    break;

                case TypeOfCommand.Return_Messages_History_Of_Chat_Room_Command:
                    ConnectionManager.GetInstance(null).HandleReturnMessagesHistoryOfChatRoom(clientServerProtocol.MessagesOfAChatRoom);
                    break;

                case TypeOfCommand.New_Participant_Join_The_Media_Room_Command:
                    ConnectionManager.GetInstance(null).HandleNewParticipantJoinTheMediaRoom(clientServerProtocol.NewParticipantIp, clientServerProtocol.MediaPort, clientServerProtocol.UserId, clientServerProtocol.Username);
                    break;

                case TypeOfCommand.Get_All_Ips_Of_Connected_Users_In_Some_Media_Room_Command:
                    ConnectionManager.GetInstance(null).HandleGetAllIpsOfConnectedUsersInSomeMediaRoom(
                        clientServerProtocol.UsersMediaConnectionDetails);
                    break;

                case TypeOfCommand.Some_User_Left_The_Media_Room_Command:
                    ConnectionManager.GetInstance(null).HandleSomeUserLeftTheMediaRoomCommand(clientServerProtocol.UserIp);
                    break;

                case TypeOfCommand.Get_All_Users_Details_Command:
                    ConnectionManager.GetInstance(null).HandleGetAllUsersDetails(clientServerProtocol.AllUsersDetails);
                    break;

                case TypeOfCommand.User_Join_Media_Channel_Command:
                    ConnectionManager.GetInstance(null).HandleUserJoinMediaRoom(clientServerProtocol.UserId, clientServerProtocol.MediaRoomId,
                        clientServerProtocol.Username, clientServerProtocol.ProfilePicture, clientServerProtocol.Role,
                        clientServerProtocol.IsMuted, clientServerProtocol.IsDeafened, clientServerProtocol.IsVideoMuted);
                    break;

                case TypeOfCommand.User_Leave_Media_Channel_Command:
                    ConnectionManager.GetInstance(null).HandleUserLeaveMediaRoom(clientServerProtocol.UserId, clientServerProtocol.MediaRoomId);
                    break;

                case TypeOfCommand.User_Muted_Command:
                    ConnectionManager.GetInstance(null).HandleUserMuted(clientServerProtocol.UserId, clientServerProtocol.IsMuted);
                    break;

                case TypeOfCommand.User_Deafened_Command:
                    ConnectionManager.GetInstance(null).HandleUserDeafened(clientServerProtocol.UserId, clientServerProtocol.IsDeafened);
                    break;

                case TypeOfCommand.User_Disconnected_Command:
                    ConnectionManager.GetInstance(null).HandleUserDisconnected(clientServerProtocol.UserId, clientServerProtocol.MediaRoomId);
                    break;

                case TypeOfCommand.User_Video_Muted_Command:
                    ConnectionManager.GetInstance(null).HandleUserVideoMuted(clientServerProtocol.UserId, clientServerProtocol.IsVideoMuted);
                    break;

                case TypeOfCommand.User_Role_Has_Been_Updated_Command:
                    ConnectionManager.GetInstance(null).HandleUserRoleHasBeenUpdated(clientServerProtocol.UserId, clientServerProtocol.Role);
                    break;

                



            }
        }
    }
}
