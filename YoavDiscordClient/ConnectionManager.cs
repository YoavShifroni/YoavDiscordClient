using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoavDiscordClient.Enums;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace YoavDiscordClient
{
    public class ConnectionManager
    {
#pragma warning disable CA1416

        /// <summary>
        /// The code that sent to the user when he want to change his password
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The username of this user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The only instance of this class, used in order to send and receive messages to/ from the server
        /// </summary>
        public ConnectionWithServer ConnectionWithServer { get; set; }

        /// <summary>
        /// The instance of this class per singleton design pattern
        /// </summary>
        private static ConnectionManager instance = null;




        /// <summary>
        /// Static getInstance method, as in Singleton patterns. Protected with mutex
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        /// <param name="ipAddress"></param>
        private ConnectionManager(string ipAddress)
        {
            this.ConnectionWithServer = ConnectionWithServer.getInstance(ipAddress);
        }

        /// <summary>
        /// The function send message to the server with the login command and the username and password that the user entered
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void ProcessLogin(string username, string password)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Login_Command;
            clientServerProtocol.Username = username;
            clientServerProtocol.Password = password;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// The function send message to the server with the registration command and the username, password,
        /// first name, last name, email, city, gender and profile picture that the user entered/ choosed
        /// </summary>
        /// <param name="registrationInfo"></param>
        /// <param name="imageToByteArray"></param>
        public void ProcessRegistration(RegistrationInfo registrationInfo, byte[] imageToByteArray) 
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Registration_Command;
            clientServerProtocol.Username = registrationInfo.Username;
            clientServerProtocol.Password = registrationInfo.Password;
            clientServerProtocol.FirstName = registrationInfo.FirstName;
            clientServerProtocol.LastName = registrationInfo.LastName;
            clientServerProtocol.Email = registrationInfo.Email;
            clientServerProtocol.City = registrationInfo.City;
            clientServerProtocol.Gender = registrationInfo.Gender;
            clientServerProtocol.ProfilePicture = imageToByteArray;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));

        }

        /// <summary>
        /// The function send message to the server with the forgot password command and with the username that the user entered and the code 
        /// that will be sent to his mail
        /// </summary>
        /// <param name="username"></param>
        public void ProcessForgotPassword(string username)
        {
            this.Code = this.GetRandomCode();
            this.Username = username;
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Forgot_Password_Command;
            clientServerProtocol.Username = username;
            clientServerProtocol.Code = this.Code;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// The function create a random code with a length of 6 characters and return it
        /// </summary>
        /// <returns></returns>
        public string GetRandomCode()
        {
            var charsALL = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz#?!@$%^&*-";
            var randomIns = new Random();
            var rndChars = Enumerable.Range(0, 6)
                            .Select(_ => charsALL[randomIns.Next(charsALL.Length)])
                            .ToArray();
            return new string(rndChars);
        }

        /// <summary>
        /// The function send message to the server with the update password command and the username and the new password that the user entered
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        public void ProcessUpdatePassword(string username, string newPassword)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Update_Password_Command;
            clientServerProtocol.Username = username;
            clientServerProtocol.Password = newPassword;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// The function show to the user an error message when an error command was received from the server
        /// </summary>
        /// <param name="message"></param>
        public void ProcessError(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// The function show the user a message that tell him that code was sent to his mail and will call other function that will show him
        /// where he need to enter the code
        /// </summary>
        /// <param name="code"></param>
        public void ProcessCodeSentToEmail(string code)
        {
            MessageBox.Show("code sent to your email, check what is the code and enter it in the right place");
            DiscordFormsHolder.getInstance().LoginForm.Invoke(new Action(() => DiscordFormsHolder.getInstance().LoginForm.ShowNext(code)));
        }

        /// <summary>
        /// The function show the user a message that tell him that he login/ register successesfuly and .....
        /// </summary>
        public void ProcessSuccessConnctedToTheApplication(byte[] profilePicture, string username, int userId, int role)
        {
            MessageBox.Show("Login / Registration successesfuly!!");
            DiscordFormsHolder.getInstance().GetActiveForm().Invoke(new Action(() => DiscordFormsHolder.getInstance().MoveToTheDiscordAppWindow(profilePicture,
                username, userId, role)));
        }

        /// <summary>
        /// The function send message to the server with the check if the username already exist command and the username that the user entered
        /// </summary>
        /// <param name="username"></param>
        public void ProcessCheckIfUsernameAlreadyExist(string username)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Check_If_Username_Already_Exist_Command;
            clientServerProtocol.Username = username;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// The function show the user a message that tells him that all the details that he entered are ok and will call another function
        /// that will display him the captcha part
        /// </summary>
        public void ProcessSuccessesUsernameNotInTheSystem()
        {
            MessageBox.Show("All details are correct, now lets check that you are not a bot");
            DiscordFormsHolder.getInstance().RegistrationForm.Invoke(new Action(() => DiscordFormsHolder.getInstance().RegistrationForm.NextStage()));
        }

        /// <summary>
        /// The function show the user a message that tells him that because he got his username or password wrong lots of times he will need to wait some time
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeToCooldown"></param>
        public void ProcessLoginCooldown(string message, int timeToCooldown)
        {
            MessageBox.Show(message);
            DiscordFormsHolder.getInstance().LoginForm.Invoke(new Action(() => DiscordFormsHolder.getInstance().LoginForm.ShowCooldownTimer(timeToCooldown)));
        }

        public void ProcessSuccessesForgotPassword()
        {
            DiscordFormsHolder.getInstance().LoginForm.Invoke(new Action(() => DiscordFormsHolder.getInstance().LoginForm.ForgotPasswordNextStage()));
        }

        public void ProcessGetProfilePictureAndUsername()
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Get_Username_And_Profile_Picture_Command;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessSendMessage(string text, int chatRoomId)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Send_Message_Command;
            clientServerProtocol.MessageThatTheUserSent = text;
            clientServerProtocol.ChatRoomId = chatRoomId;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessMessageFromOtherUserCommand(int userId, string username, string messageThatTheUserSent, DateTime timeThatTheMessageWasSent,
            int chatRoomId)
        {
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() => DiscordFormsHolder.getInstance().DiscordApp.AddMessageToChatFromOtherUser(
                username, userId, messageThatTheUserSent, timeThatTheMessageWasSent, chatRoomId)));
        }

        public void ProcessFetchImageOfUser(int userId)
        {
            ClientServerProtocol clientServerProtocol= new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Fetch_Image_Of_User_Command;
            clientServerProtocol.UserId = userId;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessReturnImageOfUser(int userId, byte[] profilePicture)
        {
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() => DiscordFormsHolder.getInstance().DiscordApp.UpdateUserImage(
                userId, profilePicture)));
        }

        public void ProcessGetMessagesHistoryOfChatRoom(int chatRoomId)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Get_Messages_History_Of_Chat_Room_Command;
            clientServerProtocol.ChatRoomId= chatRoomId;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessReturnMessagesHistoryOfChatRoom(List<UserMessage> messagesOfAChatRoom)
        {
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() => DiscordFormsHolder.getInstance().DiscordApp.SetMessagesHistoryOfAChatRoom(messagesOfAChatRoom)));
        }

        public void ProcessConnectToMediaRoom(int mediaRoomId)
        {
            MediaRoom mediaRoom = new MediaRoom(mediaRoomId);
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Connect_To_Media_Room_Command;
            clientServerProtocol.MediaRoomId = mediaRoomId;
            clientServerProtocol.MediaPort = mediaRoom.GetPort();

            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessDisconnectFromMediaRoom(int mediaRoomId)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Disconnect_From_Media_Room_Command;
            clientServerProtocol.MediaRoomId = mediaRoomId;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public async void ProcessNewParticipantJoinTheMediaRoom(string newParticipantIp, int newParticipantPort, int userId, string username)
        {
            await DiscordApp.VideoStreamConnection.ConnectToParticipant(newParticipantIp, newParticipantPort, this.ImageToByteArray(DiscordFormsHolder.getInstance().DiscordApp.UsersImages[userId]), username, userId);
        }

        public void ProcessGetAllIpsOfConnectedUsersInSomeMediaRoom(List<UserMediaConnectionDetails> UsersMediaConnectionDetails)
        {

            foreach (UserMediaConnectionDetails userMediaConnectionDetail in UsersMediaConnectionDetails)
            {
                this.ProcessNewParticipantJoinTheMediaRoom(userMediaConnectionDetail.Ip, userMediaConnectionDetail.Port,
                    userMediaConnectionDetail.UserId, userMediaConnectionDetail.Username);

                // Apply status effects if they exist
                bool isMuted = userMediaConnectionDetail.IsAudioMuted;
                bool isDeafened = userMediaConnectionDetail.IsDeafened;
                bool isVideoMuted = userMediaConnectionDetail.IsVideoMuted;

                int userId = userMediaConnectionDetail.UserId;

                if (isMuted)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.HandleUserMuteStatusChanged(userId, true)));
                }

                if (isDeafened)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.HandleUserDeafenStatusChanged(userId, true)));
                }

                if (isVideoMuted)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.HandleUserVideoMuteStatusChanged(userId, true)));
                }
            }
        }

        public void ProcessSomeUserLeftTheMediaRoomCommand(string userIp)
        {
            DiscordApp.VideoStreamConnection.DisconnectFromParticipant(userIp);
        }

        public void ProcessFetchAllUsers()
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Fetch_All_Users_Command;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessGetAllUsersDetails(List<UserDetails> allUsersDetails)
        {
            if (DiscordFormsHolder.getInstance().DiscordApp.IsHandleCreated)
            {
                DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() => DiscordFormsHolder.getInstance().DiscordApp.ShowAllUsersDetails(allUsersDetails)));
            }
            
        }

        public void ProcessUserJoinMediaRoom(int userId, int mediaRoomId, string username, byte[] profilePicture, int role, bool isMuted,
            bool isDeafened, bool isVideoMuted)
        {
            if (DiscordFormsHolder.getInstance().DiscordApp.IsHandleCreated)
            {
                // First, add the user to the channel
                DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() => DiscordFormsHolder.getInstance().DiscordApp.AddUserToMediaChannel
                    (mediaRoomId, new UserDetails(userId, username, profilePicture, role))));

                // Then, apply any status effects that were active
                if (isMuted)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.HandleUserMuteStatusChanged(userId, true)));
                }

                if (isDeafened)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.HandleUserDeafenStatusChanged(userId, true)));
                }

                if (isVideoMuted)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.HandleUserVideoMuteStatusChanged(userId, true)));
                }
            }
            
        }

        public void ProcessUserLeaveMediaRoom(int userId, int mediaRoomId)
        {
            if (DiscordFormsHolder.getInstance().DiscordApp.IsHandleCreated)
            {
                DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() => DiscordFormsHolder.getInstance().DiscordApp.RemoveUserFromMediaChannel
                    (mediaRoomId, userId)));
            }
            
        }

       

        private byte[] ImageToByteArray(Image image)
        {
            try
            {
                // Create a copy of the image to avoid disposal issues
                using (Bitmap bmp = new Bitmap(image))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting image: {ex.Message}");
                // Return a default/placeholder image bytes if conversion fails
                using (Bitmap defaultBmp = new Bitmap(40, 40))
                {
                    using (Graphics g = Graphics.FromImage(defaultBmp))
                    {
                        g.Clear(Color.Gray); // Create a gray placeholder
                        using (MemoryStream ms = new MemoryStream())
                        {
                            defaultBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            return ms.ToArray();
                        }
                    }
                }
            }
        }

        public void ProcessSetUserMuted(int userId, bool isMuted)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Set_Mute_User_Command;
            clientServerProtocol.UserId = userId;
            clientServerProtocol.IsMuted = isMuted;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessSetUserDeafened(int userId, bool isDeafened)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Set_Deafen_User_Command;
            clientServerProtocol.UserId = userId;
            clientServerProtocol.IsDeafened = isDeafened;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessDisconnectUserFromMediaRoom(int userId, int mediaRoomId)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Disconnect_User_From_Media_Room_Command;
            clientServerProtocol.UserId = userId;
            clientServerProtocol.MediaRoomId = mediaRoomId;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessUserMuted(int userId, bool isMuted)
        {
            // Forward the mute status change to the Discord app
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                DiscordFormsHolder.getInstance().DiscordApp.HandleUserMuteStatusChanged(userId, isMuted)));
        }

        public void ProcessUserDeafened(int userId, bool isDeafened)
        {
            // Forward the deafen status change to the Discord app
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                DiscordFormsHolder.getInstance().DiscordApp.HandleUserDeafenStatusChanged(userId, isDeafened)));
        }

        public void ProcessUserDisconnected(int userId, int mediaRoomId)
        {
            // Forward the disconnect command to the Discord app
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                DiscordFormsHolder.getInstance().DiscordApp.HandleUserDisconnect(userId, mediaRoomId)));
        }

        public void ProcessSetUserVideoMuted(int userId, bool isVideoMuted)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Set_Video_Mute_User_Command;
            clientServerProtocol.UserId = userId;
            clientServerProtocol.IsVideoMuted = isVideoMuted;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessUserVideoMuted(int userId, bool isVideoMuted)
        {
            // Forward the video mute status change to the Discord app
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                DiscordFormsHolder.getInstance().DiscordApp.HandleUserVideoMuteStatusChanged(userId, isVideoMuted)));
        }

        public void ProcessUpdateUserRole(int userId, int newRole)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Update_User_Role_Command;
            clientServerProtocol.UserId = userId;
            clientServerProtocol.Role = newRole;
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        public void ProcessUserRoleHasBeenUpdated(int userId, int newRole)
        {
            // Update the user's role in the UI
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                DiscordFormsHolder.getInstance().DiscordApp.HandleUserRoleUpdated(userId, newRole)));

        }



        


    }
}
