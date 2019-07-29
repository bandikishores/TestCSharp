using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    public class AsynchronousExample2
    {
        static ThreadLocal<int> threadLocal = new ThreadLocal<int>(() => 1) { Value = 2 };

        /// <summary>
        /// Constructor to intitialize AzureAdlsProvider
        /// </summary>
        public AsynchronousExample2()
        {
        }

        public static void Main1(string[] args)
        {
            Console.WriteLine("Main Program Started Thread Name " + Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Returned Value " + AsyncFunc(null).GetAwaiter().GetResult());
            Console.WriteLine("Main Program Exited Thread Name " + Thread.CurrentThread.ManagedThreadId);
          //  Console.WriteLine("Value in Thread Local {0} in Main for Thread {1}", threadLocal, Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// method to intitialize Azurlsovider
        /// </summary>
        /// <param name="args">uniquely identifies an extraction</param>
        public static async Task<int> AsyncFunc(string[] args)
        {
          //  Console.WriteLine("Value in Thread Local {0} for Thread {1}", threadLocal, Thread.CurrentThread.ManagedThreadId);

            int i = 0;
            List<Task> tasks = new List<Task>();
            for (i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    await Task.Delay(100);
                    Console.WriteLine("Value is {0} for Thread {1}", i, Thread.CurrentThread.ManagedThreadId);
                }));
            }

            i += 2;

            await Task.WhenAll(tasks);

         //   Console.WriteLine("Value in Thread Local {0} for Thread {1}", threadLocal, Thread.CurrentThread.ManagedThreadId);

            return i;
        }

    }
}
