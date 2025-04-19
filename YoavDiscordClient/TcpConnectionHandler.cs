using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoavDiscordClient;

namespace YoavDiscordClient
{
    /// <summary>
    /// Handles TCP communication between the client and the server.
    /// Manages sending and receiving of encrypted messages, message buffering, and stream handling.
    /// </summary>
    public class TcpConnectionHandler
    {
        /// <summary>
        /// Length of the current message being read from the client.
        /// </summary>
        private int messageLength = -1;

        /// <summary>
        /// Total number of bytes read from the current message.
        /// </summary>
        private int totalBytesRead = 0;

        /// <summary>
        /// Indicates if the first message has been received from the client.
        /// </summary>
        private bool _isFirstMessage = true;

        /// <summary>
        /// Indicates whether the RSA public key has been sent.
        /// </summary>
        private bool _isRsaSent = false;

        /// <summary>
        /// Buffer for reading data from the client.
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// Memory stream used for assembling incoming message data.
        /// </summary>
        private MemoryStream memoryStream = new MemoryStream();

        /// <summary>
        /// The TCP client representing the connection.
        /// </summary>
        private TcpClient _client;

        /// <summary>
        /// Reference to the class that handles server message processing.
        /// </summary>
        private ConnectionWithServer _connectionWithServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpConnectionHandler"/> class.
        /// </summary>
        /// <param name="tcpClient">The TCP client used for communication.</param>
        /// <param name="connectionWithServer">The server connection handler.</param>
        public TcpConnectionHandler(TcpClient tcpClient, ConnectionWithServer connectionWithServer)
        {
            this._client = tcpClient;
            this._connectionWithServer = connectionWithServer;
            this._data = new byte[this._client.ReceiveBufferSize];
        }

        /// <summary>
        /// Starts listening for incoming messages from the server.
        /// </summary>
        public void StartListen()
        {
            _client.GetStream().BeginRead(this._data,
                                          0,
                                          Convert.ToInt32(this._client.ReceiveBufferSize),
                                          ReceiveMessage,
                                          _client.GetStream());
        }

        /// <summary>
        /// Connects the client to a server using the specified IP and port.
        /// </summary>
        /// <param name="ip">Server IP address.</param>
        /// <param name="port">Server port.</param>
        public void Connect(string ip, int port)
        {
            this._client.Connect(ip, port);
        }

        /// <summary>
        /// Sends the RSA public key to the server.
        /// This is used to initiate secure communication.
        /// </summary>
        public void SendRsaPublicKey()
        {
            var jsonString = JsonConvert.SerializeObject(RsaFunctions.PublicKey);
            this.SendMessage(jsonString);
            this._isRsaSent = true;
        }

        /// <summary>
        /// Sends a message to the server.
        /// Encrypts the message if RSA exchange has already been completed.
        /// </summary>
        /// <param name="message">The plaintext message to send.</param>
        public void SendMessage(string message)
        {
            try
            {
                NetworkStream ns;

                // Prevent simultaneous stream access
                lock (this._client.GetStream())
                {
                    ns = this._client.GetStream();
                }

                if (this._isRsaSent)
                {
                    while (this._isFirstMessage)
                    {
                        Thread.Sleep(1000);
                    }
                    message = AesFunctions.Encrypt(message);
                }

                byte[] data = Encoding.UTF8.GetBytes(message);
                byte[] length = BitConverter.GetBytes(data.Length);
                byte[] bytes = new byte[data.Length + 4];

                Buffer.BlockCopy(length, 0, bytes, 0, length.Length);
                Buffer.BlockCopy(data, 0, bytes, length.Length, data.Length);

                ns.Write(bytes, 0, bytes.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Callback function for asynchronous read operation.
        /// Handles incoming messages and starts the next read.
        /// </summary>
        /// <param name="ar">The result of the asynchronous operation.</param>
        private void ReceiveMessage(IAsyncResult ar)
        {
            NetworkStream stream = (NetworkStream)ar.AsyncState;

            try
            {
                int bytesRead = stream.EndRead(ar);
                if (bytesRead > 0)
                {
                    HandleReceivedMessage(bytesRead);
                }

                lock (this._client.GetStream())
                {
                    this._client.GetStream().BeginRead(this._data, 0, Convert.ToInt32(this._client.ReceiveBufferSize),
                        ReceiveMessage, _client.GetStream());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                stream.Close();
            }
        }

        /// <summary>
        /// Processes incoming message data and checks for complete messages.
        /// If a full message is received, it is passed to the connection handler.
        /// </summary>
        /// <param name="bytesRead">Number of bytes read from the stream.</param>
        private void HandleReceivedMessage(int bytesRead)
        {
            if (this.messageLength == -1 && this.totalBytesRead < 4)
            {
                int remainingLengthBytes = 4 - this.totalBytesRead;
                int bytesToCopy = Math.Min(bytesRead, remainingLengthBytes);
                memoryStream.Write(this._data, 0, bytesToCopy);
                this.totalBytesRead += bytesToCopy;

                if (this.totalBytesRead >= 4)
                {
                    this.memoryStream.Seek(0, SeekOrigin.Begin);
                    byte[] lengthBytes = new byte[4];
                    this.memoryStream.Read(lengthBytes, 0, 4);
                    this.messageLength = BitConverter.ToInt32(lengthBytes, 0);
                    this.memoryStream.SetLength(0);
                }

                if (bytesRead > bytesToCopy)
                {
                    this.memoryStream.Write(this._data, bytesToCopy, bytesRead - bytesToCopy);
                    this.totalBytesRead += bytesRead - bytesToCopy;
                }
            }
            else
            {
                this.memoryStream.Write(this._data, 0, bytesRead);
                this.totalBytesRead += bytesRead;
            }

            if (this.messageLength > 0 && this.totalBytesRead >= this.messageLength + 4)
            {
                this._connectionWithServer.ProcessMessage(this.memoryStream.ToArray(), this.messageLength, this._isFirstMessage);
                this._isFirstMessage = false;

                int remainingDataBytes = this.totalBytesRead - (this.messageLength + 4);

                this.messageLength = -1;
                this.totalBytesRead = 0;
                this.memoryStream.SetLength(0);

                if (remainingDataBytes > 0)
                {
                    byte[] newData = new byte[this._client.ReceiveBufferSize];
                    Array.Copy(this._data, bytesRead - remainingDataBytes, newData, 0, remainingDataBytes);
                    this._data = newData;
                    HandleReceivedMessage(remainingDataBytes);
                }
            }
        }
    }
}
