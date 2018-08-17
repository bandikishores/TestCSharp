using System;
using System.Linq;
using first;
using second;
using System.Threading;

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
    class Program
    {
        public unsafe static void Main()
        {
            Thread myThread = new Thread(delegate ()
            {
                Thread.Sleep(50);
                Console.WriteLine("myThread Called");
                Console.ReadKey();
            });

            myThread.Start();
            Console.WriteLine("Hi");

            getMyValue getMyObj = new getMyValue(new Lion().printIValue);
            Console.WriteLine(getMyObj.BeginInvoke(delegate(System.IAsyncResult ar)
            {
                Console.WriteLine("Callback invoked!!");
             //   Func<int> myFunc = ar.AsyncState as Func<int>;
              //  Console.WriteLine(  myFunc.EndInvoke(ar) + " Result found after callback");
            }, null) + " After Being Invoke");
        }
    }
}
