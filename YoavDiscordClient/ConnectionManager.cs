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
using YoavDiscordClient.Managers;
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
        public static ConnectionManager GetInstance(string ipAddress)
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
            this.ConnectionWithServer = ConnectionWithServer.GetInstance(ipAddress);
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
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
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
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
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
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
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
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// The function show to the user an error message when an error command was received from the server
        /// </summary>
        /// <param name="message"></param>
        public void HandleError(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// The function show the user a message that tell him that code was sent to his mail and will call other function that will show him
        /// where he need to enter the code
        /// </summary>
        /// <param name="code"></param>
        public void HandleCodeSentToEmail(string code)
        {
            MessageBox.Show("code sent to your email, check what is the code and enter it in the right place");
            DiscordFormsHolder.getInstance().LoginForm.Invoke(new Action(() => DiscordFormsHolder.getInstance().LoginForm.ShowNext(code)));
        }

        /// <summary>
        /// The function show the user a message that tell him that he login/ register successesfuly and .....
        /// </summary>
        public void HandleSuccessConnctedToTheApplication(byte[] profilePicture, string username, int userId, int role)
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
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// The function show the user a message that tells him that all the details that he entered are ok and will call another function
        /// that will display him the captcha part
        /// </summary>
        public void HandleSuccessesUsernameNotInTheSystem()
        {
            MessageBox.Show("All details are correct, now lets check that you are not a bot");
            DiscordFormsHolder.getInstance().RegistrationForm.Invoke(new Action(() => DiscordFormsHolder.getInstance().RegistrationForm.NextStage()));
        }

        /// <summary>
        /// The function show the user a message that tells him that because he got his username or password wrong lots of times he will need to wait some time
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeToCooldown"></param>
        public void HandleLoginCooldown(string message, int timeToCooldown)
        {
            MessageBox.Show(message);
            DiscordFormsHolder.getInstance().LoginForm.Invoke(new Action(() => DiscordFormsHolder.getInstance().LoginForm.ShowCooldownTimer(timeToCooldown)));
        }

        /// <summary>
        /// Handles the successful completion of the forgot password operation.
        /// Updates the Login form to proceed to the next stage of the password recovery process.
        /// </summary>
        public void HandleSuccessesForgotPassword()
        {
            DiscordFormsHolder.getInstance().LoginForm.Invoke(new Action(() => DiscordFormsHolder.getInstance().LoginForm.ForgotPasswordNextStage()));
        }

        /// <summary>
        /// Requests the profile picture and username of the current user from the server.
        /// Sends a Get_Username_And_Profile_Picture_Command to retrieve the user's profile information.
        /// </summary>
        public void ProcessGetProfilePictureAndUsername()
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Get_Username_And_Profile_Picture_Command;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Sends a chat message to a specific chat room.
        /// </summary>
        /// <param name="text">The message text to send.</param>
        /// <param name="chatRoomId">The ID of the chat room to send the message to.</param>
        /// <remarks>
        /// Creates a Send_Message_Command and forwards it to the server through the established connection.
        /// </remarks>
        public void ProcessSendMessage(string text, int chatRoomId)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Send_Message_Command;
            clientServerProtocol.MessageThatTheUserSent = text;
            clientServerProtocol.ChatRoomId = chatRoomId;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Handles a message received from another user in a chat room.
        /// Updates the chat UI to display the received message.
        /// </summary>
        /// <param name="userId">The ID of the user who sent the message.</param>
        /// <param name="username">The username of the user who sent the message.</param>
        /// <param name="messageThatTheUserSent">The content of the message.</param>
        /// <param name="timeThatTheMessageWasSent">The timestamp when the message was sent.</param>
        /// <param name="chatRoomId">The ID of the chat room where the message was sent.</param>
        public void HandleMessageFromOtherUserCommand(int userId, string username, string messageThatTheUserSent, DateTime timeThatTheMessageWasSent,
            int chatRoomId)
        {
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(
                new Action(() => DiscordFormsHolder.getInstance().DiscordApp.GetChatManager().AddMessageToChatFromOtherUser(username, userId,
                messageThatTheUserSent, timeThatTheMessageWasSent, chatRoomId)));
        }

        /// <summary>
        /// Requests the profile picture of a specific user from the server.
        /// </summary>
        /// <param name="userId">The ID of the user whose profile picture is being requested.</param>
        /// <remarks>
        /// This method is typically called when the application needs to display a user's
        /// profile picture but does not have it cached locally.
        /// </remarks>
        public void ProcessFetchImageOfUser(int userId)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Fetch_Image_Of_User_Command;
            clientServerProtocol.UserId = userId;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Handles the server's response to a profile picture request.
        /// Updates the UserManager with the received profile picture.
        /// </summary>
        /// <param name="userId">The ID of the user whose profile picture was received.</param>
        /// <param name="profilePicture">The profile picture data as a byte array.</param>
        public void HandleReturnImageOfUser(int userId, byte[] profilePicture)
        {
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(
                new Action(() => DiscordFormsHolder.getInstance().DiscordApp.GetUserManager().UpdateUserImage(userId, profilePicture)));
        }

        /// <summary>
        /// Requests the message history for a specific chat room from the server.
        /// </summary>
        /// <param name="chatRoomId">The ID of the chat room whose message history is being requested.</param>
        /// <remarks>
        /// This is typically called when a user navigates to a chat room to display
        /// previous messages that were sent while the user was not actively viewing the chat.
        /// </remarks>
        public void ProcessGetMessagesHistoryOfChatRoom(int chatRoomId)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Get_Messages_History_Of_Chat_Room_Command;
            clientServerProtocol.ChatRoomId = chatRoomId;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Handles the server's response containing the message history of a chat room.
        /// Updates the ChatManager with the received message history.
        /// </summary>
        /// <param name="messagesOfAChatRoom">A list of user messages representing the chat history.</param>
        public void HandleReturnMessagesHistoryOfChatRoom(List<UserMessage> messagesOfAChatRoom)
        {
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(
                new Action(() => DiscordFormsHolder.getInstance().DiscordApp.GetChatManager().SetMessagesHistoryOfAChatRoom(messagesOfAChatRoom)));
        }

        /// <summary>
        /// Initiates connection to a media (voice/video) room.
        /// Creates a local media room, then notifies the server to connect the user to the room.
        /// </summary>
        /// <param name="mediaRoomId">The ID of the media room to connect to.</param>
        /// <remarks>
        /// This method:
        /// 1. Creates a new local MediaRoom instance to handle incoming media traffic
        /// 2. Sends a Connect_To_Media_Room_Command to the server
        /// 3. Includes the locally allocated port for media communication
        /// </remarks>
        public void ProcessConnectToMediaRoom(int mediaRoomId)
        {
            MediaRoom mediaRoom = new MediaRoom(mediaRoomId);
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Connect_To_Media_Room_Command;
            clientServerProtocol.MediaRoomId = mediaRoomId;
            clientServerProtocol.MediaPort = mediaRoom.GetPort();
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Disconnects the user from a media room.
        /// Notifies the server that the user is leaving the specified media room.
        /// </summary>
        /// <param name="mediaRoomId">The ID of the media room to disconnect from.</param>
        public void ProcessDisconnectFromMediaRoom(int mediaRoomId)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Disconnect_From_Media_Room_Command;
            clientServerProtocol.MediaRoomId = mediaRoomId;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Handles notification of a new participant joining a media room.
        /// Establishes a connection to the new participant for media exchange.
        /// </summary>
        /// <param name="newParticipantIp">The IP address of the new participant.</param>
        /// <param name="newParticipantPort">The port number on which the new participant is listening.</param>
        /// <param name="userId">The user ID of the new participant.</param>
        /// <param name="username">The username of the new participant.</param>
        public void HandleNewParticipantJoinTheMediaRoom(string newParticipantIp, int newParticipantPort, int userId, string username)
        {
            MediaChannelManager.VideoStreamConnection.ConnectToParticipant(
                    newParticipantIp,
                    newParticipantPort,
                    this.ImageToByteArray(DiscordFormsHolder.getInstance().DiscordApp.GetUserManager().UsersImages[userId]),
                    username,
                    userId);
        }

        /// <summary>
        /// Handles information about all users currently connected to a media room.
        /// Establishes connections to each participant and applies their status effects.
        /// </summary>
        /// <param name="UsersMediaConnectionDetails">A list of details for all users in the media room.</param>
        /// <remarks>
        /// This method is typically called when a user joins a media room to establish
        /// connections with all existing participants. It also applies status effects such as
        /// mute, deafen, and video mute for each connected user.
        /// </remarks>
        public void HandleGetAllIpsOfConnectedUsersInSomeMediaRoom(List<UserMediaConnectionDetails> UsersMediaConnectionDetails)
        {
            foreach (UserMediaConnectionDetails userMediaConnectionDetail in UsersMediaConnectionDetails)
            {
                this.HandleNewParticipantJoinTheMediaRoom(userMediaConnectionDetail.Ip, userMediaConnectionDetail.Port,
                    userMediaConnectionDetail.UserId, userMediaConnectionDetail.Username);

                // Apply status effects if they exist
                bool isMuted = userMediaConnectionDetail.IsAudioMuted;
                bool isDeafened = userMediaConnectionDetail.IsDeafened;
                bool isVideoMuted = userMediaConnectionDetail.IsVideoMuted;

                int userId = userMediaConnectionDetail.UserId;

                if (isMuted)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.OnUserMuteStatusChanged(userId, true)));
                }

                if (isDeafened)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.OnUserDeafenStatusChanged(userId, true)));
                }

                if (isVideoMuted)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.OnUserVideoMuteStatusChanged(userId, true)));
                }
            }
        }

        /// <summary>
        /// Handles notification that a user has left a media room.
        /// Disconnects from the specified user's media stream.
        /// </summary>
        /// <param name="userIp">The IP address of the user who left.</param>
        public void HandleSomeUserLeftTheMediaRoomCommand(string userIp)
        {
            MediaChannelManager.VideoStreamConnection.DisconnectFromParticipant(userIp);
        }

        /// <summary>
        /// Requests a list of all users from the server.
        /// Used to populate the user list in the application.
        /// </summary>
        public void ProcessFetchAllUsers()
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Fetch_All_Users_Command;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Handles the server's response containing details of all users.
        /// Updates the UserManager with the received user details.
        /// </summary>
        /// <param name="allUsersDetails">A list of user details for all users.</param>
        public void HandleGetAllUsersDetails(List<UserDetails> allUsersDetails)
        {
            if (DiscordFormsHolder.getInstance().DiscordApp.IsHandleCreated)
            {
                DiscordFormsHolder.getInstance().DiscordApp.Invoke(
                    new Action(() => DiscordFormsHolder.getInstance().DiscordApp.GetUserManager().ShowAllUsersDetails(allUsersDetails)));
            }

        }

        /// <summary>
        /// Handles notification that a user has joined a media room.
        /// Updates the UI to show the user in the specified media channel and applies their status effects.
        /// </summary>
        /// <param name="userId">The ID of the user who joined.</param>
        /// <param name="mediaRoomId">The ID of the media room the user joined.</param>
        /// <param name="username">The username of the user who joined.</param>
        /// <param name="profilePicture">The profile picture of the user who joined.</param>
        /// <param name="role">The role of the user who joined.</param>
        /// <param name="isMuted">Whether the user's audio is muted.</param>
        /// <param name="isDeafened">Whether the user is deafened.</param>
        /// <param name="isVideoMuted">Whether the user's video is muted.</param>
        public void HandleUserJoinMediaRoom(int userId, int mediaRoomId, string username, byte[] profilePicture, int role, bool isMuted,
            bool isDeafened, bool isVideoMuted)
        {
            if (DiscordFormsHolder.getInstance().DiscordApp.IsHandleCreated)
            {
                // First, add the user to the channel
                DiscordFormsHolder.getInstance().DiscordApp.Invoke(
                    new Action(() => DiscordFormsHolder.getInstance().DiscordApp.GetMediaChannelManager().AddUserToMediaChannel(mediaRoomId,
                    new UserDetails(userId, username, profilePicture, role))));

                // Then, apply any status effects that were active
                if (isMuted)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.OnUserMuteStatusChanged(userId, true)));
                }

                if (isDeafened)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.OnUserDeafenStatusChanged(userId, true)));
                }

                if (isVideoMuted)
                {
                    DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                        DiscordFormsHolder.getInstance().DiscordApp.OnUserVideoMuteStatusChanged(userId, true)));
                }
            }

        }

        /// <summary>
        /// Handles notification that a user has left a media room.
        /// Updates the UI to remove the user from the specified media channel.
        /// </summary>
        /// <param name="userId">The ID of the user who left.</param>
        /// <param name="mediaRoomId">The ID of the media room the user left.</param>
        public void HandleUserLeaveMediaRoom(int userId, int mediaRoomId)
        {
            if (DiscordFormsHolder.getInstance().DiscordApp.IsHandleCreated)
            {
                DiscordFormsHolder.getInstance().DiscordApp.Invoke(
                    new Action(() => DiscordFormsHolder.getInstance().DiscordApp.GetMediaChannelManager().RemoveUserFromMediaChannel(mediaRoomId, userId)));
            }

        }

        /// <summary>
        /// Converts an Image object to a byte array for transmission.
        /// </summary>
        /// <param name="image">The Image to convert.</param>
        /// <returns>A byte array containing the image data in PNG format.</returns>
        /// <remarks>
        /// If the conversion fails, a placeholder gray image is generated and returned.
        /// This ensures that operations dependent on image data don't fail completely.
        /// </remarks>
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

        /// <summary>
        /// Sends a request to the server to update a user's audio mute status.
        /// </summary>
        /// <param name="userId">The ID of the user whose mute status should be updated.</param>
        /// <param name="isMuted">True to mute the user, false to unmute.</param>
        public void ProcessSetUserMuted(int userId, bool isMuted)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Set_Mute_User_Command;
            clientServerProtocol.UserId = userId;
            clientServerProtocol.IsMuted = isMuted;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Sends a request to the server to update a user's deafen status.
        /// </summary>
        /// <param name="userId">The ID of the user whose deafen status should be updated.</param>
        /// <param name="isDeafened">True to deafen the user, false to undeafen.</param>
        public void ProcessSetUserDeafened(int userId, bool isDeafened)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Set_Deafen_User_Command;
            clientServerProtocol.UserId = userId;
            clientServerProtocol.IsDeafened = isDeafened;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Sends a request to the server to disconnect a user from a media room.
        /// </summary>
        /// <param name="userId">The ID of the user to disconnect.</param>
        /// <param name="mediaRoomId">The ID of the media room from which to disconnect the user.</param>
        /// <remarks>
        /// This is typically used by moderators or administrators to forcibly disconnect
        /// a user from a voice or video channel.
        /// </remarks>
        public void ProcessDisconnectUserFromMediaRoom(int userId, int mediaRoomId)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Disconnect_User_From_Media_Room_Command;
            clientServerProtocol.UserId = userId;
            clientServerProtocol.MediaRoomId = mediaRoomId;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Handles notification of a user's audio mute status change.
        /// Updates the UI to reflect the user's new mute status.
        /// </summary>
        /// <param name="userId">The ID of the user whose mute status changed.</param>
        /// <param name="isMuted">The new mute status (true for muted, false for unmuted).</param>
        public void HandleUserMuted(int userId, bool isMuted)
        {
            // Forward the mute status change to the Discord app
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                DiscordFormsHolder.getInstance().DiscordApp.OnUserMuteStatusChanged(userId, isMuted)));
        }

        /// <summary>
        /// Handles notification of a user's deafen status change.
        /// Updates the UI to reflect the user's new deafen status.
        /// </summary>
        /// <param name="userId">The ID of the user whose deafen status changed.</param>
        /// <param name="isDeafened">The new deafen status (true for deafened, false for undeafened).</param>
        public void HandleUserDeafened(int userId, bool isDeafened)
        {
            // Forward the deafen status change to the Discord app
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                DiscordFormsHolder.getInstance().DiscordApp.OnUserDeafenStatusChanged(userId, isDeafened)));
        }

        /// <summary>
        /// Handles notification that a user has been forcibly disconnected from a media room.
        /// Updates the UI to reflect the user's disconnection.
        /// </summary>
        /// <param name="userId">The ID of the user who was disconnected.</param>
        /// <param name="mediaRoomId">The ID of the media room from which the user was disconnected.</param>
        public void HandleUserDisconnected(int userId, int mediaRoomId)
        {
            // Forward the disconnect command to the Discord app
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                DiscordFormsHolder.getInstance().DiscordApp.OnUserDisconnected(userId, mediaRoomId)));
        }

        /// <summary>
        /// Sends a request to the server to update a user's video mute status.
        /// </summary>
        /// <param name="userId">The ID of the user whose video mute status should be updated.</param>
        /// <param name="isVideoMuted">True to mute the user's video, false to unmute.</param>
        public void ProcessSetUserVideoMuted(int userId, bool isVideoMuted)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Set_Video_Mute_User_Command;
            clientServerProtocol.UserId = userId;
            clientServerProtocol.IsVideoMuted = isVideoMuted;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Handles notification of a user's video mute status change.
        /// Updates the UI to reflect the user's new video mute status.
        /// </summary>
        /// <param name="userId">The ID of the user whose video mute status changed.</param>
        /// <param name="isVideoMuted">The new video mute status (true for muted, false for unmuted).</param>
        public void HandleUserVideoMuted(int userId, bool isVideoMuted)
        {
            // Forward the video mute status change to the Discord app
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                DiscordFormsHolder.getInstance().DiscordApp.OnUserVideoMuteStatusChanged(userId, isVideoMuted)));
        }

        /// <summary>
        /// Sends a request to the server to update a user's role.
        /// </summary>
        /// <param name="userId">The ID of the user whose role should be updated.</param>
        /// <param name="newRole">The new role ID to assign to the user.</param>
        /// <remarks>
        /// Roles typically define permissions levels in the application,
        /// such as regular user, moderator, or administrator.
        /// </remarks>
        public void ProcessUpdateUserRole(int userId, int newRole)
        {
            ClientServerProtocol clientServerProtocol = new ClientServerProtocol();
            clientServerProtocol.TypeOfCommand = TypeOfCommand.Update_User_Role_Command;
            clientServerProtocol.UserId = userId;
            clientServerProtocol.Role = newRole;
            System.Diagnostics.Debug.WriteLine("Message sent to server: " + clientServerProtocol.ToString());
            this.ConnectionWithServer.SendMessage(ClientServerProtocolParser.Generate(clientServerProtocol));
        }

        /// <summary>
        /// Handles notification that a user's role has been updated.
        /// Updates the UI to reflect the user's new role.
        /// </summary>
        /// <param name="userId">The ID of the user whose role was updated.</param>
        /// <param name="newRole">The new role ID assigned to the user.</param>
        public void HandleUserRoleHasBeenUpdated(int userId, int newRole)
        {
            // Update the user's role in the UI
            DiscordFormsHolder.getInstance().DiscordApp.Invoke(new Action(() =>
                DiscordFormsHolder.getInstance().DiscordApp.OnUserRoleUpdated(userId, newRole)));
        }






    }
}
