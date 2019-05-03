using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;

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

    public static async Task Main1()
    {
        Console.WriteLine("Executing ");
        var firstArray = new byte[10];
        var tempArray = new byte[10];
        for (int i = 0; i < firstArray.Length; i++)
        {
            firstArray[i] = 1;
        }

        var secondArray = new byte[3];
        for (int i = 0; i < secondArray.Length; i++)
        {
            secondArray[i] = 2;
        }

        var thirdArray = new byte[255];
        for (int i = 0; i < thirdArray.Length; i++)
        {
            thirdArray[i] = (byte)i;
        }

        var memoryStream = new MemoryStream(firstArray);
        //var memoryStream1 = new MemoryStream(secondArray);
        var memoryStream1 = new MemoryStream();
        var writer = new StreamWriter(memoryStream1);
        writer.Write("Hey!");
        writer.Flush();
        memoryStream1.Position = 0;
        //var memoryStream2 = new MemoryStream(thirdArray);
        var memoryStream2 = new MemoryStream();
        var writer1 = new StreamWriter(memoryStream2);
        writer1.Write("alksdhjf ladiauhsd ajsdg SDf sdf @$#% RWSG$ d ad7ftawo8ef");
        writer1.Flush();
        memoryStream2.Position = 0;

        for (int i = 0; i < firstArray.Length; i++)
        {
            Console.Write(firstArray[i] + ",");
        }
        Console.WriteLine("");

        Array.Copy(firstArray, tempArray, firstArray.Length);
        Array.Clear(firstArray, 0, firstArray.Length);
        int currentBytesRead = 0;
        int bytesToRead = firstArray.Length;

        do
        {
            currentBytesRead = await memoryStream1?.ReadAsync(firstArray, 0, firstArray.Length);
            bytesToRead -= currentBytesRead;
        } while (bytesToRead > 0 && currentBytesRead != 0);

        Console.Write(Encoding.UTF8.GetString(firstArray));
        Console.WriteLine("");


        currentBytesRead = 0;
        bytesToRead = firstArray.Length;
        do
        {
            Array.Copy(firstArray, tempArray, firstArray.Length);
            Array.Clear(firstArray, 0, firstArray.Length);
            do
            {
                currentBytesRead = await memoryStream2?.ReadAsync(firstArray, 0, firstArray.Length);
                bytesToRead -= currentBytesRead;
            } while (bytesToRead > 0 && currentBytesRead != 0);

            if (currentBytesRead > 0)
            {
                Console.Write(Encoding.UTF8.GetString(firstArray));
            }
            else
            {
                Console.Write(Encoding.UTF8.GetString(tempArray));
                Console.Write(tempArray.Length);
            }

            Console.WriteLine("");
        } while (currentBytesRead > 0);

        Array.Clear(firstArray, 0, firstArray.Length);

        var sdf = await memoryStream2?.ReadAsync(firstArray, 0, firstArray.Length);

        /*
         
        Array.Copy(secondArray, firstArray, secondArray.Length);
        memoryStream.Read(buffer, 0, buffer.Length);
        await memoryStream2.CopyToAsync(memoryStream, firstArray.Length);

        for (int i = 0; i < firstArray.Length; i++)
        {
            Console.Write(firstArray[i] + ",");
        }
        
         */

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
    
