    using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public class ConnectionWithServer
    {
        /// <summary>
        /// The port number that will be used in order to open tcp connection to the server
        /// </summary>
        private const int PORT_NUMBER = 500;

        /// <summary>
        /// The tcp client that will connect to the server
        /// </summary>
        private TcpClient _tcpClient;

        /// <summary>
        /// Boolean that tell if the rsa public key already sent to the server or not
        /// </summary>
        private bool _isRsaSent = false;

        /// <summary>
        /// Instance of the class handle command from server that will be used everytime that the user will receive a message from the server
        /// </summary>
        private HandleCommandFromServer _handleCommandFromServer;

        private TcpConnectionHandler _tcpConnectionHandler;

        /// <summary>
        /// The instance of this class per singleton design pattern
        /// </summary>
        private static ConnectionWithServer _instance = null;


        /// <summary>
        /// Static getInstance method, as in Singleton patterns. Protected with mutex
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ConnectionWithServer getInstance(string ipAddress)
        {
            if (_instance == null)
            {
                if (ipAddress == null)
                {
                    throw new ArgumentNullException("ip address cannot be null when instance is null");
                }
                _instance = new ConnectionWithServer(ipAddress);
            }
            return _instance;
        }

        /// <summary>
        /// Constructor that get the server ip as a parameter. The constructor create a tcp connection to the server and after that send
        /// the rsa public key to the server
        /// </summary>
        /// <param name="ipAddress"></param>
        private ConnectionWithServer(string ipAddress)
        {
            this._tcpConnectionHandler = new TcpConnectionHandler(new TcpClient(), this);
            this._handleCommandFromServer = new HandleCommandFromServer();
            this._tcpConnectionHandler.Connect(ipAddress, PORT_NUMBER);
            this._tcpConnectionHandler.SendRsaPublicKey();
            this._tcpConnectionHandler.StartListen();
            
        }

        /// <summary>
        /// The function convert the string that we want to send to the server into byte array and send it to the server over the tcp connection
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            this._tcpConnectionHandler.SendMessage(message);
        }

        public void ProcessMessage(byte[] messageData, int bytesRead, bool isFirstMessage)
        {
            string textFromServer = System.Text.Encoding.UTF8.GetString(messageData, 0, bytesRead);

            if (isFirstMessage)
            {
                textFromServer = RsaFunctions.Decrypt(textFromServer);
                AesFunctions.AesKeys = JsonConvert.DeserializeObject<AesKeys>(textFromServer);
            }
            else
            {
                textFromServer = AesFunctions.Decrypt(textFromServer);
                string[] stringSeparators = new string[] { ClientServerProtocolParser.MessageTrailingDelimiter };
                System.Diagnostics.Debug.WriteLine("Recived from server: " + textFromServer);
                string[] lines = textFromServer.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    Task.Run(() => this._handleCommandFromServer.HandleCommand(line));
                }
            }

            if (bytesRead < 1)
            {
                MessageBox.Show("You are disconnected");
                //GameViewManager.getInstance(null).StopGame();
                return;
            }
        }
    }
}
