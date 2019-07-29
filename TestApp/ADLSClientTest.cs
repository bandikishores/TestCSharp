using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.DataLake.Store;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestApp
{
    public class ADLSClientTest
    {
        public static async Task Main1(string[] args)
        {
            String path = @"C:\Users\kibandi\Desktop\ADLSWrite.txt";
            var jToken = JToken.Parse(File.ReadAllText(path));

            var data = File.ReadAllText(@"C:\Users\kibandi\Desktop\MediumMessageExchange.txt");
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            var adlsClient = AdlsClient.CreateClient(jToken["AccountFqdn"].ToString(), jToken["Token"].ToString());
            await adlsClient.ConcurrentAppendAsync("", true, buffer, 0, buffer.Length);
            Console.ReadKey();
        }
    }
}
