
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using first;
using second;
using System.Threading;
using System.Threading.Tasks;

delegate int getMyValue();

namespace TestApp
{
    public class Lion
    {
        public int IValue { get; set; }
        public Lion()
        {
            IValue = 5;
        }

        public int printIValue()
        {
            Console.WriteLine(IValue + " value has been printed");
            Thread.Sleep(100);
            return 123;
        }
    }

    public class A
    {
        public virtual void Print()
        {
            Console.WriteLine("Class A");
        }
    }
    public class B : A
    {
        public sealed override void Print()
        {
            Console.WriteLine("Class B");
        }
    }
    public class C : B
    {
        public new void Print()
        {
            Console.WriteLine("Class C");
        }
    }

    class MyClass
    {
        private string name;

        public string Name { get => name; set => name = value; }
    }

    class Inheritance
    {
        public unsafe static void Main()
        {
            /*
            List<int> intList = new List<int>();
            intList.Add(1);
            intList.Add(2);
            intList.Add(3);

            Console.WriteLine(intList.Count(i => i != 2));


            System.Threading.Thread.Sleep(5000);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("hf", "sdf");
            dic.Add("as", "as");
            dic.Add("as1", "as2");
            dic.Add("as2", "as3");
            dic.Add("as3", "as4");
            dic.Add("as4", "as5");
            dic.Add("as5", "as2");
            dic.Add("as6", "as3");
            dic.Add("as7", "as6");

            Dictionary<MyClass, string> dict = new Dictionary<MyClass, string>();

            dict.Where(k => k.Key != null).Where(k => k.Key.Name.Contains("as")).ToList().ForEach(v => Console.WriteLine(v));
            dict.Keys.Where(k => k != null).Where(k => k.Name.Contains("as")).ToList().ForEach(v => Console.WriteLine(v));

            InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.ImportPSModule(new string[]
            {
                @"D:\enlistments\MARS\target\Distrib\product\all\debug\amd64\MarsAutoPilot\Mars_InternalCommandlets\Microsoft.Exchange.Mars.MarsInternalCommandlets.dll"
            });

            using (Runspace myRunSpace = RunspaceFactory.CreateRunspace(initialSessionState))
            {
                myRunSpace.Open();
                using (PowerShell powerShellInstance = PowerShell.Create())
                {
                    powerShellInstance.Commands.AddCommand("Invoke-SetupJob");
                    Collection<PSObject> PSOutput = powerShellInstance.Invoke();
                    foreach (PSObject outputItem in PSOutput)
                    {
                        // if null object was dumped to the pipeline during the script then a null
                        // object may be present here. check for null to prevent potential NRE.
                        if (outputItem != null)
                        {
                            Console.WriteLine(outputItem);
                        }
                    }
                }
            }
            */

            /* Thread myThread = new Thread(delegate ()
             {
                 Thread.Sleep(50);
                 Console.WriteLine("myThread Called");
                 Console.ReadKey();
                 Console.WriteLine("Exiting Thread");
             });

             myThread.Start();
             Console.WriteLine("Hi");

             getMyValue getMyObj = new getMyValue(new Lion().printIValue);
             Console.WriteLine(getMyObj.BeginInvoke((System.IAsyncResult ar) =>
             {
                 Console.WriteLine("Callback invoked!!");
                 //   Func<int> myFunc = ar.AsyncState as Func<int>;
                 //  Console.WriteLine(  myFunc.EndInvoke(ar) + " Result found after callback");
             }, null) + " After Being Invoke");

             myThread.Join();

             Console.WriteLine("Completed joining");*/

            // Console.WriteLine("System Error: Error: {\"ExceptionString\":\"System.IO.IOException: The network path was not found.\r\n\r\n at Microsoft.CloudTest.Extensibility.Plugins.Native.NativeWorker.<SetupResourceAsync>d__0.MoveNext() in d:\\dbs\\sh\\che2\\1024_104035_0\\cmd\\27\\private\\Extensibility\\NativePlugins\\Worker\\NativeWorker.cs:line 147\r\n--- End of stack trace from previous location where exception was thrown ---\r\n at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()\r\n at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n at Microsoft.CloudTest.WorkerService.Controllers.WorkerController.<SetupCallback>d__6b.MoveNext() in d:\\dbs\\sh\\che2\\1024_104035_0\\cmd\\2\\private\\CloudTestWorker\\WorkerHost\\Controllers\\WorkerController.cs:line 621\",\"Type\":\"System.IO.IOException\",\"ErrorType\":2,\"Message\":\"The network path was not found.\r\n\",\"HelpText\":\"Test execution failed. Check log files for details.\",\"SubSystem\":\"NativeWorker\",\"HResult\":\"0x80070035\",\"IsUserError\":false,\"IsSystemError\":true} {\"ExceptionString\":\"System.NullReferenceException: Object reference not set to an instance of an object.\r\n at Microsoft.CloudTest.Extensibility.Plugins.Native.NativeWorker.<CleanupAsync>d__d.MoveNext() in d:\\dbs\\sh\\che2\\1024_104035_0\\cmd\\27\\private\\Extensibility\\NativePlugins\\Worker\\NativeWorker.cs:line 216\r\n--- End of stack trace from previous location where exception was thrown ---\r\n at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()\r\n at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n at Microsoft.CloudTest.WorkerService.Controllers.WorkerController.<>c__DisplayClass4e.<<CleanupCallback>b__4c>d__50.MoveNext() in d:\\dbs\\sh\\che2\\1024_104035_0\\cmd\\2\\private\\CloudTestWorker\\WorkerHost\\Controllers\\WorkerController.cs:line 498\r\n--- End of stack trace from previous location where exception was thrown ---\r\n at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()\r\n at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n at Microsoft.CloudTest.TaskHelper.<TimeoutAfter>d__10`1.MoveNext() in d:\\dbs\\sh\\che2\\1023_145311\\cmd\\5\\private\\CloudTestBase\\CloudTestBase\\TaskHelper.cs:line 0\r\n--- End of stack trace from previous location where exception was thrown ---\r\n at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()\r\n at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n at Microsoft.CloudTest.WorkerService.Controllers.WorkerController.<CleanupCallback>d__53.MoveNext() in d:\\dbs\\sh\\che2\\1024_104035_0\\cmd\\2\\private\\CloudTestWorker\\WorkerHost\\Controllers\\WorkerController.cs:line 491\",\"Type\":\"System.NullReferenceException\",\"ErrorType\":2,\"Message\":\"Object reference not set to an instance of an object.\",\"HelpText\":\"Test execution failed. Check log files for details.\",\"SubSystem\":\"NativeWorker\",\"HResult\":\"0x80004003\",\"IsUserError\":false,\"IsSystemError\":true}");

            testAsync();
            Console.WriteLine("Called TestAsync " + Thread.CurrentThread.GetHashCode());

            Console.ReadKey();
        }

