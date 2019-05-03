using Microsoft.OData.JsonLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace TestApp
{
    public class ODataTest
    {
        public static async Task Main(string[] args)
        {
            String path = @"C:\Users\kibandi\Desktop\SimpleExchangeOutput.txt";

            using (StreamReader sr = File.OpenText(path))
            {
                IEdmModel model = builder
                    .BuildAddressType()
                    .BuildCategoryType()
                    .BuildCustomerType()
                    .BuildDefaultContainer()
                    .BuildCustomerSet()
                    .GetModel();

                Stream stream = sr.BaseStream;
                ODataMessageReaderSettings settings = new ODataMessageReaderSettings();
                InMemoryMessage message = new InMemoryMessage { Stream = stream };
                ODataMessageReader reader = new ODataMessageReader((IODataResponseMessage)message, settings);



                stream.Position = 0;
                //var asynchronousReader = reader.CreateODataAsynchronousReader();
                var oDataResourceReader = reader.CreateODataResourceReader();
                while (oDataResourceReader.Read())
                {
                    var oItem = oDataResourceReader.Item;
                    Console.WriteLine(oItem.ToString());
                }
                //var responseMessage = asynchronousReader.CreateResponseMessage();

            }
        }
    }
}
