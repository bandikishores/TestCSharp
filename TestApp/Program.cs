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

    public class IntHolder
    {
        public IntHolder(int value)
        {
            this.Value = value;
        }
        public int Value;
    }

    public static void Main1()
    {
        Console.WriteLine("Executing ");
        var first = new IntHolder(0);
        var second = new IntHolder(1);
        Swap(first, second);
        Console.WriteLine(first.Value);
        Console.WriteLine(second.Value);
        // RunAsync();
        Console.WriteLine("Completed Await");
        Console.ReadKey();
    }

    public static void Swap(IntHolder a, IntHolder b)
    {
        IntHolder temp = a;
        a = b;
        b = temp;
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
    
