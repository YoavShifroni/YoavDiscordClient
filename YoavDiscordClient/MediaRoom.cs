using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using YoavDiscordClient.Managers;

namespace YoavDiscordClient
{
    /// <summary>
    /// Represents a virtual media room for audio/video communication in the Discord clone application.
    /// Each MediaRoom is associated with a unique room ID and handles UDP communication
    /// between participants on a dynamically assigned port.
    /// </summary>
    /// <remarks>
    /// The MediaRoom class manages the network communication aspects of a voice/video channel.
    /// It dynamically allocates an available port, sets up a UDP listener, and delegates
    /// data processing to the VideoStreamConnection component.
    /// </remarks>
    public class MediaRoom
    {
        /// <summary>
        /// The unique identifier for this media room.
        /// Corresponds to voice channel IDs in the application.
        /// </summary>
        private int _mediaRoomId;

        /// <summary>
        /// The dynamically assigned port number used for UDP communication in this room.
        /// </summary>
        private int _port;

        /// <summary>
        /// Initializes a new instance of the MediaRoom class with the specified room ID.
        /// </summary>
        /// <param name="mediaRoomId">The unique identifier for this media room.</param>
        /// <remarks>
        /// The constructor automatically:
        /// 1. Assigns the room ID
        /// 2. Dynamically allocates an available port through GetAvailablePort()
        /// 3. Starts the UDP listener for incoming media data
        /// </remarks>
        public MediaRoom(int mediaRoomId)
        {
            this._mediaRoomId = mediaRoomId;
            this._port = this.GetAvailablePort();
            this.StartListeningAndReceiving();
        }

        /// <summary>
        /// Finds and returns an available port on the local machine.
        /// </summary>
        /// <returns>An available port number that can be used for UDP communication.</returns>
        /// <remarks>
        /// This method works by:
        /// 1. Creating a TCP listener bound to port 0 (which tells the OS to assign any available port)
        /// 2. Retrieving the assigned port number
        /// 3. Immediately releasing the port by stopping the listener
        /// 
        /// Note that there is a small possibility that another application could take
        /// the port between when it's released and when it's used for the UDP listener.
        /// </remarks>
        private int GetAvailablePort()
        {
            // Bind to port 0 to let the OS pick an available port
            TcpListener listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop(); // Release the port immediately
            return port;
        }

        /// <summary>
        /// Initializes the UDP listener for receiving media data from participants.
        /// </summary>
        /// <remarks>
        /// This method:
        /// 1. Creates a UdpListener bound to the port obtained from GetAvailablePort()
        /// 2. Sets the callback to process incoming data to the VideoStreamConnection's
        ///    ProcessDataFromOtherUser method
        /// 3. Starts the listener in a background thread
        /// 
        /// The listener will continue running until the application exits or the
        /// MediaRoom is explicitly disposed.
        /// </remarks>
        private void StartListeningAndReceiving()
        {
            UdpListener udpListener = new UdpListener(this._port, MediaChannelManager.VideoStreamConnection.ProcessDataFromOtherUser);
            udpListener.Start();
        }

        /// <summary>
        /// Gets the port number used for UDP communication in this media room.
        /// </summary>
        /// <returns>The port number assigned to this media room.</returns>
        /// <remarks>
        /// This port number is needed by remote clients to connect to this media room.
        /// It should be shared through the application's signaling mechanism.
        /// </remarks>
        public int GetPort()
        {
            return this._port;
        }
    }
}