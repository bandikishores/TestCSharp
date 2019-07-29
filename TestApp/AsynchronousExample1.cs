using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    public class AsynchronousExample1
    {
        /// <summary>
        /// To use lock inside async method
        /// </summary>
        private SemaphoreSlim readLock;

        /// <summary>
        /// Constructor to intitialize AzureAdlsProvider
        /// </summary>
        public AsynchronousExample1()
        {
            this.readLock = new SemaphoreSlim(1, 1);
        }

        public static void Main1(string[] args)
        {
            Console.WriteLine("Main Program Started Thread Name " + Thread.CurrentThread.ManagedThreadId);
            AsyncFunc(null).GetAwaiter().GetResult();
            Console.WriteLine("Main Program Exited Thread Name " + Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// method to intitialize Azurlsovider
        /// </summary>
        /// <param name="args">uniquely identifies an extraction</param>
        public static async Task AsyncFunc(string[] args)
        {
            AsynchronousExample1 sample = new AsynchronousExample1();
          /*  Task first = Task.Run(async () => {
                await sample.readLock.WaitAsync();
                Console.WriteLine("First inside Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
                // await Task.Delay(100);
                Thread.Sleep(100);
                Console.WriteLine("Released inside Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
                sample.readLock.Release();
            });
            */
            // Display the number of command line arguments:
            Console.WriteLine("AsyncFunc Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);

            Task k = sample.RentAsync();
            string g = "Some Random String Creation";
            Console.WriteLine(g);
            await k;
            //await first;
            Console.WriteLine("Finished AsyncFunc Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// method to intitialize AzulsProvider
        /// </summary>
        /// <returns>The tt stream</returns>
        public async Task RentAsync()
        {
            // Create a new one if the pool is not full
            await this.readLock.WaitAsync();

            try
            {
                Console.WriteLine("RentAsync Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
            }
            finally
            {
                this.readLock.Release();
            }

            // thread.Name = "My Thread";
            Console.WriteLine("Finished RentAsync Thread Name = {0}", Thread.CurrentThread.ManagedThreadId);
        }

    }
}
