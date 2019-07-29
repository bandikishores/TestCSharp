namespace TestApp
{
    using System;
    using System.Buffers;
    using System.Collections.Concurrent;

    /// <summary>
    /// Class which creates an array of arraypools of type T
    /// </summary>
    /// <typeparam name="T">the type parameter for the class</typeparam>
    public class FixedStandardArrayPool<T> : ArrayPool<T>
    {
        /// <summary>
        /// Size of the each buffer array
        /// </summary>
        private readonly int bufferSize;

        /// <summary>
        /// Pool of buffer arrays that may be reused
        /// </summary>
        private readonly BlockingCollection<T[]> pool;

        /// <summary>
        /// Total number of objects created 
        /// </summary>
        private int totalObjectsCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedStandardArrayPool{T}"/> class.
        /// </summary>
        /// <param name="bufferSize">Size of the buffers that can be rented</param>
        /// <param name="concurrency">Number of concurrent rents allowed</param>
        public FixedStandardArrayPool(int bufferSize, int concurrency)
        {
            this.pool = new BlockingCollection<T[]>(concurrency);
            this.totalObjectsCreated = 0;
            this.bufferSize = bufferSize;
        }

        /// <summary>
        /// Rent a buffer from the pool.
        /// 
        /// The array returned by Rent is owned by the caller of rent, but should be returned to 
        /// the pool via a call to Return when the renter no longer needs a copy.
        /// </summary>
        /// <param name="minimumLength">ignored value</param>
        /// <returns>Array of type <see cref="T"/> with length <see cref="bufferSize"/></returns>
        public override T[] Rent(int minimumLength)
        {
            T[] buffer;

            if (!this.pool.TryTake(out buffer))
            {
                // Create a new one if the pool is not full
                lock (this)
                {
                    if (totalObjectsCreated < this.pool.BoundedCapacity)
                    {
                        buffer = new T[this.bufferSize];
                        totalObjectsCreated++;
                    }
                }

                // No new T was created, so wait for a release to happen.
                if (buffer == null)
                {
                    buffer = this.pool.Take();
                }
            }

            return buffer;
        }

        /// <summary>
        /// Return a buffer to the pool
        /// </summary>
        /// <param name="buffer">Buffer for return</param>
        /// <param name="clearArray">Should array be cleared before returning to the pool</param>
        public override void Return(T[] buffer, bool clearArray = false)
        {
            if (clearArray)
            {
                Array.Clear(buffer, 0, buffer.Length);
            }

            lock (this)
            {
                // If we have over allocated or returned an array which is not matching in length, then we will not store this buffer.
                if (this.pool.Count >= this.pool.BoundedCapacity || buffer.Length != this.bufferSize)
                {
                    return;
                }

                // Otherwise Add it to Queue
                this.pool.Add(buffer);
            }
        }
    }
}
