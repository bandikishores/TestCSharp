﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    // Tests ConfigureAwait and ThreadContext details.
    public class AsynchronousExample3
    {
        static ThreadLocal<int> threadLocal = new ThreadLocal<int>(() => 1) { Value = 2 };

        /// <summary>
        /// Constructor to intitialize AzureAdlsProvider
        /// </summary>
        public AsynchronousExample3()
        {
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Main Program Started Thread Name " + Thread.CurrentThread.ManagedThreadId);
            // Console.WriteLine("Returned Value " + AsyncFunc(null).GetAwaiter().GetResult());
            AsyncPump.Run(async delegate
            {
                await AsyncFunc(null);
            });
            Console.WriteLine("Main Program Exited Thread Name " + Thread.CurrentThread.ManagedThreadId);
          //  Console.WriteLine("Value in Thread Local {0} in Main for Thread {1}", threadLocal, Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// method to intitialize Azurlsovider
        /// </summary>
        /// <param name="args">uniquely identifies an extraction</param>
        public static async Task<int> AsyncFunc(string[] args)
        {
            Console.WriteLine("Value in Thread Local {0} for Thread {1}", threadLocal, Thread.CurrentThread.ManagedThreadId);
            HttpClient client = new HttpClient();
            var t = client.GetAsync("http://www.google.com");
            var currentContext = SynchronizationContext.Current;

            await t.ContinueWith(delegate
            {
                if (currentContext == null)
                    Console.WriteLine("Null Context");
                else
                    currentContext.Post(delegate { Console.WriteLine("Context Available"); }, null);

            }, TaskScheduler.Current);//.ConfigureAwait(false);

            Console.WriteLine("Value in Thread Local {0} for Thread {1}", threadLocal, Thread.CurrentThread.ManagedThreadId);

            return 0;
        }

    }

    /// <summary>Provides a pump that supports running asynchronous methods on the current thread.</summary>
    public static class AsyncPump
    {
        /// <summary>Runs the specified asynchronous function.</summary>
        /// <param name="func">The asynchronous function to execute.</param>
        public static void Run(Func<Task> func)
        {
            if (func == null) throw new ArgumentNullException("func");

            var prevCtx = SynchronizationContext.Current;
            try
            {
                // Establish the new context
                var syncCtx = new SingleThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(syncCtx);

                // Invoke the function and alert the context to when it completes
                var t = func();
                if (t == null) throw new InvalidOperationException("No task provided.");
                t.ContinueWith(delegate { syncCtx.Complete(); }, TaskScheduler.Default);

                // Pump continuations and propagate any exceptions
                syncCtx.RunOnCurrentThread();
                t.GetAwaiter().GetResult();
            }
            finally { SynchronizationContext.SetSynchronizationContext(prevCtx); }
        }

        /// <summary>Provides a SynchronizationContext that's single-threaded.</summary>
        private sealed class SingleThreadSynchronizationContext : SynchronizationContext
        {
            /// <summary>The queue of work items.</summary>
            private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> m_queue =
                new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();
            /// <summary>The processing thread.</summary>
            private readonly Thread m_thread = Thread.CurrentThread;

            /// <summary>Dispatches an asynchronous message to the synchronization context.</summary>
            /// <param name="d">The System.Threading.SendOrPostCallback delegate to call.</param>
            /// <param name="state">The object passed to the delegate.</param>
            public override void Post(SendOrPostCallback d, object state)
            {
                if (d == null) throw new ArgumentNullException("d");
                m_queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
            }

            /// <summary>Not supported.</summary>
            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("Synchronously sending is not supported.");
            }

            /// <summary>Runs an loop to process all queued work items.</summary>
            public void RunOnCurrentThread()
            {
                foreach (var workItem in m_queue.GetConsumingEnumerable())
                    workItem.Key(workItem.Value);
            }

            /// <summary>Notifies the context that no more work will arrive.</summary>
            public void Complete() { m_queue.CompleteAdding(); }
        }
    }
}
