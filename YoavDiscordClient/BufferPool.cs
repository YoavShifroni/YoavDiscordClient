using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    /// <summary>
    /// Provides a thread-safe pool of reusable byte arrays to reduce memory allocations and garbage collection.
    /// </summary>
    /// <remarks>
    /// The BufferPool class improves application performance by:
    /// <list type="bullet">
    /// <item><description>Reducing memory fragmentation from frequent large byte array allocations</description></item>
    /// <item><description>Minimizing garbage collection pressure for short-lived buffers</description></item>
    /// <item><description>Providing efficient buffer management for network operations</description></item>
    /// </list>
    /// 
    /// This implementation uses a <see cref="ConcurrentBag{T}"/> to provide thread-safe access
    /// to the pool of buffers without requiring explicit synchronization.
    /// </remarks>
    public class BufferPool
    {
        /// <summary>
        /// Thread-safe collection of available byte arrays in the pool.
        /// </summary>
        private readonly ConcurrentBag<byte[]> pool;

        /// <summary>
        /// The fixed size of each buffer in the pool, in bytes.
        /// </summary>
        private readonly int bufferSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferPool"/> class with the specified buffer size and count.
        /// </summary>
        /// <param name="bufferSize">The size of each buffer in the pool, in bytes.</param>
        /// <param name="initialCount">The initial number of buffers to pre-allocate in the pool.</param>
        /// <remarks>
        /// The constructor pre-allocates <paramref name="initialCount"/> buffers of size <paramref name="bufferSize"/>
        /// to minimize allocation delays when the application begins requesting buffers.
        /// </remarks>
        public BufferPool(int bufferSize, int initialCount)
        {
            this.bufferSize = bufferSize;
            pool = new ConcurrentBag<byte[]>();

            // Pre-allocate the initial set of buffers
            for (int i = 0; i < initialCount; i++)
            {
                pool.Add(new byte[bufferSize]);
            }
        }

        /// <summary>
        /// Acquires a buffer from the pool, or creates a new one if none are available.
        /// </summary>
        /// <returns>A byte array of the configured size.</returns>
        /// <remarks>
        /// This method attempts to take an existing buffer from the pool. If the pool is empty,
        /// it automatically creates a new buffer of the specified size.
        /// 
        /// The returned buffer may contain data from previous uses. The caller is responsible for
        /// overwriting this data or ensuring it is not read before it is overwritten.
        /// </remarks>
        public byte[] Rent()
        {
            return pool.TryTake(out var buffer) ? buffer : new byte[bufferSize];
        }

        /// <summary>
        /// Returns a buffer to the pool for future reuse.
        /// </summary>
        /// <param name="buffer">The buffer to return to the pool.</param>
        /// <remarks>
        /// Only buffers of the correct size are added back to the pool. Buffers of different sizes
        /// are ignored, allowing them to be garbage collected.
        /// 
        /// After returning a buffer to the pool, the caller should not access or modify it, as it
        /// may be provided to other parts of the application.
        /// </remarks>
        public void Return(byte[] buffer)
        {
            if (buffer.Length == bufferSize)
            {
                pool.Add(buffer);
            }
        }
    }
}