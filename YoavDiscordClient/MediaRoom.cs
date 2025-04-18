using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using YoavDiscordClient.Managers;


namespace YoavDiscordClient
{
    public class MediaRoom
    {
        private int _mediaRoomId;

        private int _port;
        public MediaRoom(int mediaRoomId) 
        {
            this._mediaRoomId = mediaRoomId;
            this._port = this.GetAvailablePort();
            this.StartListeningAndReceiving();

        }

        private int GetAvailablePort()
        {
            // Bind to port 0 to let the OS pick an available port
            TcpListener listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();

            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop(); // Release the port immediately
            return port;
        }

        private void StartListeningAndReceiving()
        {
            UdpListener udpListener = new UdpListener(this._port, MediaChannelManager.VideoStreamConnection.ProcessDataFromOtherUser);
            udpListener.Start();
        }

        public int GetPort()
        {
            return this._port;
        }
    }
}
