using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    /// <summary>
    /// Provides asynchronous UDP listening capabilities for receiving data packets
    /// from remote clients in the Discord clone application.
    /// </summary>
    /// <remarks>
    /// This class handles UDP socket communications, including starting and stopping
    /// the listener, receiving messages asynchronously, and processing received data.
    /// It uses a buffer pool to efficiently manage memory when processing network packets.
    /// 
    /// The class implements IDisposable to ensure proper cleanup of network resources
    /// and cancellation tokens when the listener is no longer needed.
    /// </remarks>
    public class UdpListener : IDisposable
    {
        /// <summary>
        /// The UDP client used for receiving network packets.
        /// </summary>
        private readonly UdpClient udpClient;

        /// <summary>
        /// The port number on which the UDP listener is bound.
        /// </summary>
        private readonly int port;

        /// <summary>
        /// Callback action that is invoked when a message is received.
        /// Takes the sender's IP address and the message data as arguments.
        /// </summary>
        private readonly Action<string, byte[]> messageCallback;

        /// <summary>
        /// Cancellation token source for gracefully stopping the listen operation.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// The background task that performs the asynchronous listening operation.
        /// </summary>
        private Task listenTask;

        /// <summary>
        /// Flag indicating whether this instance has been disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Pool of reusable buffers for efficient memory management when processing packets.
        /// </summary>
        private readonly BufferPool bufferPool;

        /// <summary>
        /// Initializes a new instance of the UdpListener class with the specified port and callback.
        /// </summary>
        /// <param name="port">The port number on which to listen for UDP packets.</param>
        /// <param name="callback">The callback action to invoke when a message is received.
        /// Takes the sender's IP address and the message data as arguments.</param>
        /// <exception cref="ArgumentNullException">Thrown when the callback is null.</exception>
        /// <remarks>
        /// The constructor initializes the UDP client, buffer pool, and cancellation token,
        /// but does not start listening until the Start method is called.
        /// </remarks>
        public UdpListener(int port, Action<string, byte[]> callback)
        {
            this.port = port;
            this.messageCallback = callback ?? throw new ArgumentNullException(nameof(callback));
            this.udpClient = new UdpClient(this.port);
            this.cancellationTokenSource = new CancellationTokenSource();
            this.bufferPool = new BufferPool(65536, 10); // Buffer size of 64KB, pool of 10 buffers
        }

        /// <summary>
        /// Starts the UDP listener in a background task.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attempting to start a listener that is already running.
        /// </exception>
        /// <remarks>
        /// This method starts the asynchronous listening operation in a background task.
        /// The listener will continue running until the Stop method is called.
        /// </remarks>
        public void Start()
        {
            if (listenTask != null)
            {
                throw new InvalidOperationException("Listener is already running");
            }
            listenTask = Task.Run(ListenAsync);
        }

        /// <summary>
        /// Stops the UDP listener and releases associated network resources.
        /// </summary>
        /// <remarks>
        /// This method signals cancellation to the listening task and closes the UDP client.
        /// It can be called safely even if the listener is not currently running.
        /// </remarks>
        public void Stop()
        {
            cancellationTokenSource.Cancel();
            udpClient.Close();
        }

        /// <summary>
        /// Asynchronously listens for incoming UDP packets and processes them.
        /// </summary>
        /// <returns>A task representing the asynchronous listening operation.</returns>
        /// <remarks>
        /// This method runs in a loop until cancellation is requested. For each received
        /// packet, it invokes the message callback with the sender's IP address and the
        /// received data. It uses the buffer pool to efficiently manage memory.
        /// 
        /// Exceptions in the callback are caught and logged to prevent the listener
        /// from crashing due to errors in message processing.
        /// </remarks>
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
                        Buffer.BlockCopy(result.Buffer, 0, buffer, 0, result.Buffer.Length);
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

        /// <summary>
        /// Releases all resources used by the UdpListener.
        /// </summary>
        /// <remarks>
        /// This method stops the listener if it's running and releases all network resources.
        /// It implements the IDisposable pattern to ensure proper cleanup.
        /// </remarks>
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