using Org.BouncyCastle.Utilities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace YoavDiscordClient
{
    public class UdpListener : IDisposable
    {
        private readonly UdpClient udpClient;
        private readonly int port;
        private readonly Action<string, byte[]> messageCallback;
        private CancellationTokenSource cancellationTokenSource;
        private Task listenTask;
        private bool isDisposed;

        private readonly BufferPool bufferPool;

        public UdpListener(int port, Action< string, byte[]> callback)
        {
            this.port = port;
            this.messageCallback = callback ?? throw new ArgumentNullException(nameof(callback));
            this.udpClient = new UdpClient(this.port);
            this.cancellationTokenSource = new CancellationTokenSource();
            this.bufferPool = new BufferPool(65536, 10);
        }

        public void Start()
        {
            if (listenTask != null)
            {
                throw new InvalidOperationException("Listener is already running");
            }

            listenTask = Task.Run(ListenAsync);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            udpClient.Close();
        }

        private async Task ListenAsync()
        {
            try
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var buffer = this.bufferPool.Rent();
                    try
                    {
                        var result = await udpClient.ReceiveAsync();
                        Buffer.BlockCopy(result.Buffer,0,buffer,0,result.Buffer.Length);
                        messageCallback(result.RemoteEndPoint.Address.ToString(), buffer.AsSpan(0, result.Buffer.Length).ToArray());
                    }
                    catch (Exception callbackEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in callback: {callbackEx.Message}");
                    }
                    finally
                    {
                        bufferPool.Return(buffer);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // UDP client was closed, exit gracefully
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error listening: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                Stop();
                udpClient.Dispose();
                cancellationTokenSource.Dispose();
                isDisposed = true;
            }
        }
    }

}


