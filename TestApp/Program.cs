using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{

    /// <summary>
    /// Compares if 2 IEnumerable have the same values
    /// </summary>
    internal class EnumerableComparer : IEqualityComparer<IEnumerable<string>>
    {
        public bool Equals(IEnumerable<string> x, IEnumerable<string> y)
        {
            return (x.Count() == y.Count() && (!x.Except(y).Any() || !y.Except(x).Any()));
        }

        public int GetHashCode(IEnumerable<string> obj)
        {
            return obj.Count();
        }
    }

    public static void Main1()
    {
        Console.WriteLine("Executing ");
        RunAsync();
        Console.WriteLine("Completed Await");
        Console.Read();
    }

    public static async Task RunAsync()
    {
        Console.WriteLine("Executing RunAsync");
        await Task.Run(() => WaitForInt());
        Thread.Sleep(100);
        Console.WriteLine("Completed Await RunAsync");
    }

    public static async Task<int> WaitForInt()
    {
        Console.WriteLine("Wait for Int Call");
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine();
            Thread.Sleep(100);
        }

        Console.WriteLine("Completed Wait Call");
        return 1;
    }
}
    
