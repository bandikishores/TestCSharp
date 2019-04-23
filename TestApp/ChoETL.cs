using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoETL;

namespace TestApp
{
    public class ChoETL
    {
        static void Main1(string[] args)
        {
            String path = @"C:\Users\kibandi\Desktop\700KExchangeOutput.txt";

            using (StreamReader sr = File.OpenText(path))
            {
                using (var p = new ChoJSONReader(sr))
                {
                    foreach (var rec in p)
                    {
                        Console.WriteLine($"Name: {rec.name}, Id: {rec.id}");
                    }
                }
            }
        }
    }
}
