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
using YoavDiscordServer;

namespace YoavDiscordClient
{
    public class ConnectionWithServer
    {
        private const int PORT_NUMBER = 500;

        private TcpClient _tcpClient;

        private byte[] _data;

        private bool _isFirstMessageReceived = true;

        private bool _isRsaSent = false;

        private HandleCommandFromServer _handleCommandFromServer;

        private static ConnectionWithServer _instance = null;



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


        private ConnectionWithServer(string ipAddress)
        {
            Console.WriteLine("Enter function constructor connection: " + DateTime.Now.Millisecond);
            this._tcpClient = new TcpClient();
            this._handleCommandFromServer = new HandleCommandFromServer();
            this._tcpClient.Connect(ipAddress, PORT_NUMBER);
            var jsonString = JsonConvert.SerializeObject(RsaFunctions.PublicKey);
            this.SendMessage(jsonString);
            this._isRsaSent = true;
            this._data = new byte[this._tcpClient.ReceiveBufferSize];
            Console.WriteLine("Leave function constructor connection: " + DateTime.Now.Millisecond);
            this._tcpClient.GetStream().BeginRead(this._data,
                                                 0,
                                                 System.Convert.ToInt32(this._tcpClient.ReceiveBufferSize),
                                                 ReceiveMessage,
                                                 null);
        }


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
                    Console.WriteLine("out of the while");
                    message = AesFunctions.Encrypt(message);
                    Console.WriteLine("message sent to the server: " + message);
                }

                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);


                // send the text
                ns.Write(data, 0, data.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// This function is the endless loop of reading messages that the server sends to the client
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
                        Console.WriteLine("Enter function Receive message first: " + DateTime.Now.Millisecond);
                        textFromServer = RsaFunctions.Decrypt(textFromServer);
                        AesFunctions.AesKeys = JsonConvert.DeserializeObject<AesKeys>(textFromServer);
                        this._isFirstMessageReceived = false;
                        Console.WriteLine("Leave function Receive message first: " + DateTime.Now.Millisecond);
                    }
                    else
                    {
                        Console.WriteLine("Enter function Receive message not first: " + DateTime.Now.Millisecond);
                        textFromServer = AesFunctions.Decrypt(textFromServer);
                        string[] stringSeparators = new string[] { "\r\n" };
                        string[] lines = textFromServer.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            this._handleCommandFromServer.HandleCommand(lines[i]);

                        }
                        Console.WriteLine("Leave function Receive message not first: " + DateTime.Now.Millisecond);
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
