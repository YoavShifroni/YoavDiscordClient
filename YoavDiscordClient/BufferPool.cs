using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    public class BufferPool
    {

        private readonly ConcurrentBag<byte[]> pool;
        private readonly int bufferSize;

        public BufferPool(int bufferSize, int initialCount)
        {
            this.bufferSize = bufferSize;
            pool = new ConcurrentBag<byte[]>();

            for (int i = 0; i < initialCount; i++)
            {
                pool.Add(new byte[bufferSize]);
            }
        }

        public byte[] Rent()
        {
            return pool.TryTake(out var buffer) ? buffer : new byte[bufferSize];
        }

        public void Return(byte[] buffer)
        {
            if (buffer.Length == bufferSize)
            {
                pool.Add(buffer);
            }
        }
    }
}
