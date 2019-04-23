﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestApp
{

    public class JsonArrayPool : IArrayPool<char>
    {
        ArrayPool<char> pool = ArrayPool<char>.Create();

        private int currentCounter = 0;

        public static int maxConcurrent = 10;

        private Semaphore sem = new Semaphore(maxConcurrent, maxConcurrent);
        
        public char[] Rent(int minimumLength)
        {
           // sem.WaitOne();
            // get char array from System.Buffers shared pool
            return pool.Rent(minimumLength);////ArrayPool<char>.Shared.Rent(minimumLength));
        }

        public void Return(char[] array)
        {
            // return char array to System.Buffers shared pool
            pool.Return(array);
           // sem.Release();
        }
    }


    public class NewtonsoftJsonTest
    {
        static bool isMessageExtractionStarted = false;

        static void Main(string[] args)
        {
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            System.GC.TryStartNoGCRegion(100 * 1024 * 1024);

            var pool = new JsonArrayPool();
            for (int i = 0; i < JsonArrayPool.maxConcurrent * 10; i++)
            {
                pool.Return(pool.Rent((i + 1) * 10 * 1024 ));
            }

            for (int i = 0; i < 100; i++)
            {
                if (i % 2 == 0)
                {
                    Thread.Sleep(1000);
                }
                String path = @"C:\Users\kibandi\Desktop\700KExchangeOutput.txt";

                using (StreamReader sr = File.OpenText(path))
                {
                    var jsonTextReader = new JsonTextReader(sr);
                    // Checking if pooling will help with memory
                    jsonTextReader.ArrayPool = pool;

                    while (true)
                    {
                        while (jsonTextReader.Read())
                        {
                            // As per Rest/Exchange Contract, we look at Value tag to extract data.
                            // This is similar to logic in PageProcessor Except that we look at each chunk of data from the stream to determine the extraction step.
                            if (jsonTextReader.TokenType == JsonToken.PropertyName
                                && ((string) jsonTextReader.Value).Equals("value"))
                            {
                                isMessageExtractionStarted = true;
                                jsonTextReader.Read();

                                // This triggers the start of Each Message Extraction.
                                // Even in this scenario, do not load parse the whole content. Load one object from the array of Content in the stream.
                                // To Identify a single Object/Message is extracted, the JsonTextReader might need to load additional Bytes from the stream than what was needed.
                                // But this is fine and unavoidable. The additional Data read will be stored in The Reader and used when the next message processing takes place.
                                if (jsonTextReader.TokenType == JsonToken.StartArray)
                                {
                                    while (ExtractTokenMessage(jsonTextReader))
                                    {
                                        Console.WriteLine("One Message Extracted");
                                    }
                                }
                            }

                            // Logic to Parse & Extract the Next Link in the request processing.
                            // Its not necessary that this extraction happens before the extraction of Messages.
                            else if (jsonTextReader.TokenType == JsonToken.PropertyName
                                     && ((string) jsonTextReader.Value).Equals("@odata.nextLink"))
                            {
                                jsonTextReader.Read();
                            }
                        }

                        break;
                    }
                }
            }

            System.GC.EndNoGCRegion();

            Console.ReadKey();
        }



        /// <summary>
        /// Reads one message from the list of messages and stop extraction of messages if the whole array is completed.
        /// </summary>
        /// <returns>a Boolean indicating if a message is available for extraction.</returns>
        private static bool ExtractTokenMessage(JsonTextReader jsonTextReader)
        {
            while (jsonTextReader.Read())
            {
                if (jsonTextReader.TokenType == JsonToken.StartObject)
                {
                    var Current = JToken.Load(jsonTextReader);
                    return true;
                }
                else if (jsonTextReader.TokenType == JsonToken.EndArray)
                {
                    isMessageExtractionStarted = false;
                    break;
                }
            }

            return false;
        }
    }
}
