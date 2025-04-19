using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    /// <summary>
    /// Manages the connection between the client and the server, handles sending and receiving encrypted messages.
    /// Implements the Singleton design pattern.
    /// </summary>
    public class ConnectionWithServer
    {
        /// <summary>
        /// The port number used to open the TCP connection to the server.
        /// </summary>
        private const int PORT_NUMBER = 500;

        /// <summary>
        /// The TCP client used to communicate with the server.
        /// </summary>
        private TcpClient _tcpClient;

        /// <summary>
        /// Indicates whether the RSA public key has already been sent to the server.
        /// </summary>
        private bool _isRsaSent = false;

        /// <summary>
        /// Handles incoming commands received from the server.
        /// </summary>
        private HandleCommandFromServer _handleCommandFromServer;

        /// <summary>
        /// Handles the underlying TCP connection and message transmission logic.
        /// </summary>
        private TcpConnectionHandler _tcpConnectionHandler;

        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        private static ConnectionWithServer _instance = null;

        /// <summary>
        /// Returns the singleton instance of the class, initializing it if necessary.
        /// Thread-safe due to the synchronized method.
        /// </summary>
        /// <param name="ipAddress">The IP address of the server.</param>
        /// <returns>The singleton instance of the connection.</returns>
        /// <exception cref="ArgumentNullException">Thrown if IP address is null when the instance is being initialized.</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ConnectionWithServer GetInstance(string ipAddress)
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
        /// Private constructor used to initialize the TCP connection and send the RSA public key.
        /// </summary>
        /// <param name="ipAddress">The IP address of the server.</param>
        private ConnectionWithServer(string ipAddress)
        {
            this._tcpConnectionHandler = new TcpConnectionHandler(new TcpClient(), this);
            this._handleCommandFromServer = new HandleCommandFromServer();
            this._tcpConnectionHandler.Connect(ipAddress, PORT_NUMBER);
            this._tcpConnectionHandler.SendRsaPublicKey();
            this._tcpConnectionHandler.StartListen();
        }

        /// <summary>
        /// Sends a message to the server over the TCP connection, encrypting it as needed.
        /// </summary>
        /// <param name="message">The plaintext message to be sent.</param>
        public void SendMessage(string message)
        {
            this._tcpConnectionHandler.SendMessage(message);
        }

        /// <summary>
        /// Processes a received message from the server, decrypting it and handling commands if needed.
        /// </summary>
        /// <param name="messageData">The raw byte array representing the encrypted message.</param>
        /// <param name="bytesRead">The number of bytes read for the message.</param>
        /// <param name="isFirstMessage">True if this is the first message from the server, typically containing AES keys.</param>
        public void ProcessMessage(byte[] messageData, int bytesRead, bool isFirstMessage)
        {
            string textFromServer = Encoding.UTF8.GetString(messageData, 0, bytesRead);

            if (isFirstMessage)
            {
                // Decrypt with RSA and initialize AES keys
                textFromServer = RsaFunctions.Decrypt(textFromServer);
                AesFunctions.AesKeys = JsonConvert.DeserializeObject<AesKeys>(textFromServer);
            }
            else
            {
                // Decrypt with AES and process individual command lines
                textFromServer = AesFunctions.Decrypt(textFromServer);
                string[] stringSeparators = new string[] { ClientServerProtocolParser.MessageTrailingDelimiter };
                string[] lines = textFromServer.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    Task.Run(() => this._handleCommandFromServer.HandleCommand(line));
                }
            }

            if (bytesRead < 1)
            {
                MessageBox.Show("You are disconnected");
                //GameViewManager.getInstance(null).StopGame(); // Optional reconnect logic can be placed here
                return;
            }
        }
    }
}
