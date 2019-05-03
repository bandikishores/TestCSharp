using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestApp
{

    public class CustomArrayPool : IArrayPool<char>
    {
        ArrayPool<char> pool = ArrayPool<char>.Create();

        public volatile int currentAllocatedArray = 0;

        public LinkedList<int> values = new LinkedList<int>();

        public char[] Rent(int minimumLength)
        {
            currentAllocatedArray++;
            var arr = pool.Rent(minimumLength);
            values.AddLast(arr.Length);
            return arr;
        }

        public void Return(char[] array)
        {
            currentAllocatedArray--;
            values.Remove(array.Length);
            pool.Return(array);
        }

        public int getTotalLostArray()
        {
            return currentAllocatedArray;
        }
    }


    public class SimpleNewtonsoftJsonTest
    {
        static bool isMessageExtractionStarted = false;

        static void Main1(string[] args)
        {
            var pool = new CustomArrayPool();

            for (int i = 0; i < 10; i++)
            {
                String path = @"C:\Users\kibandi\Desktop\SampleJson.txt";

                using (StreamReader sr = File.OpenText(path))
                using (var jsonTextReader = new JsonTextReader(sr))
                {

                    // Checking if pooling will help with memory
                    jsonTextReader.ArrayPool = pool;

                    while (jsonTextReader.Read())
                    {
                        if (jsonTextReader.TokenType == JsonToken.PropertyName
                            && ((string)jsonTextReader.Value).Equals("batter"))
                        {
                            jsonTextReader.Read();

                            if (jsonTextReader.TokenType == JsonToken.StartArray)
                            {
                                while (jsonTextReader.Read())
                                {
                                    if (jsonTextReader.TokenType == JsonToken.StartObject)
                                    {
                                        var Current = JToken.Load(jsonTextReader);
                                        // Do Nothing
                                    }
                                    else if (jsonTextReader.TokenType == JsonToken.EndArray)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (jsonTextReader.TokenType == JsonToken.StartObject)
                        {
                            var Current = JToken.Load(jsonTextReader);
                            // Do Nothing.
                        }
                    }
                }
            }

            Console.WriteLine("Total Lost Arrays : " + pool.getTotalLostArray());

            Console.ReadKey();
        }
    }
}
