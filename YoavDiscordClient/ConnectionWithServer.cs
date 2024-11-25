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
        /// Byte's array that will be used for receiving messages from the server
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// Boolean that tell if this is the first message that received from the server
        /// </summary>
        private bool _isFirstMessageReceived = true;

        /// <summary>
        /// Boolean that tell if the rsa public key already sent to the server or not
        /// </summary>
        private bool _isRsaSent = false;

        /// <summary>
        /// Instance of the class handle command from server that will be used everytime that the user will receive a message from the server
        /// </summary>
        private HandleCommandFromServer _handleCommandFromServer;

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
            this._tcpClient = new TcpClient();
            this._handleCommandFromServer = new HandleCommandFromServer();
            this._tcpClient.Connect(ipAddress, PORT_NUMBER);
            var jsonString = JsonConvert.SerializeObject(RsaFunctions.PublicKey);
            this.SendMessage(jsonString);
            this._isRsaSent = true;
            this._data = new byte[this._tcpClient.ReceiveBufferSize];
            this._tcpClient.GetStream().BeginRead(this._data,
                                                 0,
                                                 System.Convert.ToInt32(this._tcpClient.ReceiveBufferSize),
                                                 ReceiveMessage,
                                                 null);
        }

        /// <summary>
        /// The function convert the string that we want to send to the server into byte array and send it to the server over the tcp connection
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            try
            {
                // send message to the server
                NetworkStream ns = this._tcpClient.GetStream();

                if(this._isRsaSent)
                {
                    while(this._isFirstMessageReceived)
                    {
                        Thread.Sleep(1000);
                    }
                    message = AesFunctions.Encrypt(message);
                    Console.WriteLine("message sent to the server: " + message);
                }

                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                byte[] length = BitConverter.GetBytes(data.Length);
                byte[] bytes = new byte[data.Length + 4];
                // combine byte array, I took this code from the website StackOverFlow in this link:
                // https://stackoverflow.com/questions/415291/best-way-to-combine-two-or-more-byte-arrays-in-c-sharp
                System.Buffer.BlockCopy(length, 0, bytes, 0,  length.Length); 
                System.Buffer.BlockCopy(data, 0, bytes, length.Length, data.Length);

                // send the text
                ns.Write(bytes, 0, bytes.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// The function is the endless loop of reading messages that the server sends to the client and works in a different thread than the forms
        /// In case the TCP connection is disconnected, it will show a message box with an error
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                int bytesRead;

                // read the data from the server
                bytesRead = this._tcpClient.GetStream().EndRead(ar);

                if (bytesRead < 1)
                {
                    MessageBox.Show("You are disconnected");
                    //GameViewManager.getInstance(null).StopGame();
                    return;
                }
                else
                {
                    // invoke the delegate to display the recived data
                    string textFromServer = System.Text.Encoding.UTF8.GetString(this._data, 0, bytesRead);

                    if(this._isFirstMessageReceived)
                    {
                        textFromServer = RsaFunctions.Decrypt(textFromServer);
                        AesFunctions.AesKeys = JsonConvert.DeserializeObject<AesKeys>(textFromServer);
                        this._isFirstMessageReceived = false;
                    }
                    else
                    {
                        textFromServer = AesFunctions.Decrypt(textFromServer);
                        string[] stringSeparators = new string[] { "\r\n" };
                        string[] lines = textFromServer.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            this._handleCommandFromServer.HandleCommand(lines[i]);

                        }
                    }


                }

                // continue reading
                this._tcpClient.GetStream().BeginRead(this._data,
                                         0,
                                         System.Convert.ToInt32(this._tcpClient.ReceiveBufferSize),
                                         ReceiveMessage,
                                         null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // ignore the error... fired when the user loggs off

            }
        }
    }
}