        public static async Task testAsync()
        {
            // Thread.Sleep(500);
            Console.WriteLine("Inside TestAsync " + Thread.CurrentThread.GetHashCode());
            await Method1(true);
            //await Method2(3);
            Thread.Sleep(100);
            Console.WriteLine("Returning TestAsync " + Thread.CurrentThread.GetHashCode());
        }

        public static async Task<int> Method1(bool firstTime)
        {
            int value = 100;
            ThreadLocal<int> threadLocalValue = new ThreadLocal<int>();
            threadLocalValue.Value = 50;
            Thread.Sleep(1000);
            Console.WriteLine(" Inside Method 1 " + Thread.CurrentThread.GetHashCode());
             await Task.Run(() =>
             {
                 for (int i = 0; i < 5; i++)
                 {
                     Thread.Sleep(50);
                     if (true)
                     {
                         Console.WriteLine(" Method 1 " + Thread.CurrentThread.GetHashCode());
                     }
                 }
             });
            Task.Delay(100);
            Thread.Sleep(1000);
            Console.WriteLine(" Returning Method 1 " + Thread.CurrentThread.GetHashCode());
            Console.WriteLine(" Value is  " + value);
            Console.WriteLine(" threadLocalValue.Value is  " + threadLocalValue.Value);

            return 3;
        }

        public static Task Method2(int count)
        {
            Console.WriteLine(" Inside Method 2 " + Thread.CurrentThread.GetHashCode());
            //new Program().Method1(false);

            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(2);
                Console.WriteLine(" Method 2 " + Thread.CurrentThread.GetHashCode());
            }
            Console.WriteLine(" Returning Method 2 " + Thread.CurrentThread.GetHashCode());
            return Task.CompletedTask;
        }
    }
}
